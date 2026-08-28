using cModLoader;
using cModLoader.Patching;
using cModLoader.UI;
using cModLoader.Utils;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cModLoader.ModComponents
{
    /// <summary> A mod class for creating mods for cModLoader. Use <see cref="PreMod"/> to initialize your mod </summary>
    public abstract class Mod
    {
        internal PreMod preMod = null;
        internal string modFullPath = "";
        internal string modFileDirectory = "";
        internal string modFileName = "";

        /// <summary> Gets a safe file name using the mod's name for the config.</summary>
        internal string GetConfigName() {
            var c1 = System.IO.Path.GetInvalidFileNameChars();
            var c2 = System.IO.Path.GetInvalidPathChars();
            var c3 = new char[] { ' ' };
            var str = string.Concat(ModName.ToCharArray().Select(c => (c1.Contains(c) || c2.Contains(c) || c3.Contains(c)) ? '_' : c));
            //TODO: check if the string is valid, could be a null or empty string or something else
            return str;
        }
        /// <summary> Gets the config file path. </summary>
        public string GetConfigFile() {
            return Path.GetCurrentConfigsFolder() + Path.DirectorySeparator + GetConfigName() + ".cmlcfg"; // hopefully this file ending isn't used for anything else
        }

        /// <summary> Gets the full mod path. <para>eg. <c>c:/path/to/mod/yourMod.dll</c> </para></summary>
        public string GetFullFilePath() => modFullPath;
        /// <summary> Gets the directory of the mod. Includes the last path seperator. This should be the default cModLoader mods folder. <para>eg. <c>c:/path/to/mod</c> </para></summary>
        public string GetDirectoryPath() => modFileDirectory;
        /// <summary> Gets the file name of the mod. This includes file ending. <para>eg. <c>yourMod.dll</c> </para></summary>
        public string GetFileName() => modFileName;
        /// <summary> Gets the file name of the mod. Does not include file ending. <para>eg. <c>yourMod</c> </para></summary>
        public string GetRawFileName() => modFileName.Substring(0, modFileName.LastIndexOf('.'));

        /// <summary> The name of your mod </summary>
        public string ModName = "";
        /// <summary> The description of your mod </summary>
        public string ModDescription = "";
        /// <summary> The mods author </summary>
        public string ModAuthor = "";
        /// <summary> The version of your mod (this can be any string instead of just "X.Y.Z") </summary>
        public string ModVersion = "";
        /// <summary> The web page associated with your mod. This could be the Github page, wiki or whatever you want. This only works for http and https protocols. </summary>
        public string ModUrl = "";

        /// <summary> The mods icon. Should be gotten using <see cref="GetModIcon()"/> as this should also load the icon. </summary>
        protected Texture2D ModIcon = null;

        private Config modConfig = null;
        /// <summary> The mod's config options. </summary>
        public Config ModConfig {
            get {
                if (modConfig == null) modConfig = new Config(this);
                return modConfig;
            }
        }

        /// <summary> Gets this mod's icon. Checks for a png in the same directory with the same name as the dll file by default.<br/>You can override this to load internal files or whatever. You are also responsible for caching into <see cref="ModIcon"/> or something.</summary>
        public virtual Texture2D GetModIcon() {
            if (ModIcon != null) return ModIcon;
            try {
                string path = GetDirectoryPath() + GetRawFileName() + ".png";
                if (File.Exists(path)) return (ModIcon = Terraria.Textures.LoadRawTexture(path));
                Output.Error($"Failed to load mod icon for mod \"{ModName}\". PNG file was not found.");
            }
            catch (Exception e) {
                Output.Error($"Failed to load mod icon for mod \"{ModName}\". An error occurred " + e.GetType());
            }
            return null;
        }

        /// <summary> 
        /// Returns a config UI, created when needed so and instance isn't stored for every mod.<br/>
        /// See <see cref="Config"/> or <see cref="Mod.ModConfig"/> to properly modify config values.
        /// <para> Override this function to modify default UI. </para>
        /// </summary>
        public virtual ModConfigMenu GetConfigMenu() => new ModConfigMenu(this);
        /// <summary> 
        /// Returns an information UI, created when needed so and instance isn't stored for every mod.<br/>
        /// By default this displays the mod description (<see cref="ModDescription"/>).
        /// <para> Override this function to modify default UI. </para>
        /// </summary>
        public virtual ModInfoMenu GetInfoMenu() => new ModInfoMenu(this);
        /// <summary> 
        /// Returns the mod panel in the mod list.<br/>
        /// By default this displays some stuff, however you could return <see langword="null"/> so no panel will be shown (the mod still exist).
        /// <para> Override this function to modify default UI. </para>
        /// </summary>
        public virtual ModListItem GetModListPanel() => new ModListItem(this);

        /// <summary>
        /// On windows this is called on the second update tick of Terraria before anything happens.<br/>
        /// On Linux this is called on the first tick of Terraria before anything happens.<br/>
        /// This is also right after you call <see cref="PreMod.RegisterMod(Mod)"/>.
        /// </summary>
        public virtual void OnInitialize() { }
        /// <summary>
        /// <para>
        /// In modern UI versions this is called right before Terraria draws the mouse in game, not in menus (See <see cref="Terraria.VersionChecks.Using_LegacyUISystem"/>).<br/>
        /// Draw scaling is set to UI.
        /// </para>
        /// <para>
        /// In legacy UI versions this is called right after everything is draw, but the mouse is drawn again after so it should match the modern UI.
        /// </para>
        /// See <see cref="Terraria.VersionChecks.Using_LegacyUISystem"/> for modern and legacy UI versions.<br/>
        /// This will never run if Terraria.Main.hideUI is <see langword="true"/>.<br/>
        /// <see langword="⚠ Is not called in Terraria versions 1.3 to 1.3.2.1 (inclusive)."/>
        /// </summary>
        public virtual void DrawInterface(GameReference game) {
            
        }


        /// <summary> Called before Terraria does anything related to drawing.</summary>
        public virtual void OnPreDraw(GameReference game) { }
        /// <summary> Called after Terraria completely finishes anything related to drawing.</summary>
        public virtual void OnPostDraw(GameReference game) { }

        /// <summary> Called before Terraria updates. <para><see langword="⚠ Requires Patches"/></para></summary>
        public virtual void OnPreUpdate(GameReference game) { }
        /// <summary> Called after Terraria updates. <para><see langword="⚠ Requires Patches"/></para></summary>
        public virtual void OnPostUpdate(GameReference game) { }

    }
}
