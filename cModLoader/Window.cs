using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Form = System.Windows.Forms;
using SDL = cModLoader.Accessibility.SDL3;

namespace cModLoader.Window
{
    public interface IWindow {
        /// <summary> Update code, return <see langword="true"/> to close and <see langword="false"/> to stay open.</summary>
        void Update(Window window);
        /// <summary> Close code, ran when the window is closed.</summary>
        void Close(Window window);
        /// <summary> Open code, ran when the window is opened.</summary>
        void Open(Window window);

        // getters
        void Get_MousePos(out int x, out int y);
        void Get_Focused(out bool isFocused);
        //void Get_MouseScroll(out float x, out float y);
        void Get_MouseDown(out bool L, out bool R, out bool M);
        void Get_WindowSize(out int w, out int h);
        bool Get_KeyIsDown(Keys key);
        // setters
        void Set_Resizable(bool set);
        void Set_Title(string title);
        void Set_WindowSize(int w, int h);
        void Set_WindowIcon(byte[] iconData);
        // drawers
        void Draw_FillRect(int x, int y, int w, int h);
        void Draw_Rect(int x, int y, int w, int h);
        void Draw_SetDrawColour(byte r, byte g, byte b, byte a = 255);
        void Draw_Clear();
        void Draw_Text(int x, int y, float scale, string str, float alignX = 0f, float alignY = 0f);
        void Draw_DefaultIcon(byte[] iconData, int x, int y);
    }
    public interface IMouseEvent {
        bool IsHover(int mouseX, int mouseY);
        event Action OnHover;
        event Action OnPress;
    }

    // copied from windows forms
    public enum Keys {
        KeyCode = 0xFFFF,
        Modifiers = -65536,
        None = 0,
        LButton = 1,
        RButton = 2,
        Cancel = 3,
        MButton = 4,
        XButton1 = 5,
        XButton2 = 6,
        Back = 8,
        Tab = 9,
        LineFeed = 0xA,
        Clear = 0xC,
        Return = 0xD,
        Enter = 0xD,
        ShiftKey = 0x10,
        ControlKey = 0x11,
        Menu = 0x12,
        Pause = 0x13,
        Capital = 0x14,
        CapsLock = 0x14,
        KanaMode = 0x15,
        HanguelMode = 0x15,
        HangulMode = 0x15,
        JunjaMode = 0x17,
        FinalMode = 0x18,
        HanjaMode = 0x19,
        KanjiMode = 0x19,
        Escape = 0x1B,
        IMEConvert = 0x1C,
        IMENonconvert = 0x1D,
        IMEAccept = 0x1E,
        IMEAceept = 0x1E,
        IMEModeChange = 0x1F,
        Space = 0x20,
        Prior = 0x21,
        PageUp = 0x21,
        Next = 0x22,
        PageDown = 0x22,
        End = 0x23,
        Home = 0x24,
        Left = 0x25,
        Up = 0x26,
        Right = 0x27,
        Down = 0x28,
        Select = 0x29,
        Print = 0x2A,
        Execute = 0x2B,
        Snapshot = 0x2C,
        PrintScreen = 0x2C,
        Insert = 0x2D,
        Delete = 0x2E,
        Help = 0x2F,
        D0 = 0x30,
        D1 = 0x31,
        D2 = 0x32,
        D3 = 0x33,
        D4 = 0x34,
        D5 = 0x35,
        D6 = 0x36,
        D7 = 0x37,
        D8 = 0x38,
        D9 = 0x39,
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,
        LWin = 0x5B,
        RWin = 0x5C,
        Apps = 0x5D,
        Sleep = 0x5F,
        NumPad0 = 0x60,
        NumPad1 = 0x61,
        NumPad2 = 0x62,
        NumPad3 = 0x63,
        NumPad4 = 0x64,
        NumPad5 = 0x65,
        NumPad6 = 0x66,
        NumPad7 = 0x67,
        NumPad8 = 0x68,
        NumPad9 = 0x69,
        Multiply = 0x6A,
        Add = 0x6B,
        Separator = 0x6C,
        Subtract = 0x6D,
        Decimal = 0x6E,
        Divide = 0x6F,
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,
        NumLock = 0x90,
        Scroll = 0x91,
        LShiftKey = 0xA0,
        RShiftKey = 0xA1,
        LControlKey = 0xA2,
        RControlKey = 0xA3,
        LMenu = 0xA4,
        RMenu = 0xA5,
        BrowserBack = 0xA6,
        BrowserForward = 0xA7,
        BrowserRefresh = 0xA8,
        BrowserStop = 0xA9,
        BrowserSearch = 0xAA,
        BrowserFavorites = 0xAB,
        BrowserHome = 0xAC,
        VolumeMute = 0xAD,
        VolumeDown = 0xAE,
        VolumeUp = 0xAF,
        MediaNextTrack = 0xB0,
        MediaPreviousTrack = 0xB1,
        MediaStop = 0xB2,
        MediaPlayPause = 0xB3,
        LaunchMail = 0xB4,
        SelectMedia = 0xB5,
        LaunchApplication1 = 0xB6,
        LaunchApplication2 = 0xB7,
        OemSemicolon = 0xBA,
        Oem1 = 0xBA,
        Oemplus = 0xBB,
        Oemcomma = 0xBC,
        OemMinus = 0xBD,
        OemPeriod = 0xBE,
        OemQuestion = 0xBF,
        Oem2 = 0xBF,
        Oemtilde = 0xC0,
        Oem3 = 0xC0,
        OemOpenBrackets = 0xDB,
        Oem4 = 0xDB,
        OemPipe = 0xDC,
        Oem5 = 0xDC,
        OemCloseBrackets = 0xDD,
        Oem6 = 0xDD,
        OemQuotes = 0xDE,
        Oem7 = 0xDE,
        Oem8 = 0xDF,
        OemBackslash = 0xE2,
        Oem102 = 0xE2,
        ProcessKey = 0xE5,
        Packet = 0xE7,
        Attn = 0xF6,
        Crsel = 0xF7,
        Exsel = 0xF8,
        EraseEof = 0xF9,
        Play = 0xFA,
        Zoom = 0xFB,
        NoName = 0xFC,
        Pa1 = 0xFD,
        OemClear = 0xFE,
        Shift = 0x10000,
        Control = 0x20000,
        Alt = 0x40000
    }

    public class FormWindow : Form.Form, IWindow
    {

        public FormWindow()
        {
            this.DoubleBuffered = true;
        }
        public System.Windows.Forms.Timer ticker;
        public static HashSet<System.Windows.Forms.Keys> PressedKeys = new HashSet<System.Windows.Forms.Keys>();
        public System.Drawing.Graphics g = null; // DO NOT USE unless you know when its being used
        public event Action<Form.PaintEventArgs> OnDraw;
        private int mouseX;
        private int mouseY;
        private bool mouseLDown;
        private bool mouseRDown;
        private bool mouseMDown;

        bool init = false;
        protected override void OnPaint(Form.PaintEventArgs paintEvent)
        {
            base.OnPaint(paintEvent);
            g = paintEvent.Graphics;

            mouseLDown = Form.Control.MouseButtons == Form.MouseButtons.Left;
            mouseRDown = Form.Control.MouseButtons == Form.MouseButtons.Right;
            mouseMDown = Form.Control.MouseButtons == Form.MouseButtons.Middle;

            try {
                OnDraw?.Invoke(paintEvent);
            } catch (Exception) { }

        }
        protected override void OnMouseMove(Form.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mouseX = e.X;
            mouseY = e.Y;
        }

        private System.Drawing.Color DrawColour = System.Drawing.Color.Black;

        public void Open(Window windowComponent) {

            this.MouseDown += (o, e) => {
                if (e.Button == MouseButtons.Left) PressedKeys.Add(System.Windows.Forms.Keys.LButton);
            };
            this.MouseUp += (o, e) => {
                if (e.Button == MouseButtons.Left) PressedKeys.Remove(System.Windows.Forms.Keys.LButton);
            };
            this.KeyDown += (o, e) => {
                PressedKeys.Add(e.KeyCode);
            };
            this.KeyUp += (o, e) => {
                PressedKeys.Remove(e.KeyCode);
            };

            ClientSize = new System.Drawing.Size(windowComponent.StartWidth, windowComponent.StartHeight);

            FormBorderStyle = windowComponent.Resizeable ? Form.FormBorderStyle.Sizable : Form.FormBorderStyle.FixedSingle;
            MaximizeBox = windowComponent.Resizeable;
            MinimizeBox = windowComponent.Resizeable;
            OnDraw += (o) => {

                windowComponent.DefaultDraw();
                windowComponent.curScrollX = 0;
                windowComponent.curScrollY = 0;
            };
            Text = windowComponent.Title;
            FormClosed += (o, e) => { windowComponent.Close(); };
            MouseWheel += (o, e) => { windowComponent.curScrollY = e.Delta; };
        }
        public void Update(Window windowComponent) {
            this.Invalidate();
        }
        public void Close(Window windowComponent) {

        }


        public void Draw_FillRect(int x, int y, int w, int h)
        {
            using (System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(DrawColour))
                g.FillRectangle(b, new System.Drawing.Rectangle(x, y, w, h));
        }
        public void Draw_Rect(int x, int y, int w, int h)
        {
            using (System.Drawing.Pen p = new System.Drawing.Pen(DrawColour))
                g.DrawRectangle(p, new System.Drawing.Rectangle(x, y, w, h));
        }
        public void Draw_SetDrawColour(byte r, byte g, byte b, byte a = 255)
        {
            DrawColour = System.Drawing.Color.FromArgb(a, r, g, b);
        }
        public void Draw_Clear()
        {
            g.Clear(System.Drawing.Color.FromArgb(DrawColour.A, DrawColour.R, DrawColour.G, DrawColour.B));
        }
        public void Draw_Text(int x, int y, float scale, string str, float alignX = 0f, float alignY = 0f)
        {
            using (System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(DrawColour))
            using (var f2 = new System.Drawing.Font(System.Drawing.FontFamily.GenericMonospace, 10f * (Math.Abs(scale)), System.Drawing.FontStyle.Bold))
            using (System.Drawing.StringFormat format = System.Drawing.StringFormat.GenericTypographic)
            {
                if (scale < 0)
                {
                    scale = Math.Abs(scale);
                    var textSize = g.MeasureString(str, f2, int.MaxValue, format);
                    var offset = (2f * scale);
                    var h = (int)(textSize.Height - (offset * 2));
                    var x2 = (int)(x + (textSize.Width * alignX));
                    var y2 = (int)(y + (h * alignY)) + (int)offset;
                    g.TranslateTransform(0, 0);
                    g.RotateTransform(180);
                    g.DrawString(str, f2, b, new System.Drawing.Point(-x2, -y2), format);
                    g.ResetTransform();
                }
                else
                {
                    format.FormatFlags |= System.Drawing.StringFormatFlags.MeasureTrailingSpaces;
                    var textSize = g.MeasureString(str, f2, int.MaxValue, format);
                    var offset = (2f * scale);
                    var h = (int)(textSize.Height - (offset * 2));
                    var x2 = (int)(x - (textSize.Width * alignX));
                    var y2 = (int)(y - (h * alignY)) - (int)offset;
                    g.DrawString(str, f2, b, new System.Drawing.Point(x2, y2), format);
                }
            }
        }
        public void Draw_DefaultIcon(byte[] iconData, int x, int y) {
            if (iconData == null) return;
            unsafe {
                fixed (byte* ptr = iconData) {
                    using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(32, 32, 32 * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, (IntPtr)ptr)) {
                        g.DrawImage(bmp, x, y);
                    }
                }
            }
        }

        public void Set_Resizable(bool set)
        {
            FormBorderStyle = (set ? Form.FormBorderStyle.Sizable : Form.FormBorderStyle.FixedSingle);
            MaximizeBox = set;
            MinimizeBox = set;
        }
        public void Set_Title(string title)
        {
            Text = title;
        }
        public void Set_WindowSize(int w, int h)
        {
            ClientSize = new System.Drawing.Size(w, h);
        }
        public void Set_WindowIcon(byte[] iconData) {
            if (iconData == null) return;
            unsafe {
                fixed (byte* ptr = iconData) {
                    using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(32, 32, 32 * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, (IntPtr)ptr)) {
                        Icon = System.Drawing.Icon.FromHandle(bmp.GetHicon());
                    }
                }
            }
        }

        public void Get_MousePos(out int x, out int y)
        {
            x = mouseX;
            y = mouseY;
        }
        public void Get_MouseDown(out bool L, out bool R, out bool M)
        {
            L = mouseLDown;
            R = mouseRDown;
            M = mouseMDown;
        }
        public void Get_WindowSize(out int w, out int h)
        {
            w = ClientRectangle.Size.Width;
            h = ClientRectangle.Size.Height;
        }

        public bool Get_KeyIsDown(Keys key) {
            return PressedKeys.Contains((System.Windows.Forms.Keys)(int)key);
        }

        public void Get_Focused(out bool isFocused) {
            isFocused = this.Focused;
        }
    }

    public abstract class Component {
        public bool Enabled = true;
        public Component Parent = null;
        public List<Component> Children = new List<Component>();
        public int PosX;
        public int PosY;

        public event Action<bool> OnUpdate;

        public void AddChild(Component comp) {
            if (comp.Parent != null) comp.Parent.RemoveChild(comp);
            comp.Parent = this;
            Children.Add(comp);
        }
        public void AddChildren(params Component[] comps) {
            for (int i = 0; i < comps.Length; i++) AddChild(comps[i]);
        }
        public void AddChildren(List<Component> comps) {
            for (int i = 0; i < comps.Count; i++) AddChild(comps[i]);
        }
        public void RemoveChild(Component comp) {
            if (comp.Parent != null) comp.Parent.RemoveChild(comp);
            comp.Parent = null;
        }

        public bool IsType<T>(out T c) where T : Component {
            c = null;
            if (GetType() == typeof(T)) {
                c = (T)this;
                return true;
            }
            return false;
        }

        /// <summary> WARNING: On windows if using SDL this is NOT called when moving the window, this requires a lot of work to fix so i didn't fix it. </summary>
        protected virtual void Update(bool isSDL) {
            OnUpdate?.Invoke(isSDL);
            for (int i = 0; i < Children.Count; i++)
                Children[i].Update(isSDL);
            if (isSDL) UpdateSDL();
            else UpdateForm();
        }

        public virtual void UpdateForm() { }
        public virtual void UpdateSDL() { }
        public virtual void Draw(Window window) {

        }
    }
    public class Button : Component, IMouseEvent {

        public bool IsHovered = false;
        public bool WasHovered = false;

        public bool WasPressed = false;
        public bool IsPressed = false;

        public bool WasLDown = false;

        public int Width;
        public int Height;
        public string Text;
        public float TextScale = 1f;
        public Color BackgroundColour = Color.Red;
        public Color ForgroundColour = Color.Green;

        public Button(string text, int x, int y, int width, int height) {
            PosX = x;
            PosY = y;
            Width = width;
            Height = height;
            Text = text;
        }
        public event Action OnHover;
        public event Action OnPress;
        public bool IsHover(int mouseX, int mouseY) {
            return (mouseX >= PosX && mouseX < PosX + Width) && (mouseY >= PosY && mouseY < PosY + Height);
        }

        public void DoHover() => OnHover?.Invoke();
        public void DoPress() => OnPress?.Invoke();

        public override void Draw(Window window) {

            window.Get_Focused(out var focused);
            if (focused) {
                //Get_MousePos(out int mouseX, out int mouseY);
                WasHovered = IsHovered;
                WasPressed = IsPressed;
                IsHovered = false;
                IsPressed = false;
                IsHovered = IsHover(window.mouseX, window.mouseY);
                window.Get_MouseDown(out bool Ldown, out _, out _);
                if (IsHovered) {
                    DoHover();
                    if (!Ldown && WasLDown) {
                        DoPress();
                        IsPressed = true;
                    }
                    WasLDown = Ldown;
                }
                else {
                    WasLDown = false;
                }
            }

            // draw rect
            if (IsHovered) window.Draw_SetDrawColour((byte)Math.Min((BackgroundColour.R * 1.5f), 0xFF), (byte)Math.Min((BackgroundColour.G * 1.5f), 0xFF), (byte)Math.Min((BackgroundColour.B * 1.5f), 0xFF), BackgroundColour.A);
            else window.Draw_SetDrawColour(BackgroundColour.R, BackgroundColour.G, BackgroundColour.B, BackgroundColour.A);
            window.Draw_FillRect(PosX, PosY, Width, Height);
            // black
            window.Draw_SetDrawColour(0, 0, 0, 255);
            window.Draw_Rect(PosX, PosY, Width, Height);
            // window.darker
            window.Draw_SetDrawColour((byte)(BackgroundColour.R / 2), (byte)(BackgroundColour.G / 2), (byte)(BackgroundColour.B / 2), BackgroundColour.A);
            window.Draw_Rect(PosX, PosY, Width - 1, Height - 1);
            // lighter
            window.Draw_SetDrawColour((byte)Math.Min((BackgroundColour.R * 2), 0xFF), (byte)Math.Min((BackgroundColour.G * 2), 0xFF), (byte)Math.Min((BackgroundColour.B * 2), 0xFF), BackgroundColour.A);
            window.Draw_Rect(PosX, PosY, Width - 1, 1);
            window.Draw_Rect(PosX, PosY, 1, Height - 1);
            // text
            window.Draw_SetDrawColour(ForgroundColour.R, ForgroundColour.G, ForgroundColour.B, ForgroundColour.A);
            window.Draw_Text(PosX + (Width / 2), PosY + (Height / 2), TextScale, Text, 0.5f, 0.5f);
        }

    }
    public class Label : Component {
        public Color ForgroundColour = Color.Black;
        public string Text;
        public float TextScale = 1f;
        public float AlignX = 0f;
        public float AlignY = 0f;
        public Label(string text, int x, int y) {
            PosX = x;
            PosY = y;
            Text = text;
        }
        public override void Draw(Window window) {
            window.Draw_SetDrawColour(ForgroundColour.R, ForgroundColour.G, ForgroundColour.B, ForgroundColour.A);
            window.Draw_Text(PosX, PosY, TextScale, Text, AlignX, AlignY);
        }
    }
    public class CheckBox : Button {
        public bool Checked;
        public CheckBox(string text, int x, int y, int width, int height) : base(text, x, y, width, height) {
            Checked = false;
        }
        public void DoPress() {
            Checked = !Checked;
            base.DoPress();
        }
        public override void Draw(Window window) {

            window.Get_Focused(out var focused);
            if (focused) {
                //Get_MousePos(out int mouseX, out int mouseY);
                WasHovered = IsHovered;
                WasPressed = IsPressed;
                IsHovered = false;
                IsPressed = false;
                IsHovered = IsHover(window.mouseX, window.mouseY);
                window.Get_MouseDown(out bool Ldown, out _, out _);
                if (IsHovered) {
                    DoHover();
                    if (!Ldown && WasLDown) {
                        DoPress();
                        IsPressed = true;
                    }
                    WasLDown = Ldown;
                }
                else {
                    WasLDown = false;
                }
            }

            // draw rect
            if (IsHovered) window.Draw_SetDrawColour((byte)Math.Min((BackgroundColour.R * 1.5f), 0xFF), (byte)Math.Min((BackgroundColour.G * 1.5f), 0xFF), (byte)Math.Min((BackgroundColour.B * 1.5f), 0xFF), BackgroundColour.A);
            else window.Draw_SetDrawColour(BackgroundColour.R, BackgroundColour.G, BackgroundColour.B, BackgroundColour.A);
            window.Draw_FillRect(PosX, PosY, Width, Height);
            // black
            window.Draw_SetDrawColour(0, 0, 0, 255);
            window.Draw_Rect(PosX, PosY, Width, Height);
            // window.darker
            window.Draw_SetDrawColour((byte)(BackgroundColour.R / 2), (byte)(BackgroundColour.G / 2), (byte)(BackgroundColour.B / 2), BackgroundColour.A);
            window.Draw_Rect(PosX, PosY, Width - 1, Height - 1);
            // lighter
            window.Draw_SetDrawColour((byte)Math.Min((BackgroundColour.R * 2), 0xFF), (byte)Math.Min((BackgroundColour.G * 2), 0xFF), (byte)Math.Min((BackgroundColour.B * 2), 0xFF), BackgroundColour.A);
            window.Draw_Rect(PosX, PosY, Width - 1, 1);
            window.Draw_Rect(PosX, PosY, 1, Height - 1);
            // text
            window.Draw_SetDrawColour(ForgroundColour.R, ForgroundColour.G, ForgroundColour.B, ForgroundColour.A);
            window.Draw_Text(PosX + Width + 2, PosY + (Height / 2), TextScale, Text, 0f, 0.5f);
        }
    }

    public class WindowComponent : Component {

    }

    public class Window {

        /// <summary> List of active non-blocking windows, these are cModLoader windows not any Terraria, Xna, or FNA made windows.</summary>
        public static List<Window> ActiveWindows = new List<Window>();
        /// <summary> Update all <see cref="ActiveWindows"/>. This can only be ran on the main thread (mainly due to an SDL thing).</summary>
        internal static void UpdateWindows() {
            for (int i = 0; i < ActiveWindows.Count; i++) {
                if (ActiveWindows[i].UsingSDL)
                    ActiveWindows[i].Update();
            }
            for (int i = 0; i < ActiveWindows.Count; i++) {
                if (ActiveWindows[i].shouldClose) {
                    ActiveWindows.RemoveAt(i--);
                }
            }
        }

        private WindowComponent mainComponent;

        public List<Component> Children => mainComponent.Children;
        public void AddChild(Component comp) => mainComponent.AddChild(comp);
        public void AddChildren(params Component[] comps) => mainComponent.AddChildren(comps);
        public void AddChildren(List<Component> comps) => mainComponent.AddChildren(comps);
        public void RemoveChild(Component comp) => mainComponent.RemoveChild(comp);

        public void Close() {
            shouldClose = true;
        }
        private bool didOpen = false;
        private bool shouldClose = false;
        public bool UsingSDL = false;
        public bool IsBlocking = false;

        public Color BackgroundColour = Color.Red;

        /// <summary> Either <see cref="FormWindow"/> or <see cref="SDL.SDL3_Window"/> (this is a custom cModLoader SDL class). This will likely also have an SDL2 window but thats not implamented yet. </summary>
        public IWindow nativeType = null;

        /// <summary> This is called after the default stuff is drawn </summary>
        public event Action<bool> OnDraw;

        public int StartWidth;
        public int StartHeight;
        private string _Title;
        public string Title;
        public bool Resizeable;
        private bool _Resizeable;
        public Window(string title, int width, int height) {
            StartWidth = width;
            StartHeight = height;
            Title = title;
            mainComponent = new WindowComponent();
        }

        public float curScrollX = 0;
        public float curScrollY = 0;

        public int mouseX = 0;
        public int mouseY = 0;

        public Accessibility.MessageBoxIcon Icon = Accessibility.MessageBoxIcon.cModLoader;
        private Accessibility.MessageBoxIcon _Icon = Accessibility.MessageBoxIcon.None;


        // update code
        private void Update() {
            if (!didOpen) {
                nativeType.Open(this);
                didOpen = true;
            }

            if (_Resizeable != Resizeable)
                nativeType.Set_Resizable(Resizeable);
            _Resizeable = Resizeable;

            if (_Title != Title) nativeType.Set_Title(Title);
            _Title = Title;

            if (_Icon != Icon) Set_WindowIcon(Icon);
            _Icon = Icon;

            if (!shouldClose) {
                nativeType.Update(this);
            }
            if (shouldClose) {
                nativeType.Close(this);
                nativeType = null;
            }
        }

        /// <summary>
        /// Set the window to be opened. Non-<paramref name="blocking"/> windows do not work with SDL.<br/>
        /// If <paramref name="blocking"/> is <see langword="true"/>, this can only be called on the main thread and will start updating immediately until closed.<br/>
        /// If <paramref name="blocking"/> is <see langword="false"/>, this window will be updated on the main thread either by Terraria or cModLauncher.<br/>
        /// <para><see langword="⚠ Blocking Windows.Forms create a new thread."/></para>
        /// </summary>
        public void Open(bool blocking = true, bool forceSDL = false) {
            var doSdl = Accessibility.ShouldUseSDL || forceSDL;
            // only check for SDL, window's Forms require a thread or Terraria continues to render (Draw is called but not Update), this is bad because the original Draw call never finishes since we stop it.
            if (doSdl && (cModLoaderInitializer.IsMainThread && blocking)) {
                Output.Error("Attempted to open a blocking SDL window on a non-main thread. This is not allowed.");
                return;
            }
            if (!shouldClose && ActiveWindows.Contains(this)) { // if open and exist in array
                Output.Error("Attempted to open an already open window. This is not allowed.");
                return;
            }
            shouldClose = false;
            IsBlocking = blocking;
            if (doSdl) {
                OpenSDL(blocking);
            } else {
                OpenForm(blocking);
            }
        }
        private void OpenSDL(bool blocking) {
            UsingSDL = true;
            if (Accessibility.PreferWindow == Accessibility.WindowType.SDL2 || OS.IsSDL2) {
                if (blocking) {
                    FNA.FNA_SDL2.SDL_ShowSimpleMessageBox(FNA.FNA_SDL2.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "SDL2 Error", "SDL2 custom windows are not implemented.\nReason: SDL2 requires a custom text renderer.", IntPtr.Zero);
                } else {
                    Output.Error("SDL2 Error: SDL2 custom windows are not implemented.\nReason: SDL2 requires a custom text renderer.");
                }
                return;
            }
            nativeType = new SDL.SDL3_Window();
            if (blocking) {
                var sw = Stopwatch.StartNew();
                while (!shouldClose) {
                    Update();
                    if (sw.ElapsedMilliseconds < 16) {
                        Thread.Sleep(Math.Max((16 - (int)sw.ElapsedMilliseconds), 0));
                    }
                    sw.Restart();
                }
            }
            else {
                nativeType.Close(this);
                nativeType = null;
                Accessibility.Show("Non-blocking SDL windows are not implemented yet.", "Not implemented yet.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void OpenForm(bool blocking) {
            UsingSDL = false;
            if (blocking) {
                var threadEnd = false;
                var t = new Thread(() => {
                    var form = new FormWindow();
                    nativeType = form;
                    form.ticker = new System.Windows.Forms.Timer();
                    form.ticker.Interval = 16;
                    form.ticker.Tick += (o, e) => {
                        Update();
                        if (shouldClose) {
                            form.ticker.Stop();
                            form.Close();
                        }
                    };
                    form.ticker.Start();
                    form.ShowDialog();
                    threadEnd = true;
                });
                t.Start();
                while (!threadEnd) {
                    Thread.Sleep(1);
                }
            }
            else {
                var form = new FormWindow();
                nativeType = form;
                form.ticker = new System.Windows.Forms.Timer();
                form.ticker.Interval = 16;
                form.ticker.Tick += (o, e) => {
                    Update();
                    if (shouldClose) {
                        form.ticker.Stop();
                        form.Close();
                    }
                };
                form.ticker.Start();
                form.Show();
            }
        }

        // draw code
        /// <summary> Draws the elements and its children </summary>
        public void DrawElement(Component parent) {
            if (!parent.Enabled) return;
            parent.Draw(this);
            for (int i = 0; i < parent.Children.Count; i++)
            {
                DrawElement(parent.Children[i]);
            }
        }
        public void DefaultDraw() {

            Get_MousePos(out mouseX, out mouseY);

            // clear screen
            Draw_SetDrawColour(BackgroundColour.R, BackgroundColour.G, BackgroundColour.B, BackgroundColour.A);
            Draw_Clear();
            
            for (int i = 0; i < Children.Count; i++){
                DrawElement(Children[i]);
            }

            OnDraw?.Invoke(UsingSDL);

        }

        /// <summary> Draws a filled rectangle </summary>
        public void Draw_FillRect(int x, int y, int w, int h) => nativeType.Draw_FillRect(x, y, w, h);
        /// <summary> Draws a rectangle outline </summary>
        public void Draw_Rect(int x, int y, int w, int h) => nativeType.Draw_Rect(x, y, w, h);
        /// <summary> Sets the draw colour </summary>
        public void Draw_SetDrawColour(byte r, byte g, byte b, byte a = 255) => nativeType.Draw_SetDrawColour(r, g, b, a);
        /// <summary> Clears the drawing space </summary>
        public void Draw_Clear() => nativeType.Draw_Clear();
        /// <summary> Draws text.<br/>SDL has a limited characters set so some charecters wont draw. </summary>
        public void Draw_Text(int x, int y, float scale, string str, float alignX = 0f, float alignY = 0f) => nativeType.Draw_Text(x, y, scale, str, alignX, alignY);
        /// <summary> Gets mouse position <para>TODO: Change to variables so its not called a million times per draw </para> </summary>
        public void Get_MousePos(out int x, out int y) => nativeType.Get_MousePos(out x, out y);
        /// <summary> Gets if the mouse buttons are down <para>TODO: Change to variables so its not called a million times per draw </para> </summary>
        public void Get_MouseDown(out bool L, out bool R, out bool M) => nativeType.Get_MouseDown(out L, out R, out M);
        /// <summary> Sets if the window size, this does not account for any border </summary>
        public void Set_WindowSize(int w, int h) => nativeType.Set_WindowSize(w, h);
        /// <summary> Gets if the window size, this does not account for any border </summary>
        public void Get_WindowSize(out int w, out int h) => nativeType.Get_WindowSize(out w, out h);
        /// <summary> Returns if a key is pressed.<br/>This may not work in SDL, see <see cref="SDL.KeysToSDL3ScanCode(Keys)"/> if the key converts properly, most basic ones do but others dont. </summary>
        public bool Get_KeyIsDown(Keys key) => nativeType.Get_KeyIsDown(key);
        /// <summary> Gets if the window is focused</summary>
        public void Get_Focused(out bool isFocused) => nativeType.Get_Focused(out isFocused);
        // I don't actually want to cache the icons but its either i save RAM by using extra CPU power or use extra RAM and save CPU power. CPU is a priority for drawing related things.
        // i might consider making this extendable with custom user defined icons but it needs to be a very specific file type as a binary file and be exactly 32x32 [i don't remember how i created those files anyways]
        private byte[][] iconCache = new byte[8][]{
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        };
        private byte[] getIcon(Accessibility.MessageBoxIcon icon) {
            if (icon != Accessibility.MessageBoxIcon.None) {
                if (iconCache[(int)icon] == null) {
                    byte[] iconData = null;
                    string iconName = "";
                    switch (icon) {
                        case Accessibility.MessageBoxIcon.Application: iconName = "cModLoader.Resources.Application.bin"; break;
                        case Accessibility.MessageBoxIcon.Error: iconName = "cModLoader.Resources.Error.bin"; break;
                        case Accessibility.MessageBoxIcon.Information: iconName = "cModLoader.Resources.Information.bin"; break;
                        case Accessibility.MessageBoxIcon.Question: iconName = "cModLoader.Resources.Question.bin"; break;
                        case Accessibility.MessageBoxIcon.Shield: iconName = "cModLoader.Resources.Shield.bin"; break;
                        case Accessibility.MessageBoxIcon.Warning: iconName = "cModLoader.Resources.Warning.bin"; break;
                        case Accessibility.MessageBoxIcon.cModLoader: iconName = "cModLoader.Resources.cModLoader.bin"; break;
                    }
                    using (Stream stream = iconName == "" ? null : Assembly.GetExecutingAssembly().GetManifestResourceStream(iconName))
                    using (MemoryStream ms = new MemoryStream()) {
                        int width = 32;
                        int height = 32;
                        iconData = new byte[width * height * 4];
                        stream?.Read(iconData, 0, iconData.Length);
                        iconCache[(int)icon] = iconData;
                    }
                }
            }
            return iconCache[(int)icon];
        }
        /// <summary> Draws a default icon from <see cref="Accessibility.MessageBoxIcon"/> </summary>
        public void Draw_DefaultIcon(Accessibility.MessageBoxIcon icon, int x, int y) => nativeType.Draw_DefaultIcon(getIcon(icon), x, y);
        private void Set_WindowIcon(Accessibility.MessageBoxIcon icon) => nativeType.Set_WindowIcon(getIcon(Icon = icon));
    }
}
