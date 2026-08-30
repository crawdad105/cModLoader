
using Mono.Cecil;

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using cModLoader.ModComponents;
using cModLoader.Patching;
using cModLoader.UI;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Security.Principal;
using System.CodeDom;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Linq;
using Mono.Cecil.Cil;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Xml.Schema;
using Microsoft.Xna.Framework.GamerServices;
using cModLoader.Utils;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms.VisualStyles;


/*
    IMPORTANT:
        Harmony (And MonoMod) patches will not work on specific functions, functions that return a double (or other 64 bit types) seem to cause stack misalignments
        i think this is related (https://github.com/pardeike/Harmony/issues/640), this seems to be an issue with MonoMod and even by it self i was also unable to get working.
        I was also unable to find a working solution to bypass this using Harmony or MonoMod, so i create a custom patching class, but it doesn't patch at runtime and can't be undone (but mods can't be unloaded anyway)
*/

namespace cModLoader
{
    /// <summary> Config for cModLoader, this is not the same as <see cref="ModComponents.Config"/></summary>
    internal class cModLoaderConfig {

        public static Config ModConfig;

        //TODO: make mods load this way as well
        public static void LoadConfig() {
            ModConfig = new Config();
            ModConfig.Add(new ConfigBoolean("DebugConsole", "Keep Console Alive (Requires Restart)", cModLoaderInitializer.Debug) {
                OnSetValue = (_old, _new) => {
                    cModLoaderInitializer.Debug = _new;
                }
            });
            ModConfig.Add(new ConfigBoolean("ShowDebugText", "Show Debug Text", ShowDebugText) {
                OnSetValue = (_old, _new) => {
                    ShowDebugText = _new;
                }
            });
            ModConfig.Add(new ConfigBoolean("PatchDisable", "Disable Patches", ForceDisablePatches, "Disabled", "Enabled") {
                OnSetValue = (_old, _new) => {
                    ForceDisablePatches = _new;
                }
            });
            ModConfig.Add(new ConfigText("Text1", "Console Tests"));
            ModConfig.Add(new ConfigButton("TextConsole", "Test Console", "Test") {
                OnButtonPress = () => {
                    Output.Print("Test Console");
                }
            });
            ModConfig.Add(new ConfigButton("TextConsole2", "Test Console Error", "Test") {
                OnButtonPress = () => {
                    Output.Error("Test Console");
                }
            });
            ModConfig.Add(new ConfigButton("TextConsole3", "Clear Console", "Clear") {
                OnButtonPress = () => {
                    Output.CustomWriter.ConsoleOutput.Clear();
                }
            });
            ModConfig.Add(new ConfigText("Text2", "Other Tests"));
            ModConfig.Add(new ConfigSlider("Slider1", "Slider Test", 0.5f));
            ModConfig.Add(new ConfigButton("TestMessage", "Test Message Box", "Test") {
                OnButtonPress = () => {
                    try {
                        Accessibility.Show("Message Box");
                    }
                    catch (Exception e) {
                        Accessibility.PreferWindow = Accessibility.WindowType.Default;
                        (ModConfig["ForceWindow"] as ConfigSelectToggle).SetValue(0);
                        Accessibility.Show("Message box failed to open. Resetting to Default message box.\n\n" + e.ToString());
                    }
                }
            });
            // does not work, would need to recreate all UI elements before the next draw (i dont think this is true but something definitely goes wrong)
            /*
            if (!Terraria.VersionChecks.Raw_Using_LegacyUISystem) {
                ModConfig.Add(new ConfigBoolean("ForceLegacy", "Force Legacy UI", false, "Disabled", "Enabled") {
                    OnSetValue = (_old, _new) => {
                        
                        Terraria.VersionChecks.Using_LegacyUISystem = _new ? true : Terraria.VersionChecks.Raw_Using_LegacyUISystem;
                    }
                });
            }
            */
            ModConfig.Add(new ConfigBoolean("LegacyBackground", "Show Debug Legacy Panel Background", false, "Showing", "Hidden") {
                OnSetValue = (_old, _new) => {
                    UIUtils.Test_DrawBoundsLegacyUI = _new;
                }
            });
            ModConfig.Add(new ConfigText("Text3", "Unsafe Options, do not change."));
            ModConfig.Add(new ConfigSelectToggle("ForceWindow", "Force Windows Mode", 0, new string[] { "Default", "Forms", "SDL3", "SDL2" }) {
                OnSetValue = (_old, _new) => {
                    Accessibility.PreferWindow = (Accessibility.WindowType)_new;
                }
            });
            // pre-load config
            var path = Path.GetCurrentConfigsFolder() + Path.DirectorySeparator + "cModLoader.cmlcfg";
            ModConfig.LoadConfig(path);
        }

        public static bool ForceLegacyMenues = false;
        public static bool ForceDisablePatches = true;
        public static bool ShowDebugText = true;
        public static bool ShowSplashDebug = true;

        /// <summary> Definitely not making this one public.<br/>Currently only works in versions before 1.3. </summary>
        internal static bool SteamSpoofer = true;

    }

    /// <summary>
    /// <para> ModContent, has the main function for loading mod content </para>
    /// <para> This class is because a lot of <see cref="ModLoader"/>'s code is ran before terraria is loaded and accessing Terraria's assembly before it is loaded will cause errors </para>
    /// </summary>
    public class ModContent
    {
        internal static List<cModPatch.Patch> patches = new List<cModPatch.Patch>();
        internal static Dictionary<PreMod, Mod> preModLinkList = new Dictionary<PreMod, Mod>();
        internal static List<Assembly> loadedDlls = new List<Assembly>();
        internal static List<Mod> modList = new List<Mod>();
        internal static List<PreMod> preModList = new List<PreMod>();

        /// <summary> Gets the number of loaded dlls </summary>
        internal static int DllCount => loadedDlls.Count;
        /// <summary> Gets the number of loaded mods </summary>
        public static int ModCount => modList.Count;
        /// <summary> Gets the number of loaded patches </summary>
        public static int PreModCount => preModList.Count;
        /// <summary> Gets a <paramref name="dll"/> with a given <paramref name="index"/> </summary> <returns> if the <paramref name="dll"/> at the given <paramref name="index"/> exists </returns>
        internal static bool GetDll(int index, out Mod dll) => (dll = index > -1 && index < modList.Count ? modList[index] : null) != null;
        /// <summary> Gets a <paramref name="mod"/> with a given <paramref name="index"/> </summary> <returns> if the <paramref name="mod"/> at the given <paramref name="index"/> exists </returns>
        public static bool GetMod(int index, out Mod mod) => (mod = index > -1 && index < modList.Count ? modList[index] : null) != null;
        /// <summary> Gets a <paramref name="preMod"/> with a given <paramref name="index"/> </summary> <returns> if the <paramref name="preMod"/> at the given <paramref name="index"/> exists </returns>
        public static bool GetPreMod(int index, out PreMod preMod) => (preMod = index > -1 && index < preModList.Count ? preModList[index] : null) != null;
        /// <summary> Gets a <paramref name="preMod"/> linked to a <paramref name="mod"/> </summary> <returns> if the <paramref name="preMod"/> links to a <paramref name="mod"/> </returns>
        internal static bool GetMod(PreMod preMod, out Mod mod) => preModLinkList.TryGetValue(preMod, out mod);
        internal static bool GetPreMod(Mod mod, out PreMod preMod) {
            preMod = null;
            foreach(var link in preModLinkList) {
                if (link.Value == mod) {
                    preMod = link.Key;
                    return true;
                }
            }
            return false;
        }

        internal static void RegisterMod(PreMod preMod, Mod mod) {
            preMod.modReference = mod;
            var asm = preMod.assembly;
            if (mod != null) {
                mod.preMod = preMod;
                mod.OnInitialize();
                mod.modFullPath = asm.Location;
                mod.modFileDirectory = asm.Location.Substring(0, asm.Location.LastIndexOf(Path.DirectorySeparator) + 1);
                mod.modFileName = asm.Location.Substring(asm.Location.LastIndexOf(Path.DirectorySeparator) + 1);
                modList.Add(mod);
                Output.Print($"Loaded mod of type \"{mod.GetType().FullName}\" in \"{asm.Location}\"!");
                mod.ModConfig.LoadConfig(mod.GetConfigFile());
            }
        }
        /// <summary>
        /// Registers an item with the given <paramref name="internalName"/>.<br/>
        /// <paramref name="internalName"/> can only contain letters A-Z (capital or lowercase), numbers 0-9 (can not start with a number), and underscores (_).<br/>
        /// If the <paramref name="internalName"/> already exists it will be renamed to "<paramref name="internalName"/>_N" where N is the number of previous instances.
        /// </summary>
        //internal static void RegisterItem(string internalName) {
        //
        //
        //}
    }

    /// <summary>
    /// Mod loader, has the main function for loading dlls, mods and patches, also has some functions to get mods and stuff.
    /// </summary>
    public class ModLoader
    {
        /// <summary> Used for error handling, if a mod fails the game wont crash, tModLoader uses c#'s built in environments but this does not exist in .net Framework 4.5.2 </summary>
        internal struct ModContext {
            private static Stack<ModContext> ContextStack = new Stack<ModContext>();
            private static ModContext CurrentModContext;
            public string ContextName;
            public string ModName;
            public Type ModType;
            public Type PreModType;
            public Assembly ModAssembily;
            private static void UpdateContextForModLoader(string name = null) {
                if (cModLoaderInstance == null) {
                    CurrentModContext.ContextName = name ?? "cModLoader";
                    CurrentModContext.ModName = "cModLoader";
                    CurrentModContext.ModType = typeof(cModLoader); // this could be problematic if it tries loading a type
                    CurrentModContext.PreModType = cModLoaderPreModInstance?.GetType();
                    CurrentModContext.ModAssembily = cModLoaderPreModInstance?.assembly; // Assembly.GetExecutingAssembly()
                }
                else {
                    CurrentModContext.ContextName = name ?? cModLoaderInstance.ModName;
                    CurrentModContext.ModName = cModLoaderInstance.ModName;
                    CurrentModContext.ModType = cModLoaderInstance.GetType();
                    CurrentModContext.PreModType = cModLoaderInstance.preMod.GetType();
                    CurrentModContext.ModAssembily = cModLoaderInstance.preMod.assembly; // Assembly.GetExecutingAssembly()
                }
            }
            private static void UpdateModContext(Mod mod, string name = null) {
                if (mod == null) {
                    CurrentModContext.ContextName = name ?? "Null Context";
                    CurrentModContext.ModName = "Null Mod";
                    CurrentModContext.ModType = null;
                    CurrentModContext.PreModType = null;
                    CurrentModContext.ModAssembily = null;
                } else {
                    CurrentModContext.ContextName = name ?? mod.ModName;
                    CurrentModContext.ModName = mod.ModName;
                    CurrentModContext.ModType = mod.GetType();
                    CurrentModContext.PreModType = mod.preMod.GetType();
                    CurrentModContext.ModAssembily = mod.preMod.assembly;
                }
            }
            private static void UpdatePreModContext(PreMod preMod, string name = null) {
                if (preMod == null) {
                    CurrentModContext.ContextName = name ?? "Null PreMod Context";
                    CurrentModContext.ModName = "Null Mod";
                    CurrentModContext.ModType = null;
                    CurrentModContext.PreModType = null;
                    CurrentModContext.ModAssembily = null;
                }
                else {
                    CurrentModContext.ContextName = name ?? "Null PreMod Context";
                    CurrentModContext.ModName = "N/A (PreMod Context)";
                    CurrentModContext.ModType = null;
                    CurrentModContext.PreModType = preMod.GetType();
                    CurrentModContext.ModAssembily = preMod.assembly;
                }
            }

            /// <summary> Runs code under no context. Typically used for base game.</summary>
            internal static void RunUnderNullContext(Action foo, string name = null) { UpdateModContext(null, name); Try(foo); }
            /// <summary> Runs code under cModLoader's context.</summary>
            internal static void RunUnderModLoaderContext(Action foo, string name = null){ UpdateContextForModLoader(name); Try(foo); }
            /// <summary> Runs code under a given <paramref name="preMod"/>'s context.</summary>
            internal static void RunUnderPreModContext(PreMod preMod, Action foo, string name = null){ UpdatePreModContext(preMod, name); Try(foo); }
            /// <summary> Runs code under a given <paramref name="mod"/>'s context.</summary>
            internal static void RunUnderModContext(Mod mod, Action foo, string name = null){ UpdateModContext(mod, name); Try(foo); }


            /// <summary> Runs code under no context. Typically used for base game.<br/>If an error occurs <typeparamref name="T"/> will be <see langword="default"/>.</summary>
            internal static T RunUnderNullContext<T>(Func<T> foo, string name = null) { UpdateModContext(null, name); return Try(foo); }
            /// <summary> Runs code under cModLoader's context.<br/>If an error occurs <typeparamref name="T"/> will be <see langword="default"/>.</summary>
            internal static T RunUnderModLoaderContext<T>(Func<T> foo, string name = null) { UpdateContextForModLoader(name); return Try(foo); }
            /// <summary> Runs code under a given <paramref name="preMod"/>'s context.<br/>If an error occurs <typeparamref name="T"/> will be <see langword="default"/>.</summary>
            internal static T RunUnderPreModContext<T>(PreMod preMod, Func<T> foo, string name = null) { UpdatePreModContext(preMod, name); return Try(foo); }
            /// <summary> Runs code under a given <paramref name="mod"/>'s context.<br/>If an error occurs <typeparamref name="T"/> will be <see langword="default"/>. </summary>
            internal static T RunUnderModContext<T>(Mod mod, Func<T> foo, string name = null) { UpdateModContext(mod, name); return Try(foo); }
            
            private static T Try<T>(Func<T> foo) {
                ContextStack.Push(CurrentModContext);
                try {
                    return foo();
                }
                catch (Exception e) {
                    Accessibility.DisplayException(ContextStack, e, new StackTrace(true).GetFrames());
                    return default(T);
                }
                finally {
                    CurrentModContext = ContextStack.Pop();
                }
            }
            private static void Try(Action foo) {
                ContextStack.Push(CurrentModContext);
                try {
                    foo();
                }
                catch (Exception e) {
                    Accessibility.DisplayException(ContextStack, e, new StackTrace(true).GetFrames());
                }
                finally {
                    CurrentModContext = ContextStack.Pop();
                }
            }

            public static void DisplayException(Exception e) {
                Accessibility.DisplayException(ContextStack, e, new StackTrace(true).GetFrames());
            }

        }

        internal static bool CanLoadContent;

        internal static Mod cModLoaderInstance;
        internal static PreMod cModLoaderPreModInstance;

        internal static TypeReference[] GetAssemblyTypes(Assembly _asm, Type compType) {
            List<TypeReference> types = new List<TypeReference>();
            var asm = AssemblyDefinition.ReadAssembly(_asm.Location);
            foreach (var module in asm.Modules) {
                foreach (var type in module.Types) {
                    if (type.BaseType != null && type.BaseType.FullName == compType.FullName) {
                        types.Add(type);
                    }
                }
            }
            return types.ToArray();
        }

        internal static bool ModsFolderFound = false;
        internal static bool ConfigFolderFound = false;
        internal static void UpdateFolders() {
            Path.EnsureFolder(Path.GetCurrentModsFolder(), out _);
            Path.EnsureFolder(Path.GetCurrentConfigsFolder(), out _);
            ModsFolderFound = Directory.Exists(Path.GetCurrentModsFolder());
            ConfigFolderFound = Directory.Exists(Path.GetCurrentConfigsFolder());
        }

        internal static bool LoadedPatches = false;
        internal static bool LoadedModLoader = false;
        internal static void LoadNativeDll(string path)
        {
            // TODO: take loading from legacy version of cModLoader
        }
        internal static bool LoadDlls() {
            UpdateFolders();
            if (!ConfigFolderFound) {
                Output.Error("Config folder missing. Configs will not save.");
            }
            if (!ModsFolderFound) {
                Output.Error("Mods folder missing. Can not load mods.");
                return false;
            }
            string modFolderPath = Path.GetCurrentModsFolder();
            string[] files = Directory.GetFiles(modFolderPath);
            foreach (string file in files) {
                var name = file.Substring(modFolderPath.Length + 1);
                // ignore files that start with underscore, might change later
                if (name.StartsWith("_")) continue;
                if (file.ToLower().EndsWith(".tmc")) // plans for future
                    Output.Print("Terraria Mod Creator (.tmc) files are currently not supported.");
                else if (file.ToLower().EndsWith(".cml")) // plans for future
                    Output.Print("cModLoader (.cml) files are currently not supported (these are not going to be normal mods).");
                else if (file.ToLower().EndsWith(".dll")) {
                    if (LoadDll(file, out var asm)) {
                        ModContent.loadedDlls.Add(asm);
                        Output.Print("Loaded dll \"" + file + "\"");
                    } else {
                        Output.Print("Failed to load dll \"" + file + "\"");
                    }
                }
            }
            return true;
        }
        internal static bool LoadDll(string path, out Assembly asm)
        {
            asm = null;
            for (int i = 0; i < ModContent.loadedDlls.Count; i++)
                if (ModContent.loadedDlls[i].Location == path) {
                    Output.Error($"The dll \"{path}\" was already loaded");
                    return false;
                }
            if (!File.Exists(path)) {
                Output.Error($"The dll \"{path}\" does not exist (how did you do this?)");
                return false;
            }
            try {
                AssemblyName name = AssemblyName.GetAssemblyName(path);
                for (int i = 0; i < ModContent.loadedDlls.Count; i++)
                    if (ModContent.loadedDlls[i].FullName == name.FullName) {
                        Output.Error($"The dll \"{path}\" was already loaded as a different file");
                        return false;
                    }
            } catch (Exception e) {
                Output.Error($"An error occurred while loading \"{path}\" (1)\n{e.GetType()}\n{e.Message}\n{e.StackTrace}");
            }
            try {
                asm = Assembly.LoadFile(path); // this is way easier then what cModLoaderLegacy had to do (but we cant unload)
            } catch (Exception e) {
                Output.Error($"An error occurred while loading \"{path}\" (2)\n{e.GetType()}\n{e.Message}\n{e.StackTrace}");
                return false;
            }
            return true;
        }

        internal static Type[] GetPreModTypes(Assembly asm) {
            List<Type> preMod = new List<Type>();
            foreach (var _t in GetAssemblyTypes(asm, typeof(PreMod))) {
                var t = asm.GetType(_t.FullName);
                if (t.GetConstructor(Type.EmptyTypes) != null) preMod.Add(t);
                else Output.Print($"Skipping pre-mod of type \"{t.FullName}\" in \"{asm.Location}\" because constructor is not parameterless");
            }
            return preMod.ToArray();
        }
        internal static int TempPatchLoadCount = 0;
        internal static bool LoadPreMod(Type preModType, Assembly asm) {
            var preMod = (PreMod)Activator.CreateInstance(preModType);
            if (preMod == null) {
                Output.Error($"Failed to create a pre-mod instance of \"{preModType.FullName}\"");
                return false;
            }
            TempPatchLoadCount = ModContent.patches.Count;
            if (!preMod.ValidVersion()) {
                Output.Print($"Skipped loading pre-mod \"{preModType.FullName}\"");
                return false;
            }
            preMod.assembly = asm;
            preMod.OnLoad();
            for (int i = TempPatchLoadCount; i < ModContent.patches.Count; i++)
                preMod.modPatches.Add(ModContent.patches[i]);
            ModContent.preModList.Add(preMod);
            return true;
        }
        internal static void LoadPreMods() {
            // cModLoader first
            LoadPreMod(typeof(cModLoaderPre), Assembly.GetExecutingAssembly());
            foreach (var asm in ModContent.loadedDlls) {
                try { // try; to be safe
                    var preModTypes = GetPreModTypes(asm);
                    foreach (var preModType in preModTypes) {
                        LoadPreMod(preModType, asm);
                    }
                }
                catch (Exception e) {
                    Output.Error($"An error occurred while loading \"{asm.Location}\" (3)\n{e.GetType()}\n{e.Message}\n{e.StackTrace}");
                }
            }
            LoadedPatches = true;
        }
        internal static void LoadMods() {
            foreach (var preMod in ModContent.preModList) {
                // registers mods
                Mod mod = null;
                ModContext.RunUnderPreModContext(preMod, () => {
                    preMod.OnStart();
                    mod = preMod.RegisterMod();
                }, "PreMod Init " + preMod.assembly.FullName);
                ModContent.RegisterMod(preMod, mod);
            }
        }

        /*
        // gets all instances of a given type in an assembly

        // gets all patch types in a given assembly
        internal static Type[] GetPatchTypes(Assembly asm) {
            List<Type> patches = new List<Type>();
            foreach (var _t in GetAssemblyTypes(asm, typeof(cModPatch))) {
                var t = asm.GetType(_t.FullName);
                if (t.GetConstructor(Type.EmptyTypes) != null)
                    patches.Add(t);
                else
                    Output.Print($"Skipping the patch of type \"{t.FullName}\" in \"{asm.Location}\" because constructor is not parameterless");
            }
            return patches.ToArray();
        }
        internal static void LoadPatches()
        {
            cModPatch.AddcModLoaderDefaultPatches(); // load cModLoader patches first because its my code i can do want i want
            foreach (var asm in loadedDllList) {
                try { // try; to be safe
                    var patches = GetPatchTypes(asm);
                    foreach (var patchType in patches) {
                        var patch = (cModPatch)Activator.CreateInstance(patchType);
                        if (patch != null) {
                            ModContent.patchList.Add(patch);
                            patch.LoadPatches();
                        }
                    }
                }
                catch (Exception e) {
                    Output.Error($"An error occurred while loading \"{asm.Location}\" (3)\n{e.GetType()}\n{e.Message}\n{e.StackTrace}");
                }
            }
            LoadedPatches = true;
            foreach (var patch in ModContent.patchList) patch.PostLoadPatches();
        }

        // gets all mod types in a given assembly
        private static Type[] GetModTypes(Assembly asm)
        {
            List<Type> mods = new List<Type>();
            foreach (var _t in GetAssemblyTypes(asm, typeof(Mod))) {
                var t = asm.GetType(_t.FullName);
                if (t.GetConstructor(Type.EmptyTypes) != null)
                    mods.Add(t);
                else
                    Output.Print($"Skipping the mod of type \"{t.FullName}\" in \"{asm.Location}\" because constructor is not parameterless");
            }
            return mods.ToArray();
        }
        internal static void LoadMod(Assembly asm)
        {
            try { // try; to be safe
                var mods = GetModTypes(asm);
                foreach (var modType in mods) {
                    var mod = (Mod)Activator.CreateInstance(modType);
                    if (mod != null) {
                        if (!mod.OnInitialize()) continue;
                        mod.rawAssembily = asm;
                        mod.modFilePath = asm.Location.Substring(0, asm.Location.LastIndexOf("\\") + 1);
                        mod.modFileName = asm.Location.Substring(asm.Location.LastIndexOf("\\") + 1);
                        ModContent.modList.Add(mod);
                        Output.Print($"Loaded mod of type \"{modType.FullName}\" in \"{asm.Location}\"!");
                    }
                }
            } catch (Exception e) {
                Output.Error($"An error occurred while loading \"{asm.Location}\"] (4)\n{e.GetType()}\n{e.Message}\n{e.StackTrace}");
            }
        }
        internal static void LoadMods() {
            foreach (var asm in ModLoader.loadedDllList) LoadMod(asm);
        } // calls LoadMod for every loaded dll
        */

        // initialize cModLoader, this is the main entry point, this is called after the opening page of terraria loads
        internal static void InitModLoader(GameReference game) {
            LoadMods();
            LoadedModLoader = true;
        }

        // the fallowing functions are relays for hooks
        // i think it would be better if they didn't exist
        // as it would be a cleaner and smaller stack while being less complicated

        internal static void DrawInterface(GameReference game) {
            if (!ModHelper.HideUI) {
                foreach (var mod in ModContent.modList) {
                    ModContext.RunUnderModContext(mod, () => {
                        mod.DrawInterface(game);
                    });
                }
            }
        }
        /*
        internal static void PreUpdate(GameReference game) {
            foreach (var mod in ModContent.modList) {
                ModContext.RunUnderModContext(mod, () => {
                    mod.OnPreUpdate(game);
                });
            }
        }
        internal static void PostUpdate(GameReference game) {
            foreach (var mod in ModContent.modList) {
                ModContext.RunUnderModContext(mod, () => {
                    mod.OnPostUpdate(game);
                });
            }
        }
        */
        internal static void PreDraw(GameReference game) {
            game.spriteBatch.Begin();
            cModLoader._BasePreDraw(game);
            foreach (var mod in ModContent.modList) {
                ModContext.RunUnderModContext(mod, () => {
                    mod.OnPreDraw(game);
                });
            }
            game.spriteBatch.End();
        }
        internal static void PostDraw(GameReference game) {
            game.spriteBatch.Begin();
            foreach (var mod in ModContent.modList) {
                ModContext.RunUnderModContext(mod, () => {
                    mod.OnPostDraw(game);
                });
            }
            if (Terraria.GameVersionType <= Terraria.VersionType.Old && ModHelper.IsInWorld) {
                DrawInterface(game);
            }
            cModLoader._BasePostDraw(game);
            game.spriteBatch.End();
        }

    }

    internal class cModLoaderPre : PreMod {
        public cModLoaderPre() {
            ModLoader.cModLoaderPreModInstance = this;
        }
        public override bool ValidVersion() => true; // all versions
        // runs in LoadTerraria() before terraria is loaded and after AssembilyInit()
        public override void OnLoad() {
            // this needs to be done differently incase in the future patching are extended to any assembily
            // we need this so we can load either Xna or FNA
            var c = new Microsoft.Xna.Framework.Color();
            if (!Terraria.IsFNALoaded) {
                PatchlessXnaPreHook();
            } else {
                PatchlessFNAPreHook();
            }
            if (cModLoaderConfig.SteamSpoofer && Terraria.GameVersion < new Version(1, 3, 0, 0)) {
                cModPatch.ForcePatch("Terraria.Steam", "Init", "System.Void", new string[] { }, typeof(cModLoaderPre).GetMethod(nameof(InitSteam1)), null);
                cModPatch.ForcePatch("Terraria.Social.SocialAPI", "Initialize", "System.Void", new string[] { "System.Nullable`1<Terraria.Social.SocialMode>" }, typeof(cModLoaderPre).GetMethod(nameof(InitSteam2)), null);
            }

            // used for debugging
            //cModPatch.ForcePatch("Terraria.Program", "DisplayException", "System.Void", new string[] { "System.Exception" }, typeof(cModLoaderPre).GetMethod(nameof(DisplayException)), null);
            
            /*
            
            // Patch fix for most 1.3 versions, does not work in most 1.3 versions because "name" is not an item property but im too lazy to fix it
            // this type if fix is not good either, if cModLoader needs to fix Terraria to run it shouldn't exist.

            var t = typeof(List<int>).ToString().Replace("[", "<").Replace("]", ">");
            //Accessibility.Show($"PAtch Terraria.UI.ItemSorting/ItemSortingLayers::<.cctor>b__72\n{typeof(List<int>)}\n{typeof(List<int>).FullName}\n{typeof(List<int>).Name}\n{typeof(List<int>).AssemblyQualifiedName}\n{typeof(List<int>).IsSpecialName}\n{typeof(List<int>).Namespace}\nNew: {t}");
            cModPatch.AddPatch(
                "Terraria.UI.ItemSorting/ItemSortingLayers",  // base type
                "<.cctor>b__72", // name
                t, // return
                new string[] {  // parameters
                    "Terraria.UI.ItemSorting/ItemSortingLayer", 
                    "Terraria.Item[]",
                    t
                },  
                typeof(cModLoaderPre).GetMethod(nameof(Prefix)), // New
                null // IL
            );
            */
            
            // removed other patches, maybe i add them properly later but idk
        }
        public override Mod RegisterMod() {
            return new cModLoader();
        }

        /// 
        /// Other
        /// 
        public static void InitSteam1() {
            Terraria.TerrariaAsm.GetType("Terraria.Steam").GetField("SteamInit", BindingFlags.Static | BindingFlags.Public).SetValue(null, true);
        }
        public static void InitSteam2(object mode) {
            
        }
        public static void DisplayException(Exception e) {
            ModLoader.ModContext.DisplayException(e);
        }
        public static List<int> Prefix(object _layer, object _inv, List<int> itemsToSort) {
            Dynamic layer = new Dynamic(_layer);
            Array inv = (Array)_inv;

            List<int> indexesSortable = itemsToSort.Where(i => new Dynamic(inv.GetValue(i)).GetValue<int>("createWall") > 0 || new Dynamic(inv.GetValue(i)).GetValue<int>("createTile") >= 0).ToList();
            var param = new object[] { indexesSortable, (object)inv };
            layer.Invoke2("Validate", param);
            foreach (var item in indexesSortable) itemsToSort.Remove(item);
            indexesSortable.Sort((x, y) => {
                int num = string.Compare(new Dynamic(inv.GetValue(x)).GetValue<string>("name"), new Dynamic(inv.GetValue(y)).GetValue<string>("name"), StringComparison.OrdinalIgnoreCase);
                if (num == 0) num = new Dynamic(inv.GetValue(y)).GetValue<int>("stack").CompareTo(new Dynamic(inv.GetValue(x)).GetValue<int>("stack"));
                if (num == 0) num = x.CompareTo(y); // fix
                // old version does not work for some reason
                //if (num == 0) num = ((x != y) ? (-1) : 0);
                return num;
            });

            return itemsToSort;
        }

        /// 
        /// Window Patch Stuff
        /// 

        /// <summary> Wrapper for the graphics device so we can hook the begin and end drawing functions without IL modifications.<br/>Hopefully this doesn't change in game or else we are hooped.<br/>This works on windows and some Linux versions.</summary>
        public class WrappedGraphicsDeviceManager : IGraphicsDeviceManager {
            public IGraphicsDeviceManager _original;
            public GameReference _ref;
            public WrappedGraphicsDeviceManager(IGraphicsDeviceManager original, Game game) {
                _original = original;
                _ref = new GameReference(game);
            }
            bool IGraphicsDeviceManager.BeginDraw() {
                var flag = false;
                ModLoader.ModContext.RunUnderNullContext(() => {
                    flag = _original.BeginDraw();
                }, "Base Game, BeginDraw");
                ModLoader.ModContext.RunUnderModLoaderContext(() => {
                    // on Linux the constructor is ran sooner and spriteBatch is not create yet
                    if (_ref.spriteBatch == null) {
                        _ref = new GameReference(_ref.game);
                    }
                    if (flag) GlobalHooks.RawHooks.RawPreDraw(_ref);
                });
                return flag;
            }
            void IGraphicsDeviceManager.EndDraw() {
                ModLoader.ModContext.RunUnderModLoaderContext(() => {
                    GlobalHooks.RawHooks.RawPostDraw(_ref);
                });
                ModLoader.ModContext.RunUnderNullContext(() => {
                    _original.EndDraw();
                }, "Base Game, EndDraw");
            }
            void IGraphicsDeviceManager.CreateDevice() { // not sure if this ends up being called
                _original.CreateDevice();
            }
        }
        // i want a different name for this
        public static bool did = false;
        // what is this for?
        public static bool allowIdle = false;
        /// <summary> Sets up things so cModLoader can run without patches. This function only works on Xna versions. </summary>
        public static void PatchlessXnaPreHook() {
            /// Overrides some function that deals with adding handles to a global data base, the base Form could be gotten this way
            //var asm = cModLoaderInitializer.LoadedAssembilies["System.Windows.Forms"];
            //var HandleCollectorType = asm.GetType("System.Internal.HandleCollector");
            //var eventInfo = HandleCollectorType.GetEvent("HandleAdded", BindingFlags.Static | BindingFlags.NonPublic);
            //
            //var delegateType = eventInfo.EventHandlerType;
            //var handlerDelegate = Delegate.CreateDelegate(delegateType, typeof(cModLoaderPre).GetMethod(nameof(MyHandler), BindingFlags.Static | BindingFlags.Public));
            //
            //var addMethod = eventInfo.GetAddMethod(nonPublic: true);
            //addMethod.Invoke(null, new object[] { handlerDelegate });

            // should run after the first frame is ran (i think)
            Application.Idle += (o, e) => {
                if (did) return; // could also remove this function from Application.Idle but this is easier

                // get nested type
                var t = typeof(Application).GetNestedType("ThreadContext", BindingFlags.NonPublic);
                // get current thread context instance
                var instance = t.GetField("currentThreadContext", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                // get idle handler
                var fs = (EventHandler)t.GetField("idleHandler", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(instance);
                Game g = null;
                // get Game instance from WindowsGameHost instance extracted from the "ApplicationIdle" function/event 
                foreach (var item in fs.GetInvocationList()) {
                    if (item.Method.Name == "ApplicationIdle") {
                        g = (Game)item.Target.GetType().GetField("game", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(item.Target);
                        break;
                    }
                }
                // get host instance from game instance
                var host = typeof(Game).GetField("host", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(g);
                // get Idle field
                var Idle = host.GetType().BaseType.GetField("Idle", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(host) as Delegate;
                // extract "HostIdle" function from Game from Idle
                foreach (var item in Idle.GetInvocationList()) {
                    if (item.Method.Name == "HostIdle") {
                        // hook HostIdle which calls Tick which encapsulates Update and Draw
                        var original = (EventHandler<EventArgs>)item;
                        EventHandler<EventArgs> wrapper = (o_, e_) => { // o_ is WindowsGameHost
                            // get WrappedGraphicsDeviceManager from Game, using Dynamic is faster then pure reflection but i feel is still too slow (i guess we could just use variable "g" but idk how that works).
                            Game g2 = new Dynamic(o_).GetValue<Game>("game");
                            var graphics = (WrappedGraphicsDeviceManager)g.Services.GetService(typeof(IGraphicsDeviceManager));
                            ModLoader.ModContext.RunUnderModLoaderContext(() => {
                                GlobalHooks.RawHooks.PreTick(graphics._ref);
                            }, "HostIdle PreTick");
                            ModLoader.ModContext.RunUnderNullContext(() => {
                                original(o_, e_);
                            }, "Base Game, HostIdle");
                            ModLoader.ModContext.RunUnderModLoaderContext(() => {
                                GlobalHooks.RawHooks.PostTick(graphics._ref);
                            }, "HostIdle PostTick");
                        };
                        Idle = Delegate.Remove(Idle, item);
                        Idle = Delegate.Combine(Idle, wrapper);
                    }
                }
                // set new hooked value
                host.GetType().BaseType.GetField("Idle", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(host, Idle);

                // hook BeginDraw and EndDraw
                // wrap original graphics
                // manually set graphicsDeviceManager because its cached
                var gameManagerField = typeof(Game).GetField("graphicsDeviceManager", BindingFlags.Instance | BindingFlags.NonPublic);
                var graphicsDeviceManager = (IGraphicsDeviceManager)gameManagerField.GetValue(g);
                var wrapped = new WrappedGraphicsDeviceManager(graphicsDeviceManager, g);
                gameManagerField.SetValue(g, wrapped);
                // set value in services to custom wrapped value to be safe
                var data = (Dictionary<Type, object>)typeof(GameServiceContainer).GetField("services", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(g.Services);
                data[typeof(IGraphicsDeviceManager)] = wrapped;
                typeof(GameServiceContainer).GetField("services", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(g.Services, data);

                // hook the UI draw call, this was an idea but cant do what i wanted so we dont run unsafe code for no reason
                //GlobalHooks.RawHooks.UnsafeHooks.HookUIElementDrawCall(wrapped._ref);

                did = true;
            };
        }
        /// <summary> Sets up things so cModLoader can run without patches. This function only works on FNA (or some versions of Linux). </summary>
        public static void PatchlessFNAPreHook() {
            // Application.Idle does not exist so we need to find another way to run code

            // FNA 0.0.0.1 (Terraria 1.3.0.7 - 1.3.4.4)
            //   Setting ALDevice will cause console output when its set normal, we can hook this runs at the end of Game.Game()
            // FNA 20.8.0.0+ (Terraria 1.4.1+)
            //   hook graphicsDeviceManager like windows
            //   FNAPlatform.PollEvents runs before tick every time (and we get a game instance so cherry on top)
            //   FNAPlatform.NeedsPlatformMainLoop runs before tick once in RunLoop
            //   FNAPlatform.SupportsOrientationChanges is called when creating GraphicsDeviceManager this is inside of Main.Main()
            // FNA 22.2.0.0+ (Terraria 1.4.3.6+)
            //   FNAPlatform.PollEvents now runs almost first in tick every time

            // original used Microsoft.Xna.Framework and this would return the same but if both "Microsoft.Xna.Framework" and "FNA" exists and are different then we get issues.
            var FNA = cModLoaderInitializer.LoadedAssembilies["FNA"];
            var ver = FNA.GetName().Version;
            // if its this version do this, idk if it works Linux lines taking the console back
            if (ver == new Version(0, 0, 0, 1)) { // 1.3.0.7 to 1.3.4.4
                // set AudioDevice.ALDevice to a null device to do nothing
                var AudioDevice_ALDevice = FNA.GetType("Microsoft.Xna.Framework.Audio.AudioDevice").GetField("ALDevice", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                var NullDevice = FNA.GetType("Microsoft.Xna.Framework.Audio.NullDevice");
                AudioDevice_ALDevice.SetValue(null, Activator.CreateInstance(NullDevice));
                // when AudioDevice.ALDevice is not null text will be printed to console
                GlobalHooks.RawHooks.OnConsoleOutput += FNAConsoleHook;
            }
            else if (ver >= new Version(20, 8, 0, 0)) { // 1.4.1+
                // this wont run on servers ins some Terraria versions but we don't care because there's no graphics there anyways.
                var SupportsOrientationChanges_Field = FNA.GetType("Microsoft.Xna.Framework.FNAPlatform").GetField("SupportsOrientationChanges", BindingFlags.Public | BindingFlags.Static);
                SupportsOrientationChanges_Field.SetValue(null,
                    Delegate.Combine(
                        Delegate.CreateDelegate(
                            FNA.GetType("Microsoft.Xna.Framework.FNAPlatform+SupportsOrientationChangesFunc"), 
                            typeof(cModLoaderPre).GetMethod(nameof(FNASupportsOrientationChangesHook))
                        ),
                        (Delegate)SupportsOrientationChanges_Field.GetValue(null)
                    )
                );

                //           public delegate void PollEventsFunc(Game game, ref GraphicsAdapter currentAdapter, bool[] textInputControlDown, ref bool textInputSuppress);
                // 26.1.0.0: public delegate void PollEventsFunc(Game game, ref GraphicsAdapter currentAdapter, bool[] textInputControlDown, ref bool textInputSuppress);
                // 25.8.0.0: public delegate void PollEventsFunc(Game game, ref GraphicsAdapter currentAdapter, bool[] textInputControlDown, ref bool textInputSuppress);
                // 22.2.0.0: public delegate void PollEventsFunc(Game game, ref GraphicsAdapter currentAdapter, bool[] textInputControlDown, int[] textInputControlRepeat, ref bool textInputSuppress);
                // 20.8.0.0: public delegate void PollEventsFunc(Game game, ref GraphicsAdapter currentAdapter, bool[] textInputControlDown, int[] textInputControlRepeat, ref bool textInputSuppress);
                // FNAPlatform.PollEvents runs before update and draw are called
                var PollEvents_Field = FNA.GetType("Microsoft.Xna.Framework.FNAPlatform").GetField("PollEvents", BindingFlags.Public | BindingFlags.Static);
                // function changes at some points so try both
                try {
                    PollEvents_Field.SetValue(null,
                        Delegate.Combine(
                            Delegate.CreateDelegate(
                                FNA.GetType("Microsoft.Xna.Framework.FNAPlatform+PollEventsFunc"),
                                typeof(cModLoaderPre).GetMethod(nameof(FNAPreTickPollEventsHook2))
                            ),
                            (Delegate)PollEvents_Field.GetValue(null)
                        )
                    );
                }
                catch (Exception) {
                    PollEvents_Field.SetValue(null,
                        Delegate.Combine(
                            Delegate.CreateDelegate(
                                FNA.GetType("Microsoft.Xna.Framework.FNAPlatform+PollEventsFunc"),
                                typeof(cModLoaderPre).GetMethod(nameof(FNAPreTickPollEventsHook))
                            ),
                            (Delegate)PollEvents_Field.GetValue(null)
                        )
                    );
                }
            }
            else {
                // fallback incase FNA is version 4.0.0.0 because then its probably actually using Xna
                if (ver == new Version(4, 0, 0, 0)) {
                    Accessibility.Show("FNA was likely incorrectly used instead of XNA\nthis is likely a cModLoader bug or an un-accounted for Terraria versions, report it.");
                } else {
                    GlobalHooks.RawHooks.NoHooks($"No hooks found in FNA version {ver} yet.");
                }
            }

        }
        /// <summary> FNA pre-hook for Terraria 1.4.1 and up. FNA >= 20.8.0.0.</summary>
        public static bool FNASupportsOrientationChangesHook() {
            // g should not be null here i hope
            Game g = Terraria.Utils.GetMain();
            var FNA = cModLoaderInitializer.LoadedAssembilies["Microsoft.Xna.Framework"]; // gets FNA dll because we redirected it
            // set value in services to custom wrapped value

            var graphicsDeviceManager = (IGraphicsDeviceManager)g.Services.GetService(typeof(IGraphicsDeviceManager));
            var wrapped = new WrappedGraphicsDeviceManager(graphicsDeviceManager, g);
            var data = (Dictionary<Type, object>)typeof(GameServiceContainer).GetField("services", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(g.Services);
            data[typeof(IGraphicsDeviceManager)] = wrapped;
            typeof(GameServiceContainer).GetField("services", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(g.Services, data);

            return false; // this value wont be used but we return it because that's what FNAPlatform.SupportsOrientationChanges needs
        }
        // We can not output to console here so we need to be carful
        /// <summary> FAN pre-hook for Terraria 1.3.4.4 and below. FNA 0.0.0.1 </summary>
        public static void FNAConsoleHook(string str) {
            // hopefully this never changes
            if (str == "ALDevice already exists, overwriting!") {
                // remove this so we don't call it again
                GlobalHooks.RawHooks.OnConsoleOutput -= FNAConsoleHook;
                GlobalHooks.RawHooks.NoHooks($"No hooks found in FNA version 0.0.0.1 yet that allow for useful code execution.");
            }
        }

        /// <summary> FAN pre-tick-hook intermediate state for Terraria 1.4.1 to 1.4.4.9. FNA >= 20.8.0.0 &lt; 25.8.0.0.</summary>
        public static void FNAPreTickPollEventsHook(Game game, ref GraphicsAdapter currentAdapter, bool[] textInputControlDown, int[] textInputControlRepeat, ref bool textInputSuppress) {
            ModLoader.ModContext.RunUnderModLoaderContext(() => {
                var graphics = (WrappedGraphicsDeviceManager)game.Services.GetService(typeof(IGraphicsDeviceManager));
                GlobalHooks.RawHooks.PreTick(graphics._ref);
            }, "FNAPreTickPollEventsHook PreTick");
        }
        /// <summary> FAN pre-tick-hook intermediate state for Terraria 1.4.5 and up. FNA >= 25.8.0.0.</summary>
        public static void FNAPreTickPollEventsHook2(Game game, ref GraphicsAdapter currentAdapter, bool[] textInputControlDown, ref bool textInputSuppress) {
            ModLoader.ModContext.RunUnderModLoaderContext(() => {
                var graphics = (WrappedGraphicsDeviceManager)game.Services.GetService(typeof(IGraphicsDeviceManager));
                GlobalHooks.RawHooks.PreTick(graphics._ref);
            }, "FNAPreTickPollEventsHook2 PreTick");
        }

        /// <summary> Added to System.Internal.HandleCollector.HandleAdded.<br/>Not used through </summary>
        public static void MyHandler(string handleType, IntPtr handleValue, int currentHandleCount) {
            if (handleType == "Window") {
                var f = typeof(Control).GetMethod("FromHandleInternal", BindingFlags.Static | BindingFlags.NonPublic);
                var control = (Control)f.Invoke(null, new object[] { handleValue });
                if (control == null) return;
                if (control.GetType().Name != "Microsoft.Xna.Framework.WindowsGameForm") return;
                var form = (control as Form);
            }
        }

    }

    /// <summary> Main cModLoader mod, this is a default mod that handles some basic things </summary>
    internal class cModLoader : Mod {
        public bool ShowConsoleText = false;

        public cModLoader() { ModLoader.cModLoaderInstance = this; }
        public override void OnInitialize() {
            ModName = "cModLoader";
            ModDescription = "This is the default built in mod for cModLoader.\nIt does stuff.";
            ModAuthor = "crawdad105";
            ModVersion = BuildData.GET_DISPLAY(!Terraria.VersionChecks.Using_LegacyFontSystem);
            ModUrl = "https://github.com/crawdad105/cModLoader";

            // set mod config
            modConfig = cModLoaderConfig.ModConfig;
        }

        public override Texture2D GetModIcon() {
            if (ModIcon == null) {
                // TODO: make this a build in options for all mods
                foreach (var name in Assembly.GetExecutingAssembly().GetManifestResourceNames()) {
                    if (name == "cModLoader.Resources.cModLoader.png") {
                        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name)) {
                            ModIcon = Terraria.Textures.LoadRawTexture(stream);
                        }
                    }
                }
            }
            return ModIcon;
        }

        public override void DrawInterface(GameReference game) {
            //UIUtils.DrawString(game.spriteBatch, "Some User Interface Text For In Game.", new Vector2(10, 10), Color.Red, 1f, true);
        }

        internal static bool _AddedInterface = false;
        internal static void _BasePreDraw(GameReference game) {
            // add interface hooks
            if (!_AddedInterface) {
                if (Terraria.VersionChecks.Using_Modern_InGameInterface) {
                    var _needToSetupDrawInterfaceLayers = game.main.GetValue<bool>("_needToSetupDrawInterfaceLayers");
                    if (!_needToSetupDrawInterfaceLayers) { // if false: we can add to the array
                        // the interface array was either called "_drawInterfaceLayers" or "_gameInterfaceLayers"
                        if (Terraria.GameVersion >= new Version(1, 3, 5, 0)) {
                            var list = game.main.GetValue<IList>("_gameInterfaceLayers"); // List<GameInterfaceLayer>
                            var t = new Dynamic(list[0]);
                            foreach (var item in list) {
                                t.Value = item;
                                string name = t.GetValue<string>("Name");
                                if (name == "Vanilla: Cursor") {
                                    var oldDel = t.GetValue<Delegate>("_drawMethod");
                                    var newDel = Delegate.CreateDelegate(oldDel.GetType(), null, typeof(GlobalHooks.RawHooks)
                                        .GetMethod(nameof(GlobalHooks.RawHooks.PreMouseInterfaceDraw), BindingFlags.Static | BindingFlags.NonPublic));
                                    t.SetValue("_drawMethod", Delegate.Combine(newDel, oldDel));
                                    continue;
                                }
                                if (name == "Vanilla: Ingame Options") {
                                    var oldDel = t.GetValue<Delegate>("_drawMethod");
                                    var newDel = Delegate.CreateDelegate(oldDel.GetType(), null, typeof(GlobalHooks.RawHooks)
                                        .GetMethod(nameof(GlobalHooks.RawHooks.OptionsHookInterfaceDraw), BindingFlags.Static | BindingFlags.NonPublic));
                                    t.SetValue("_drawMethod", newDel); // override
                                    continue;
                                }
                            }
                            Dynamic.CleanType(t.ValueType);
                        } else {
                            var list = game.main.GetValue<IList>("_drawInterfaceLayers"); // List<MethodSequenceListItem>
                            var t = new Dynamic(list[0]);
                            foreach (var item in list) {
                                t.Value = item;
                                string name = t.GetValue<string>("Name");
                                if (name == "Vanilla: Cursor") {
                                    Func<bool> hook = GlobalHooks.RawHooks.PreMouseInterfaceDraw;
                                    t.SetValue("Method", Delegate.Combine(hook, t.GetValue<Func<bool>>("Method")));
                                    break;
                                }
                                if (name == "Vanilla: Ingame Options") {
                                    Func<bool> hook = GlobalHooks.RawHooks.OptionsHookInterfaceDraw;
                                    t.SetValue("Method", hook); // override
                                    break;
                                }
                            }
                            Dynamic.CleanType(t.ValueType);
                        }
                        _AddedInterface = true;
                    }
                }
                else {
                    _AddedInterface = true;
                }
            }
        }
        internal static void _BasePostDraw(GameReference game) {
            if (Terraria.GameVersion >= new Version(1, 0)) {
                var splash = (bool)Terraria._Main.Get("showSplash");
                if (splash) {
                    if (cModLoaderConfig.ShowSplashDebug) {
                        _BaseDrawDebug(game);
                    }
                } else {
                    if (cModLoaderConfig.ShowDebugText) {
                        _BaseDrawDebug(game);
                    }
                    ModMenu.DrawModMenu(game);
                }
            } else if (Terraria.GameVersion == new Version(0, 7, 0, 0)) { // 0.7
                ModMenu.DrawModMenu(game);
            } else { // 0.1 (or fallback)
                if (cModLoaderConfig.ShowDebugText)
                    _BaseDrawDebug(game);
                ModMenu.DrawModMenu(game);
                //VeryLegacyVersionPatch._BaseDraw(game);
            }
        }
        internal static void _BaseDrawDebug(GameReference game) {
            var str = string.Join("\n", Output.CustomWriter.ConsoleOutput.GetOutput(30));
            var pos = new Vector2(10, 10);
            var col = new Color(255, 255, 255, 100);
            var scale = 0.8f;
            if (Terraria.VersionChecks.Using_LegacyFontSystem) {
                UIUtils.DrawString(game.spriteBatch, str, pos, col, scale, new Vector2(0, 0));
            } else {
                var t = Terraria.TerrariaAsm.GetType("Terraria.GameContent.FontAssets");
                var v1 = t.GetField("MouseText").GetValue(null);
                if (v1 == null) {
                    UIUtils.DrawSafeString(game, str, pos, col, scale, new Vector2(0, 0));
                } else {
                    var v2 = new ReLogicAsset<object>(v1).Value;
                    if (v2 == null) {
                        UIUtils.DrawSafeString(game, str, pos, col, scale, new Vector2(0, 0));
                    } else {
                        UIUtils.DrawString(game.spriteBatch, str, pos, col, scale, new Vector2(0, 0));
                    }
                }
            }
        }

    }

}