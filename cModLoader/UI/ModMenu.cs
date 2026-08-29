using cModLoader.ModComponents;
using cModLoader.Patching;
using cModLoader.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mono.CompilerServices.SymbolWriter;
using System;


namespace cModLoader.UI
{
    public abstract class UIMenu {
        private string title = "Default Text";
        /// <summary> Gets or sets the title of the menu. </summary>
        public string Title {
            get => title;
            set => MenuTitle.Text = (title = value);
        }

        public cUIState uiState = null;
        public cUITextPanel<string> MenuTitle = null;
        public cUITextPanel<string> BackMenuButton = null;
        public cUIElement mainContainer = null;

        public UIMenu(string title) {
            MenuTitle = new cUITextPanel<string>("Default Text", 1f, true);
            BackMenuButton = new cUITextPanel<string>("Back", 0.7f, true, true);
            if (Terraria.VersionChecks.Using_LegacyUISystem) {
                (MenuTitle.LegacyNative as LegacyUIText).FitToText = true;
            }

            uiState = new cUIState();

            mainContainer = new cUIElement();
            mainContainer.Width = new Positioning(0f, 0.6f);
            mainContainer.MaxWidth = new Positioning(600f, 0f);
            mainContainer.MinWidth = new Positioning(400f, 0f);
            mainContainer.Top = new Positioning(200f, 0f);
            mainContainer.Height = new Positioning(-200f - 50f, 1f);
            mainContainer.Alignment = new Vector2(0.5f, 0f);

            MenuTitle.Alignment = new Vector2(0.5f, 0f);
            MenuTitle.Top = new Positioning(-40f, 0f);
            mainContainer.Append(MenuTitle);

            BackMenuButton.Width = new Positioning(-10f, 0.5f);
            BackMenuButton.Height = new Positioning(50f, 0f);
            BackMenuButton.Alignment = new Vector2(0.5f, 1f);
            BackMenuButton.OnMouseOver += (e, o) => {
                Terraria.Audio.PlaySound(12, -1, -1, 1);
            };
            BackMenuButton.OnClick += (e, o) => {
                Terraria.Audio.PlaySound(11, -1, -1, 1);
                BackPress();
                if (BackMenuButton.IsLegacy) (BackMenuButton.LegacyNative as LegacyUIText).buttonScale = 0f;
            };
            mainContainer.Append(BackMenuButton);

            uiState.Append(mainContainer);

            Title = title;
        }

        public virtual void OnOpenMenu() { }
        public virtual void OnCloseMenu() { }
        public abstract void BackPress();

    }

    public class ModListMenu : UIMenu {

        public cUIPanel modListContainer;

        public ModListMenu(string title) : base(title) {
            modListContainer = new cUIPanel();
            modListContainer.Width = new Positioning(0, 1f);
            modListContainer.Height = new Positioning(-60f, 1f);
            mainContainer.Append(modListContainer);

            var modList = new cUIList();
            modList.Width = new Positioning(0f, 1f);
            modList.Height = new Positioning(0f, 1f);
            modList.ListPadding = 5f;
            modList.OverflowHidden = true;
            modListContainer.Append(modList);

            if (!Terraria.VersionChecks.Using_LegacyUISystem) {
                modListContainer.BackgroundColor = new Color(33, 43, 79) * 0.8f;

                var modListScrollBar = new cUIScrollbar();
                modListScrollBar.SetView(100f, 1000f);
                modListScrollBar.Height = new Positioning(0f, 1f);
                modListScrollBar.Alignment = new Vector2(1f, 0f);
                modList.SetScrollbar(modListScrollBar);

                // make top
                mainContainer.RemoveChild(MenuTitle);
                mainContainer.Append(MenuTitle);
            }
            for (int i = 0; i < ModContent.modList.Count; i++) {
                modList.Add(ModContent.modList[i].GetModListPanel());
            }
            //if (Terraria.VersionChecks.Using_LegacyUISystem) {
            //    for (int i = 0; i < 10; i++) {
            //        var spacer = new cUIText("________", 1f, true) {
            //            Width = new Positioning(0f, 1f),
            //            Height = new Positioning(0f, 0f)
            //        };
            //        (spacer.LegacyNative as LegacyUIText).TextPixelOffset = new Vector2(0, -20);
            //        modList.Add(spacer);
            //        modList.Add(new ModListItem("Mod Test " + i, "Mod Version " + i, "Mod Author " + i));
            //    }
            //} else {
            //    for (int i = 0; i < 10; i++) {
            //        modList.Add(new ModListItem("Mod Test " + i, "Mod Version " + i, "Mod Author " + i));
            //    }
            //}

        }
        
        public override void BackPress() {
            ModMenu.CloseGameMenu();
        }
    }
    public class ModConfigMenu : UIMenu {

        public cUIPanel configListContainer;
        public cUITextPanel<string> BackAndSave = null;

        public ModConfigMenu(Mod mod) : base(mod.ModName + " Config") {
            BackMenuButton.Width = new Positioning(-10f, 0.5f);
            BackMenuButton.Height = new Positioning(50f, 0f);
            BackMenuButton.Alignment = new Vector2(0f, 1f);

            BackAndSave = new cUITextPanel<string>("Back and Save", 0.7f, true, true);
            BackAndSave.Width = new Positioning(-10f, 0.5f);
            BackAndSave.Height = new Positioning(50f, 0f);
            BackAndSave.Alignment = new Vector2(1f, 1f);
            BackAndSave.OnMouseOver += (e, o) => {
                Terraria.Audio.PlaySound(12, -1, -1, 1);
            };
            BackAndSave.OnClick += (e, o) => {
                Terraria.Audio.PlaySound(11, -1, -1, 1);
                BackPress();
                mod.ModConfig.SaveConfig(mod.GetConfigFile());
                if (BackAndSave.IsLegacy) (BackAndSave.LegacyNative as LegacyUIText).buttonScale = 0f;
            };
            mainContainer.Append(BackAndSave);

            configListContainer = new cUIPanel();
            configListContainer.Width = new Positioning(0, 1f);
            configListContainer.Height = new Positioning(-60f, 1f);
            mainContainer.Append(configListContainer);

            var configList = new cUIList();
            configList.Width = new Positioning(0f, 1f);
            configList.Height = new Positioning(0f, 1f);
            configList.ListPadding = 5f;
            configList.OverflowHidden = true;
            configListContainer.Append(configList);

            if (!Terraria.VersionChecks.Using_LegacyUISystem) {
                configListContainer.BackgroundColor = new Color(33, 43, 79) * 0.8f;

                var modListScrollBar = new cUIScrollbar();
                modListScrollBar.SetView(100f, 1000f);
                modListScrollBar.Height = new Positioning(0f, 1f);
                modListScrollBar.Alignment = new Vector2(1f, 0f);
                configList.SetScrollbar(modListScrollBar);

                // make top
                mainContainer.RemoveChild(MenuTitle);
                mainContainer.Append(MenuTitle);
            }
            else {

            }

            var elements = mod.ModConfig.Items;
            foreach (var configElm in elements) {
                configList.Add(configElm.GetUIElement());
            }

        }

        public override void BackPress() {
            ModMenu.OpenGameMenu(null);
        }
    }
    public class ModInfoMenu : UIMenu {

        public cUIPanel configListContainer;

        public ModInfoMenu(Mod mod) : base(mod.ModName + " Info") {
            configListContainer = new cUIPanel();
            configListContainer.Width = new Positioning(0, 1f);
            configListContainer.Height = new Positioning(-60f, 1f);
            mainContainer.Append(configListContainer);

            var elementList = new cUIList();
            elementList.Width = new Positioning(0f, 1f);
            elementList.Height = new Positioning(0f, 1f);
            elementList.ListPadding = 5f;
            elementList.OverflowHidden = true;
            configListContainer.Append(elementList);

            if (!Terraria.VersionChecks.Using_LegacyUISystem) {
                elementList.SetPadding(8f);
                configListContainer.BackgroundColor = new Color(33, 43, 79) * 0.8f;

                var modListScrollBar = new cUIScrollbar();
                modListScrollBar.SetView(100f, 1000f);
                modListScrollBar.Height = new Positioning(0f, 1f);
                modListScrollBar.Alignment = new Vector2(1f, 0f);
                elementList.SetScrollbar(modListScrollBar);

                // make top
                mainContainer.RemoveChild(MenuTitle);
                mainContainer.Append(MenuTitle);
            }
            else {

            }

            var text = new cUIText(mod.ModDescription);
            if (text.IsLegacy) {
                (text.LegacyNative as LegacyUIText).FitToText = true;
            }
            elementList.Add(text);

        }

        public override void BackPress() {
            ModMenu.OpenGameMenu(null);
        }
    }

    public class ModMenu {
        public static cUITextPanel<string> ModMenuBtn = null;
        public static ModListMenu modListMenu = new ModListMenu("Mod List");
        public static cUserInterface subInterface = null;
        public static cUIState uiState = null;

        /// <summary> Is there a modded menu open. This is set by <see cref="OpenGameMenu(UIMenu)"/> and <see cref="CloseGameMenu"/></summary>
        public static bool ModMenuOpen = false;

        /// <summary> Opens a mod menu. Set <paramref name="menu"/> to <see langword="null"/> to open the mod list. </summary>
        public static void OpenGameMenu(UIMenu menu) {
            menu = menu ?? modListMenu;
            if (ModHelper.IsInWorld) {
                if (!Terraria.VersionChecks.Is0_1) {
                    Terraria.StaticReference.Main.SetValue("menuMode", 888);
                }
                if (Terraria.VersionChecks.Using_LegacyUISystem) {
                    subInterface.SetState(menu);
                } else {
                    var MenuUI = new Dynamic(Terraria.StaticReference.Main.GetValue<object>("MenuUI"));
                    MenuUI.Invoke("SetState", menu.uiState.ModernNative.Value);
                }
            } else {
                if (Terraria.VersionChecks.Using_Modern_InGameInterface) {
                    // do nothing, all we need to set is ModMenuOpen
                }
                else {
                    if (Terraria.VersionChecks.Using_InGamePauseMenu) {
                        new Dynamic(Terraria.GetType("Terraria.IngameOptions")).Invoke("Close");
                    }
                }
                subInterface.SetState(menu);
            }
            ModMenuOpen = true;
        }
        /// <summary> Completely quits out of the mod menu. </summary>
        public static void CloseGameMenu(bool setMenuMode = true) {
            if (ModHelper.IsInWorld) {
                if (!Terraria.VersionChecks.Is0_1) {
                    if (setMenuMode) Terraria.StaticReference.Main.SetValue("menuMode", 0);
                }
                subInterface.SetState(uiState);
            } else {
                if (Terraria.VersionChecks.Using_Modern_InGameInterface) {
                    // do nothing, all we need to set is ModMenuOpen
                }
                else {
                    if (Terraria.VersionChecks.Using_InGamePauseMenu) {
                        new Dynamic(Terraria.GetType("Terraria.IngameOptions")).Invoke("Open");
                    }
                }
                subInterface.SetState(uiState);
            }
            ModMenuOpen = false;
        }

        /// <summary> Hook for "DrawInterface_11_IngameOptionsMenu" UI interface, this is hooked in <see cref="cModLoader"/>.<br/>Used in versions after and including 1.3.3.</summary>
        public static bool DrawInterface_11_IngameOptionsMenu_Hook(GameReference game) {
            bool optionsWindow = game.main.GetValue<bool>("ingameOptionsWindow");
            if (optionsWindow) {
                if (ModMenuOpen) {
                    try {
                        // newer versions draw this while others don't, i guess we will always
                        game.main.Invoke("DrawInterface_16_MapOrMinimap");
                        ModHelper.SetZoom_UI();
                        try { // older versions don't do this
                            var uIScaleMatrix = game.main.GetValue<Matrix>("UIScaleMatrix");
                            game.spriteBatch.End();
                            game.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, uIScaleMatrix);
                        } catch (Exception) { } // we should really check if these fail incase they do and they shouldn't be
                        subInterface.Draw(game);
                        // some stuff are drawn at the end of IngameOptions.Draw()
                        game.main.Invoke("DrawGamepadInstructions");
                        game.main.SetValue("mouseText", false);
                        game.main.Invoke("GUIBarsDraw");
                        game.main.Invoke("DrawMouseOver");
                        try { // older versions don't do this
                            var UIScaleMatrix = game.main.GetValue<Matrix>("UIScaleMatrix");
                            var SamplerStateForCursor = game.main.GetValue<SamplerState>("SamplerStateForCursor");
                            game.spriteBatch.End();
                            game.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, UIScaleMatrix);
                        } catch (Exception) { } // we should really check if these fail incase they do and they shouldn't be
                        game.main.Invoke("DrawCursor", game.main.Invoke<Vector2>("DrawThickCursor", false), false); // Main.DrawCursor(Main.DrawThickCursor(smart = false), smart = false);

                        try { // not it some versions
                            game.main.SetValue("_MouseOversCanClear", true);
                        } catch (Exception) { } // we should really check if these fail incase they do and they shouldn't be
                        try { // not it some versions
                            game.main.Invoke("DrawInterface_40_InteractItemIcon");
                        } catch (Exception) { } // we should really check if these fail incase they do and they shouldn't be
                    }
                    catch (Exception e) {
                        Output.Error("DrawInterface_11_IngameOptionsMenu_Hook Error\n" + e.ToString());
                    }
                    return false; // false because we know the menu is open
                }
                else {
                    ModHelper.SetZoom_UI();
                    try { // older versions don't do this
                        var uIScaleMatrix = game.main.GetValue<Matrix>("UIScaleMatrix");
                        game.spriteBatch.End();
                        game.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, uIScaleMatrix);
                    } catch (Exception) { } // we should really check if these fail incase they do and they shouldn't be
                    subInterface.Draw(game);
                }
            } else {
                // close if open
                if (ModMenuOpen) CloseGameMenu();
            }
            // call original of menu is not open
            return game.main.Invoke<bool>("DrawInterface_11_IngameOptionsMenu");
        }

        private static bool previous_ingameOptionsWindow = false;
        private static bool previous_inGame = false;
        public static void DrawModMenu(GameReference game) {
            if (subInterface == null) {
                uiState = new cUIState();
                subInterface = new cUserInterface();
                subInterface.SetState(uiState);
            }
            if (ModMenuBtn == null) {
                ModMenuBtn = new UI.cUITextPanel<string>("Open cModLoader Mod Menu", 1f, false, true) {
                    Margin = new Vector4(10, 0, 10, 0)
                };
                ModMenuBtn.OnClick += (e, o) => {
                    OpenGameMenu(null);
                    Terraria.Audio.PlaySound(10, -1, -1, 1);
                };
                if (Terraria.VersionChecks.Using_LegacyUISystem) {
                    (ModMenuBtn.LegacyNative as LegacyUIText).FitToText = true;
                    var obj = ModMenuBtn.LegacyNative as LegacyUIText;
                    obj.EndColor = Color.White * 0.5f;
                    obj.EndSize = obj.StartSize;
                    ModMenuBtn.OnClick += (e, o) => {
                        obj.buttonScale = 0f; // resets button colour
                    };
                }

                uiState.Append(ModMenuBtn);
            }
            bool inGame = ModHelper.IsInWorld;
            int menuMode = 0;
            if (!Terraria.VersionChecks.Is0_1) {
                menuMode = game.main.GetValue<int>("menuMode");
            }

            if (inGame) { // in game
                if (Terraria.VersionChecks.Using_Modern_InGameInterface) {
                    // do nothing, this is draws by Terraria
                } else {
                    if (Terraria.VersionChecks.Using_InGamePauseMenu) {
                        bool optionsWindow = game.main.GetValue<bool>("ingameOptionsWindow");
                        // close if changed
                        if (ModMenuOpen && optionsWindow != previous_ingameOptionsWindow) CloseGameMenu();
                        if (optionsWindow || ModMenuOpen) {
                            subInterface.Draw(game);
                        }
                    }
                    else {
                        ModMenuBtn.Text = "Mod Menu";
                        ModMenuBtn.Alignment = new Vector2(1f, 0);
                        ModMenuBtn.Margin = new Vector4(0, 10, 10, 0);
                        // always draw
                        subInterface.Draw(game);
                    }
                }
            } else { // not in game / in main menus
                // close if changed
                if (ModMenuOpen && previous_inGame != !inGame) CloseGameMenu(false);
                ModMenuBtn.Text = "Open cModLoader Mod Menu";
                ModMenuBtn.Alignment = new Vector2(0f, 0);
                ModMenuBtn.Margin = new Vector4(10, 0, 10, 0);
                if (menuMode == 888) {
                    // only draw on legacy because Terraria will draw this through MenuUI
                    if (Terraria.VersionChecks.Using_LegacyUISystem) {
                        subInterface.Draw(game);
                    }
                } else if (menuMode == 0) {
                    subInterface.Draw(game);
                }
            }
            // draw cursor over original so the menu isn't on top (only works in versions below 1.3)
            if (Terraria.GameVersionType <= Terraria.VersionType.Old) {
                DrawRawCursor(game);
            }

            previous_inGame = inGame;
            if (Terraria.VersionChecks.Using_InGamePauseMenu) {
                previous_ingameOptionsWindow = game.main.GetValue<bool>("ingameOptionsWindow");
            }

        }

        private static bool checkedExists = false;
        private static bool cursorColorExists = false;
        private static bool fadeTextureExists = false;
        /// <summary> Only works in the legacy UI system. See <see cref="Terraria.VersionChecks.Using_LegacyUISystem"/> </summary>
        public static void DrawRawCursor(GameReference game) {
            if (!checkedExists) {
                try {
                    game.main.GetValue<Color>("cursorColor");
                    cursorColorExists = true;
                }
                catch (Exception) {
                    cursorColorExists = false;
                }
                try {
                    game.main.GetValue<Texture2D>("fadeTexture");
                    fadeTextureExists = true;
                }
                catch (Exception) {
                    fadeTextureExists = false;
                }
                checkedExists = true;
            }
            var cursorTexture = game.main.GetValue<Texture2D>("cursorTexture");
            var mouseState = Mouse.GetState();
            if (!cursorColorExists) { // draw for 0.1
                var mouseTextColor = game.main.GetValue<byte>("mouseTextColor");
                game.spriteBatch.Draw(cursorTexture, new Vector2(mouseState.X, mouseState.Y), new Rectangle(0, 0, cursorTexture.Width, cursorTexture.Height), new Color(mouseTextColor, mouseTextColor, mouseTextColor, mouseTextColor), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                return;
            }
            var cursorColor = game.main.GetValue<Color>("cursorColor");
            var cursorScale = game.main.GetValue<float>("cursorScale");

            game.spriteBatch.Draw(cursorTexture, new Vector2(mouseState.X + 1, mouseState.Y + 1), new Rectangle(0, 0, cursorTexture.Width, cursorTexture.Height), new Color((int)((float)(int)cursorColor.R * 0.2f), (int)((float)(int)cursorColor.G * 0.2f), (int)((float)(int)cursorColor.B * 0.2f), (int)((float)(int)cursorColor.A * 0.5f)), 0f, default(Vector2), cursorScale * 1.1f, SpriteEffects.None, 0f);
            game.spriteBatch.Draw(cursorTexture, new Vector2(mouseState.X, mouseState.Y), new Rectangle(0, 0, cursorTexture.Width, cursorTexture.Height), cursorColor, 0f, default(Vector2), cursorScale, SpriteEffects.None, 0f);

            if (fadeTextureExists) {
                var fadeTexture = game.main.GetValue<Texture2D>("fadeTexture");
                var fadeCounter = game.main.GetValue<int>("fadeCounter");
                var screenWidth = game.main.GetValue<int>("screenWidth");
                var screenHeight = game.main.GetValue<int>("screenHeight");
                if (fadeCounter > 0) {
                    Color white = Color.White;
                    byte b2 = 0;
                    //fadeCounter--; // don't change this because the original code will
                    float num97 = (float)fadeCounter / 75f * 255f;
                    b2 = (byte)num97;
                    white = new Color(b2, b2, b2, b2);
                    game.spriteBatch.Draw(fadeTexture, new Rectangle(0, 0, screenWidth, screenHeight), white);
                }
            }

        }

    }
}
