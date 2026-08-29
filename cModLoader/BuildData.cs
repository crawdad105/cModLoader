namespace cModLoader {
    /// <summary>
    /// Build data, this is modified when building cModLoader
    /// </summary>
    public class BuildData {
        // normally valid terraria symbols, newer versions can display a lot more
        //  !"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~ ¡¢£¤¥¦§¨©

        // Upper : ΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩ
        // Lower : αβγδεζηθικλμνξοπρστυφχψω
        // Best  : αβγΔεζηθικλμνξοπρΣτυφχψΩ
        // legacy versions cant display these characters
        public static string GREEK = "_αβγΔεζηθικλμνξοπρΣτυφχψΩ";
        public static string[] GREEK_WORD = new string[25] {
            "None",
            "Alpha",
            "Beta",
            "Gamma",
            "Delta",
            "Epsilon",
            "Zeta",
            "Eta",
            "Theta",
            "Iota",
            "Kappa",
            "Lambda",
            "Mu",
            "Nu",
            "Xi",
            "Omicron",
            "Pi",
            "Rho",
            "Sigma",
            "Tau",
            "Upsilon",
            "Phi",
            "Chi",
            "Psi",
            "Omega",
        };
        /// <summary> Major update counter.<para>Its more interesting if its always 0, it then seems like its always in development and updating, like BeamNG.drive</para></summary>
        public const int MAJOR = 0;
        /// <summary>
        /// Minor update counter
        /// <para>
        /// Used for significant changes.<br/>
        /// 2: completely refactored all code.<br/>
        /// 1: original versions that did not support every versions (Closed "alpha", @Jayrony69 (youtube) is the only other person to have a version)<br/>
        /// 0: idk if it existed, i could argue there were a bunch of versions like this but i never really kept track.
        /// </para>
        /// </summary>
        public const int MINOR = 2;
        /// <summary> Patch update counter.<para>Used for smaller insignificant updates.</para></summary>
        public const int PATCH = 1;
        /// <summary> Arbitrary build number, same as <see cref="BuildNumber"/>.<para>Changes when the program is ran or build during development.</para></summary>
        public const int BUILD = BuildNumber;
        public static string ASSEMBILY_STRING => $"{MAJOR}.{MINOR}.{PATCH}.{BUILD}";
        public static string RAW_STRING => $"v{MAJOR}.{MINOR}.{PATCH}({BUILD})";
        public static string LETTER_STRING => $"{(MAJOR == 0 ? "" : MAJOR + ".")}{GREEK[MINOR]}.{PATCH}{(BUILD == 0 ? "" : "." + $"({BUILD})")}";
        public static string WORD_STRING => $"{(MAJOR == 0 ? "" : MAJOR + ".")}{GREEK_WORD[MINOR]}.{PATCH}{(BUILD == 0 ? "" : "." + $"({BUILD})")}";
        /// <summary> Gets version text including only <see cref="MINOR"/> and <see cref="PATCH"/> (and <see cref="MAJOR"/> if its not 0). <paramref name="letter"/> determines if it should use Greek letters or names.</summary>
        public static string GET_DISPLAY(bool letter = true) => $"{(MAJOR == 0 ? "" : $"({MAJOR}) ")}{(letter ? GREEK[MINOR].ToString() : GREEK_WORD[MINOR])}.{PATCH}";

        /// <summary> The build number/count, increments every time the project was built or ran.<br/>Don't assume this counts every single versions of cModLoader, it can be set to whatever by the developer anyways.</summary>
        public const int BuildNumber = 815;
        /// <summary> The build time, the Unix time stamp that the latest version was built. Uses <see cref="System.DateTime.Ticks"/> on <see cref="System.DateTime.Now"/></summary>
        public const long BuildTime = 639235363228317446;
    }
}
