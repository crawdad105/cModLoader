using cModLoader.ModComponents;
using cModLoader.Patching;
using cModLoader.UI;
using cModLoader.Utils;
using cModLoader.Window;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace cModLoader.UI
{
    /// <summary> A custom class used for mod list items </summary>
    public class ModListItem : cUIElement {
        public static ReLogicAsset<Texture2D> _dividerTexture;
        public static ReLogicAsset<Texture2D> _innerPanelTexture;
        public static ReLogicAsset<Texture2D> _missingModIcon;
        public static ReLogicAsset<Texture2D> _infoIcon;
        public static ReLogicAsset<Texture2D> _openFileIcon;
        public static ReLogicAsset<Texture2D> _configIcon;
        public static bool _imagesLoaded = false;
        public Texture2D _modIcon;

        public Mod modRefrence;
        public ModListItem(Mod mod) : this(mod.ModName, mod.ModVersion, mod.ModAuthor) {
            modRefrence = mod;
            _modIcon = mod.GetModIcon();
        }

        internal ModListItem(string name, string version, string author) : base(null) {

            if (IsLegacy) {
                native = new LegacyUIContainer();
                Width = new Positioning(0f, 1f);
                Height = new Positioning(96f, 0f);
                var size = 0.5f;
                var modName = new LegacyUIText(name, true, size, size, Color.White, Color.White, true) { FitToText = true, Left = new Positioning(5, 0f) };
                var modAuth = new LegacyUIText("By: " + author, true, size, size, Color.White, Color.White, true) { FitToText = true, VAlign = 0.5f, Left = new Positioning(5, 0f) };
                var modVer = new LegacyUIText("Version: " + version, true, size, size, Color.White, Color.White, true) { FitToText = true, VAlign = 1f, Left = new Positioning(5, 0f) };

                var modFolderBtn = new LegacyUIText("Mod Folder", true, size, size, Color.White, Color.White * 0.5f, true) { FitToText = true, HAlign = 1f, Left = new Positioning(-5, 0f) };
                modFolderBtn.OnClick += _OnClickOpenMod;
                var ModInfoBtn = new LegacyUIText("Info", true, size, size, Color.White, Color.White * 0.5f, true) { FitToText = true, HAlign = 1f, VAlign = 0.5f, Left = new Positioning(-5, 0f) };
                ModInfoBtn.OnClick += _OnClickModInfo;
                var configBtn = new LegacyUIText("Config", true, size, size, Color.White, Color.White * 0.5f, true) { FitToText = true, HAlign = 1f, VAlign = 1f, Left = new Positioning(-5, 0f) };
                configBtn.OnClick += _OnClickModConfig;

                // do not reorder the first 3
                LegacyNative.Append(modName);
                LegacyNative.Append(modAuth);
                LegacyNative.Append(modVer);
                LegacyNative.Append(modFolderBtn);
                LegacyNative.Append(ModInfoBtn);
                LegacyNative.Append(configBtn);

                LegacyNative.DrawSelfExtension += _DrawSelfLegacy;

                return;
            }

            if (!_imagesLoaded) {
                string assetName = "Images/UI/Divider";
                string assetName2 = "Images/UI/InnerPanelBackground";
                string assetName3 = "Images/UI/PlayerBackground";
                string assetName4 = "Images/UI/CharCreation/CharInfo";
                string assetName5 = "Images/UI/Camera_6";
                string assetName6 = "Images/UI/Creative/Research_GearB";
                // some images don't exist, we could store then in the cModLoader exe but that's cheap
                if (!Terraria.VersionChecks.Using_Modern_RichUIDirectory) {
                    assetName4 = "Images/Chat";
                    assetName6 = "Images/UI/Camera_1";
                }
                if (Terraria.VersionChecks.Using_RelogicAssets) {
                    _dividerTexture = Terraria.Relogic.Asset<Texture2D>(assetName);
                    _innerPanelTexture = Terraria.Relogic.Asset<Texture2D>(assetName2);
                    _missingModIcon = Terraria.Relogic.Asset<Texture2D>(assetName3);
                    _infoIcon = Terraria.Relogic.Asset<Texture2D>(assetName4);
                    _openFileIcon = Terraria.Relogic.Asset<Texture2D>(assetName5);
                    _configIcon = Terraria.Relogic.Asset<Texture2D>(assetName6);
                }
                else {
                    _dividerTexture = new ReLogicAsset<Texture2D>(Terraria.Textures.LoadTexture(assetName));
                    _innerPanelTexture = new ReLogicAsset<Texture2D>(Terraria.Textures.LoadTexture(assetName2));
                    _missingModIcon = new ReLogicAsset<Texture2D>(Terraria.Textures.LoadTexture(assetName3));
                    _infoIcon = new ReLogicAsset<Texture2D>(Terraria.Textures.LoadTexture(assetName4));
                    _openFileIcon = new ReLogicAsset<Texture2D>(Terraria.Textures.LoadTexture(assetName5));
                    _configIcon = new ReLogicAsset<Texture2D>(Terraria.Textures.LoadTexture(assetName6));
                }
                _imagesLoaded = true;
            }

            native = UIUtils.ModernReferences.UIPanel_Reference.New();

            // used for drawing stuff
            var _baseOverlay = new cUIElementOverride();
            _baseOverlay.Width = new Positioning(0f, 1f);
            _baseOverlay.Height = new Positioning(0f, 1f);
            _baseOverlay.DrawSelf += _DrawSelf;
            Append(_baseOverlay);

            int num = 96;
            Height = new Positioning(num, 0f);
            Width = new Positioning(-1f, 1f);
            SetPadding(6f);
            var _playerPanel = new cUIElement();
            _playerPanel.Width = new Positioning(59f, 0f);
            _playerPanel.Height = new Positioning(58f, 0f);
            _playerPanel.Left = new Positioning(4f, 0f);
            _baseOverlay.Append(_playerPanel);
            var _OpenFileButton = new cUIImageButton(_openFileIcon);
            _OpenFileButton.Alignment = new Vector2(1f, 1f);
            _OpenFileButton.Left = new Positioning(2f, 0f);
            _OpenFileButton.Top = new Positioning(4f, 0f);
            _OpenFileButton.OnClick += _OnClickOpenMod;
            _baseOverlay.Append(_OpenFileButton);
            var _OpenPageInfoButton = new cUIImageButton(_infoIcon);
            _OpenPageInfoButton.Alignment = new Vector2(1f, 1f);
            _OpenPageInfoButton.Left = new Positioning(-30f, 0f);
            _OpenPageInfoButton.Top = new Positioning(4f, 0f);
            _OpenPageInfoButton.OnClick += _OnClickModInfo;
            _baseOverlay.Append(_OpenPageInfoButton);
            var _OpenConfigButton = new cUIImageButton(_configIcon);
            _OpenConfigButton.Alignment = new Vector2(1f, 1f);
            _OpenConfigButton.Left = new Positioning(-60f, 0f);
            _OpenConfigButton.Top = new Positioning(4f, 0f);
            _OpenConfigButton.OnClick += _OnClickModConfig;
            //if (modRefrence.configMenu.ShouldShowMenu()) {
            _baseOverlay.Append(_OpenConfigButton);
            //}
            var _TitleElement = new cUIText(name);
            _TitleElement.Left = new Positioning(num + 2, 0f);
            _TitleElement.Top = new Positioning(4f, 0f);
            _baseOverlay.Append(_TitleElement);
            var _AuthorElement = new cUIText(author, 0.8f);
            _AuthorElement.Left = new Positioning(num + 4, 0f);
            _AuthorElement.Top = new Positioning(36f, 0f);
            _baseOverlay.Append(_AuthorElement);
            var _VersionElement = new cUIText(version, 0.8f);
            _VersionElement.Alignment = new Vector2(1f, 0f);
            _VersionElement.Left = new Positioning(-10f, 0f);
            _VersionElement.Top = new Positioning(36f, 0f);
            _baseOverlay.Append(_VersionElement);

        }

        private void _DrawSelfLegacy(SpriteBatch sb) {
            if (_modIcon != null) {
                var children = LegacyNative.GetChildren();
                children[0].Left = new Positioning(96f + 5f, 0f);
                children[1].Left = new Positioning(96f + 5f, 0f);
                children[2].Left = new Positioning(96f + 5f, 0f);

                var dimensions = GetDimensions();
                Color color = new Color(255, 255, 255);
                Rectangle? sourceRectangle = null;
                int num = 80;
                float num2 = (dimensions.Height - (float)num) / 2f;
                sb.Draw(_modIcon, new Rectangle((int)dimensions.X + (int)num2, (int)dimensions.Y + (int)num2, num, num), sourceRectangle, color, 0f, new Vector2(0f, 0f), SpriteEffects.None, 0f);
            }
        }

        private void _DrawSelf(SpriteBatch sb) {
            var dimensions = GetDimensions();
            Color color = new Color(255, 255, 255);
            Rectangle? sourceRectangle = null;
            int num = 80;
            float num2 = (dimensions.Height - (float)num) / 2f;
            if (_modIcon == null) {
                sb.Draw(_missingModIcon.Value, new Rectangle((int)dimensions.X + (int)num2, (int)dimensions.Y + (int)num2, num, num), sourceRectangle, color, 0f, new Vector2(0f, 0f), SpriteEffects.None, 0f);
            }
            else {
                sb.Draw(_modIcon, new Rectangle((int)dimensions.X + (int)num2, (int)dimensions.Y + (int)num2, num, num), sourceRectangle, color, 0f, new Vector2(0f, 0f), SpriteEffects.None, 0f);
            }
            sb.Draw(_dividerTexture.Value, new Vector2(dimensions.X + dimensions.Height, dimensions.Y + 30f), sourceRectangle, color, 0f, Vector2.Zero, new Vector2((dimensions.Width - dimensions.Height) / 8f, 1f), SpriteEffects.None, 0f);
            float num3 = (dimensions.Width - dimensions.Height) / 2f - 10f;
            Vector2 vector = new Vector2(dimensions.X + dimensions.Height + 4f, dimensions.Y + 36f);
            DrawPanel(sb, vector, num3);
            Vector2 position2 = vector + new Vector2(num3 + 10f, 0f);
            DrawPanel(sb, position2, num3);
        }
        private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width) {
            Color color = new Color(255, 255, 255);
            int height = _innerPanelTexture.Value.Height;
            spriteBatch.Draw(_innerPanelTexture.Value, position, new Rectangle(0, 0, 8, height), color);
            spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(position.X + 8f, position.Y), new Rectangle(8, 0, 8, height), color, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
            spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(position.X + width - 8f, position.Y), new Rectangle(16, 0, 8, height), color);
        }

        private void _OnClickOpenMod(object evt, object listeningElement) {
            Process.Start("explorer.exe", modRefrence.GetDirectoryPath());
        }

        private void _OnClickModInfo(object evt, object listeningElement) {
            Terraria.Audio.PlaySound(10, -1, -1, 1, 1f, 0f);
            ModMenu.OpenGameMenu(modRefrence.GetInfoMenu());
        }

        private void _OnClickModConfig(object evt, object listeningElement) {
            Terraria.Audio.PlaySound(10, -1, -1, 1, 1f, 0f);
            ModMenu.OpenGameMenu(modRefrence.GetConfigMenu());
        }

    }
}
