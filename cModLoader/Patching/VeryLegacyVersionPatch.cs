using cModLoader.UI;
using cModLoader.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace cModLoader.Patching
{
    /// <summary> Patch class used to patch versions 0.1 (not actually using patches since we don't need to) </summary>
    public static class VeryLegacyVersionPatch
    {
        /// <summary> Called when "Invoke cModLoader" is pressed. </summary>
        public static event Action OnClickCModLoader = null;

        internal static UI.LegacyUIText DefaultModMenuButton = new UI.LegacyUIText("Invoke cModLoader", false, 1f, 1f, Color.White, Color.White * 0.5f, true) {
            HAlign = 1f,
            MarginRight = 10,
            MarginTop = 10,
        };
        /*
        internal static void LoadPatches() {
            cModPatch.AddPatch("Terraria.Main", "Draw", "System.Void", new string[] { "Microsoft.Xna.Framework.GameTime" }, null, (body, il, inst, asm) => {
                var f = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic;
                Instruction _il = null;
                try {
                    _il = il._FindIL(new OpCode?[] { OpCodes.Ldfld, OpCodes.Callvirt }, new string[] { "spriteBatch", "Begin" }, 0);
                    if (_il != null) {
                        il._CreateCall(body, asm, typeof(VeryLegacyVersionPatch).GetMethod(nameof(PreDraw), f), out var call, out var arg);
                        il.InsertAfter(_il, call);
                        for (int i = arg.Count - 1; i >= 0; i--)
                            il.InsertAfter(_il, arg[i]);
                    }
                    else Accessibility.Show("Failed to insert PreDraw");
                } catch (Exception e) {
                    Accessibility.Show("Try Error: Failed to insert PreDraw: " + e.Message);
                }
                try {
                    _il = il._FindIL(new OpCode?[] { OpCodes.Ldfld, OpCodes.Ldsfld }, new string[] { "spriteBatch", "cursorTexture" }, -2);
                    if (_il != null) {
                        il._CreateCall(body, asm, typeof(VeryLegacyVersionPatch).GetMethod(nameof(Draw), f), out var call, out var arg);
                        for (int i = 0; i < arg.Count; i++)
                            il.InsertBefore(_il, arg[i]);
                        il.InsertBefore(_il, call);
                    }
                    else Accessibility.Show("Failed to insert Draw");
                } catch (Exception e) {
                    Accessibility.Show("Try Error: Failed to insert Draw: " + e.Message);
                }
                try {
                    _il = il._FindIL(new OpCode?[] { OpCodes.Ldfld, OpCodes.Callvirt }, new string[] { "spriteBatch", "End" }, -2);
                    if (_il != null) {
                        il._CreateCall(body, asm, typeof(VeryLegacyVersionPatch).GetMethod(nameof(PostDraw), f), out var call, out var arg);
                        for (int i = 0; i < arg.Count; i++)
                            il.InsertBefore(_il, arg[i]);
                        il.InsertBefore(_il, call);
                    }
                    else Accessibility.Show("Failed to insert PostDraw");
                } catch (Exception e) {
                    Accessibility.Show("Try Error: Failed to insert PostDraw: " + e.Message);
                }
            });
        }
        internal static void PreDraw(object main, GameTime time) { GlobalHooks.PreDraw(main, time); }
        internal static void Draw(object main, GameTime time) {
            GlobalHooks.Draw(main, time);
            
        }
        internal static void PostDraw(object main, GameTime time) { GlobalHooks.PostDraw(main, time); }
        */
        internal static void _BaseDraw(GameReference game) {
            DefaultModMenuButton.Draw(game);
            var info = DefaultModMenuButton.GetDrawnDetails();
            if (info.clicked) OnClickCModLoader?.Invoke();
        }


    }
}
