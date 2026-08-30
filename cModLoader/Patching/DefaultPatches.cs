using cModLoader.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace cModLoader.Patching
{
    /// <summary> Patch class used for base/default patches mostly required to make every versions work seamlessly </summary>
    public class DefaultPatches {
        /*
        internal static cModPatch.Patch UpdatePatch;
        internal static Action<GameTime> Update = null;
        internal static cModPatch.Patch DrawPatch;
        internal static Action<GameTime> Draw = null;
        public static void LoadPatches() {
            UpdatePatch = cModPatch.AddPatch("Terraria.Main", "Update", "System.Void", new string[] { "Microsoft.Xna.Framework.GameTime" }, typeof(DefaultPatches).GetMethod(nameof(UpdatePatchFunc)));
            DrawPatch = cModPatch.AddPatch("Terraria.Main", "Draw", "System.Void", new string[] { "Microsoft.Xna.Framework.GameTime" }, typeof(DefaultPatches).GetMethod(nameof(DrawPatchFunc)));
        }
        public static void UpdatePatchFunc(GameReference game) {
            if (Update == null) Update = Delegate.CreateDelegate(typeof(Action<GameTime>), game.game, UpdatePatch.originalFunction) as Action<GameTime>;
            GlobalHooks.PreUpdate(game);
            Update(game.gameTime);
            GlobalHooks.PostUpdate(game);
        }
        public static void DrawPatchFunc(GameReference game) {
            if (Draw == null) Draw = Delegate.CreateDelegate(typeof(Action<GameTime>), game.game, DrawPatch.originalFunction) as Action<GameTime>;
            GlobalHooks.PreDraw(game);
            Draw(game.gameTime);
            GlobalHooks.PostDraw(game);
        }
        public static void LoadPatches() {

            cModPatch.AddPatch("Terraria.Main", ".ctor", "System.Void", null, null, (body, il, instrs, asm) => {
                il._CreateCall(body, asm, typeof(DefaultPatches).GetMethod(nameof(OnMainInstance)), out var call, out var args);
                il.InsertBefore(instrs[0], call);
                for (int i = 0; i < args.Count; i++) {
                    il.InsertBefore(instrs[0], args[i]);
                }
            });

        }

        public static void OnMainInstance() {
            GlobalPatchData.OnStartTerraria += (o, e) =>
            {

            };
        }
        */
    }
}
