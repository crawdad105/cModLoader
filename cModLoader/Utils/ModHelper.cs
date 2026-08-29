using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace cModLoader.Utils {
    /// <summary>
    /// General values that should be helpful.
    /// </summary>
    public static class ModHelper {
        /// <summary> Screen width according to <c>Terraria.Main.screenWidth</c> </summary>
        public static int ScreenWidth => Terraria.StaticReference.Main.GetValue<int>("screenWidth");
        /// <summary> Screen height according to <c>Terraria.Main.screenHeight</c> </summary>
        public static int ScreenHeight => Terraria.StaticReference.Main.GetValue<int>("screenHeight");
        /// <summary> Is the player in the world according to <c>!Terraria.Main.gameMenu</c><br/>Always <see langword="true"/> for version 0.1/Alpha.</summary>
        public static bool IsInWorld => Terraria.VersionChecks.Is0_1 ? true : !Terraria.StaticReference.Main.GetValue<bool>("gameMenu");
        /// <summary> Should the in game UI be hidden according to <c>Terraria.Main.hideUI</c><br/>Always <see langword="false"/> for version 0.1/Alpha.</summary>
        public static bool HideUI => Terraria.VersionChecks.Is0_1 ? false : Terraria.StaticReference.Main.GetValue<bool>("hideUI");

        /// <summary> Gets the list of projectiles according to <c>Terraria.Main.projectile</c></summary>
        public static Array Projectiles => Terraria.StaticReference.Main.GetValue<Array>("projectile");
        /// <summary> Gets the list of projectiles according to <c>Terraria.Main.npc</c></summary>
        public static Array NPCs => Terraria.StaticReference.Main.GetValue<Array>("npc");
        /// <summary> Gets the list of players according to <c>Terraria.Main.player</c></summary>
        public static Array Players => Terraria.StaticReference.Main.GetValue<Array>("player");
        /// <summary> Gets the user's player's inventory according to <c>Terraria.Main.player[Terraria.Main.myPlayer].inventory</c></summary>
        public static Array PlayerInventory => Player.GetValue<Array>("inventory");
        /// <summary> Gets the user's player according to <c>Terraria.Main.player[Terraria.Main.myPlayer]</c> (equivalent to <c>Terraria.Main.LocalPlayer</c>) </summary>
        public static Dynamic Player => new Dynamic(Players.GetValue(MyPlayer));
        /// <summary> Gets the user's player according to <c>Terraria.Main.player[Terraria.Main.myPlayer].inventory[Terraria.Main.player[Terraria.Main.myPlayer].selectedItem]</c> (equivalent to <c>Terraria.Main.HeldItem</c>) </summary>
        public static Dynamic PlayerHeldItem => new Dynamic(PlayerInventory.GetValue(Player.GetValue<int>("selectedItem")));
        /// <summary> Gets the user's player id/index according to <c>Terraria.Main.myPlayer</c></summary>
        public static int MyPlayer => Terraria.StaticReference.Main.GetValue<int>("myPlayer");
        /// <summary> Gets the mouse position for UI stuff according to <c>Terraria.Main.mouseX</c> and <c>Terraria.Main.mouseY</c> (equivalent to <c>Terraria.Main.MouseScreen</c>)<para>Only works in versions above and including 1.1. If not in that version this returns <see cref="Mouse.GetState()"/>.</para></summary>
        public static Vector2 MousePos => !Terraria.VersionChecks.Using_Modern_MousePosXY ? new Vector2(Mouse.GetState().X, Mouse.GetState().Y) : new Vector2(Terraria.StaticReference.Main.GetValue<int>("mouseX"), Terraria.StaticReference.Main.GetValue<int>("mouseY"));


        /// <summary> Calls <c>Terraria.GameInput.PlayerInput.SetZoom_UI()</c>.<para>Only works in versions above and including 1.3.5.</para></summary>
        public static void SetZoom_UI() { if (Terraria.VersionChecks.Using_Modern_UIInterfaceScaling) Terraria.StaticReference.PlayerInput.Invoke("SetZoom_UI"); }
        /// <summary> Calls <c>Terraria.GameInput.PlayerInput.SetZoom_Background()</c>.<para>Only works in versions above and including 1.3.5.</para></summary>
        public static void SetZoom_Background() { if (Terraria.VersionChecks.Using_Modern_UIInterfaceScaling) Terraria.StaticReference.PlayerInput.Invoke("SetZoom_Background"); }
        /// <summary> Calls <c>Terraria.GameInput.PlayerInput.SetZoom_World()</c>.<para>Only works in versions above and including 1.3.5.</para></summary>
        public static void SetZoom_World() { if (Terraria.VersionChecks.Using_Modern_UIInterfaceScaling) Terraria.StaticReference.PlayerInput.Invoke("SetZoom_World"); }
        /// <summary> Calls <c>Terraria.GameInput.PlayerInput.SetZoom_Unscaled()</c>.<para>Only works in versions above and including 1.3.5.</para></summary>
        public static void SetZoom_Unscaled() { if (Terraria.VersionChecks.Using_Modern_UIInterfaceScaling) Terraria.StaticReference.PlayerInput.Invoke("SetZoom_Unscaled"); }
        /// <summary> Calls <c>Terraria.GameInput.PlayerInput.SetZoom_MouseInWorld()</c>.<para>Only works in versions above and including 1.3.5.</para></summary>
        public static void SetZoom_MouseInWorld() { if (Terraria.VersionChecks.Using_Modern_UIInterfaceScaling) Terraria.StaticReference.PlayerInput.Invoke("SetZoom_MouseInWorld"); }
        /// <summary> Calls <c>Terraria.GameInput.PlayerInput.SetZoom_Scaled()</c>.<para>Only works in versions above and including 1.3.5.</para></summary>
        public static void SetZoom_Scaled(float scale) { if (Terraria.VersionChecks.Using_Modern_UIInterfaceScaling) Terraria.StaticReference.PlayerInput.Invoke("SetZoom_Scaled", scale); }


    }
    public static class InputHelper {
        private static Dynamic PlayerInput = null;
        private static bool PlayerInputChecked = false;
        private static MouseState _oldState;
        private static MouseState _newState;
        private static KeyboardState _oldKeyboardState;
        private static KeyboardState _newKeyboardState;
        /// <summary> Set in post-draw. </summary>
        internal static void SetState() {
            _oldState = _newState;
            _newState = Mouse.GetState();
            _oldKeyboardState = _newKeyboardState;
            _newKeyboardState = Keyboard.GetState();
        }
        /// <summary> Gets the current mouse state from Terraria.GameInput.PlayerInput.MouseInfo or from <see cref="InputHelper"/> (from <see cref="Mouse.GetState()"/> which was cached before the game frame updated) if it doesn't exist. </summary>
        public static MouseState MouseInfo {
            get {
                if (!PlayerInputChecked && PlayerInput == null) {
                    var t = Terraria.GetType("Terraria.GameInput.PlayerInput");
                    if (t != null) {
                        PlayerInput = new Dynamic(t);
                    }
                    PlayerInputChecked = true;  
                }
                if (PlayerInput != null) return PlayerInput.GetValue<MouseState>("MouseInfo");
                return _newState;
            }
        }
        /// <summary> Gets the old/previous frame's mouse state from Terraria.GameInput.PlayerInput.MouseInfo or from <see cref="InputHelper"/> (from last frame's <see cref="Mouse.GetState()"/> which was cached before the game frame updated) if it doesn't exist. </summary>
        public static MouseState MouseInfoOld {
            get {
                if (!PlayerInputChecked && PlayerInput == null) {
                    var t = Terraria.GetType("Terraria.GameInput.PlayerInput");
                    if (t != null) {
                        PlayerInput = new Dynamic(t);
                    }
                    PlayerInputChecked = true;
                }
                if (PlayerInput != null) return PlayerInput.GetValue<MouseState>("MouseInfoOld");
                return _oldState;
            }
        }

        /// <summary> Gets the current keyboard state. This is not related to Terraria.GameInput.PlayerInput. </summary>
        public static KeyboardState KeyboardInfo {
            get {
                return _newKeyboardState;
            }
        }
        /// <summary> Gets the old/previous frame's keyboard state. This is not related to Terraria.GameInput.PlayerInput. </summary>
        public static KeyboardState KeyboardInfoOld {
            get {
                return _oldKeyboardState;
            }
        }

        /// <summary> Did the left mouse button just stop being held down (un-pressed). </summary>
        public static bool WasLeftMousePressUp => (MouseInfo.LeftButton == ButtonState.Released) && (MouseInfoOld.LeftButton == ButtonState.Pressed);
        /// <summary> Did the left mouse button just start being held down (on-pressed). </summary>
        public static bool WasLeftMousePressDown => (MouseInfo.LeftButton == ButtonState.Pressed) && (MouseInfoOld.LeftButton == ButtonState.Released);
        /// <summary> Is the left mouse button being held down (pressed). </summary>
        public static bool IsLeftMouseDown => (MouseInfo.LeftButton == ButtonState.Pressed);
        /// <summary> Was the left mouse button being held down (pressed). </summary>
        public static bool WasLeftMouseDown => (MouseInfoOld.LeftButton == ButtonState.Pressed);
        /// <summary> Is the left mouse button not being held down (pressed). </summary>
        public static bool IsLeftMouseUp => (MouseInfo.LeftButton == ButtonState.Released);
        /// <summary> Was the left mouse button not being held down (pressed). </summary>
        public static bool WasLeftMouseUp => (MouseInfoOld.LeftButton == ButtonState.Released);

        /// <summary> Did the right mouse button just stop being held down (un-pressed). </summary>
        public static bool WasRightMousePressUp => (MouseInfo.RightButton == ButtonState.Released) && (MouseInfoOld.RightButton == ButtonState.Pressed);
        /// <summary> Did the right mouse button just start being held down (on-pressed). </summary>
        public static bool WasRightMousePressDown => (MouseInfo.RightButton == ButtonState.Pressed) && (MouseInfoOld.RightButton == ButtonState.Released);
        /// <summary> Is the right mouse button being held down (pressed). </summary>
        public static bool IsRightMouseDown => (MouseInfo.RightButton == ButtonState.Pressed);
        /// <summary> Was the right mouse button being held down (pressed). </summary>
        public static bool WasRightMouseDown => (MouseInfoOld.RightButton == ButtonState.Pressed);
        /// <summary> Is the right mouse button not being held down (pressed). </summary>
        public static bool IsRightMouseUp => (MouseInfo.RightButton == ButtonState.Released);
        /// <summary> Was the right mouse button not being held down (pressed). </summary>
        public static bool WasRightMouseUp => (MouseInfoOld.RightButton == ButtonState.Released);


        /// <summary> The current frames mouse position. </summary>
        public static Vector2 MousePos => new Vector2(MouseInfo.X, MouseInfo.Y);

        public static bool WasKeyPressed(Keys key) => (KeyboardInfo.IsKeyUp(key) && KeyboardInfoOld.IsKeyDown(key));
        public static bool WasKeyReleaced(Keys key) => (KeyboardInfo.IsKeyDown(key) && KeyboardInfoOld.IsKeyUp(key));
        public static bool IsKeyDown(Keys key) => KeyboardInfo.IsKeyDown(key);

    }
}
