using cModLoader.UI;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using System;

namespace cModLoader.Patching
{
    /// <summary> Patch class used to patch versions 0.7 to 1.0.6.1 </summary>
    internal class LegacyVersionPatch
    {
        /*
        internal static void LoadPatches()
        {
            // add menu drawing patches
            cModPatch.AddPatch("Terraria.Main", "DrawMenu", "System.Void", new string[] { }, null, (body, il, inst, asm) => {
                var f = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic;
                Instruction _il = null;
                // there is no Begin in non version 0.1
                try {
                    _il = il._FindIL(new OpCode?[] { OpCodes.Ldfld, OpCodes.Ldsfld }, new string[] { "spriteBatch", "logoTexture" }, -2);
                    if (_il != null) {
                        il._CreateCall(body, asm, typeof(LegacyVersionPatch).GetMethod(nameof(PreDrawMenu), f), out var call, out var arg);
                        for (int i = 0; i < arg.Count; i++)
                            il.InsertAfter(_il, arg[i]);
                        il.InsertAfter(_il, call);
                    }
                    else Accessibility.Show("IL Error: Failed to insert PreDrawMenu");
                } catch (Exception e) {
                    Accessibility.Show("Try Error: Failed to insert PreDrawMenu: " + e.Message);
                }
                // 2 show up in 0.7, this gets the first by default
                try {
                    _il = il._FindIL(new OpCode?[] { OpCodes.Ldfld, OpCodes.Ldsfld }, new string[] { "spriteBatch", "cursorTexture" }, -2);
                    if (_il != null) {
                        il._CreateCall(body, asm, typeof(LegacyVersionPatch).GetMethod(nameof(DrawMenu), f), out var call, out var arg);
                        for (int i = 0; i < arg.Count; i++)
                            il.InsertAfter(_il, arg[i]);
                        il.InsertAfter(_il, call);
                    }
                    else Accessibility.Show("Failed to insert DrawMenu");
                } catch (Exception e) {
                    Accessibility.Show("Try Error: Failed to insert DrawMenu: " + e.Message);
                }
                try {
                    _il = il._FindIL(new OpCode?[] { OpCodes.Ldfld, OpCodes.Callvirt }, new string[] { "spriteBatch", "End" }, -2);
                    if (_il != null) {
                        il._CreateCall(body, asm, typeof(LegacyVersionPatch).GetMethod(nameof(PostDrawMenu), f), out var call, out var arg);
                        for (int i = 0; i < arg.Count; i++)
                            il.InsertAfter(_il, arg[i]);
                        il.InsertAfter(_il, call);
                    }
                    else Accessibility.Show("Failed to insert PostDrawMenu");
                } catch (Exception e) {
                    Accessibility.Show("Try Error: Failed to insert PostDrawMenu: " + e.Message);
                }

            });
        }

        internal static void PreDrawMenu(object main) { }
        internal static void DrawMenu(object main) { }
        internal static void PostDrawMenu(object main) { }
        */
    }

}
