using cModLoader.Patching;
using cModLoader.UI;
using cModLoader.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Assemblies;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cModLoader
{
    /// <summary> </summary>
    public class Terraria
    {
        /// <summary> Class for referencing specific game versions </summary>
        public class _Version {
            /// <summary> Number representation of the version. Eg. [ 1, 4, 4, 9 ] </summary>
            public int[] Numbers;
            /// <summary> String representation of the version. Eg. "1.4.4.9" </summary>
            public string Name;
            /// <summary> </summary>
            public _Version(params int[] n) {
                this.Numbers = n;
                this.Name = string.Join(".", n);
            }
        }

        /// <summary> Used to identify versions groups. </summary>
        public enum VersionType {
            /// <summary> 1.5 - ??? </summary>
            Future = 6,
            /// <summary> 1.4 - current </summary>
            Current = 5,
            /// <summary> 1.3.0.7 - 1.3.5.3 </summary>
            Modern = 4,
            /// <summary> 1.3 - 1.3.0.6 </summary>
            OldModern = 3,
            /// <summary> 1.2 - 1.2.4.1 </summary>
            Old = 2,
            /// <summary> 0.7 - 1.0.6.1 </summary>
            Legacy = 1,
            /// <summary> 0.1 </summary>
            VeryLegacy = 0,
            /// <summary> Unknown </summary>
            Unknown = -1
        }

        // these are for windows, and may be wrong
        // 1.0.0.0 : 0.1 - 1.0.3
        // 1.0.4.0 : 1.0.4 - 1.2.1
        // 1.2.1.1 : 1.2.1.2
        // 1.2.1.1 : 1.2.2
        // 1.3.0.1 : 1.3
        // 1.3.0.1 : 1.3.0.2
        // 1.3.0.7 : 1.3.0.8
        // 1.3.3.1 : 1.3.3
        // 1.3.3.2 : 1.3.3.1
        // 1.3.5.1 : 1.3.5.2
        // 1.4     : 1.4.0.2
        // 1.4.4.8.1 : 1.4.4.8

        /// <summary>
        /// Is Terraria the 'native' Linux builds. Versions where this is <see langword="true"/> have a <c>Terraria.LinuxLaunch</c> class.<br/>
        /// </summary>
        public static bool IsLinux => _IsLinux;
        private static bool _IsLinux = false;
        /// <summary>
        /// Is Terraria using FNA, this is only <see langword="true"/> if running on Linux, but may be <see langword="false"/> depending on Proton version.<br/>
        /// This is <see langword="false"/> until FNA loads. This means it could be <see langword="false"/> because FNA was not loaded yet.
        /// <para>
        /// This may be <see langword="true"/> while using Xna because of Wine/Proton.
        /// </para>
        /// </summary>
        public static bool IsFNALoaded => cModLoaderInitializer.LoadedAssembilies.TryGetValue("FNA", out _);
        /// <summary>
        /// Is Terraria using Xna, this is only <see langword="true"/> if running on windows or some Proton versions<br/>
        /// This is <see langword="false"/> until Xna loads. This means it could be <see langword="false"/> because FNA was not loaded yet.
        /// <para>
        /// This could give a <see langword="false-positive"/> if FNA is version 4.0.0.0 but i don't think any Terraria versions use that.
        /// </para>
        /// </summary>
        public static bool IsXnaLoaded => cModLoaderInitializer.LoadedAssembilies.TryGetValue("Microsoft.Xna.Framework", out var asm) && asm.GetName().Version == new Version(4, 0, 0, 0);

        public static Version GameVersion = null;
        public static VersionType GameVersionType = VersionType.Unknown;
        public static void AssembilyInit(string realTerrariaPath) {

            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(realTerrariaPath);
            if (assembly == null) throw new Exception("AssembilyInit Error: assembly null.");
            ModuleDefinition module = ModuleDefinition.ReadModule(realTerrariaPath);
            if (module == null) throw new Exception("AssembilyInit Error: module null.");

            var ver = assembly.Name.Version;
            var ver2 = new Version(0, 0, 0, 0);
            _IsLinux = module.GetType("Terraria.LinuxLaunch") != null;

            var str = "";
            // Try get versions number from Main.versionNumber using IL code
            // Main always existed, however versionNumber did not.
            var main = module.GetType("Terraria.Main");
            if (main == null) throw new Exception("AssembilyInit Error: main null, this is likely due to a different assembly loading in place of Terraria.");
            var cctor = main.Methods.FirstOrDefault(m => m.Name == ".cctor");
            foreach (var instruction in cctor.Body.Instructions) {
                if (instruction.OpCode == OpCodes.Ldstr) {
                    var next = instruction.Next;
                    if (next != null &&
                        next.OpCode == OpCodes.Stsfld &&
                        ((FieldReference)next.Operand).Name == "versionNumber") {
                        str = (string)instruction.Operand;
                        break;
                    }
                }
            }

            // TODO: find a different way of getting the version (maybe Main.assemblyVersionNumber)
            //   tConfig, old tModLoader, TerrariaModder (although i think TerrariaModder sets it at runtime), and beta builds for Linux change this but i think they all keep the vX.Y.Z so it should be fine
            // if str == "" then version is below 1.0.2
            if (str != "") {
                Match m = null;
                try {
                    m = Regex.Match(str, "(.[0-9])?(.[0-9])?(.[0-9])?(.[0-9])?"); // helpfully this will suffice
                    // safer then int.Parce() and less code then int.TryParce()
                    int n1 = m.Groups.Count >= 2 && m.Groups[1] == null ? 0 : (m.Groups[1].Value.Length != 2 ? 0 : m.Groups[1].Value[1] - '0');
                    int n2 = m.Groups.Count >= 3 && m.Groups[2] == null ? 0 : (m.Groups[2].Value.Length != 2 ? 0 : m.Groups[2].Value[1] - '0');
                    int n3 = m.Groups.Count >= 4 && m.Groups[3] == null ? 0 : (m.Groups[3].Value.Length != 2 ? 0 : m.Groups[3].Value[1] - '0');
                    int n4 = m.Groups.Count >= 5 && m.Groups[4] == null ? 0 : (m.Groups[4].Value.Length != 2 ? 0 : m.Groups[4].Value[1] - '0');
                    ver2 = new Version(n1, n2, n3, n4);
                } catch (Exception e) {
                    Accessibility.Show($"Failed to parse versionNumber.\nTerraria.Main.versionNumber = \"{str}\"\nDetails:\n{e.Message}", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ver2 = ver; // set ver2 incase its used
                    Output.Print("Defaulting to assembily version");
                }
            }

            if (IsLinux) {
                // technically some non-playable terraria versions exist but i don't feel like adding those
                // Linux is way cleaner then windows it seems, unless i was lazy and didn't add code to fix windows versions (i was just lazy)
                if (ver == new Version(1, 3, 0, 7)) {
                    if (ver == ver2) { // if they are the same its not 1.3.0.8
                        // possibly beta versions, we could check them but we run out of values for the Version class
                        Output.Print("This version is undetermined.\nVersion range: 1.3.0.7 Betas\nDefaulting to 1.3.0.7.");
                    }
                    else ver = ver2;
                }
                else if (ver == new Version(1, 3, 3, 0)) {
                    // check versions 1.3.3
                    // check versions 1.3.3.1
                    ver = ver2; // fixes 1.3.3.1
                    // there are 2 different 1.3.3
                }
                else if (ver == new Version(1, 3, 5, 1)) {
                    // check versions 1.3.5.1
                    // check versions 1.3.5.2
                    ver = ver2;
                }
                else if (ver == new Version(1, 4, 0, 0)) {
                    // check version 1.4.0.1
                    // check version 1.4.0.2
                    ver = ver2;
                }
                else if (ver == new Version(1, 4, 4, 8)) {
                    // check version 1.4.4.8
                    // check version 1.4.4.8.1 (idk what to do about this)
                    Output.Print("This version is undetermined.\nVersion range: 1.4.4.8 to 1.4.4.8.1\nDefaulting to 1.4.0.0\n*This range contains impossible assembily version(s).");
                }
            }
            else {
                // fix versions numbers
                if (ver == new Version(1, 0, 0, 0)) {
                    // check versions from 0.1 to 1.0.3
                    // if str != "" then its either version 1.0.2 or 1.0.3, ver2 should be this value
                    if (str != "") ver = ver2;
                    else {
                        // version 0.1 didn't have Terraria.Chest
                        var chest = module.GetType("Terraria.Chest");
                        if (chest == null) {
                            ver = new Version(0, 1, 0, 0);
                        }
                        else {
                            // version 0.7 didn't have Terraria.Steam
                            var steam = module.GetType("Terraria.Steam");
                            if (steam == null) {
                                ver = new Version(0, 7, 0, 0);
                            }
                            else {
                                // technically other versions could exist but as far as now they have not been found and would need to be leaked
                                // check versions from 1.0 to 1.0.1
                                Output.Print("This version is undetermined.\nVersion range: 1.0 to 1.0.1\nDefaulting to 1.0.0.0\n");
                            }
                        }
                    }
                }
                else if (ver == new Version(1, 0, 4, 0)) {
                    // check versions from 1.0.4.0 to 1.2.1
                    // they all seem to be fixed
                    ver = ver2;
                }
                else if (ver == new Version(1, 2, 1, 1)) {
                    // check versions from 1.2.1.1, 1.2.1.2 and 1.2.2
                    ver = ver2;
                }
                else if (ver == new Version(1, 3, 0, 1)) {
                    // check version 1.3
                    // check version 1.3.0.2
                    // i gueess these are fixed
                    ver = ver2;
                }
                else if (ver == new Version(1, 3, 0, 7)) {
                    // check version 1.3.0.7
                    // check version 1.3.0.8
                    // they should match
                    ver = ver2;
                }
                else if (ver == new Version(1, 3, 3, 0)) {
                    // check versions 1.3.3
                    // check versions 1.3.3.1
                    ver = ver2;
                }
                else if (ver == new Version(1, 3, 3, 1)) {
                    // 1.3.3.1 only appears once and its on 1.3.3.2
                    ver = new Version(1, 3, 3, 2);
                }
                else if (ver == new Version(1, 3, 5, 1)) {
                    // check version 1.3.5.1
                    // check version 1.3.5.2
                    ver = ver2;
                }
                else if (ver == new Version(1, 4, 0, 0)) {
                    // check version 1.4.0.1
                    // check version 1.4.0.2
                    ver = ver2;
                }
                else if (ver == new Version(1, 4, 4, 8)) {
                    // check version 1.4.4.8
                    // check version 1.4.4.8.1 (idk what to do about this)
                    Output.Print("This version is undetermined.\nVersion range: 1.4.4.8 to 1.4.4.8.1\nDefaulting to 1.4.0.0\n*This range contains impossible assembily version(s).");
                }
            }

            // set version
            if (ver <= new Version(0, 1, 0, 0)) GameVersionType = VersionType.VeryLegacy;
            else if (ver <= new Version(1, 0, 6, 1)) GameVersionType = VersionType.Legacy;
            else if (ver <= new Version(1, 2, 4, 1)) GameVersionType = VersionType.Old;
            else if (ver <= new Version(1, 3, 0, 6)) GameVersionType = VersionType.OldModern;
            else if (ver <= new Version(1, 3, 5, 3)) GameVersionType = VersionType.Modern;
            else if (ver <= new Version(1, 4, 5, 6)) GameVersionType = VersionType.Current;
            else GameVersionType = VersionType.Future;
            
            // set values, the corresponding values in Terraria.VersionCheck were only checked on windows.
            if (ver <= new Version(1, 2, 4, 1)) VersionChecks.Raw_Using_LegacyUISystem = true;
            if (ver <= new Version(1, 3, 4, 4)) VersionChecks.Raw_Using_LegacyFontSystem = true;
            if (ver >= new Version(1, 2, 3, 0)) VersionChecks.Raw_Using_UtilsTextDrawing = true;
            if (ver >= new Version(1, 3, 4, 0)) VersionChecks.Raw_Using_Modern_UITextPanelUsingGeneric = true;

            if (ver == new Version(0, 1, 0, 0)) VersionChecks.Is0_1 = true;
            if (ver >= new Version(1, 1, 0, 0)) VersionChecks._1_1AndUp = true;
            if (ver >= new Version(1, 2, 3, 0)) VersionChecks._1_2_3AndUp = true;
            if (ver >= new Version(1, 3, 0, 0)) VersionChecks._1_3AndUp = true;
            if (ver >= new Version(1, 3, 1, 0)) VersionChecks._1_3_1AndUp = true;
            if (ver >= new Version(1, 3, 3, 0)) VersionChecks._1_3_3AndUp = true;
            if (ver >= new Version(1, 3, 5, 0)) VersionChecks._1_3_5AndUp = true;
            if (ver >= new Version(1, 4, 0, 0)) VersionChecks._1_4AndUp = true;
            if (ver >= new Version(1, 4, 4, 8)) VersionChecks._1_4_4_8AndUp = true;
            if (ver >= new Version(1, 4, 5, 0)) VersionChecks._1_4_5AndUp = true;

            // set sound data (these were only checked on windows)
            if (ver < new Version(1, 4, 0, 0)) {
                if (ver < new Version(1, 3, 4, 0)) {
                    if (ver < new Version(0, 7))
                        Audio.SoundVersion = Audio.SoundVersions.None;
                    else Audio.SoundVersion = Audio.SoundVersions.Main;
                } else Audio.SoundVersion = Audio.SoundVersions.MainNew;
            } else Audio.SoundVersion = Audio.SoundVersions.SoundEngine;

            GameVersion = ver;

            Output.Print("VerNum: " + ver);
            Output.Print("VerType: " + GameVersionType);
            Output.Print("SoundType: " + Audio.SoundVersion);
            Output.Print("LinuxTerraria: " + IsLinux);

        }

        public static Assembly TerrariaAsm;
        public static void AssembilyPost(Assembly asm) {
            TerrariaAsm = asm;
        }

        public static void StartGame() {

            // Windows:
            //   Terraria.Program.Main() : 0.1 - 1.2.4.1
            //   Terraria.Program.InternalMain() 1.3 - 1.3.0.3
            //   Terraria.Program.LaunchGame() 1.3.0.4 - 1.4.5.5
            // Other:
            //   Terraria.Program.LaunchGame()

            // Terraria.Program always existed
            var program = TerrariaAsm.GetType("Terraria.Program");
            if (program == null) throw new Exception("StartGame Error: program null, this is likely due to a different assembly loading in place of Terraria.");

            MethodInfo entry = null;

            if (!IsLinux) {
                if (GameVersion < new Version(1, 3, 0, 0)) {
                    entry = program.GetMethod("Main", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                } else if (GameVersion < new Version(1, 3, 0, 4)) {
                    entry = program.GetMethod("InternalMain", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                } else {
                    entry = program.GetMethod("LaunchGame", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                }
            } else {
                Environment.SetEnvironmentVariable("FNA_WORKAROUND_WINDOW_RESIZABLE", "1");
                entry = program.GetMethod("LaunchGame", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            }

            var pars = entry.GetParameters();
            if (pars.Length == 1) {
                if (pars[0].ParameterType != typeof(string[])) {
                    Accessibility.Show($"(1) Failed to find proper game entry point.\nReport this to the dev to get it fixed.\nGame Versions: {GameVersion}\nEntry: {entry.Name}");
                    return;
                }
                (Delegate.CreateDelegate(typeof(Action<string[]>), null, entry) as Action<string[]>)(new string[] { });
            } else if (pars.Length == 2){
                if (pars[0].ParameterType != typeof(string[])) {
                    Accessibility.Show($"(2) Failed to find proper game entry point.\nReport this to the dev to get it fixed.\nGame Versions: {GameVersion}\nEntry: {entry.Name}");
                    return;
                }
                if (pars[1].ParameterType != typeof(bool)) {
                    Accessibility.Show($"(3) Failed to find proper game entry point.\nReport this to the dev to get it fixed.\nGame Versions: {GameVersion}\nEntry: {entry.Name}");
                    return;
                }
                (Delegate.CreateDelegate(typeof(Action<string[], bool>), null, entry) as Action<string[], bool>)(new string[] { }, IsLinux);
            } else {
                Accessibility.Show($"(4) Failed to find proper game entry point.\nReport this to the dev to get it fixed.\nGame Versions: {GameVersion}\nEntry: {entry.Name}");
            }

        }

        /// <summary> Calls <see cref="Assembly.GetType(string)"/> on <see cref="TerrariaAsm"/>. Returns <see langword="null"/> if no type is found.<br/>Expands to <c>TerrariaAsm.GetType(type)</c></summary>
        public static Type GetType(string type) => TerrariaAsm.GetType(type);

        /// <summary> Stuff for checking versions compatibility, full of a bunch of random things checks.<para>These were only checked on windows.</para></summary>
        public static class VersionChecks {

            /// <summary> Is Terraria version 0.1 (aka Alpha). </summary>
            public static bool Is0_1 { get; internal set; }
            /// <summary> Is Terraria version 1.1 and up. </summary>
            public static bool _1_1AndUp { get; internal set; }
            /// <summary> Is Terraria version 1.2.3 and up. </summary>
            public static bool _1_2_3AndUp { get; internal set;}
            /// <summary> Is Terraria version 1.3 and up. </summary>
            public static bool _1_3AndUp { get; internal set;}
            /// <summary> Is Terraria version 1.3.1 and up. </summary>
            public static bool _1_3_1AndUp { get; internal set;}
            /// <summary> Is Terraria version 1.3.3 and up. </summary>
            public static bool _1_3_3AndUp { get; internal set;}
            /// <summary> Is Terraria version 1.3.5 and up. </summary>
            public static bool _1_3_5AndUp { get; internal set;}
            /// <summary> Is Terraria version 1.4 and up. </summary>
            public static bool _1_4AndUp { get; internal set;}
            /// <summary> Is Terraria version 1.4.5 and up. </summary>
            public static bool _1_4_5AndUp { get; internal set;}
            /// <summary> Is Terraria version 1.4.4.8 and up. </summary>
            public static bool _1_4_4_8AndUp { get; internal set;}

            internal static bool Raw_Using_Modern_UITextPanelUsingGeneric = false;
            internal static bool Raw_Using_LegacyUISystem = false;
            internal static bool Raw_Using_LegacyFontSystem = false;
            internal static bool Raw_Using_UtilsTextDrawing = false;


            /// <summary> 
            /// Is Terraria's UITextPanel class using a generic type (only applies to the modern UI).<br/>
            /// This is <see langword="true"/> for versions after and including 1.3.4.
            /// <para>Older modern UI versions effectively use a string instead.</para>
            /// </summary>
            public static bool Using_Modern_UITextPanelUsingGeneric => Raw_Using_Modern_UITextPanelUsingGeneric;
            /// <summary> 
            /// Is Terraria using margins in the UI system (only applies to the modern UI).<br/>
            /// This is <see langword="true"/> for versions after and including 1.3.1.
            /// <para>Older modern UI versions do not have an option for margins.</para>
            /// </summary>
            public static bool Using_Modern_UIUsingMargin => _1_3_1AndUp;
            /// <summary> 
            /// Is Terraria using the legacy UI system.<br/>
            /// This is <see langword="true"/> for versions 0.1 to 1.2.4.1 (inclusive).
            /// <para>This also determines if Main.instance exists, in versions where this is <see langword="false"/>, Main.instance exists, otherwise you can use <see cref="Terraria.Utils.GetMain"/></para>
            /// <para>Newer versions implement a different system.</para>
            /// </summary>
            public static bool Using_LegacyUISystem => Raw_Using_LegacyUISystem;
            /// <summary> 
            /// Is Terraria using the legacy font system.<br/>
            /// This is <see langword="true"/> for versions 0.1 to 1.3.4.4 (inclusive).
            /// <para>Newer versions implement a different system.</para>
            /// </summary>
            public static bool Using_LegacyFontSystem => Raw_Using_LegacyFontSystem;
            /// <summary> 
            /// Is Terraria using "Terraria.Utils" to draw text.<br/>
            /// This is <see langword="true"/> for versions after and including 1.2.3.
            /// <para>Older versions implement a different system.</para>
            /// </summary>
            public static bool Using_UtilsTextDrawing => Raw_Using_UtilsTextDrawing;
            /// <summary> 
            /// Is Terraria using the Asset type from the Relogic.dll.<br/>
            /// This is <see langword="true"/> for versions after and including 1.4.
            /// <para>Older versions implement a different system.</para>
            /// </summary>
            public static bool Using_RelogicAssets => _1_4AndUp;
            /// <summary> 
            /// Is Terraria's UIScrollbar using an enum in its constructor's parameters.<br/>
            /// This is <see langword="true"/> for versions after and including 1.4.5
            /// <para>Older versions have nothing in its contractors parameters.</para>
            /// </summary>
            public static bool Using_Modern_UIScrollbarUsesEnum => _1_4_5AndUp;
            /// <summary> 
            /// Is Terraria using more sub-folders to better organize stuff in the UI folder.<br/>
            /// This is <see langword="true"/> for versions after and including 1.4.
            /// <para>Older versions do not have folder such as <c>Content\Images\UI\Bestiary</c> or <c>Content\Images\UI\CharCreation</c>.</para>
            /// </summary>
            public static bool Using_Modern_RichUIDirectory => _1_4AndUp;
            /// <summary> 
            /// Is Terraria using _color for text colouring. This applies to UITextPanel and UIText.<br/>
            /// This is <see langword="true"/> for versions after and including 1.3.1.<br/>
            /// Note that this only applies to modern UI version, legacy versions can always colour text.
            /// <para>Older modern UI versions only draw in white.</para>
            /// </summary>
            public static bool Using_Modern_UITextColour => _1_3_1AndUp;
            /// <summary> 
            /// Does Terraria's UIImageButton class have "<see cref="Rectangle"/>?" as a second parameter.<br/>
            /// This is <see langword="true"/> for versions after and including 1.4.5.
            /// <para>Older modern UI versions do not use this.</para>
            /// </summary>
            public static bool Using_Modern_UIImageButtonRectParams => _1_4_5AndUp;
            /// <summary> 
            /// Is Terraria's (on Linux) Using SDL2.<br/>
            /// <see langword="true"/> if SDL2, <see langword="false"/> if SDL3.<br/>
            /// This is <see langword="true"/> for Linux versions below (not including) 1.4.5.<br/>
            /// This is separate then <see cref="OS.IsSDL2"/>, this will check the versions not actual if its using SDL.
            /// <para>Newer versions switch to SDL3.</para>
            /// </summary>
            public static bool Using_Modern_LinuxUsingSDL2 => _1_4_5AndUp;
            /// <summary> 
            /// Is Terraria using a user interface array to store in game (in world) UI.<br/>
            /// This is <see langword="true"/> for versions after and including 1.3.3.
            /// <para>Older modern UI versions have mostly a single functions with all drawing things.</para>
            /// </summary>
            public static bool Using_Modern_InGameInterface => _1_3_3AndUp;
            /// <summary> 
            /// Is Terraria using a UI interface scaling through stuff like <c>Terraria.GameInput.PlayerInput.SetZoom_UI()</c>.<br/>
            /// This is <see langword="true"/> for versions after and including 1.3.5.
            /// <para>Older modern UI versions either manually calculate UI scaling or don't.</para>
            /// </summary>
            public static bool Using_Modern_UIInterfaceScaling => _1_3_5AndUp;
            /// <summary> 
            /// Is Terraria using UI class slider.<br/>
            /// This is <see langword="true"/> for versions after and including 1.4.
            /// <para>Older modern UI versions either manually draw sliders or don't use them at all.</para>
            /// </summary>
            public static bool Using_Modern_UISliders => _1_4AndUp;
            /// <summary> 
            /// Is Terraria using things like "UIElement.LeftClick" and "UIElement.RightClick" rather then just "UIElement.OnClick".<br/>
            /// This is <see langword="true"/> for versions after and including 1.4.4.8.
            /// <para>Older modern UI versions have less options and do not split right and left clicking.</para>
            /// </summary>
            public static bool Using_Modern_RichInputs => _1_4_4_8AndUp;
            /// <summary> 
            /// Is Terraria using "mouseX" and "mouseY".<br/>
            /// This is <see langword="true"/> for versions after and including 1.1.
            /// <para>Other versions use a different method <see cref="Mouse.GetState()"/> is always available.</para>
            /// </summary>
            public static bool Using_Modern_MousePosXY => _1_1AndUp;
            /// <summary> 
            /// Is Terraria using an "in game" pause menu.<br/>
            /// This is <see langword="true"/> for versions after and including 1.2.3.
            /// <para>Older versions just quit to main menu and had no pause menu.</para>
            /// </summary>
            public static bool Using_InGamePauseMenu => _1_2_3AndUp;
            /// <summary> 
            /// Is Terraria using a dynamic language system.<br/>
            /// This is <see langword="true"/> for versions after and including 1.3.
            /// <para>Older versions use hard coded strings (idk how other languages worked).</para>
            /// </summary>
            public static bool Using_LangSystem => _1_3AndUp;


        }

        /// <summary> Stuff for audio </summary>
        public static class Audio {
            /// <summary> What sound method is terraria using. </summary>
            public static SoundVersions SoundVersion = SoundVersions.SoundEngine; // default may be wrong, set in AssembilyInit
            public enum SoundVersions {
                /// <summary> 1.4 - 1.4.5.5 : SoundEngine.PlaySound(int, int, int, int, float, float) </summary>
                SoundEngine,
                /// <summary> 1.3.4 - 1.3.5.3 : Main.PlaySound(int, int, int, int, float, float) </summary>
                MainNew,
                /// <summary> 0.7 - 1.3.3.3 : Main.PlaySound(int, int, int, int) </summary>
                Main,
                /// <summary> 0.1 : Needs custom implementation </summary>
                None
            }
            private static Action<int, int, int, int> PlaySound_Cache_1 = null;
            private static Action<int, int, int, int, float, float> PlaySound_Cache_2 = null;
            private static Func<int, int, int, int, float, float, SoundEffectInstance> PlaySound_Cache_3 = null;
            public static void PlaySound(int type, int x = -1, int y = -1, int Style = 1, float volume = 1f, float pitchOffset = 0f) {
                switch (SoundVersion) {
                    case SoundVersions.SoundEngine:
                        if (PlaySound_Cache_3 == null) {
                            var field = TerrariaAsm.GetType("Terraria.Audio.SoundEngine").GetMethod("PlaySound", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(float), typeof(float) }, null);
                            PlaySound_Cache_3 = Delegate.CreateDelegate(typeof(Func<int, int, int, int, float, float, SoundEffectInstance>), null, field) as Func<int, int, int, int, float, float, SoundEffectInstance>;
                        }
                        PlaySound_Cache_3(type, x, y, Style, volume, pitchOffset);
                        break;
                    case SoundVersions.MainNew:
                        if (PlaySound_Cache_2 == null) {
                            var field = TerrariaAsm.GetType("Terraria.Main").GetMethod("PlaySound", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(float), typeof(float) }, null);
                            PlaySound_Cache_2 = Delegate.CreateDelegate(typeof(Action<int, int, int, int, float, float>), null, field) as Action<int, int, int, int, float, float>;
                        }
                        PlaySound_Cache_2(type, x, y, Style, volume, pitchOffset);
                        break;
                    case SoundVersions.Main:
                        if (PlaySound_Cache_1 == null) {
                            var field = TerrariaAsm.GetType("Terraria.Main").GetMethod("PlaySound", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) }, null);
                            PlaySound_Cache_1 = Delegate.CreateDelegate(typeof(Action<int, int, int, int>), null, field) as Action<int, int, int, int>;
                        }
                        PlaySound_Cache_1(type, x, y, Style);
                        break;
                }
            }
            public static void PlaySound(int type, int x = -1, int y = -1, int Style = 1) => PlaySound(type, x, y, Style, 1f, 0f);
            public static void PlaySound(int type) => PlaySound(type, -1, -1, 1, 1f, 0f);
        }
        /// <summary> Utils, not sure what im going to put here</summary>
        public static class Utils {
            /// <summary>
            /// Use <see cref="GameReference.StaticReference"/> instead to get cached, static results.
            /// <para>
            /// <paramref name="Windows"/>: Walks backwards from <see cref="Application.OpenForms"/> and events to get the instance of Terraria.Main (This is slow so cache the result).
            /// </para>
            /// <para>
            /// <paramref name="Linux"/>: Uses <see cref="AppDomain.CurrentDomain"/> and the "UnhandledException" event to get the instance of Terraria.Main (This is faster then windows but should still be cached).
            /// </para>
            /// This can be used in legacy UI versions (<see cref="Terraria.VersionChecks.Using_LegacyUISystem"/>) to get "main" because Main.instance does not exist.<br/>
            /// Mainly meant for testing.
            /// </summary>
            public static Game GetMain() => IsLinux ? GetMainLinux() : GetMainWindows();
            // windows is so much more complicated then Linux
            /// <summary> Gets <see cref="Game"/> instance from <see cref="Application.OpenForms"/>. </summary>
            private static Game GetMainWindows() {
                // get Form used for terraria
                Form form = null;
                foreach (Form openForm in Application.OpenForms)
                    if (openForm.GetType().Name == "WindowsGameForm")
                        form = openForm;
                if (form == null) throw new Exception("Failed to get main instance, no valid WindowsGameForm form existed.");

                // get reflection data
                var eventsProp = typeof(System.Windows.Forms.Control).GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                var eventList = (EventHandlerList)eventsProp.GetValue(form);
                var head = typeof(EventHandlerList).GetField("head", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(eventList);
                var entryType = head.GetType();
                var keyField = entryType.GetField("key", BindingFlags.NonPublic | BindingFlags.Instance);
                var handlerField = entryType.GetField("handler", BindingFlags.NonPublic | BindingFlags.Instance);
                var nextField = entryType.GetField("next", BindingFlags.NonPublic | BindingFlags.Instance);

                // get "mainForm_Paint" function from "WindowsGameWindow" within the "Paint" event of "Form"
                GameWindow gameWindow = null;
                var entry = head;
                while (gameWindow == null && entry != null) {
                    var key = keyField.GetValue(entry);
                    var handler = handlerField.GetValue(entry) as Delegate;
                    foreach (var d in handler.GetInvocationList()) {
                        if (d.Method.Name == "mainForm_Paint") {
                            gameWindow = d.Target as GameWindow;
                            break;
                        }
                    }
                    entry = nextField.GetValue(entry);
                }

                // get "Paint" functions from "Game" within the "Paint" event of "GameWindow"
                var paintField = typeof(GameWindow).GetField("Paint", BindingFlags.Instance | BindingFlags.NonPublic);
                var del = paintField.GetValue(gameWindow) as Delegate;
                Game game = null;
                if (del != null) {
                    foreach (var d in del.GetInvocationList()) {
                        if (d.Method.Name == "Paint") {
                            game = d.Target as Game;
                            break;
                        }
                    }
                }

                return game;
            }
            /// <summary> Gets <see cref="Game"/> instance from <see cref="AppDomain.CurrentDomain"/>.UnhandledException </summary>
            private static Game GetMainLinux() {
                // get game instance from AppDomain.CurrentDomain.UnhandledException, this works in any version
                var t = typeof(AppDomain);
                // for some reason BindingFlags.Instance | BindingFlags.NonPublic does not work, not sure why
                var UnhandledException = typeof(AppDomain).GetField("UnhandledException", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var eventDelegate = (MulticastDelegate)UnhandledException.GetValue(AppDomain.CurrentDomain);
                Game g = null;
                if (eventDelegate != null) {
                    foreach (Delegate handler in eventDelegate.GetInvocationList()) {
                        if (handler.Method.Name == "OnUnhandledException") {
                            if (handler.Target != null && handler.Target is Game) {
                                g = handler.Target as Game;
                            }
                        }
                    }
                }
                return g;
            }
        }
        /// <summary> Colours terraria seems to use.<br/>Yes, this uses the proper spelling of "Colour" </summary>
        public static class Colour {
            /// <summary> This doesn't exist in Terraria, its mostly a util.<br/><paramref name="hue"/> is a value between 0 and 1<para><see href="https://blog.kenanb.com/code/cg/2024/03/03/converting-hsv-to-rgb.html"/> </para></summary>
            public static Color hsv_to_rgb(float hue, float saturation, float brightness, float alpha = 1) {
                var H = hue * 360f;
                var H2 = H / 60f;
                var C = brightness * saturation; // chroma
                var X = C * (1 - Math.Abs((H2 % 2) - 1));
                switch ((int)H2) {
                    case 0: return new Color(C, X, 0, alpha);
                    case 1: return new Color(X, C, 0, alpha);
                    case 2: return new Color(0, C, X, alpha);
                    case 3: return new Color(0, X, C, alpha);
                    case 4: return new Color(X, 0, C, alpha);
                    case 5: return new Color(C, 0, X, alpha);
                    default: return new Color(0, 0, 0, alpha);
                }
            }

            public static Color OurFavoriteColor = new Color(255, 231, 69);
            public static Color FancyUIFatButtonMouseOver => OurFavoriteColor;
            public static Color UIBackgroundSolid = new Color(73, 94, 171);
            public static Color UIBackgroundTransparent = new Color(63, 82, 151) * 0.7f;
        }
        /// <summary> Some things that are in Terraria's Main class </summary>
        public static class StaticReference {
            private static Dynamic _main = null;
            private static Dynamic _playerInput = null;
            private static Dynamic _lang = null;
            /// <summary> This is only for static references only, no instance of Main is available. </summary>
            public static Dynamic Main => _main ?? (_main = new Dynamic(TerrariaAsm.GetType("Terraria.Main")));
            /// <summary> This is only for static references only, no instance of PlayerInput is available. </summary>
            public static Dynamic PlayerInput => _playerInput ?? (_playerInput = new Dynamic(TerrariaAsm.GetType("Terraria.GameInput.PlayerInput")));
            /// <summary> This is only for static references only, no instance of Lang is available. </summary>
            public static Dynamic Lang => _lang ?? (_lang = new Dynamic(TerrariaAsm.GetType("Terraria.Lang")));
        }


        /// <summary> Stuff for textures </summary>
        public static class Textures {
            private static Game Game_Cache = null;
            /// <summary> Load an XNB texture using the Xna method, Terraria implements its own methods but this should always work. </summary>
            public static Texture2D LoadTexture(string texturePath) {
                var game = Game_Cache == null ? Utils.GetMain() : Game_Cache;
                return game.Content.Load<Texture2D>(texturePath);
            }
            /// <summary> Load an image texture using the <see cref="Texture2D.FromStream(GraphicsDevice, Stream)"/> method, this is used to load image that aren't XNB files. </summary>
            public static Texture2D LoadRawTexture(string texturePath) {
                var game = Game_Cache == null ? Utils.GetMain() : Game_Cache;
                FileStream fileStream = new FileStream(texturePath, FileMode.Open);
                var tex = Texture2D.FromStream(game.GraphicsDevice, fileStream);
                fileStream.Close();
                fileStream.Dispose();
                return tex;
            }
            /// <summary> Load an image texture using the <see cref="Texture2D.FromStream(GraphicsDevice, Stream)"/> method, this is used to load image that aren't XNB files.</summary>
            public static Texture2D LoadRawTexture(Stream stream) {
                var game = Game_Cache == null ? Utils.GetMain() : Game_Cache;
                var tex = Texture2D.FromStream(game.GraphicsDevice, stream);
                return tex;
            }

        }
        /// <summary> Some reference to the Relogic.dll stuff. </summary>
        public static class Relogic {
            /// <summary>
            /// Creates an Asset type with T (<typeparamref name="T"/>), by default this will be loaded with <paramref name="value"/>.<br/>
            /// This is not put in any database in Terraria/Relogic so loading and unloading (or any other automatic features) are not implemented.<br/>
            /// So only use in cases where those features aren't needed or aren't important.
            /// <para>
            /// This is NOT the correct way of doing this, Relogic's Asset code is very confusing and relies heavily on itself<br/>
            /// therefor doing this properly would require a ridicules amount of work without compile time referencing.
            /// </para>
            /// I do not know what <paramref name="name"/> is for but its there if you need it.
            /// <para>
            /// This only works in versions where <see cref="VersionChecks.Using_RelogicAssets"/> is <see langword="true"/>.
            /// </para>
            /// </summary>
            internal static ReLogicAsset<T> HackAsset<T>(T value, string name) where T : class {
                // the normal way is going through "ReLogic.Content.AssetRepository.Request<T>()" but this probably requires a bunch of other stuff which we dont implement

                var asm = cModLoaderInitializer.LoadedAssembilies["ReLogic"];
                Type AssetStateType = asm.GetType("ReLogic.Content.AssetState");
                Type AssetType = asm.GetType("ReLogic.Content.Asset`1");
                AssetType.MakeGenericType(typeof(T));
                object instance = Activator.CreateInstance(AssetType, new object[] { name });

                // i think this should go through SubmitLoadedContent but an instance of IContentSource requires like a million things to be added
                //  and i dont want to bother using a million lines of reflection and builder code to manually add everything it wants in a custom class.
                // set state "State = AssetState.Loaded;"
                AssetType.GetField("State", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(instance, Enum.ToObject(AssetStateType, 2)); // 2 == AssetState.Loaded
                // Value = value;
                AssetType.GetField("Value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(instance, value);
                // don't set source because we don't have one

                return new ReLogicAsset<T>(instance);
            }
            /// <summary>
            /// Gets an asset using Main.Assets.Request&lt;<typeparamref name="T"/>&gt;(<paramref name="name"/>)
            /// <para>
            /// This only works in versions where <see cref="VersionChecks.Using_RelogicAssets"/> is <see langword="true"/>.
            /// </para>
            /// </summary>
            public static ReLogicAsset<T> Asset<T>(string name) where T : class {
                object Assets = TerrariaAsm.GetType("Terraria.Main").GetField("Assets", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(null);
                var foo = Assets.GetType().GetMethod("Request", BindingFlags.Instance | BindingFlags.Public);
                var fooGen = foo.MakeGenericMethod(typeof(T));
                return new ReLogicAsset<T>(fooGen.Invoke(Assets, new object[] { name, Enum.ToObject(cModLoaderInitializer.LoadedAssembilies["ReLogic"].GetType("ReLogic.Content.AssetRequestMode"), 1) }));
            }

        }

        /// 
        /// Vanilla Terraria References
        /// 

        public abstract class VanillaReference {

            protected Dictionary<string, MethodInfo> CachedFunctions = new Dictionary<string, MethodInfo>();
            protected Dictionary<string, FieldInfo> CachedFields = new Dictionary<string, FieldInfo>();

            protected Type TypeCache = null;
            protected abstract string VanillaName();
            protected virtual Type VanillaType() { return TypeCache ?? (TypeCache = TerrariaAsm.GetType(VanillaName())); }

            public virtual object CallInstance(object instance, string foo, params object[] args) {
                MethodInfo func = null;
                if (!CachedFunctions.TryGetValue(foo, out func)) {
                    CachedFunctions[foo] = (func = VanillaType().GetMethod(foo, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
                }
                return func.Invoke(instance, args);
            }
            public virtual object CallStatic(string foo, params object[] args) {
                var foo2 = foo + "_cMod_Static"; // incase there are static and non static functions with the same name 
                MethodInfo func = null;
                if (!CachedFunctions.TryGetValue(foo2, out func)) {
                    CachedFunctions[foo2] = (func = VanillaType().GetMethod(foo, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static));
                }
                return func.Invoke(null, args);
            }

            public object GetInstance(object instance, string fieldName) {
                FieldInfo field = null;
                if (!CachedFields.TryGetValue(fieldName, out field)) {
                    CachedFields[fieldName] = (field = VanillaType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
                }
                return field.GetValue(instance);
            }
            public object GetStatic(string fieldName) {
                var fieldName2 = fieldName + "_cMod_Static"; // incase there are static and non static functions with the same name
                FieldInfo field = null;
                if (!CachedFields.TryGetValue(fieldName2, out field)) {
                    CachedFields[fieldName2] = (field = VanillaType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static));
                }
                return field.GetValue(null);
            }

            public object SetInstance(object instance, string fieldName, object value) {
                FieldInfo field = null;
                if (!CachedFields.TryGetValue(fieldName, out field)) {
                    CachedFields[fieldName] = (field = VanillaType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
                }
                field.SetValue(instance, value);
                return value;
            }
            public object SetStatic(string fieldName, object value) {
                var fieldName2 = fieldName + "_cMod_Static"; // incase there are static and non static functions with the same name
                FieldInfo field = null;
                if (!CachedFields.TryGetValue(fieldName2, out field)) {
                    CachedFields[fieldName2] = (field = VanillaType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static));
                }
                field.SetValue(null, value);
                return value;
            }

        }

        /// <summary> Don't use this, used either <see cref="StaticReference"/> or <see cref="Dynamic"/>. <br/>This can be useful for some things but seems to cause problems all the time and is missing things like ambiguous reflection matches. </summary>
        public class _Main : VanillaReference {
            protected override string VanillaName() => "Terraria.Main";

            private static _Main Instance = new _Main();
            public static object Call(string function, params object[] args) => Instance.CallStatic(function, args);
            public static object Call(object instance, string function, params object[] args) => Instance.CallInstance(instance, function, args);
            public static object Get(string field) => Instance.GetStatic(field);
            public static object Get(object instance, string field) => Instance.GetInstance(instance, field);
            public static object Set(string field, object value) => Instance.SetStatic(field, value);
            public static object Set(object instance, string field, object value) => Instance.SetInstance(instance, field, value);

        }
    }
}
