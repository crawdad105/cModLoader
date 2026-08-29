

using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using PathIO = System.IO.Path; // because cModLoader has its own Path class
using System.Windows.Forms;

using cModLoader.Patching;
using System.Net.Mail;

namespace cModLoader
{
    internal class cModLoaderInitializer
    {
        internal static int mainThreadId = 0;
        public static bool IsMainThread => Environment.CurrentManagedThreadId == mainThreadId;

        // load into
        public static byte[] TerrariaAsmBytes;
        // loaded assembly cache, otherwise multiple of the same dlls could be loaded for mods
        // this is separate then "loadedDllList" in the ModLoader class
        /// <summary> A dictionary of loaded assembily (Spelt wrong but i don't feal like changing it). Eg. "Terraria", "ReLogic" or "System.Windows.Forms" </summary>
        public static Dictionary<string, Assembly> LoadedAssembilies = new Dictionary<string, Assembly>();
        private static bool ResolvingTerrariaExe = false;
        /// <summary> Used for some things but mainly for keeping console.</summary>
        internal static bool Debug = false;
        /// <summary> Should the mod loader output the patched terraria EXE, set to <see langword="false"/> when publishing. </summary>
        internal static bool PatchOutOverride = true;

        [STAThread] // needed for "openFileDialog.ShowDialog()"[Obsolete now] and other stuff
        public static void Main(string[] args) {
            // "So this is where it all starts" - crawdad105

            // make main thread identifiable
            mainThreadId = Environment.CurrentManagedThreadId;

            AppDomain.CurrentDomain.AssemblyLoad += (o, e) => {
                var name = e.LoadedAssembly.GetName().Name;
                LoadedAssembilies.Add(name, e.LoadedAssembly);
                // redirect WineMono to default named dlls, not sure if this is what we should do
                if (name.StartsWith("WineMono")) {
                    LoadedAssembilies.Add(name.Substring("WineMono.".Length), e.LoadedAssembly);
                }
            };

            // this code is almost identical [not anymore] to the entry class in Terraria.WindowsLaunch.Main
            // the only difference is it loads the terraria exe and has extra code to load dlls from the certain directory
            AppDomain.CurrentDomain.AssemblyResolve += (object o, ResolveEventArgs sargs) => {
                string baseName = new AssemblyName(sargs.Name).Name;
                // check if assembly is already loaded then return that so no duplicate assemblies are loaded
                if (LoadedAssembilies.ContainsKey(baseName))
                    return LoadedAssembilies[baseName];
                // Load terraria because LoadTerraria() ends up calling this code (this dose not happen in newer .net frameworks)
                if (baseName == "Terraria") {
                    return LoadTerraria();
                }
                // redirect Microsoft.Xna.Framework to FNA
                // normal this loads normally if not, it means FNA is being used and is redirect here
                if (
                    baseName == "Microsoft.Xna.Framework" ||
                    baseName == "Microsoft.Xna.Framework.Game" ||
                    baseName == "Microsoft.Xna.Framework.Graphics" ||
                    baseName == "Microsoft.Xna.Framework.Input.Touch"
                    // these are being included but does not exist in FNA, hopefully it does not end up being an issue
                    // baseName == "Microsoft.Xna.Framework.GamerServices"
                    // baseName == "Microsoft.Xna.Framework.Xact"
                ) {
                    Assembly asm = null;
                    if (LoadedAssembilies.TryGetValue("FNA", out asm)) {
                        Output.Print("Skip load FNA, storing as " + baseName);
                        return (LoadedAssembilies[baseName] = asm); // store FNA as Microsoft.Xna.Framework 
                    }
                    LoadedAssembilies["FNA"] = Assembly.LoadFile(Path.GetCurrentFolder() + "FNA.dll"); // load FNA and store it
                    Output.Print("Loaded " + baseName + " as FNA.");
                    return LoadedAssembilies["FNA"];
                }
                string resourceName = baseName + ".dll";
                string text = Terraria.TerrariaAsm == null ? null : Array.Find(Terraria.TerrariaAsm.GetManifestResourceNames(), (string element) => element.EndsWith(resourceName));
                if (text == null) {
                    // check for dll within self
                    text = Array.Find(Assembly.GetExecutingAssembly().GetManifestResourceNames(), (string element) => element.EndsWith(resourceName));
                    if (text != null) {        
                        using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(text)) {
                            byte[] array = new byte[stream.Length];
                            stream.Read(array, 0, array.Length);
                            Output.Print("Loaded (self) internal Dll: " + resourceName);
                            return (LoadedAssembilies[baseName] = Assembly.Load(array));
                        }
                    }
                }
                if (text == null)
                {
                    string exePath = Path.GetCurrentPath();
                    string mainDir = exePath.Substring(0, exePath.LastIndexOf("\\") + 1); // terraria\
                    string cModDir = mainDir + "cModLoader\\"; // terraria\cModLoader\
                    string modsDir = cModDir + "Mods\\"; // terraria\cModLoader\Mods
                    text = Array.Find(Directory.GetFiles(mainDir), file => file.EndsWith(resourceName));
                    if (text == null) text = Array.Find(Directory.GetFiles(cModDir), file => file.EndsWith(resourceName));
                    if (text == null) text = Array.Find(Directory.GetFiles(modsDir), file => file.EndsWith(resourceName));
                    if (text == null) {
                        Console.WriteLine("Failed to load Dll: " + resourceName);
                        Accessibility.Show("Failed to load Dll: \n\"" + resourceName + "\"\nPlace the dll in one of the following paths. If this can not be done Terraria could crash.\n" + mainDir + "\n" + cModDir + "\n" + modsDir + "\n", "Missing Dll Error.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        // check again incase the user played the dll
                        text = Array.Find(Directory.GetFiles(mainDir), file => file.EndsWith(resourceName));
                        if (text == null) text = Array.Find(Directory.GetFiles(cModDir), file => file.EndsWith(resourceName));
                        if (text == null) text = Array.Find(Directory.GetFiles(modsDir), file => file.EndsWith(resourceName));
                        if (text == null) return null;
                    }
                    Console.WriteLine("Loaded external Dll: " + text);
                    return (LoadedAssembilies[baseName] = Assembly.LoadFrom(text));
                }
                using (Stream stream = Terraria.TerrariaAsm.GetManifestResourceStream(text))
                {
                    byte[] array = new byte[stream.Length];
                    stream.Read(array, 0, array.Length);
                    Output.Print("Loaded internal Dll: " + resourceName);
                    return (LoadedAssembilies[baseName] = Assembly.Load(array));
                }
            };

            // code for testing SDL instead of moving over to Linux every time
            try {
                Accessibility.Native.LoadLibrary(@"C:\SDL3\bin32\SDL3.dll");
            } catch (Exception) { }
            try {
                Accessibility.Native.LoadLibrary(@"C:\SDL2\SDL2.dll");
            }
            catch (Exception) { }

            // run this before almost anything so we can keep track of stuff
            //   allows Output.Print() to be used
            //   enabled hook for GlobalHooks.RawHooks.ConsoleOutput() so we can run code whenever console it printed to
            Output.CustomWriter.InitOutputWriter();

            try {
                OS.CheckOS();
            }
            catch (TypeLoadException t) {
                Accessibility.Show($"{t.Message} ({t.GetType()}, {t.InnerException})\nType: {t.TypeName}\n\n{t.StackTrace}");
            }

            // check virtual launch
            if (args.Length == 2 && args[0] == "virtual") {
                cModLoaderInstaller.InitVirtualLaunch(args[1]);
                Output.Print("Enabled Virtual Launch. " + args[1]);
            }

            if (!cModLoaderInstaller.CheckState(out bool startTerraria)) {
                return;
            }

            if (!startTerraria) {
                if (!Debug) Output.ShutDownConsole(true);
                cModLauncher.Open();
            } else {
                cModLoaderConfig.LoadConfig();
                // try stops working once Terraria starts
                try {
                    var c = ModLoader.LoadDlls(); // Load Dlls so patches can work
                    if (c) Output.Print("Mods Loaded");
                    // load like this only if its a virtual launch
                    LoadTerraria(); // load terraria before its loaded normally
                    LaunchTerraria(); // launch terraria
                }
                catch (Exception e) {
                    Accessibility.Show(e.ToString());
                }
            }
            Environment.Exit(0); // Force full shutdown just in case
        }
        
        public static Assembly LoadTerraria() {
            if (ResolvingTerrariaExe) {
                Output.Print("Error while resolving Terraria, a loop was entered, likely due to code referencing the assembly before it is loaded.");
                Output.Print("If you are developing a mod make sure your patch function's parameters do not contain any terraria classes, make sure they are all 'object' types.");
                return null;
            }
            ResolvingTerrariaExe = true;

            if (cModLoaderInstaller.VirtualLaunch)
                Path.LoadedTerrariaDirectory = Path.VirtualTerrariaPath;
            else {
                Path.LoadedTerrariaDirectory = Path.GetFromDirectory(Path.RealTerrariaPathEnding, out bool found);
                if (!found) {
                    Accessibility.ShowMessageBox("Terraria was missing, RealTerraria.exe does not exist, reinstall cModLoader and try again.", "Missing Terraria", new string[] { "Ok"}, Accessibility.MessageBoxIcon.Error);
                    return null;
                }
            }

            // get version and pre data
            Terraria.AssembilyInit(Path.LoadedTerrariaDirectory);
            // load patches
            ModLoader.LoadPreMods();
            Output.Print("Patches Loaded");

            // patch and load terraria
            var asm = cModPatch.LoadAndPatchTerraria(Path.LoadedTerrariaDirectory);
            TerrariaAsmBytes = asm;
            var loadedAsm = Assembly.Load(asm);
            Terraria.AssembilyPost(loadedAsm);

            // finalize patch
            cModPatch.FinishPatch();
            Output.Print("Finalized Patches");

            ResolvingTerrariaExe = false;
            LoadedAssembilies["Terraria"] = loadedAsm;
            if (!ModLoader.LoadedPatches) {
                Output.Error($"Error: Terraria was loaded before patches were, this likely means a mod incorrectly references the terraria assembly.");
            }
            Output.Print("Terraria.exe Loaded");
            return loadedAsm;
        }

        public static void LaunchTerraria()
        {
            Output.Print("Current Path: " + Path.GetCurrentPath());
            if (!cModLoaderInstaller.VirtualLaunch) // this cant be used in virtual mode
                Output.Print("Terraria: " + Path.LoadedTerrariaDirectory);
            if (cModLoaderInstaller.VirtualLaunch)
                Output.Print("Virtual Path: " + Path.VirtualTerrariaPath);
            Output.Print("Starting Terraria...");

            if (!Debug) Output.ShutDownConsole(true);

            cModLoaderPre.allowIdle = true;
            Terraria.StartGame();
        }
        
    }
    public class cModLoaderInstaller
    {

        public static bool VirtualLaunch = false;
        internal static void InitVirtualLaunch(string virtualPath) {
            VirtualLaunch = true;
            Path.VirtualTerrariaPath = virtualPath;
        }

        // checks the status of the exe this determines weather it should install, uninstall or run terraria
        //  uses startTerraria to determine if it should start terraria
        //  returns if the program should quit
        internal static bool CheckState(out bool startTerraria) {
            startTerraria = false;
            var curPath = Path.GetCurrentPath();
            if (curPath == "") {
                DisplayError("Failed to find self execution path.");
                return false;
            }
            if (curPath.EndsWith(Path.TerrariaPathEnding)) { // launch terraria normally
                startTerraria = true;
                return true;
            } else { // Launch cModLauncher
                if (VirtualLaunch) {
                    startTerraria = true;
                }
                return true;
            }
        }

        // Uninstall and Install are safe for any virtual path use
        // technically its possible they are ran with invalid parameters but it would be because the user is trash

        internal static bool Uninstall(bool keepMods = true, string installPath = null)
        {
            string executionPath = Path.GetCurrentPath();
            string terrariaPath = installPath;
            // check terraria exist
            if (terrariaPath == null && !Path.FindTerrariaGlobally(out terrariaPath, out _))
                return DisplayError("Could not find the Terraria file within the normal steam directories.\nMake sure Terraria is properly installed.");
            Print("Found Terraria!");
            string cModLoaderPath = terrariaPath.Substring(0, terrariaPath.LastIndexOf("\\") + 1) + "Terraria.exe";
            string cModLoaderPath2 = terrariaPath.Substring(0, terrariaPath.LastIndexOf("\\") + 1) + "cModLoader.exe";
            string RealTerrariaPath = terrariaPath.Substring(0, terrariaPath.LastIndexOf("\\") + 1) + "RealTerraria.exe";
            if (!File.Exists(RealTerrariaPath)) {
                return DisplayError("cModLoader was not installed.");
            }
            string cModLoaderFolderPath = terrariaPath.Substring(0, terrariaPath.LastIndexOf("\\") + 1) + "cModLoader\\";
            if (!keepMods && Directory.Exists(cModLoaderFolderPath)) {
                try {
                    Directory.Delete(cModLoaderFolderPath, true);
                    Print("Deleted Folder at " + cModLoaderFolderPath);
                } catch (Exception) {
                    return DisplayError("Could not remove cModLoader file directory.");
                }
            }
            try {
                File.Delete(cModLoaderPath);
                Print("Deleted File at " + cModLoaderPath);
            } catch (Exception) {
                return DisplayError("Failed to remove Terraria.exe.\nTo fix this, do it yourself then rename RealTerraria.exe to Terraria.exe or reinstall terraria.");
            }
            if (cModLoaderPath2 == Path.GetCurrentPath()) { // check if file is the current one running
                Print("cModLoader.exe can not be removed since its the programing running. It will remain in the terraria directory, this will not interfere with terraria and you may delete it if you wish.");
            } else {
                try {
                    File.Delete(cModLoaderPath2);
                    Print("Deleted File at " + cModLoaderPath2);
                } catch (Exception) {
                    return DisplayError("Failed to remove cModLoader.exe.\nTo fix this, do it yourself or reinstall terraria.");
                }
            }
            try {
                File.Move(RealTerrariaPath, cModLoaderPath);
                Print("Moved File " + RealTerrariaPath + " to " + cModLoaderPath);
            } catch (Exception) {
                return DisplayError("Failed to rename ReadTerraria.exe to Terraria.exe\nTo fix this, do it yourself or reinstall terraria.");
            }
            Print("Uninstallation Success!");
            return true;
        }
        internal static bool Install(string installPath = null) {

            string executionPath = Path.GetCurrentPath();
            string terrariaPath = installPath;
            // check terraria exist
            if (terrariaPath == null && !Path.FindTerrariaGlobally(out terrariaPath, out _))
                return DisplayError("Could not find the Terraria file within the normal steam directories.\nMake sure Terraria is properly installed.");
            Print("Found Terraria!");
            // TODO: check if the terraria running is running from 'terrariaPath'
            // check terraria is not running
            foreach (var proc in Process.GetProcessesByName("Terraria")) {
                // its possible this never resolves resulting in an infinite loop
                while (true) {
                    try {
                        for (int i = 0; i < proc.Modules.Count; i++)
                            if (proc.Modules[i].FileName == terrariaPath)
                                return DisplayError("Terraria was found running.\nClose Terraria before running this EXE.");
                    } catch (Exception) {
                        proc.Kill(); // >:)
                        Console.WriteLine("Unable to access proc.Modules");
                    }
                    Thread.Sleep(100);
                }
            }
            string basePath = terrariaPath.Substring(0, terrariaPath.LastIndexOf("\\") + 1);
            string RealTerrariaPath = basePath + "RealTerraria.exe";
            //string removerPath = basePath + "cModLoaderRemover.exe";
            string cModLoaderFolderPath = terrariaPath.Substring(0, terrariaPath.LastIndexOf("\\") + 1) + "cModLoader\\";
            string cModLoaderFolderTmpPath = terrariaPath.Substring(0, terrariaPath.LastIndexOf("\\") + 1) + "cModLoader_temp\\";
            string cModLoaderModsFolderPath = terrariaPath.Substring(0, terrariaPath.LastIndexOf("\\") + 1) + "cModLoader\\Mods";
            // check if terraria is named "RealTerraria", if so it means cModLoader was installed (or the user renamed it)
            if (File.Exists(RealTerrariaPath)) {
                var result = Accessibility.Show("cModLoader is already Initialized.\nDo you wish to update it?", "Already installed.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Yes) {
                    Print("Starting uninstall...");
                    Uninstall(true, terrariaPath);
                }
                else {
                    Print("Ok, installation aborted.");
                    return false;
                }
            }
            if (!Path.EnsureFolder(cModLoaderFolderPath, out bool folderExisted)) {
                if (Accessibility.Show($"Failed to create folder \"{cModLoaderFolderPath}\"\nDo you wish to continue? Installation does not require this folder.", "Create Folder Fail!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error) != DialogResult.Yes) {
                    Print("Ok, installation aborted.");
                    return false;
                }
            }
            Print((folderExisted ? "Skipping" : "Created") + " Folder at " + cModLoaderFolderPath);
            if (!Path.EnsureFolder(cModLoaderModsFolderPath, out folderExisted)) {
                if (Accessibility.Show($"Failed to create folder \"{cModLoaderModsFolderPath}\"\nDo you wish to continue? Installation does not require this folder.", "Create Folder Fail!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error) != DialogResult.Yes) {
                    Print("Ok, installation aborted.");
                    return false;
                }
            }
            Print((folderExisted ? "Skipping" : "Created") + " Folder at " + cModLoaderModsFolderPath);
            // string used for error printing
            string str = "Failed to rename terraria.";
            string str1 = terrariaPath;
            string str2 = RealTerrariaPath;
            string terrariaDirectory = terrariaPath.Substring(0, terrariaPath.LastIndexOf("\\") + 1);
            try {
                // rename terraria to RealTerraria
                File.Move(str1, str2);
                Print("Renamed " + str1 + " to " + str2);
                str = "Could not move the current executing exe file.";
                // copy the current executing exe to the terraria directory and rename it terraria.exe
                str1 = executionPath;
                str2 = terrariaDirectory + "Terraria.exe";
                File.Copy(str1, str2);
                Print("Copied " + str1 + " to " + str2);
                // copy the current executing exe to the terraria directory and rename it cModLoader.exe this is for developering mods
                str2 = terrariaDirectory + "cModLoader.exe";
                if (str2 != Path.GetCurrentPath()) { // check if file is the current one running
                    File.Copy(str1, str2, true); // override incase it was left there after being uninstalled
                    Print("Copied " + str1 + " to " + str2);
                } else {
                    Print("Skipped Copying" + str1 + " to " + str2);
                }
            }
            catch (Exception e) {
                return DisplayError(str + "\n" + e.Message);
            }

            // if through proton or wine (ideally we check the installing path to see what versions it is so we can install Linux files into windows for wsl or something)
            if (OS.IsLinux) {
                try {
                    foreach (var name in Assembly.GetExecutingAssembly().GetManifestResourceNames()) {
                        if (name == "cModLoader.Resources.Terraria.exe.config") {
                            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name)) {
                                byte[] array = new byte[stream.Length];
                                stream.Read(array, 0, array.Length);
                                File.WriteAllBytes(terrariaDirectory + "Terraria.exe.config", array);
                                Print("Created " + terrariaDirectory + "Terraria.exe.config");
                            }
                        }
                    }
                }
                catch (Exception e) {
                    return DisplayError("Failed to save internal Terraria.exe.config file.\n" + e.Message);
                }
            }

            Accessibility.ShowMessageBox("Installation complete!", "Success", new string[] { "Ok" }, Accessibility.MessageBoxIcon.Information);
            return true;
        }

        /// <summary> Launches a new terraria instance using <see cref="Process.Start()"/> (This uses <see cref="Path.FindTerrariaGlobally(out string, out string)"/> to get the path so it could be a cModLoader instance) </summary>
        public static void LaunchNewTerraria() {
            Path.FindTerrariaGlobally(out string exe, out _);
            var psi = new ProcessStartInfo {
                FileName = exe,
                WorkingDirectory = exe.Substring(0, exe.LastIndexOf("\\")),
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private static void Print(string str, ConsoleColor colour = ConsoleColor.White) {
            Console.ForegroundColor = colour;
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary> returns false to make cleaner code as this is almost always used right before `return false;` </summary>
        private static bool DisplayError(string error) {
            Print("Error: " + error, ConsoleColor.Red);
            var result = Accessibility.Show("Error: \n" + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

    }

    public class Output
    {
        /// <summary> Used to close console and its window,  </summary>
        internal static void ShutDownConsole(bool kill, int time = 0) {
            CustomWriter.KillOriginal();
            // if no kernal32 exists this will fail so don't do it
            if (OS.CheckKernel32) {
                // wait to close console so the user knows somethings happening
                new Thread(() => {
                    Thread.Sleep(time); // wait before killing the console
                    // try because this may fail
                    try {
                        IntPtr consoleWindow = Accessibility.Native.GetConsoleWindow();
                        Accessibility.Native.FreeConsole();
                        if (kill && consoleWindow != IntPtr.Zero)
                            Accessibility.Native.PostMessage(consoleWindow, 0x0010 /*WM_CLOSE*/, IntPtr.Zero, IntPtr.Zero);
                    }
                    catch (Exception e) {
                        Output.Print($"ShutDownConsole Error, {e.Message} ({e.GetType()})");
                    }
                }).Start();
            } else {
                Output.Print($"Skipping shutting down console, no Kernel32");
            }
        }

        /// <summary> Prints console text if <see cref="cModLoaderInitializer.Debug"/> is <see langword="true"/>. </summary>
        public static void Debug(object obj) {
            if (cModLoaderInitializer.Debug) Console.WriteLine(obj);
        }
        /// <summary> Prints console text. </summary>
        public static void Print(object obj) {
            Console.WriteLine(obj);
        }
        /// <summary> Prints red console text this includes Terraria's formatting so "[c/FF0000:...]" is added. </summary>
        public static void Error(object obj) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(obj);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public class CustomWriter : TextWriter
        {
            public static TextWriter OriginalOut;
            public static TextWriter OriginalErr;
            public static bool killedOriginal;
            public static CustomWriter ConsoleOutput;
            public static event Action<char> OnOutput;
            public static void InitOutputWriter() {
                OriginalOut = Console.Out;
                OriginalErr = Console.Error;
                ConsoleOutput = new CustomWriter();
                Console.SetOut(ConsoleOutput);
                Console.SetError(new ErrorRedirectWriter(ConsoleOutput));
                Console.SetIn(TextReader.Null); // idk what to do for this, just leave it i guess
            }
            public static void KillOriginal() {
                OriginalOut = Null;
                OriginalErr = Null;
                killedOriginal = true;
            }
            public List<string> Output = new List<string>();
            public override Encoding Encoding => Encoding.UTF8;
            public CustomWriter() { Output.Add(""); }
            // called from Console.Writeline, the base calls Write(char value) which is the lowest level
            public override void Write(char[] buffer, int index, int count) {
                base.Write(buffer, index, count);
                string str = "";
                // the base already makes sure this is safe
                for (int i = 0; i < count; i++) {
                    str += buffer[index + i];
                }
                GlobalHooks.RawHooks.ConsoleOutput(str);
            }
            // this is the lowest level write
            public override void Write(char value) {
                if (!killedOriginal) OriginalOut.Write(value);
                if (value == '\n') {
                    // MessageBox.Show(Output[Output.Count - 1]); // for debugging
                    Output.Add("");
                }
                else
                    Output[Output.Count - 1] += value;
                OnOutput?.Invoke(value);
            }
            public string[] GetOutput(int maxLines = 30) {
                int start = Math.Max(0, Output.Count - maxLines);
                return Output.GetRange(start, Output.Count - start).ToArray();
            }

            public void Clear() {
                Output.Clear();
                Output.Add("");
            }

            // error redirect to add colouring for in game on screen console
            private class ErrorRedirectWriter : TextWriter
            {
                public CustomWriter errorOutput;
                public ErrorRedirectWriter(CustomWriter target) => errorOutput = target;
                public override Encoding Encoding => Encoding.UTF8;
                public override void Write(char value) { if (!killedOriginal) OriginalErr.Write(value); errorOutput.Write(value); }
                public override void Write(string value) {
                    if (!killedOriginal) OriginalErr.Write(value);
                    foreach (var c in value)
                        errorOutput.Write(c);
                }

                public override void WriteLine(string value) // terraria colour formatting
                {
                    if (!killedOriginal) OriginalErr.WriteLine(value);
                    Write("[c/FF0000:");
                    Write(value);
                    Write("]");
                    Write('\n');
                }
            }
        }

    }

    public class Accessibility
    {
        public class Native {
            [DllImport("kernel32", SetLastError = true)] public static extern IntPtr LoadLibrary(string lpFileName);
            [DllImport("kernel32", SetLastError = true)] public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
            [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)] public static extern IntPtr GetModuleHandle(string lpModuleName);
            [DllImport("ntdll.dll")] public static extern int wine_get_version();
            [DllImport("kernel32", SetLastError = true)] public static extern bool FreeConsole();
            [DllImport("kernel32", SetLastError = true)] public static extern IntPtr GetConsoleWindow();
            [DllImport("user32", SetLastError = true)] public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        }

        public static bool NeedsAccessibility = false;
        public static void CheckAccessibility() {
            // TODO: do something?
        }

        public enum WindowType { Default, Form, SDL3, SDL2 }

        /// <summary> Mainly used for testing. </summary>
        public static WindowType PreferWindow = WindowType.Default;
        /// <summary> Should cModLoader use SDL. Checks (if it is running on not windows and <see cref="PreferWindow"/> is not forms) or (<see cref="PreferWindow"/> is SDL3 or SDL3). </summary>
        public static bool ShouldUseSDL => (!OS.PlatformWindows && PreferWindow != WindowType.Form) || (PreferWindow == WindowType.SDL3 || PreferWindow == WindowType.SDL2);

        public class SDL3 {
            static SDL3() {
                // could fail if touching SDL3 when not active
                try {
                    FNA.FNA_SDL3.SDL_Init(0);
                }
                catch (Exception) { }
            }
            // needed because otherwise FNA requires a FNA.FNA_SDL3.SDL_FRect instance, which means you cant pass NULL
            [DllImport("SDL3", CallingConvention = CallingConvention.Cdecl)] public static extern int SDL_RenderTexture(IntPtr renderer, IntPtr texture, IntPtr src, IntPtr dst);

            /// <summary> This is a hand-done conversion so things that were not obvious was convert to <see cref="FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN"/></summary>
            public static FNA.FNA_SDL3.SDL_Scancode KeysToSDL3ScanCode(Window.Keys key) {
                switch (key) {
                    case Window.Keys.Cancel: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_CANCEL;
                    case Window.Keys.Tab: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_TAB;
                    case Window.Keys.Clear: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_CLEAR;
                    case Window.Keys.Return: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_RETURN;
                    case Window.Keys.ShiftKey: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_LSHIFT | FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_RSHIFT;
                    case Window.Keys.ControlKey: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_LCTRL | FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_RCTRL;
                    case Window.Keys.Menu: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_MENU;
                    case Window.Keys.Pause: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_MEDIA_PAUSE;
                    case Window.Keys.Escape: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_ESCAPE;
                    case Window.Keys.Space: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_SPACE;
                    case Window.Keys.Prior: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_PERIOD;
                    case Window.Keys.End: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_END;
                    case Window.Keys.Home: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_HOME;
                    case Window.Keys.Left: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_LEFT;
                    case Window.Keys.Up: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UP;
                    case Window.Keys.Right: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_RIGHT;
                    case Window.Keys.Down: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_DOWN;
                    case Window.Keys.Select: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_SELECT;
                    case Window.Keys.Print: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_AC_PRINT;
                    case Window.Keys.Execute: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_EXECUTE;
                    case Window.Keys.Insert: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_INSERT;
                    case Window.Keys.Delete: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_DELETE;
                    case Window.Keys.Help: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_HELP;
                    case Window.Keys.D0: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_0;
                    case Window.Keys.D1: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_1;
                    case Window.Keys.D2: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_2;
                    case Window.Keys.D3: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_3;
                    case Window.Keys.D4: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_4;
                    case Window.Keys.D5: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_5;
                    case Window.Keys.D6: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_6;
                    case Window.Keys.D7: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_7;
                    case Window.Keys.D8: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_8;
                    case Window.Keys.D9: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_9;
                    case Window.Keys.A: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_A;
                    case Window.Keys.B: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_B;
                    case Window.Keys.C: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_C;
                    case Window.Keys.D: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_D;
                    case Window.Keys.E: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_E;
                    case Window.Keys.F: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_F;
                    case Window.Keys.G: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_G;
                    case Window.Keys.H: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_H;
                    case Window.Keys.I: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_I;
                    case Window.Keys.J: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_J;
                    case Window.Keys.K: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_K;
                    case Window.Keys.L: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_L;
                    case Window.Keys.M: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_M;
                    case Window.Keys.N: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_N;
                    case Window.Keys.O: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_O;
                    case Window.Keys.P: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_P;
                    case Window.Keys.Q: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_Q;
                    case Window.Keys.R: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_R;
                    case Window.Keys.S: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_S;
                    case Window.Keys.T: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_T;
                    case Window.Keys.U: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_U;
                    case Window.Keys.V: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_V;
                    case Window.Keys.W: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_W;
                    case Window.Keys.X: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_X;
                    case Window.Keys.Y: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_Y;
                    case Window.Keys.Z: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_Z;
                    case Window.Keys.Sleep: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_SLEEP;
                    case Window.Keys.Separator: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_SEPARATOR;
                    case Window.Keys.F1: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_F1;
                    case Window.Keys.F2: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_F2;
                    case Window.Keys.F3: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_F3;
                    case Window.Keys.F4: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_F4;
                    case Window.Keys.F5: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_F5;
                    case Window.Keys.F6: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_F6;
                    case Window.Keys.F7: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_F7;
                    case Window.Keys.F8: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_F8;
                    case Window.Keys.F9: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_F9;
                    case Window.Keys.F10: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_F10;
                    case Window.Keys.F11: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_F11;
                    case Window.Keys.F12: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_F12;
                    case Window.Keys.LShiftKey: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_LSHIFT;
                    case Window.Keys.RShiftKey: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_RSHIFT;
                    case Window.Keys.LControlKey: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_LCTRL;
                    case Window.Keys.RControlKey: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_RCTRL;
                    case Window.Keys.VolumeMute: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_MUTE;
                    case Window.Keys.VolumeDown: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_VOLUMEDOWN;
                    case Window.Keys.VolumeUp: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_VOLUMEUP;
                    case Window.Keys.MediaNextTrack: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_MEDIA_NEXT_TRACK;
                    case Window.Keys.MediaPreviousTrack: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_MEDIA_PREVIOUS_TRACK;
                    case Window.Keys.MediaStop: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_MEDIA_STOP;
                    case Window.Keys.MediaPlayPause: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_MEDIA_PLAY_PAUSE;
                    case Window.Keys.SelectMedia: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_MEDIA_SELECT;
                    case Window.Keys.Shift: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_LSHIFT | FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_RSHIFT;
                    case Window.Keys.Control: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_LCTRL | FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_RCTRL;
                    case Window.Keys.Alt: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_LALT | FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_RALT;

                    case Window.Keys.KeyCode: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Modifiers: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.None: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.LButton: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.RButton: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.MButton: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.XButton1: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.XButton2: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Back: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.LineFeed: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Capital: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.KanaMode: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.JunjaMode: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.FinalMode: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.HanjaMode: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.IMEConvert: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.IMENonconvert: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.IMEAccept: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.IMEModeChange: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Next: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Snapshot: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.LWin: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.RWin: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Apps: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.NumPad0: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.NumPad1: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.NumPad2: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.NumPad3: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.NumPad4: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.NumPad5: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.NumPad6: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.NumPad7: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.NumPad8: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.NumPad9: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Multiply: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Add: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Subtract: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Decimal: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Divide: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.F13: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.F14: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.F15: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.F16: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.F17: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.F18: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.F19: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.F20: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.F21: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.F22: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.F23: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.F24: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.NumLock: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Scroll: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.LMenu: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.RMenu: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.BrowserBack: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.BrowserForward: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.BrowserRefresh: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.BrowserStop: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.BrowserSearch: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.BrowserFavorites: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.BrowserHome: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.LaunchMail: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.LaunchApplication1: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.LaunchApplication2: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.OemSemicolon: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Oemplus: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Oemcomma: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.OemMinus: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.OemPeriod: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.OemQuestion: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Oemtilde: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.OemOpenBrackets: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.OemPipe: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.OemCloseBrackets: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.OemQuotes: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Oem8: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.OemBackslash: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.ProcessKey: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Packet:return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Attn: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Crsel: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Exsel: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.EraseEof: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Play: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Zoom: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.NoName: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.Pa1: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                    case Window.Keys.OemClear: return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
                }
                return FNA.FNA_SDL3.SDL_Scancode.SDL_SCANCODE_UNKNOWN;
            }

            /// <summary> An object used to create windows with SDL. It has nothing to do with SDL's actual SDL_Window.<br/>This is drawn using software, because otherwise it interferes with Terraria's rendering. </summary>
            public class SDL3_Window : IDisposable, Window.IWindow {

                private bool disposed = false;
                private Dictionary<string, IntPtr> allocations = new Dictionary<string, IntPtr>();
                public IntPtr window = IntPtr.Zero;
                public IntPtr renderer = IntPtr.Zero;
                private FNA.FNA_SDL3.SDL_FRect rectCache;
                private byte rCache = 0;
                private byte gCache = 0;
                private byte bCache = 0;
                private byte aCache = 0;
                private IntPtr fnaGLContext = IntPtr.Zero;
                private IntPtr fnaGLWindow = IntPtr.Zero;

                public bool ShouldClose = false;
                public bool Focused = true;
                public bool MouseOver = false;

                public void Open(Window.Window windowComponent) {
                    CreateWindow(windowComponent.Title);
                    Set_WindowSize(windowComponent.StartWidth, windowComponent.StartHeight);
                    CenterWindow();
                }
                public void Update(Window.Window windowComponent) {
                    var windowId = FNA.FNA_SDL3.SDL_GetWindowID(window);
                    while (FNA.FNA_SDL3.SDL_PollEvent(out FNA.FNA_SDL3.SDL_Event ev)) {
                        var v = ev.type;
                        var type = (FNA.FNA_SDL3.SDL_EventType)ev.type;
                        if (ev.window.windowID == windowId) {
                            Console.WriteLine($"0x{windowId.ToString("X8")}: Event: {type} ({v})");
                            if (type == FNA.FNA_SDL3.SDL_EventType.SDL_EVENT_WINDOW_FOCUS_GAINED) Focused = true;
                            if (type == FNA.FNA_SDL3.SDL_EventType.SDL_EVENT_WINDOW_FOCUS_LOST) Focused = false;
                            if (type == FNA.FNA_SDL3.SDL_EventType.SDL_EVENT_WINDOW_MOUSE_ENTER) MouseOver = true;
                            if (type == FNA.FNA_SDL3.SDL_EventType.SDL_EVENT_WINDOW_MOUSE_LEAVE) MouseOver = false;
                            if (type == FNA.FNA_SDL3.SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED) windowComponent.Close();
                            if (type == FNA.FNA_SDL3.SDL_EventType.SDL_EVENT_MOUSE_WHEEL) {
                                windowComponent.curScrollX = ev.wheel.x;
                                windowComponent.curScrollY = ev.wheel.y;
                            }
                        } else {
                            Console.WriteLine($"0x{windowId.ToString("X8")}: Event miss: {type} ({v})");
                        }
                    }
                    
                    windowComponent.DefaultDraw();
                    Draw_Finalize();

                    windowComponent.curScrollX = 0f;
                    windowComponent.curScrollY = 0f;
                }
                public void Close(Window.Window windowComponent) {
                    this.Dispose();
                }

                /// <summary> Create an instance of this <see cref="SDL3_Window"/> </summary>
                public SDL3_Window() {
                    disposed = false;
                }
                /// <summary> Calls <see cref="Dispose"/> </summary>
                ~SDL3_Window() {
                    Dispose();
                }
                /// <summary> Frees all pointers </summary>
                public void Dispose() {
                    if (disposed) return;
                    disposed = true;
                    foreach (var val in allocations) {
                        if (val.Value != IntPtr.Zero) {
                            Marshal.FreeHGlobal(val.Value);
                        }
                    }
                    if (renderer != IntPtr.Zero) {
                        FNA.FNA_SDL3.SDL_DestroyRenderer(renderer);
                        renderer = IntPtr.Zero;
                    }
                    if (window != IntPtr.Zero) {
                        FNA.FNA_SDL3.SDL_DestroyWindow(window);
                        window = IntPtr.Zero;
                    }
                    if (fnaGLContext != IntPtr.Zero) {
                        FNA.FNA_SDL3.SDL_GL_MakeCurrent(fnaGLWindow, fnaGLContext);
                    }
                }

                /// <summary> Sets a pointer to be freed when the instance is disposed </summary>
                public IntPtr SetAlloc(IntPtr ptr, string name) {
                    if (allocations.ContainsKey(name)) {
                        throw new Exception("A pointer with name \"" + name + "\" already exists.");
                    }
                    allocations.Add(name, ptr);
                    return ptr;
                }
                /// <summary> Frees the previous pointer and replaces it</summary>
                public IntPtr ReAlloc(IntPtr ptr, string name) {
                    Marshal.FreeHGlobal(allocations[name]);
                    allocations[name] = ptr;
                    return ptr;
                }

                /// <summary> Creates an SDL window with a title and a renderer </summary>
                public void CreateWindow(string title) {
                    if (window != IntPtr.Zero) throw new Exception("Can not create new window from the same instance that already has a window");
                    fnaGLContext = FNA.FNA_SDL3.SDL_GL_GetCurrentContext();
                    fnaGLWindow = FNA.FNA_SDL3.SDL_GL_GetCurrentWindow();
                    window = FNA.FNA_SDL3.SDL_CreateWindow(title, 300, 100, (FNA.FNA_SDL3.SDL_WindowFlags)0);
                    renderer = FNA.FNA_SDL3.SDL_CreateRenderer(window, null);
                    FNA.FNA_SDL3.SDL_SetWindowPosition(window, 100, 100);
                }
                /// <summary> Centers the window on screen </summary>
                public void CenterWindow() {
                    if (window == IntPtr.Zero) throw new Exception("Can not modify window properties if it does not exist");
                    //FNA.FNA_SDL3.SDL_SetWindowPosition(window, (int)0x2FFF0000u, (int)0x2FFF0000u);
                    FNA.FNA_SDL3.SDL_SetWindowPosition(window, 100, 100);
                }
                /// <summary> Outputs draws elements to screen </summary>
                public void Draw_Finalize() {
                    if (renderer == IntPtr.Zero) throw new Exception("Can not draw graphics without a renderer");
                    FNA.FNA_SDL3.SDL_RenderPresent(renderer);
                }
                
                // Drawers

                /// <summary> Clears the renderer </summary>
                public void Draw_SetDrawColour(byte r, byte g, byte b, byte a) {
                    if (renderer == IntPtr.Zero) throw new Exception("Can not draw graphics without a renderer");
                    rCache = r;
                    gCache = g;
                    bCache = b;
                    aCache = a;
                    FNA.FNA_SDL3.SDL_SetRenderDrawColor(renderer, r, g, b, a);
                }
                /// <summary> Clears the renderer </summary>
                public void Draw_Clear() {
                    if (renderer == IntPtr.Zero) throw new Exception("Can not draw graphics without a renderer");
                    FNA.FNA_SDL3.SDL_RenderClear(renderer);
                }
                /// <summary> Draws a rectangle outline </summary>
                public void Draw_Rect(int x, int y, int w, int h)
                {
                    if (renderer == IntPtr.Zero) throw new Exception("Can not draw graphics without a renderer");
                    rectCache.x = x;
                    rectCache.y = y;
                    rectCache.w = w;
                    rectCache.h = h;
                    FNA.FNA_SDL3.SDL_RenderRect(renderer, ref rectCache);
                }
                /// <summary> Draws a filled rectangle </summary>
                public void Draw_FillRect(int x, int y, int w, int h) {
                    if (renderer == IntPtr.Zero) throw new Exception("Can not draw graphics without a renderer");
                    rectCache.x = x;
                    rectCache.y = y;
                    rectCache.w = w;
                    rectCache.h = h;
                    FNA.FNA_SDL3.SDL_RenderFillRect(renderer, ref rectCache);
                }
                public void Draw_Text(int x, int y, float scale, string str, float alignX = 0f, float alignY = 0f) {
                    var lines = str.Split('\n');
                    var maxLineLen = 0;
                    foreach (var line in lines) {
                        maxLineLen = Math.Max(maxLineLen, line.Length);
                    }
                    FNA.FNA_SDL3.SDL_SetRenderScale(renderer, scale, scale);
                    var size = 8;
                    var w = size * maxLineLen * scale;
                    var h = size * scale;
                    var x2 = x - (w * alignX);
                    var y2 = y - (h * alignY);
                    int yPos = 0;
                    foreach (var line in lines) {
                        FNA.FNA_SDL3.SDL_RenderDebugText(renderer, (int)(x2 / scale), (int)(y2 / scale) + yPos, line);
                        yPos += size + 2;
                    }
                    FNA.FNA_SDL3.SDL_SetRenderScale(renderer, 1f, 1f);
                }
                public void Draw_DefaultIcon(byte[] iconData, int x, int y) {
                    unsafe {
                        fixed (byte* ptr = iconData) {
                            var surface = FNA.FNA_SDL3.SDL_CreateSurfaceFrom(32, 32, FNA.FNA_SDL3.SDL_PixelFormat.SDL_PIXELFORMAT_BGRA32, (IntPtr)ptr, 32 * 4);
                            IntPtr texture = FNA.FNA_SDL3.SDL_CreateTextureFromSurface(renderer, surface);
                            FNA.FNA_SDL3.SDL_FRect r = new FNA.FNA_SDL3.SDL_FRect() { x = x, y = y, w = 32, h = 32 };
                            SDL_RenderTexture(renderer, texture, IntPtr.Zero, (IntPtr)(void*)&r);
                            FNA.FNA_SDL3.SDL_DestroyTexture(texture);
                            FNA.FNA_SDL3.SDL_DestroySurface(surface);
                        }
                    }
                }

                // setters

                public void Set_Resizable(bool set) {
                    if (window == IntPtr.Zero) throw new Exception("Can not modify window properties if it does not exist");
                    FNA.FNA_SDL3.SDL_SetWindowResizable(window, set);
                }
                public void Set_Title(string title) {
                    if (window == IntPtr.Zero) throw new Exception("Can not modify window properties if it does not exist");
                    FNA.FNA_SDL3.SDL_SetWindowTitle(window, title);
                }
                public void Set_WindowSize(int w, int h) {
                    FNA.FNA_SDL3.SDL_SetWindowSize(window, w, h);
                }
                public void Set_WindowIcon(byte[] iconData) {
                    unsafe {
                        fixed (byte* ptr = iconData) {
                            var surface = FNA.FNA_SDL3.SDL_CreateSurfaceFrom(32, 32, FNA.FNA_SDL3.SDL_PixelFormat.SDL_PIXELFORMAT_BGRA32, (IntPtr)ptr, 32 * 4);
                            FNA.FNA_SDL3.SDL_SetWindowIcon(window, surface);
                            FNA.FNA_SDL3.SDL_DestroySurface(surface);
                        }
                    }
                }

                // getters
                public void Get_MousePos(out int x, out int y) {
                    FNA.FNA_SDL3.SDL_GetGlobalMouseState(out float x1, out float y1);
                    FNA.FNA_SDL3.SDL_GetWindowPosition(window, out int x2, out int y2);
                    x = (int)(x1 - x2);
                    y = (int)(y1 - y2);
                }
                public void Get_MouseDown(out bool L, out bool R, out bool M) {
                    var flags = FNA.FNA_SDL3.SDL_GetMouseState(out float _x, out float _y);
                    L = (int)(flags & FNA.FNA_SDL3.SDL_MouseButtonFlags.SDL_BUTTON_LMASK) > 0;
                    R = (int)(flags & FNA.FNA_SDL3.SDL_MouseButtonFlags.SDL_BUTTON_RMASK) > 0;
                    M = (int)(flags & FNA.FNA_SDL3.SDL_MouseButtonFlags.SDL_BUTTON_MMASK) > 0;
                }
                public void Get_WindowSize(out int w, out int h) {
                    FNA.FNA_SDL3.SDL_GetWindowSize(window, out w, out h);
                }

                public bool Get_KeyIsDown(Window.Keys key) {
                    IntPtr state = FNA.FNA_SDL3.SDL_GetKeyboardState(out int numkeys);
                    byte[] keyStates = new byte[numkeys];
                    Marshal.Copy(state, keyStates, 0, numkeys);
                    return keyStates[(int)KeysToSDL3ScanCode(key)] != 0;
                }

                public void Get_Focused(out bool isFocused) => isFocused = Focused && MouseOver;
            }
        }

        /// <summary> Icons for drawing. cModLoader is included but the rest are window icons, so idk if im aloud to use them </summary>
        public enum MessageBoxIcon {
            /// <summary> nothing </summary>
            None,
            /// <summary> The default windows application icon </summary>
            Application,
            /// <summary> White "X" inside of a red circle. </summary>
            Error,
            /// <summary> White "i" inside of a blue circle. </summary>
            Information,
            /// <summary> White "?" inside of a blue circle. </summary>
            Question,
            /// <summary> This is not used in <see cref="System.Windows.Forms.MessageBoxIcon"/> but exist so i figured id include it. Shield icon, i think the same one as the admin icon. </summary>
            Shield,
            /// <summary> Black "!" inside of a orange/yellow triangle. </summary>
            Warning,
            /// <summary> The cModLoader icon, added because i can. </summary>
            cModLoader
        }

        /// <summary>
        /// <para>Works the same as <see cref="MessageBox.Show(string, string, System.Windows.Forms.MessageBoxButtons, System.Windows.Forms.MessageBoxIcon)"/></para>
        /// <para>WARNING: Creates a thread to work properly. See <see cref="Window.Window.Open(bool, bool)"/> for more info on why. </para>
        /// </summary>
        public static DialogResult Show(string message, string title = "", System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon icon = System.Windows.Forms.MessageBoxIcon.None) {
            string[] str = null;
            switch (buttons) {
                case MessageBoxButtons.OKCancel: str = new string[] { "Ok", "Cancel" }; break;
                case MessageBoxButtons.AbortRetryIgnore: str = new string[] { "Abort","Retry","Ignore" }; break;
                case MessageBoxButtons.YesNoCancel: str = new string[] { "Yes", "No", "Cancel" }; break;
                case MessageBoxButtons.YesNo: str = new string[] { "Yes", "No" }; break;
                case MessageBoxButtons.RetryCancel: str = new string[] { "Retry", "Cancel" }; break;
                default: str = new string[] { "Ok" }; break;
            }
            var _icon = MessageBoxIcon.None;
            switch (icon) {
                case System.Windows.Forms.MessageBoxIcon.Error: _icon = MessageBoxIcon.Error; break;
                case System.Windows.Forms.MessageBoxIcon.Question: _icon = MessageBoxIcon.Question; break;
                case System.Windows.Forms.MessageBoxIcon.Warning: _icon = MessageBoxIcon.Warning; break;
                case System.Windows.Forms.MessageBoxIcon.Information: _icon = MessageBoxIcon.Information; break;
            }
            
            var num = ShowMessageBox(message, title, str, _icon);
            DialogResult dialog = 0;
            switch (buttons) {
                case MessageBoxButtons.OKCancel:
                    switch (num) {
                        case -1: dialog = DialogResult.Cancel; break;
                        case 0: dialog = DialogResult.OK; break;
                        case 1: dialog = DialogResult.Cancel; break;
                    } break;
                case MessageBoxButtons.AbortRetryIgnore:
                    switch (num) {
                        case -1: dialog = DialogResult.Ignore; break;
                        case 0: dialog = DialogResult.Abort; break;
                        case 1: dialog = DialogResult.Retry; break;
                        case 2: dialog = DialogResult.Ignore; break;
                    } break;
                case MessageBoxButtons.YesNoCancel:
                    switch (num) {
                        case -1: dialog = DialogResult.Cancel; break;
                        case 0: dialog = DialogResult.Yes; break;
                        case 1: dialog = DialogResult.No; break;
                        case 2: dialog = DialogResult.Cancel; break;
                    } break;
                case MessageBoxButtons.YesNo:
                    switch (num) {
                        case -1: dialog = DialogResult.Cancel; break;
                        case 0: dialog = DialogResult.Yes; break;
                        case 1: dialog = DialogResult.No; break;
                    } break;
                case MessageBoxButtons.RetryCancel:
                    switch (num) {
                        case -1: dialog = DialogResult.Cancel; break;
                        case 0: dialog = DialogResult.Retry; break;
                        case 1: dialog = DialogResult.Cancel; break;
                    } break; 
            }
            return dialog;
        }

        /// <summary>
        /// <para>See <see cref="ShowMessageBox(string[], string, string[], MessageBoxIcon)"/></para>
        /// <para>WARNING: Creates a thread to work properly. See <see cref="Window.Window.Open(bool, bool)"/> for more info on why. </para>
        /// </summary>
        public static int ShowMessageBox(string message, string title, string[] buttonStrings, MessageBoxIcon icon = MessageBoxIcon.None) => ShowMessageBox(message.Split('\n'), title, buttonStrings, icon);
        /// <summary>
        /// <para>Displays a message box with more customization then the normal <see cref="MessageBox"/>, this was needed because Linux and macOS do not give the needed assemblies to use it.</para>
        /// <para>Works with SDL2 but is limited and may not work as you'd want.</para>
        /// <para>WARNING: Creates a thread to work properly. See <see cref="Window.Window.Open(bool, bool)"/> for more info on why. </para>
        /// <para><paramref name="blocking"/> was used for testing, keep as <see langword="true"/>.</para>
        /// <para><paramref name="blocking"/> was used for testing, keep as <see langword="true"/>.</para>
        /// </summary>
        public static int ShowMessageBox(string[] message, string title, string[] buttonStrings, MessageBoxIcon icon = MessageBoxIcon.None, bool blocking = true) {
            // SDL2 fallback
            if (PreferWindow == WindowType.SDL2 || (OS.IsSDL && OS.IsSDL2)) {
                FNA.FNA_SDL2.SDL_MessageBoxData data = new FNA.FNA_SDL2.SDL_MessageBoxData();
                data.window = IntPtr.Zero;
                data.title = title;
                data.message = string.Join("\n", message);
                data.numbuttons = buttonStrings.Length;
                List<FNA.FNA_SDL2.SDL_MessageBoxButtonData> btns = new List<FNA.FNA_SDL2.SDL_MessageBoxButtonData>();
                foreach (var item in buttonStrings) {
                    btns.Add(new FNA.FNA_SDL2.SDL_MessageBoxButtonData() {
                        buttonid = btns.Count,
                        text = item
                    });
                }
                data.buttons = btns.ToArray();
                data.colorScheme = null;
                FNA.FNA_SDL2.SDL_ShowMessageBox(ref data, out int buttonId);
                return buttonId;
            }

            int maxLen = 0;
            for (int i = 0; i < message.Length; i++) {
                if (message[i].Length > maxLen)
                    maxLen = message[i].Length;
            }

            int ReturnNumber = -1;
            int padding = 10;
            int TextX = icon == MessageBoxIcon.None ? padding : padding + 32 + padding;
            int btnHeight = 25;
            int _w = Math.Max((maxLen * 8) + TextX, 100) + (padding * 2);
            Window.Window win = new Window.Window(title, _w, ((message.Length - 1) * 14) + (padding * 4) + btnHeight);
            Window.Button[] buttons = new Window.Button[buttonStrings.Length];
            for (int i = 0; i < buttonStrings.Length; i++) {
                buttons[i] = new Window.Button(buttonStrings[i], 0, 0, Math.Max(100, buttonStrings[i].Length * 8 + 20), btnHeight);
                buttons[i].BackgroundColour = Color.DarkGray;
                buttons[i].ForgroundColour = Color.Black;
                int num = i;
                buttons[i].OnPress += () => { ReturnNumber = num; win.Close(); };
            }
            win.Resizeable = false;
            win.BackgroundColour = Color.Gray;
            //int updateCounter = 0;
            win.OnDraw += (isSDL) => {
                win.Draw_SetDrawColour(0, 0, 0, 255);
                win.Get_WindowSize(out int w, out int h);
                int y = padding;
                int x = w / 2;
                for (int i = 0; i < message.Length; i++){
                    win.Draw_Text(TextX, y, 1f, message[i]);
                    y += 14;
                }
                int x2 = w - padding;
                int y2 = h - padding;
                for (int i = buttons.Length - 1; i >= 0; i--) {
                    buttons[i].PosX = x2 - buttons[i].Width;
                    buttons[i].PosY = y2 - buttons[i].Height;
                    win.DrawElement(buttons[i]);
                    x2 = buttons[i].PosX - 10;
                }

                if (icon != MessageBoxIcon.None) {
                    win.Draw_DefaultIcon(icon, padding, padding);
                }
                // win.Get_MousePos(out int MouseX, out int MouseY);
                // win.Get_MouseDown(out bool MouseLDown, out _, out _);
                // win.Draw_SetDrawColour(190, 115, 255); // some arbitrary colour, purple-ish pink
                // win.Draw_Text(1, 1, 1f, $"{MouseX},{MouseY},{(MouseLDown.ToString().Substring(0, 1))},{updateCounter++}");

            };
            win.Open(blocking);

            return ReturnNumber;
        }

        private static string directoryCache = "";
        /// <summary>
        /// <para>Replacement for <see cref="System.Windows.Forms.FileDialog"/> (or whatever it is) that works on Windows and Linux (and probably macOS)</para>
        /// <para>Does not work with SDL2 on Linux. </para>
        /// <para>WARNING: Creates a thread to work properly. See <see cref="Window.Window.Open(bool, bool)"/> for more info on why. </para>
        /// </summary>
        public static string DirectorySearch(string title, out bool passed, string _defaultPath = "") {
            bool _passed = false;
            passed = false;

            int padding = 10;
            int btnHeight = 25;
            Window.Window win = new Window.Window(title, 800, 600);
            win.Resizeable = true;
            win.BackgroundColour = Color.Gray;
            win.Icon = MessageBoxIcon.Information;

            Window.Button windowBtn = new Window.Button("Native Window", 0, 0, 120, 25);
            windowBtn.BackgroundColour = Color.LightGray;
            windowBtn.ForgroundColour = Color.Black;

            Window.Button btn = new Window.Button("Select", 0, 0, 80, 25);
            btn.BackgroundColour = Color.LightGray;
            btn.ForgroundColour = Color.Black;

            Window.Button backBtn = new Window.Button("Back", 0, 0, 80, 25);
            backBtn.BackgroundColour = Color.LightGray;
            backBtn.ForgroundColour = Color.Black;

            Window.Button cancelBtn = new Window.Button("Cancel", 0, 0, 80, 25);
            cancelBtn.BackgroundColour = Color.LightGray;
            cancelBtn.ForgroundColour = Color.Black;

            var defaultPath = _defaultPath == "" ? (OS.PlatformLinux ? "/" : "C:\\") : _defaultPath;
            var currentPath = directoryCache == "" ? defaultPath : directoryCache;
            var selectedFile = defaultPath;

            List<string> allFiles = new List<string>();
            bool needsUpdate = true;

            int scroll = 0;
            bool wasDown = false;

            windowBtn.OnPress += () => {
                if (GetDefaultWindowsDirectorySearch(title, out string str, currentPath)) {
                    currentPath = str;
                    _passed = true;
                    win.Close();
                }
            };

            backBtn.OnPress += () => { 
                currentPath = Path.GoBack(currentPath);
                //if (currentPath == "") currentPath = defaultPath;
                needsUpdate = true;
            };
            btn.OnPress += () => {
                _passed = true;
                win.Close();
            };
            cancelBtn.OnPress += () => {
                selectedFile = defaultPath;
                win.Close();
            };
            win.OnDraw += (isSDL) => {
                float textScale = 1f;


                // get stuff
                win.Get_MousePos(out int mx, out int my);
                win.Get_WindowSize(out int windowW, out int windowH);
                win.Get_MouseDown(out bool Ldown, out _, out _);

                //// draw option for default windows window
                //if (!OS.PlatformLinux) {
                //    windowBtn.PosX = padding;
                //    windowBtn.PosY = windowH - padding - windowBtn.Height;
                //    // this does not work
                //    win.DrawElement(windowBtn);
                //}

                // draw cancel button
                cancelBtn.PosX = windowW - padding - backBtn.Width;
                cancelBtn.PosY = windowH - padding - btn.Height;
                win.DrawElement(cancelBtn);
                // draw select button
                btn.PosX = windowW - padding - btn.Width;
                btn.PosY = windowH - (padding * 2) - (btn.Height * 2);
                win.DrawElement(btn);
                // draw bottom path
                win.Draw_SetDrawColour(100, 100, 100);
                win.Draw_FillRect(padding, btn.PosY, btn.PosX - (padding * 2), 25);
                win.Draw_SetDrawColour(0, 0, 0);
                win.Draw_Rect(padding, btn.PosY, btn.PosX - (padding * 2), 25);
                win.Draw_Text(padding + 4, btn.PosY + 6, 1f, selectedFile);
                // draw back button
                backBtn.PosX = windowW - padding - backBtn.Width;
                backBtn.PosY = padding;
                win.DrawElement(backBtn);
                // draw top path
                win.Draw_SetDrawColour(100, 100, 100);
                win.Draw_FillRect(padding, backBtn.PosY, backBtn.PosX - (padding * 2), 25);
                win.Draw_SetDrawColour(0, 0, 0);
                win.Draw_Rect(padding, backBtn.PosY, backBtn.PosX - (padding * 2), 25);
                win.Draw_Text(padding + 4, backBtn.PosY + 6, 1f, currentPath);

                // draw scroll background
                win.Draw_SetDrawColour(100, 100, 100);
                var scrollBarX = windowW - padding - 20;
                var scrollBarY = backBtn.PosY + backBtn.Height + padding;
                var scrollBarH = btn.PosY - padding - scrollBarY;
                win.Draw_FillRect(scrollBarX, scrollBarY, 20, scrollBarH);

                // compute scroll
                if (win.curScrollY < 0) scroll += 1;
                if (win.curScrollY > 0) scroll -= 1;
                if (scroll < 0) scroll = 0;
                var scrollViewCount = (scrollBarH / (12 * textScale));
                var maxScroll = Math.Max(allFiles.Count - scrollViewCount, 1);
                if (allFiles.Count >= 1 && scroll > maxScroll) scroll = (int)(maxScroll);

                // draw scroll handle
                var scrollBarSize = (maxScroll == 0 ? scrollBarH : scrollBarH / (float)maxScroll);
                var scrollBarPos = (int)(scrollBarSize * scroll);
                if (mx >= scrollBarX && mx <= scrollBarX + 20 && my >= scrollBarY && my <= scrollBarY + scrollBarH) {
                    win.Draw_SetDrawColour(255, 255, 255);
                    if (Ldown) {
                        scroll = (int)((my - scrollBarY) / scrollBarSize);
                    }
                } else {
                    win.Draw_SetDrawColour(200, 200, 200);
                }
                win.Draw_FillRect(windowW - padding - 20, scrollBarY + scrollBarPos, 20, (int)scrollBarSize);

                int y = backBtn.PosY + backBtn.Height + padding;
                int x = padding * 2;
                bool printMore = false;

                win.Draw_SetDrawColour(255, 255, 255);
                if (scroll > 0) win.Draw_Text(x, y - 8, 1f, "^^^^^");

                var fileEndCount = Math.Min(scroll + scrollViewCount, allFiles.Count);
                printMore = scroll + scrollViewCount < allFiles.Count - 1;
                for (int i = scroll; i < fileEndCount; i++) {
                    var str = allFiles[i];
                    var name = Path.GetName(str.Substring(2));
                    name = name == "" ? str.Substring(2) : name; // if nothing is display set to raw value (used for drive names)
                    var w = (int)(name.Length * 8 * textScale);
                    var h = (int)(10 * textScale);
                    //win.Draw_Rect(x, y, w, h);
                    if (mx > x && mx < x + w && my > y && my < y + h) {
                        win.Draw_SetDrawColour(255, 255, 255);
                        if (!Ldown && wasDown) {
                            if (str[0] == 'F') {
                                selectedFile = str.Substring(2);
                            } else if (str[0] == 'D') {
                                currentPath = str.Substring(2);
                                needsUpdate = true;
                            }
                        }
                    } else if (str[0] == 'F'){
                        win.Draw_SetDrawColour(0, 0, 0);
                    } else if (str[0] == 'D'){
                        win.Draw_SetDrawColour(0xFF, 0xCC, 0x41);
                    }
                    win.Draw_Text(x, y, textScale, name);
                    win.Draw_SetDrawColour(0, 0, 0);
                    y += (int)(12 * textScale);         
                }

                win.Draw_SetDrawColour(255, 255, 255);
                if (printMore) win.Draw_Text(x, y - 6, -1f, "^^^^^", 1f, 1f);

                if (needsUpdate) {
                    needsUpdate = false;
                    scroll = 0;
                    allFiles.Clear();
                    
                    // everything is under /, unlike windows
                    if (OS.PlatformLinux && currentPath == "") {
                        currentPath = "/";
                    }

                    // if blank path display drives
                    if (currentPath == "") {
                        var arr1 = Directory.GetLogicalDrives();
                        allFiles.AddRange(arr1.Select(s => "D?" + s));
                    } else {
                        var arr1 = Directory.GetDirectories(currentPath).ToList();
                        arr1.Sort();
                        allFiles.AddRange(arr1.Select(s => "D?" + s));

                        var arr2 = Directory.GetFiles(currentPath).ToList();
                        arr2.Sort();
                        allFiles.AddRange(arr2.Select(s => "F?" + s));
                    }

                }

                wasDown = Ldown;
            };

            win.Open();

            directoryCache = currentPath;

            passed = _passed;
            return selectedFile;
        }

        /// <summary> Only use on windows, JIT is saving Linux from failing.<br/>This is scrap code, it does not work when a blocking window is open, it cant do what i wanted.</summary>
        internal static bool GetDefaultWindowsDirectorySearch(string title, out string outputString, string _defaultPath = "") {
            outputString = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                openFileDialog.InitialDirectory = _defaultPath;
                openFileDialog.Filter = "exe files (*.exe)|*.exe";
                openFileDialog.Title = title;
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    var filePath = openFileDialog.FileName;
                    if (File.Exists(filePath)) {
                        outputString = filePath;
                        return true;
                    } else {
                        return false;
                    }
                }
            }
            return false;
        }

        internal static string GetMethodString(MethodBase _method, bool extra = false) {
            if (_method == null) return $"Method Null";
            if (extra) {
                var str = $"{(_method is MethodInfo method ? method.ReturnType?.FullName + " " : "")}{_method?.DeclaringType?.FullName}::{_method?.Name}(";
                var para = _method.GetParameters();
                for (int i = 0; i < para.Length; i++) {
                    str += $"{para[i].ParameterType.FullName} {para[i].Name}";
                    if (i != para.Length - 1) str += ", ";
                }
                str += ")";
                return str;
            }
            return $"{_method?.DeclaringType?.FullName}::{_method?.Name}";
        }
        internal static string GetExceptionMessage(Exception e, ref List<MethodBase> trace) {
            var mainMessage = "\n";
            mainMessage += $"Exception type: {e.GetType()}\n";
            mainMessage += $"Message: {e.Message}\n";
            StackTrace stackTrace = new StackTrace(e, true);
            for (int i = 0; i < stackTrace.FrameCount; i++) {
                trace.Add(stackTrace.GetFrame(i).GetMethod());
            }
            if (e.InnerException != null) {
                mainMessage += GetExceptionMessage(e.InnerException, ref trace);
            }
            return mainMessage;
        }
        internal static void DisplayException(Stack<ModLoader.ModContext> modContexts, Exception e, StackFrame[] localStack) {

            int padding = 10;
            Window.Window win = new Window.Window("An Error Occurred.", 750, 600);
            win.Resizeable = true;
            win.BackgroundColour = Color.Gray;

            StackTrace stackTrace = new StackTrace(e, true);

            List<MethodBase> stackStringTrace = new List<MethodBase>();

            var mainMessage = $"Error: modContexts = null\n{GetExceptionMessage(e, ref stackStringTrace)}";
            if (modContexts != null) {
                // clear because we use it again
                stackStringTrace.Clear();
                ModLoader.ModContext mainContext = modContexts.Peek();
                mainMessage = $"An error occurred in mod \"{(mainContext.ModName)}\" ({mainContext.ContextName}).\n{GetExceptionMessage(e, ref stackStringTrace)}";

            }
            var simpleStack = string.Join("\n", stackStringTrace.Select(method => $"  at {GetMethodString(method)}"));

            Window.Button Close = new Window.Button("Ignore", 0, 0, 100, 25);
            Close.BackgroundColour = Color.DarkGray;
            Close.ForgroundColour = Color.Black;
            Close.OnPress += () => { win.Close(); };

            Window.Button Quit = new Window.Button("Quit Terraria", 0, 0, 120, 25);
            Quit.BackgroundColour = Color.DarkGray;
            Quit.ForgroundColour = Color.Black;
            Quit.OnPress += () => { win.Close(); Environment.Exit(0); };

            Window.Button Save = new Window.Button("Save", 0, 0, 100, 25);
            Save.BackgroundColour = Color.DarkGray;
            Save.ForgroundColour = Color.Black;
            Save.OnPress += () => {
                string newData = "";
                try {
                    var contexts = string.Join("", modContexts.Select((x, i) => {
                        var str =
                        $"in {x.ModName} ({x.ContextName}):\n" +
                        $"  Mod Type: {x.ModType?.FullName}\n" +
                        $"  PreMod Type: {x.PreModType?.FullName}\n" + 
                        $"  Path: {x.ModAssembily?.Location}\n" +
                        $"  Assembily: {x.ModAssembily}\n" +
                        "";
                        return str;
                    }));
                    newData =
                    $"============================================================\n" +
                    $"cModLoader: {BuildData.RAW_STRING}\n" +
                    $"Path: {Path.GetCurrentPath()}\n" +
                    $"Time: {DateTime.Now}\n" +
                    $"Terraria: {Terraria.GameVersion}\n" +
                    $"IsLinux: {Terraria.IsLinux}\n" +
                    $"OS: {OS.CurrentOSType}\n" +
                    $"Wine: {OS.IsWine}\n" +
                    $"SDL: {OS.SDLVersion}\n" +
                    $"------------------------------------------------------------\n" +
                    $"{contexts}\n" +
                    $"------------------------------------------------------------\n" +
                    $"{mainMessage}\n" +
                    $"Stack Trace:\n{string.Join("\n", stackStringTrace.Select(method => $"  at {GetMethodString(method, true)}"))}\n" +
                    $"Full Stack Trace:\n{string.Join("\n", localStack.Select(method => $"  at {(method == null ? "StackFrame Null" : (method.GetMethod() == null ? method.ToString() : GetMethodString(method.GetMethod(), true)))}"))}\n" +
                    $"============================================================\n" +
                    "";
                }
                catch (Exception _e) {
                    Show($"Failed to compile error text.\n" + _e.ToString(), "Error compiling text.");
                }
                var path = Path.GetCurrentcModLoaderFolder() + Path.DirectorySeparator + "ErrorLogs.txt";
                try {
                    if (File.Exists(path)) {
                        File.AppendAllText(path, newData);
                    } else {
                        File.WriteAllText(path, newData);
                    }
                }
                catch (Exception _e) {
                    Show($"Failed to write to file \"{path}\".\n" + _e.ToString(), "Error writing file.");
                }
                Show($"Error text saved to \"{path}\"!", "Saved!");
            };

            win.AddChildren(Close, Save, Quit);

            win.OnDraw += (isSDL) => {
                win.Draw_SetDrawColour(0, 0, 0, 255);
                win.Get_WindowSize(out int w, out int h);

                win.Draw_DefaultIcon(MessageBoxIcon.Error, padding, padding);
                int x = padding + 32 + padding;
                int y = padding;

                var str = mainMessage + simpleStack;

                win.Draw_Text(x, y, 1f, str);

                Close.PosX = w - Close.Width - padding;
                Close.PosY = h - Close.Height - padding;
                Save.PosX = w - Save.Width - padding - Close.Width - padding;
                Save.PosY = h - Save.Height - padding;
                Quit.PosX = padding;
                Quit.PosY = h - Quit.Height - padding;

            };
            win.Open();
        }

    }

    public class Path {

        /// <summary> Platform specific directory separator according to <see cref="System.IO.Path.DirectorySeparatorChar"/> </summary>
        public static char DirectorySeparator = PathIO.DirectorySeparatorChar;

        public static void GetSteamPaths(out string terrariaPath, out string realTerrariaPath) {
            var path = $@"{DirectorySeparator}steamapps{DirectorySeparator}common{DirectorySeparator}Terraria{DirectorySeparator}";
            realTerrariaPath = path + "RealTerraria.exe";
            terrariaPath = path + "Terraria.exe";
        }

        /// <summary> The directory at which Terraria is loaded from.<br/>If a virtual launch then it will be the same as <see cref="VirtualTerrariaPath"/> </summary>
        public static string LoadedTerrariaDirectory = "";

        /// <summary> <code>"\RealTerraria.exe"</code> </summary>
        public static string RealTerrariaPathEnding = DirectorySeparator + @"RealTerraria.exe";
        /// <summary> <code>"\Terraria.exe"</code> </summary>
        public static string TerrariaPathEnding = DirectorySeparator + @"Terraria.exe";
        /// <summary> Path where terraria was virtually launched from. </summary>
        public static string VirtualTerrariaPath = "";

        /// <summary> Gets and returns the current executing assemblies path, this is always the actual cModLoader exe (This includes the file name and path)<br/>Returns <c>...\Terraria\RealTerraria.exe</c> or <c>...\Terraria\cModLoader.exe</c> if running virtually. </summary>
        // code from ReLogic.Content.Sources.XnaContentSource::GetTitleLocationPath() with slight modifications
        // also seems to be from Microsoft.Xna.Framework.TitleLocation::get_Path()
        public static string GetCurrentPath() {
            string titleLocation = string.Empty;
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
                assembly = Assembly.GetCallingAssembly();
            if (assembly != null)
                titleLocation = PathIO.GetFullPath(assembly.Location);
            return titleLocation;
        }
        /// <summary> Returns the <c>...\Terraria\cModLoader</c> folder path.</summary>
        public static string GetCurrentcModLoaderFolder() => GoBack(GetCurrentPath()) + "cModLoader";
        /// <summary> Returns the <c>...\Terraria\cModLoader\Mods</c> folder path.</summary>
        public static string GetCurrentModsFolder() => GoBack(GetCurrentPath()) + "cModLoader" + DirectorySeparator + "Mods";
        /// <summary> Returns the <c>...\Terraria\cModLoader\Config</c> folder path.</summary>
        public static string GetCurrentConfigsFolder() => GoBack(GetCurrentPath()) + "cModLoader" + DirectorySeparator + "Config";

        /// <summary> Gets the full file path from a name from the current directory.<br/>Will trim and starting directory separators from <paramref name="fileName"/>. </summary>
        public static string GetFromDirectory(string fileName, out bool fileExists) {
            var path = GoBack(GetCurrentPath()) + fileName.TrimStart(DirectorySeparator);
            fileExists = File.Exists(path);
            return path;
        }

        /// <summary> Returns the current folder cModLoader is executing from. This is NOT the path terraria is in. <para>This includes the last path separator. this will retune something like <code>C:\Path\To\Directory\</code></para> </summary>
        public static string GetCurrentFolder() {
            var path = GetCurrentPath();
            return path.Substring(0, path.LastIndexOf(DirectorySeparator) + 1);
        }
        /// <summary>
        /// Tries to get the terraria directory (currently only works on windows). This will search every drive for a standard steam directory. These paths will include the file name.<br/>
        /// </summary>
        // TODO: (maybe) add VirtualTerrariaPath to this so it can be used instead of referencing VirtualTerrariaPath, this could be hard because realTerrariaPath could not exist (or add a second FindRealTerraria() function)
        // TODO: make work on Linux, it should be in .steam, but idk where from there
        public static bool FindTerrariaGlobally(out string terrariaPath, out string realTerrariaPath)
        {
            terrariaPath = "";
            realTerrariaPath = "";
            // search entire computer
            if (OS.PlatformWindows) {
                GetSteamPaths(out string SteamPathEnding, out string RealSteamPathEnding);
                // path for main steam path
                string defaultPath = $@":\Program Files (x86)\Steam{SteamPathEnding}";
                string realDefaultPath = $@":\Program Files (x86)\Steam{RealSteamPathEnding}";
                // path for external drive
                string movedPath = $@":\SteamLibrary{SteamPathEnding}";
                string realMovedPath = $@":\SteamLibrary{RealSteamPathEnding}";
                // check all drive letters (apparently there just cant be more then 26)
                foreach (var letter in "ABCDEFGHJKLMNOPQRSTUVWXYZ".ToCharArray())
                {
                    if (File.Exists(letter + defaultPath))
                    {
                        terrariaPath = letter + defaultPath;
                        realTerrariaPath = letter + realDefaultPath;
                    }
                    if (File.Exists(letter + movedPath))
                    {
                        terrariaPath = letter + movedPath;
                        realTerrariaPath = letter + realMovedPath;
                    }
                }
                return terrariaPath == "" ? false : true;
            }
            return false;
        }
        /// <summary> Copies a file from one place to another. </summary> <returns> Whether or not the file was copied. </returns>
        public static bool CopyFile(string path1, string path2, bool overwrite = true) {
            try { File.Copy(path1, path2, true); return true; }
            catch (Exception) { return false; }
        }
        
        
        /// <summary> Gets the name of the file or directory. Expands to <see cref="PathIO.GetFileName(string)"/> </summary>
        public static string GetName(string path) {
            return PathIO.GetFileName(path);
        }
        /// <summary> Returns the sub directory. Will end with backslash.<para>Eg. "C:\Some\Directory\" becomes "C:\Some\"</para><para>Eg. "C:\Some\Directory\File.txt" becomes "C:\Some\Directory\"</para> </summary>
        public static string GoBack(string path) {
            // don't count the last character incase its a "\"
            return path.Substring(0, path.Substring(0, path.Length - 1).LastIndexOf(DirectorySeparator) + 1);
        }
        
        /// <summary> Ensure a folder exists. Creates one if it does not exist. </summary>
        public static bool EnsureFolder(string path, out bool folderExisted) {
            folderExisted = Directory.Exists(path);
            if (!folderExisted) {
                Directory.CreateDirectory(path);
                if (!Directory.Exists(path)) {
                    return false; // failed to create folder
                }
            }
            return true;
        }

        public static string GetDefaultPath() {
            var defaultPath = "C:\\";
            if (OS.PlatformWindows) {
                // why? because i can do what i want
                if (Environment.UserName == "crawm") {
                    defaultPath = @"C:\Files\All Terraria Versions\cModLoader\Legacy Test\1.0.6.1 (1) (Archive)";
                }
            }
            if (OS.IsWine) {
                defaultPath = @"Z:\home\" + Environment.UserName; // i think this is standard
                // why? because i can do what i want
                if (Environment.UserName == "crawdad105") {
                    // WSL installed in "debian-installation" normal Linux installs in "steam"
                    if (Directory.Exists(@"\.steam\debian-installation\steamapps\common\Terraria"))
                        defaultPath += @"\.steam\debian-installation\steamapps\common\Terraria";
                    else defaultPath += @"\.steam\steam\steamapps\common\Terraria";
                }
            }
            if (OS.PlatformLinux) {
                defaultPath = "/home/" + Environment.UserName;
                // why? because i can do what i want
                if (Environment.UserName == "crawdad105") {
                    // WSL installed in "debian-installation" normal Linux installs in "steam"
                    if (Directory.Exists(@"/.steam/debian-installation/steamapps/common/Terraria"))
                        defaultPath += @"/.steam/debian-installation/steamapps/common/Terraria";
                    else defaultPath += @"/.steam/steam/steamapps/common/Terraria";
                }
            }
            return defaultPath;
        }

    }

}