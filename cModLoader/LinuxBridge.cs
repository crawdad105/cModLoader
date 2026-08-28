using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace cModLoader
{
    /// <summary>
    /// Class with some stuff for checking operating system or underlying runtime stuff.
    /// </summary>
    public class OS {

        internal static void CheckOS() {
            
            // get 32 or 64 bit
            Check32Bit = Environment.Is64BitProcess == false;
            // get OS, might be possible to get working on macOS but its not supported
            CurrentPlatform = Environment.OSVersion.Platform;
            if (CurrentPlatform == PlatformID.Win32NT) CheckWindows = true;
            if (CurrentPlatform == PlatformID.Unix) CheckLinux = true; // might be true for macOS as well
            if (CurrentPlatform == PlatformID.MacOSX) CheckMacOS = true;

            // check for wine
            try {
                Accessibility.Native.wine_get_version();
                Output.Print($"Wine check success");
                CheckWine = true;
            }
            catch (Exception e) { Output.Print($"Wine check error, {e.GetType()}"); }
            // check for kernel32
            try {
                Accessibility.Native.GetConsoleWindow();
                CheckKernel32 = true;
            }
            catch (Exception e) {
                Output.Print($"Kernel32 check error, {e.GetType()}");
            }

            SDLVersion = SDLVersions.None;
            CheckSDL = false;
            // check SDL3
            try {
                // for some reason this always works for me (crawdad105) in WSL, but i did test SDL2 and it also works so i guess we just use whatever works
                FNA.FNA_SDL3.SDL_Init(0);
                SDLVersion = SDLVersions.SDL3;
                CheckSDL = true;
                CheckSDL3 = true;
            }
            catch (Exception e) {
                Output.Print($"SDL3 check error, {e.GetType()}");
            }
            if (!CheckSDL) {
                // check SDL2 (if SDL3 failed)
                try {
                    FNA.FNA_SDL2.SDL_Init(0);
                    CheckSDL = true;
                    CheckSDL3 = false;
                    SDLVersion = SDLVersions.SDL2;
                }
                catch (Exception e2) {
                    Output.Print($"SDL2 check error, {e2.GetType()}");
                }
            }

            // set resulting OS
            if (CheckWindows) {
                CurrentOSType = CheckWine ? OSTypes.LinuxWine : OSTypes.Windows;
            }
            else {
                // TODO: check for proton and maybe proton versions incase they are different
                CurrentOSType = OSTypes.Linux;
            }

            Output.Print($"OS Result: {CurrentOSType}");
            Output.Print($"  Platform: {CurrentPlatform}");
            Output.Print($"  32-Bit: {Check32Bit}");
            Output.Print($"  Wine: {CheckWine}");
            Output.Print($"  SDL: {SDLVersion}");
            Output.Print($"  Kernel32: {CheckKernel32}");

        }

        internal static bool Check32Bit = false;
        internal static bool CheckWindows = false;
        internal static bool CheckLinux = false;
        internal static bool CheckMacOS = false;
        internal static bool CheckWine = false;
        internal static bool CheckKernel32 = false;
        internal static bool CheckSDL = false;
        internal static bool CheckSDL3 = false;
        
        internal static bool CheckFNA = false;

        internal static PlatformID CurrentPlatform;
        public enum OSTypes {
            Windows,
            LinuxWine,
            Linux,
        }
        /// <summary> The current OS type. </summary>
        public static OSTypes CurrentOSType;
        public enum SDLVersions {
            None,
            SDL2,
            SDL3,
        }
        /// <summary> The SDL version being used. </summary>
        public static SDLVersions SDLVersion;

        /// <summary>
        /// Does the program thinks its running in windows. <see langword="true"/> for windows OR if its running through Wine (might depend on configuration).<br/>
        /// This does not necessary prove its running in windows.
        /// </summary>
        public static bool PlatformWindows => CheckWindows;

        /// <summary>
        /// Does the program thinks its running in Linux. Seems to be <see langword="true"/> when running using proton (but could be tru for something else).<br/>
        /// If this is <see langword="true"/> we can't use a lot of windows features. <br/>
        /// This seems to be <see langword="false"/> when using Wine.<br/>
        /// This may also be <see langword="true"/> for macOS but i cant confirm this.<br/>
        /// To check if Terraria is actually the Linux versions use <see cref="Terraria.IsLinux"/>
        /// </summary>
        public static bool PlatformLinux => CheckLinux;


        /// <summary>
        /// Is the program being ran in windows. This is <see langword="true"/> if the platform is windows AND Wine is not detected.<br/>
        /// </summary>
        public static bool IsWindows => !CheckWine && CheckWindows;
        /// <summary>
        /// Is the program being ran through Wine. <see langword="true"/> if <see cref="Accessibility.Native.wine_get_version"/> didn't fail.<br/>
        /// </summary>
        public static bool IsWine => CheckWine;

        /// <summary>
        /// Is the program running in Linux. This is <see langword="true"/> if it detected Linux or if Wine was detected (could be edge cases).<br/>
        /// To check if Terraria is actually the Linux versions use <see cref="Terraria.IsLinux"/>
        /// </summary>
        public static bool IsLinux => CheckWine || CheckLinux;

        /// <summary>
        /// Is SDL available. This is <see langword="true"/> if it detected SDL which should be the same as if <see cref="IsLinux"/> is <see langword="true"/>.<br/>
        /// This may be <see langword="true"/> on windows if i forgot to remove the dll path and you have SDL3 installed at the same directory that i (crawdad105) did.
        /// </summary>
        public static bool IsSDL => CheckSDL;
        /// <summary>
        /// Is SDL2 available. This is <see langword="true"/> if it detected SDL2 otherwise if its <see langword="false"/> SDL3 was detected OR no sdl was detected.<br/>
        /// </summary>
        public static bool IsSDL2 => CheckSDL && !CheckSDL3;


    }


}
