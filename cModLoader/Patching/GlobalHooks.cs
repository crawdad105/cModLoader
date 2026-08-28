using cModLoader.ModComponents;
using cModLoader.UI;
using cModLoader.Utils;
using Microsoft.SqlServer.Server;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using static cModLoader.ModLoader;

namespace cModLoader.Patching
{
    /// <summary>
    /// Class storing events for common Terraria functions.<br/>
    /// The events in this class are NOT guaranteed to be invoked if patches are disabled, many of the functions can be found elsewhere (still requiring patches) like in <see cref="ModComponents.Mod"/>.
    /// <para>See <see cref="RawHooks"/> for hooks that do not require patches.</para>
    /// </summary>
    public static class GlobalHooks {

        /// <summary>
        /// These raw hooks do not require patches and work in any version of windows and works on Linux versions 1.4.1 and up.<br/>
        /// </summary>
        public static class RawHooks {
            /// <summary>
            /// Called before both Main.Update and Main.Draw are called every frame.
            /// <para>
            /// <paramref name="Windows"/>: Called right before <see cref="Game"/>.HostIdle() (right before <see cref="Game"/>.Tick()) from inside Game.host.Idle<br/>This is also not called on the first frame of the game, but rather after it.<br/>
            /// Note: this runs way faster then Terraria, it is not tied to Terraria's framerate.
            /// </para>
            /// <para>
            /// <paramref name="Linux"/>: Only works in Linux versions 1.4.1 and up.<br/>Called right before <see cref="Game"/>.Tick() (or in <see cref="Game"/>.Tick() but before Main.Update and Main.Draw) <br/>
            /// </para>
            /// </summary>
            public static event Action<GameReference> OnPreTick;
            /// <summary>
            /// Called after both Main.Update and Main.Draw are called every frame. Main.Draw may not have been called.
            /// <para>
            /// <paramref name="Windows"/>: Called right after <see cref="Game"/>.HostIdle() (right after <see cref="Game"/>.Tick()) from inside Game.host.Idle
            /// </para>
            /// <para>
            /// <paramref name="Linux"/>: Called NEVER, FINED A PLACE TO CALL THIS. <br/>
            /// </para>
            /// </summary>
            public static event Action<GameReference> OnPostTick;
            /// <summary>
            /// Called before everything else has been draw, this is directly before <see cref="IGraphicsDeviceManager.BeginDraw()"/> called inside <see cref="Game.BeginDraw()"/>
            /// <para>
            /// <paramref name="Linux"/>: Only works in Linux versions 1.4.1 and up.
            /// </para>
            /// </summary>
            public static event Action<GameReference> OnRawPreDraw;
            /// <summary>
            /// Called after everything else has been draw, this is directly after <see cref="IGraphicsDeviceManager.EndDraw()"/> called inside <see cref="Game.EndDraw()"/>
            /// <para>
            /// <paramref name="Linux"/>: Only works in Linux versions 1.4.1 and up.
            /// </para>
            /// </summary>
            public static event Action<GameReference> OnRawPostDraw;
            /// <summary> Called when <see cref="Console.WriteLine()"/> is called<br/>Do NOT call anything that will write to console here or it will create an infinite loop. </summary>
            public static event Action<string> OnConsoleOutput;
            /// <summary>
            /// Called right before the mouse is drawn when not "in game" (not in a world) . Only works in versions after and including 1.3.3
            /// </summary>
            public static event Action<GameReference> OnPreMouseInterface;
            /// <summary>
            /// Called right after the mouse "in game" (in a world) options are draw. Only works in versions after and including 1.3.3
            /// </summary>
            public static event Action<GameReference> OnOptionsInterface;

            /// <summary> Before <see cref="Game"/>.HostIdle() (<see cref="Game"/>.Tick()) </summary>
            internal static void PreTick(GameReference game) {
                if (!ModLoader.LoadedModLoader) ModLoader.InitModLoader(game);
                OnPreTick?.Invoke(game);
            }
            /// <summary> After <see cref="Game"/>.HostIdle() (<see cref="Game"/>.Tick()) </summary>
            internal static void PostTick(GameReference game) {
                OnPostTick?.Invoke(game);
            }
            /// <summary> Before <see cref="Game.BeginDraw()"/> </summary>
            internal static void RawPreDraw(GameReference game) {
                if (DefaultPatches.Draw == null) { // only run if not patched
                    ModLoader.PreDraw(game);
                }
                OnRawPreDraw?.Invoke(game);
            }
            /// <summary> After <see cref="Game.EndDraw()"/> </summary>
            internal static void RawPostDraw(GameReference game) {
                OnRawPostDraw?.Invoke(game);
                if (DefaultPatches.Draw == null) { // only run if not patched
                    ModLoader.PostDraw(game);
                }
                InputHelper.SetState();
            }
            /// <summary> Called when <see cref="Console.WriteLine()"/> </summary>
            internal static void ConsoleOutput(string text) {
                OnConsoleOutput?.Invoke(text);
            }
            internal static bool PreMouseInterfaceDraw() {
                ModContext.RunUnderModLoaderContext(() => {
                    var _ref = GameReference.StaticReference;
                    ModLoader.DrawInterface(_ref);
                    OnPreMouseInterface?.Invoke(_ref);                    
                }, "PreMouseInterfaceDraw");
                return true;
            }
            internal static bool OptionsHookInterfaceDraw() {
                var result = false;
                ModContext.RunUnderModLoaderContext(() => {
                    var _ref = GameReference.StaticReference;
                    result = ModMenu.DrawInterface_11_IngameOptionsMenu_Hook(_ref);
                    OnOptionsInterface?.Invoke(_ref);
                }, "OptionsHookInterfaceDraw Hook");
                return result;
            }

            public unsafe static class UnsafeHooks {
                [DllImport("kernel32.dll")]
                private static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
                [StructLayout(LayoutKind.Sequential)]
                private struct RECT { public int Left, Top, Right, Bottom; }
                [UnmanagedFunctionPointer(CallingConvention.StdCall)]
                private delegate int FooDelegate(IntPtr self, IntPtr rectPtr);


                private static FooDelegate _original;
                private static FooDelegate _hook;
                private static bool didHook;

                /// <summary>
                /// <para> Don't use this because its not useful, it can't not do what i wanted. </para>
                /// This only works on modern UI on windows (because it only supports 32 bit) and is "<see langword="unsafe"/>".<br/>
                /// This will hook (replaced the pointer of) an <see langword="unsafe"/> (native unmanaged) function within the "<see langword="get"/>" method of <see cref="GraphicsDevice.ScissorRectangle"/>.<br/>
                /// This modifies the pointer with offset 304 of <see cref="IDirect3DDevice9"/>.
                /// <para>Returns if the hook was successful.</para>
                /// </summary>
                internal static bool HookUIElementDrawCall(GameReference game) {
                    if (didHook) {
                        return false;
                    }
                    // checks to make sure we don't cause issues
                    if (IntPtr.Size != 4) {
                        Output.Error("HookUIElementDrawCall was called but IntPtr.Size is not 4.");
                        return false;
                    }
                    // get pComPtr
                    var field = typeof(GraphicsDevice).GetField("pComPtr", BindingFlags.NonPublic | BindingFlags.Instance);
                    // get ptr
                    void* ptr = Pointer.Unbox(field.GetValue(game.spriteBatch.GraphicsDevice));
                    int rawPtrAddr = *(int*)ptr;
                    uint* ptr2 = (uint*)(rawPtrAddr + 304);
                    uint ptr2Addr = *ptr2;
                    void* functionPtr = (void*)(int)ptr2Addr;

                    _original = (FooDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)functionPtr, typeof(FooDelegate)); // cast to function
                    // create hook
                    _hook = new FooDelegate(Hook);
                    IntPtr hookPtr = Marshal.GetFunctionPointerForDelegate(_hook);

                    // write hook to original location
                    uint oldProtect;
                    VirtualProtect((IntPtr)ptr2, (UIntPtr)IntPtr.Size, 0x40, out oldProtect); // 0x40 = PAGE_EXECUTE_READWRITE
                    Marshal.WriteIntPtr((IntPtr)ptr2, hookPtr);
                    VirtualProtect((IntPtr)ptr2, (UIntPtr)IntPtr.Size, oldProtect, out oldProtect);
                    
                    didHook = true;
                    return true;
                }
                private static int Hook(IntPtr self, IntPtr rectPtr) {
                    int num = _original(self, rectPtr);

                    return num;
                }
            }
        
            internal static void NoHooks(string reason = null) {
                Accessibility.Show($"No hooks have been found for this versions of Terraria.\n{(reason != null ? $"Reason: {reason}" : "")}");
            }

        }


        /// <summary>  Use <see cref="ModComponents.Mod.OnPreUpdate(GameReference)"/> instead. </summary>
        public static event Action<GameReference> OnPreUpdate;
        /// <summary> Use <see cref="ModComponents.Mod.OnPostUpdate(GameReference)"/> instead. </summary>
        public static event Action<GameReference> OnPostUpdate;
        /// <summary> Use <see cref="ModComponents.Mod.OnPreDraw(GameReference)"/> instead. </summary>
        public static event Action<GameReference> OnPreDraw;
        /// <summary> Use <see cref="ModComponents.Mod.OnPostDraw(GameReference)"/> instead. </summary>
        public static event Action<GameReference> OnPostDraw;
        internal static void PreUpdate(GameReference game) {
            ModLoader.PreUpdate(game);
            OnPreUpdate?.Invoke(game);
        }
        internal static void PostUpdate(GameReference game) {
            ModLoader.PostUpdate(game);
            OnPostUpdate?.Invoke(game);
        }
        internal static void PreDraw(GameReference game) {
            ModLoader.PreDraw(game);
            OnPreDraw?.Invoke(game);
        }
        internal static void PostDraw(GameReference game) {
            ModLoader.PostDraw(game);
            OnPostDraw?.Invoke(game);
        }

    }
}
