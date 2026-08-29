using cModLoader.ModComponents;
using cModLoader.Patching;
using cModLoader.Utils;
using cModLoader.Window;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.SqlServer.Server;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mono.Cecil;
using Mono.Cecil.Pdb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Linq;
using static cModLoader.Accessibility;
using static System.Windows.Forms.AxHost;


namespace cModLoader.UI {

    /// <summary> Copy of Terraria's StyleDimension, similar to this <see cref="UIUtils.ModernReferences.StyleDimension_Reference"/> but not only for the modern UI.<para>Developer note: we can not put instance functions in here that modify values because the getters and setters in <see cref="cUIElement"/> wont work.</para></summary>
    public struct Positioning {
        public static Positioning Fill = new Positioning(0f, 1f);
        public static Positioning Empty = new Positioning(0f, 0f);
        public float Pixels;
        public float Precent;
        public Positioning(float pixels, float precent) {
            Pixels = pixels;
            Precent = precent;
        }
        public float GetValue(float containerSize) {
            return Pixels + Precent * containerSize;
        }
        public override string ToString() {
            return $"{{{Pixels},{Precent}}}";
        }
    }
    /// <summary> Copy of Terraria's CalculatedStyle, similar to this <see cref="Rectangle"/> but for floats. System.Drawing has a float rectangle but Linux doesn't like that. </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RectangleF {
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public RectangleF(float x, float y, float width, float height) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public Rectangle ToRectangle() => new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        public Vector2 Position() => new Vector2(X, Y);
        public Vector2 Center() => new Vector2(X + Width * 0.5f, Y + Height * 0.5f);
        public override string ToString() {
            return $"{{{X},{Y},{Width},{Height}}}";
        }
    
        public static RectangleF FromNative(object nativeRect) {
            // hopfuly they are aligned properly, Terraria does not use [StructLayout(LayoutKind.Sequential)]
            Type originalType = nativeRect.GetType();
            Type rectType = typeof(RectangleF);
            if (Marshal.SizeOf(originalType) != Marshal.SizeOf(rectType)) throw new Exception($"cUIElement.GetDimensions() failed. Size of {originalType.GetType().FullName} and RectangleF did not match.");
            // Allocate memory
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rectType));
            try {
                // convert struct to pointer and visa versa
                Marshal.StructureToPtr(nativeRect, ptr, false);
                return (RectangleF)Marshal.PtrToStructure(ptr, rectType);
            }
            finally {
                Marshal.FreeHGlobal(ptr);
            }
        }

    }

    /// <summary> General handler used for events.<br/><paramref name="event"/> will be the normal Terraria event type and <see langword="null"/> in the legacy UI.<br/><paramref name="listeningElement"/> will be the native object regardless of version.</summary>
    public delegate void EventReference(object @event, object listeningElement);
    /// <summary>cModLoader specific UI wrapper. This wraps Terraria's UserInterface for modern UI and <see cref="LegacyUIState"/> for legacy UI.</summary>
    public class cUserInterface {
        /// <summary>
        /// Some versions using modern UI put update and draw in the same place and dont have a dedicated update function<br/>
        /// so this is used to disable it if it fails.
        /// </summary>
        private static bool? forceDisableModernUpdate = null;

        protected Dynamic dynamicNative;
        /// <summary> Not accessible outside of the class because the type may be unknown.<br/>Ideally you should use <see cref="ModernNative"/>, <see cref="LegacyNative"/> or <see cref="GetNative{T}"/> to get the value.</summary>
        protected object native {
            set { dynamicNative = new Dynamic(value); }
        }
        /// <summary> Used instead of directly using <see cref="Terraria.VersionChecks.Using_LegacyUISystem"/>, this could be used to force modern or legacy UI but i have not tested that. </summary>
        public bool IsLegacy = false;

        /// <summary> Gets the modern Terraria UI element as a <see cref="Dynamic"/> type. </summary>
        public Dynamic ModernNative => dynamicNative;
        /// <summary> Gets the legacy UI element as a <see cref="LegacyUserInterface"/> type. </summary>
        public LegacyUserInterface LegacyNative => dynamicNative.Value as LegacyUserInterface;

        private cUIState _currentState;
        /// <summary> Gets or sets the state of the interface.</summary>
        public cUIState CurrentState {
            get => _currentState;
            set => SetState(value);
        }
        /// <summary> Sets the state of the interface to <paramref name="newState"/>.</summary>
        public void SetState(cUIState newState) {
            _currentState = newState;
            if (IsLegacy) {
                LegacyNative.CurrentState = newState.LegacyNative as LegacyUIState;
            }
            else {
                ModernNative.Invoke("SetState", newState.ModernNative.Value);
            }
        }
        /// <summary> Sets the state of the interface using a <see cref="UIMenu"/>.</summary>
        public void SetState(UIMenu menu) => SetState(menu.uiState);
        public cUserInterface() {
            IsLegacy = Terraria.VersionChecks.Using_LegacyUISystem;
            native = IsLegacy ? new LegacyUserInterface() : UIUtils.ModernReferences.UserInterface_Reference.New();
        }
    
        /// <summary> Draws and updates the UserInterface </summary>
        public void Draw(GameReference game) {
            if (IsLegacy) {
                LegacyNative.Draw(game);
            }
            else {
                ModernNative.Invoke("Recalculate");
                if (forceDisableModernUpdate == null) {
                    try {
                        ModernNative.Invoke("Update", game.gameTime);
                        forceDisableModernUpdate = false;
                    }
                    catch (Exception e) { // should be Microsoft.CSharp.RuntimeBinder.RuntimeBinderException, but we cant use Microsoft.CSharp on Linux
                        forceDisableModernUpdate = true;
                        //Accessibility.Show($"subInterface.Update Error ({e.GetType().Name})\n\n" + e.ToString());
                        Output.Print("Old modern UI versions detected. Update disabled. (forceDisableModernUpdate)");
                    }
                }
                else {
                    if (forceDisableModernUpdate == false) {
                        ModernNative.Invoke("Update", game.gameTime);
                    }
                }
                ModernNative.Invoke("Draw", game.spriteBatch, game.gameTime);
            }
        }

    }
    /// <summary>cModLoader specific UI wrapper. This is the same as <see cref="cUIElement"/> but gives access to override functions. (only 1 for now)<br/>These override functions are implemented by default for the legacy UI, this just exposes them easily.<para>Overridden functions WILL NOT run the original (in modern UI), use <see cref="Dynamic"/> or something to do that.</para></summary>
    public class cUIElementOverride : cUIElement {
        /// <summary> An intermediate stage for Terraria's UIElement DrawSelf override, this isn't needed but its easier and good for debugging.<para>Don't call this, this is public because its easier.</para></summary>
        internal static void DrawSelf_Intermediate(object terrariaElement, SpriteBatch sb) {
            var foo = new Dynamic(terrariaElement).GetValue<Action<SpriteBatch>>("DrawSelfCallback");
            foo(sb);
        }
        /// <summary> Modern UI: Adds code to the DrawSelf function in UIElement.<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/>, this runs after calculations but before anything should be draw.</summary>
        public event Action<SpriteBatch> DrawSelf {
            add {
                if (IsLegacy) LegacyNative.DrawSelfExtension += value;
                else {
                    ModernNative.SetValue("DrawSelfCallback", Delegate.Combine(ModernNative.GetValue<Action<SpriteBatch>>("DrawSelfCallback"), value));
                }
            }
            remove {
                if (IsLegacy) LegacyNative.DrawSelfExtension -= value;
                else {
                    ModernNative.SetValue("DrawSelfCallback", Delegate.Remove(ModernNative.GetValue<Action<SpriteBatch>>("DrawSelfCallback"), value));
                }
            }
        }
        public cUIElementOverride() {
            IsLegacy = Terraria.VersionChecks.Using_LegacyUISystem;
            native = IsLegacy ? new LegacyUIContainer() : UIUtils.ModernReferences.InstanceCreator.CreateUIElementWithOverrides(UIUtils.ModernReferences.UIElement_Reference.GetNativeType());
        }
    }
    
    /// <summary>cModLoader specific UI wrapper. This wraps Terraria's UIElement for modern UI and <see cref="LegacyUIContainer"/> for legacy UI. <br/>Not the same as Terraria's UIElement, this is specifically used for cModLoader as a wrapper </summary>
    public class cUIElement {
        /// <summary> 
        /// The <see cref="cUIElement"/> wrapper parent, this is not the native object's parent, although you could obtain it through this.<br/>
        /// If the native objects parent was set manually this will not update.
        /// <para>
        /// This is modified through <see cref="Append(cUIElement)"/>, <see cref="RemoveChild(cUIElement)"/> and <see cref="RemoveFromParent"/>
        /// </para>
        /// </summary>
        public cUIElement WrapperParent = null;
        /// <summary>
        /// The <see cref="cUIElement"/> wrapper children, this is not the native object's children, although you could obtain it through this.<br/>
        /// If the native objects children are changed manually this will not update.
        /// <para>
        /// This is modified through <see cref="Append(cUIElement)"/>, <see cref="RemoveChild(cUIElement)"/> and <see cref="RemoveFromParent"/>
        /// </para>
        /// </summary>
        public List<cUIElement> WrapperChildren = new List<cUIElement>();

        protected Dynamic dynamicNative;
        /// <summary> Not accessible outside of the class because the type may be unknown.<br/>Ideally you should use <see cref="ModernNative"/>, <see cref="LegacyNative"/> or <see cref="GetNative{T}"/> to get the value.</summary>
        protected object native {
            set { dynamicNative = new Dynamic(value); }
        }
        /// <summary> Used instead of directly using <see cref="Terraria.VersionChecks.Using_LegacyUISystem"/>, this could be used to force modern or legacy UI but i have not tested that. </summary>
        public bool IsLegacy = false;

        /// <summary> Gets the modern Terraria UI element as a <see cref="Dynamic"/> type. </summary>
        public Dynamic ModernNative => dynamicNative;
        /// <summary> Gets the legacy UI element as a <see cref="LegacyUIContainer"/> type. </summary>
        public LegacyUIContainer LegacyNative => dynamicNative.Value as LegacyUIContainer;
        /// <summary> Gets the UI element as <typeparamref name="T"/>, this can be useful for modern Terraria. </summary>
        public T GetNative<T>() => (T)dynamicNative.Value;

        /// <summary> Modern UI: Modifies Terraria's UIElement.Width<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public Positioning Width {
            get => IsLegacy ? LegacyNative.Width : new Positioning(
                ModernNative.GetValue<float>("Width.Pixels"),
                ModernNative.GetValue<float>("Width.Precents")
                );
            set {
                if (IsLegacy) LegacyNative.Width = value;
                else ModernNative.SetValue("Width", UIUtils.ModernReferences.StyleDimension_Reference.New(value));
                Recalculate();
            }
        }
        /// <summary> Modern UI: Modifies Terraria's UIElement.Height<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public Positioning Height {
            get => IsLegacy ? LegacyNative.Height : new Positioning(
                ModernNative.GetValue<float>("Height.Pixels"),
                ModernNative.GetValue<float>("Height.Precents")
                );
            set {
                if (IsLegacy) LegacyNative.Height = value;
                else  ModernNative.SetValue("Height", UIUtils.ModernReferences.StyleDimension_Reference.New(value));
                Recalculate();
            }
        }
        /// <summary> Modern UI: Modifies Terraria's UIElement.MaxWidth<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/><br/>
        /// This can be used to create an offset when there is no parent object.<br/>Using <see cref="Vector4"/> as <c><see langword="new"/> <see cref="Vector4"/>(<paramref name="left"/>, <paramref name="right"/>, <paramref name="top"/>, <paramref name="bottom"/>)</c><para>This will not work in modern UI versions before 1.3.1. See <see cref="UIUtils.Using_Modern_UIUsingMargin"/></para></summary>
        public Vector4 Margin {
            get {
                if (IsLegacy) new Vector4(LegacyNative.MarginLeft, LegacyNative.MarginRight, LegacyNative.MarginTop, LegacyNative.MarginBottom);
                return new Vector4(
                    ModernNative.GetValue<float>("MarginLeft"),
                    ModernNative.GetValue<float>("MarginRight"),
                    ModernNative.GetValue<float>("MarginTop"),
                    ModernNative.GetValue<float>("MarginBottom")
                );
            }
            set {
                if (IsLegacy) {
                    LegacyNative.MarginLeft = value.X;
                    LegacyNative.MarginRight = value.Y;
                    LegacyNative.MarginTop = value.Z;
                    LegacyNative.MarginBottom = value.W;
                }
                if (Terraria.VersionChecks.Using_Modern_UIUsingMargin) {
                    ModernNative.SetValue<float>("MarginLeft", value.X);
                    ModernNative.SetValue<float>("MarginRight", value.Y);
                    ModernNative.SetValue<float>("MarginTop", value.Z);
                    ModernNative.SetValue<float>("MarginBottom", value.W);
                }
                Recalculate();
            }
        }
        /// <summary> Modern UI: Modifies Terraria's UIElement.MaxWidth<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public Positioning MaxWidth {
            get => IsLegacy ? LegacyNative.MaxWidth : new Positioning(
                ModernNative.GetValue<float>("MaxWidth.Pixels"),
                ModernNative.GetValue<float>("MaxWidth.Precents")
                );
            set {
                if (IsLegacy) LegacyNative.MaxWidth = value;
                else ModernNative.SetValue("MaxWidth", UIUtils.ModernReferences.StyleDimension_Reference.New(value));
                Recalculate();
            }
        }
        /// <summary> Modern UI: Modifies Terraria's UIElement.MaxHeight<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public Positioning MaxHeight {
            get => IsLegacy ? LegacyNative.MaxHeight : new Positioning(
                ModernNative.GetValue<float>("MaxHeight.Pixels"),
                ModernNative.GetValue<float>("MaxHeight.Precents")
                );
            set {
                if (IsLegacy) LegacyNative.MaxHeight = value;
                else ModernNative.SetValue("MaxHeight", UIUtils.ModernReferences.StyleDimension_Reference.New(value));
                Recalculate();
            }
        }
        /// <summary> Modern UI: Modifies Terraria's UIElement.MinWidth<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public Positioning MinWidth {
            get => IsLegacy ? LegacyNative.MinWidth : new Positioning(
                ModernNative.GetValue<float>("MinWidth.Pixels"),
                ModernNative.GetValue<float>("MinWidth.Precents")
                );
            set {
                if (IsLegacy) LegacyNative.MinWidth = value;
                else ModernNative.SetValue("MinWidth", UIUtils.ModernReferences.StyleDimension_Reference.New(value));
                Recalculate();
            }
        }
        /// <summary> Modern UI: Modifies Terraria's UIElement.MinHeight<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public Positioning MinHeight {
            get => IsLegacy ? LegacyNative.MinHeight : new Positioning(
                ModernNative.GetValue<float>("MinHeight.Pixels"),
                ModernNative.GetValue<float>("MinHeight.Precents")
                );
            set {
                if (IsLegacy) LegacyNative.MinHeight = value;
                else ModernNative.SetValue("MinHeight", UIUtils.ModernReferences.StyleDimension_Reference.New(value));
                Recalculate();
            }
        }
        /// <summary> Modern UI: Modifies Terraria's UIElement.Top<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public Positioning Top {
            get => IsLegacy ? LegacyNative.Top : new Positioning(
                ModernNative.GetValue<float>("Top.Pixels"),
                ModernNative.GetValue<float>("Top.Precents")
                );
            set {
                if (IsLegacy) LegacyNative.Top = value;
                else ModernNative.SetValue("Top", UIUtils.ModernReferences.StyleDimension_Reference.New(value));
                Recalculate();
            }
        }
        /// <summary> Modern UI: Modifies Terraria's UIElement.Left<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public Positioning Left {
            get => IsLegacy ? LegacyNative.Left : new Positioning(
                ModernNative.GetValue<float>("Left.Pixels"),
                ModernNative.GetValue<float>("Left.Precents")
                );
            set {
                if (IsLegacy) LegacyNative.Left = value;
                else ModernNative.SetValue("Left", UIUtils.ModernReferences.StyleDimension_Reference.New(value));
                Recalculate();
            }
        }
        /// <summary> Modern UI: Modifies Terraria's UIElement.HAlign and UIElement.VAlign<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public Vector2 Alignment {
            get => IsLegacy ? new Vector2(LegacyNative.HAlign, LegacyNative.VAlign) : new Vector2(ModernNative.GetValue<float>("HAlign"), ModernNative.GetValue<float>("VAlign"));
            set {
                if (IsLegacy) {
                    LegacyNative.HAlign = value.X;
                    LegacyNative.VAlign = value.Y;
                } else {
                    ModernNative.SetValue<float>("HAlign", value.X);
                    ModernNative.SetValue<float>("VAlign", value.Y);
                }
                Recalculate();
            }
        }

        /// <summary> Modern UI: Modifies Terraria's UIElement.OverflowHidden<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public bool OverflowHidden {
            get => IsLegacy ? LegacyNative.OverflowHidden : ModernNative.GetValue<bool>("OverflowHidden");
            set {
                if (IsLegacy) {
                    LegacyNative.OverflowHidden = value;
                } else {
                    ModernNative.SetValue<bool>("OverflowHidden", value);
                }
            }
        }

        /// <summary> Gets the dimension not including margin or padding </summary>
        public RectangleF GetInnerDimensions() => IsLegacy ? LegacyNative.GetInnerDimensions() : RectangleF.FromNative(ModernNative.Invoke<object>("GetInnerDimensions"));
        /// <summary> Gets the dimension including margin and padding (padding not implemented for legacy) </summary>
        public RectangleF GetDimensions() => IsLegacy ? LegacyNative.GetDimensions() : RectangleF.FromNative(ModernNative.Invoke<object>("GetDimensions"));
        /// <summary> Recalculates the UI element. </summary>
        public void Recalculate() { if (IsLegacy) LegacyNative.Recalculate(); else ModernNative.Invoke("Recalculate"); }
        /// <summary> Sets all side's padding to <paramref name="pixels"/>. </summary>
        public void SetPadding(float pixels) { if (IsLegacy) LegacyNative.SetPadding(pixels); else ModernNative.Invoke("SetPadding", pixels); }
        /// <summary> Gets the list of children, normally Terraria does not allow for this. </summary>
        public IList GetChildren() => IsLegacy ? LegacyNative.GetChildren() : ModernNative.GetValue<IList>("Elements");
        /// <summary> Does the element contain <paramref name="point"/>. </summary>
        public bool ContainsPoint(Vector2 point) => IsLegacy ? LegacyNative.ContainsPoint(point) : ModernNative.Invoke<bool>("ContainsPoint", point);

        /// <summary> Modern UI: Modifies Terraria's UIElement.OnLeftClick event (or UIElement.OnClick in versions before 1.4.4.8)<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public event EventReference OnClick {
            add {
                if (IsLegacy) {
                    LegacyNative.OnClick += value;
                } else {
                    if (Terraria.VersionChecks.Using_Modern_RichInputs)
                        UIUtils.ModernReferences.UIElement_Reference.AddMouseEvent(this, "OnLeftClick", value);
                    else UIUtils.ModernReferences.UIElement_Reference.AddMouseEvent(this, "OnClick", value);
                }
            }
            remove {
                if (IsLegacy) {
                    LegacyNative.OnClick -= value;
                }
                else {
                    if (Terraria.VersionChecks.Using_Modern_RichInputs)
                        UIUtils.ModernReferences.UIElement_Reference.RemoveMouseEvent(this, "OnLeftClick", value);
                    else UIUtils.ModernReferences.UIElement_Reference.RemoveMouseEvent(this, "OnClick", value);
                }
            }
        }
        /// <summary> Modern UI: Modifies Terraria's UIElement.OnMouseOver event<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public event EventReference OnMouseOver {
            add {
                if (IsLegacy) LegacyNative.OnMouseEnter += value;
                else UIUtils.ModernReferences.UIElement_Reference.AddMouseEvent(this, "OnMouseOver", value);
            }
            remove {
                if (IsLegacy) LegacyNative.OnMouseEnter -= value;
                else UIUtils.ModernReferences.UIElement_Reference.RemoveMouseEvent(this, "OnMouseOver", value);
            }
        }
        /// <summary> Modern UI: Modifies Terraria's UIElement.OnMouseOver event<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public event EventReference OnMouseOut {
            add {
                if (IsLegacy) LegacyNative.OnMouseExit += value;
                else UIUtils.ModernReferences.UIElement_Reference.AddMouseEvent(this, "OnMouseOut", value);
            }
            remove {
                if (IsLegacy) LegacyNative.OnMouseExit -= value;
                else UIUtils.ModernReferences.UIElement_Reference.RemoveMouseEvent(this, "OnMouseOut", value);
            }
        }

        /// <summary> Removes a child. </summary>
        public void RemoveChild(cUIElement child) {
            WrapperChildren.Remove(child);
            child.WrapperParent = null;
            if (IsLegacy) LegacyNative.RemoveChild(child.LegacyNative);
            else ModernNative.Invoke("RemoveChild", child.ModernNative.Value);
        }
        /// <summary> Removes this object from its parent. </summary>
        public void RemoveFromParent() {
            if (WrapperParent != null) WrapperParent.RemoveChild(this);
            if (IsLegacy) LegacyNative.RemoveFromParent();
            else ModernNative.Invoke("Remove");
        }
        /// <summary> Appends a new child. </summary>
        public void Append(cUIElement child) {
            child.RemoveFromParent();
            child.WrapperParent = null;
            WrapperChildren.Add(child);
            if (IsLegacy) LegacyNative.Append(child.LegacyNative);
            else ModernNative.Invoke("Append", child.ModernNative.Value);
        }


        public cUIElement() {
            IsLegacy = Terraria.VersionChecks.Using_LegacyUISystem;
            native = IsLegacy ? new LegacyUIContainer() : UIUtils.ModernReferences.UIElement_Reference.New();
        }
        public cUIElement(object nativeElement) {
            IsLegacy = Terraria.VersionChecks.Using_LegacyUISystem;
            native = nativeElement;
        }
    }
    /// <summary>cModLoader specific UI wrapper. This wraps Terraria's UIElement but simulates UIPanel for modern UI and <see cref="LegacyUIContainer"/> for legacy UI. <br/>Not the same as Terraria's UITextPanel, this is specifically used for cModLoader as a wrapper </summary>
    public class cUIPanel : cUIElementOverride {
        public Texture2D _borderTexture = null;
        public Texture2D _backgroundTexture = null;
        /// <summary> Draws a panel using <paramref name="texture"/>. This is copied from Terraria's UIPanel.<br/>Leave <paramref name="color"/> <see langword="null"/> for <see cref="Color.White"/>.</summary>
        public static void DrawPanel(cUIElement element, SpriteBatch spriteBatch, Texture2D texture, Color? _color = null, int _cornerSize = 12, int _barSize = 4) {
            var color = _color ?? Color.White;

            var dimensions = element.GetDimensions();
            Point point = new Point((int)dimensions.X, (int)dimensions.Y);
            Point point2 = new Point(point.X + (int)dimensions.Width - _cornerSize, point.Y + (int)dimensions.Height - _cornerSize);
            int width = point2.X - point.X - _cornerSize;
            int height = point2.Y - point.Y - _cornerSize;
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, _cornerSize, _cornerSize), new Rectangle(0, 0, _cornerSize, _cornerSize), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, _cornerSize, _cornerSize), new Rectangle(_cornerSize + _barSize, 0, _cornerSize, _cornerSize), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, _cornerSize, _cornerSize), new Rectangle(0, _cornerSize + _barSize, _cornerSize, _cornerSize), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, _cornerSize, _cornerSize), new Rectangle(_cornerSize + _barSize, _cornerSize + _barSize, _cornerSize, _cornerSize), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y, width, _cornerSize), new Rectangle(_cornerSize, 0, _barSize, _cornerSize), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point2.Y, width, _cornerSize), new Rectangle(_cornerSize, _cornerSize + _barSize, _barSize, _cornerSize), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + _cornerSize, _cornerSize, height), new Rectangle(0, _cornerSize, _cornerSize, _barSize), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + _cornerSize, _cornerSize, height), new Rectangle(_cornerSize + _barSize, _cornerSize, _cornerSize, _barSize), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y + _cornerSize, width, height), new Rectangle(_cornerSize, _cornerSize, _barSize, _barSize), color);
        }

        /*
        /// <summary> Modern UI: Gets or sets Terraria's UIPanel.BackgroundColor<br/>Legacy UI: Does nothing for now.</summary>
        public Color BackgroundColor {
            get {
                if (IsLegacy) return Color.Transparent;
                return ModernNative.GetValue<Color>("BackgroundColor");
            }
            set {
                if (IsLegacy) return;
                ModernNative.SetValue<Color>("BackgroundColor", value);
            }
        }
        /// <summary> Modern UI: Gets or sets Terraria's UIPanel.BorderColor<br/>Legacy UI: Does nothing for now.</summary>
        public Color BorderColor {
            get {
                if (IsLegacy) return Color.Transparent;
                return ModernNative.GetValue<Color>("BorderColor");
            }
            set {
                if (IsLegacy) return;
                ModernNative.SetValue<Color>("BorderColor", value);
            }
        }
        */
        public Color BackgroundColor = Terraria.Colour.UIBackgroundTransparent;
        public Color BorderColor = Color.Black;

        public cUIPanel() : base() {
            if (_borderTexture == null) {
                if (Terraria.VersionChecks.Using_RelogicAssets)
                    _borderTexture = Terraria.Relogic.Asset<Texture2D>("Images/UI/PanelBorder").Value;
                else _borderTexture = Terraria.Textures.LoadTexture("Images/UI/PanelBorder");
            }
            if (_backgroundTexture == null) {
                if (Terraria.VersionChecks.Using_RelogicAssets)
                    _backgroundTexture = Terraria.Relogic.Asset<Texture2D>("Images/UI/PanelBackground").Value;
                else _backgroundTexture = Terraria.Textures.LoadTexture("Images/UI/PanelBackground");
            }
            SetPadding(12f);
            //native = IsLegacy ? new LegacyUIContainer() : UIUtils.ModernReferences.UIPanel_Reference.New();
            if (!IsLegacy) DrawSelf += _DrawSelf;
        }
        protected virtual void _DrawSelf(SpriteBatch sb) {
            DrawPanel(this, sb, _backgroundTexture, BackgroundColor);
            DrawPanel(this, sb, _borderTexture, BorderColor);
        }
    }
    /// <summary>cModLoader specific UI wrapper. This wraps Terraria's UITextPanel for modern UI and <see cref="LegacyUIText"/> for legacy UI.<br/>T (<typeparamref name="T"/>) must be a <see cref="string"/> in all versions below 1.3.4 otherwise idk what happens. <br/>Not the same as Terraria's UITextPanel, this is specifically used for cModLoader as a wrapper </summary>
    public class cUITextPanel<T> : cUIPanel {
        //internal static FieldInfoCache _colour_Cache = new FieldInfoCache("_color");
        //internal static FieldInfoCache _textField_Cache = new FieldInfoCache("_text");
        //internal static FieldInfoCache _textScaleField_Cache = new FieldInfoCache("_textScale");
        //internal static FieldInfoCache _isLargeField_Cache = new FieldInfoCache("_isLarge");
        // stored but not used, this ends up being called through cModLoader's dynamic type
        internal static MethodInfo SetText_Cache = null;

        /// <summary> Uses a lot of reflection in modern UIs, use sparingly. </summary>
        public T Text {
            get {
                // i cant just cast string to T... and we cant use dynamic anymore
                if (IsLegacy) return (T)Convert.ChangeType((LegacyNative as LegacyUIText).Text, typeof(T));
                // older versions don't make this accessible so we are just going to always use reflections
                return ModernNative.GetValue<T>("_text");
            }
            set {
                if (IsLegacy) { (LegacyNative as LegacyUIText).Text = (value as string); return; };
                // older versions don't make this accessible so we are just going to always use reflections
                if (SetText_Cache == null) {
                    // SetText(string text)
                    // SetText(string text, float textScale, bool large)
                    // SetText(T text, float textScale, bool large)
                    // 3 different functions could exist we want the one with T and 3 parameters
                    var foo = ModernNative.Value.GetType().GetMethods().Where(x => x.Name == "SetText" && x.GetParameters().Length == 3).ToArray();
                    if (foo.Length == 1) {
                        SetText_Cache = foo[0];
                    } else {
                        var foo2 = foo.Where(x => x.GetParameters()[0].ParameterType.IsGenericType).ToArray();
                        if (foo2.Length != 1) {
                            throw new Exception("Set text cUITextPanel<T> failed, could not find 1 \"SetText\" function (" + foo2.Length + ").");
                        }
                        SetText_Cache = foo2[0];
                    }
                    ModernNative.OverrideCachedMethod("SetText", SetText_Cache.GetParameters().Select(x => x.ParameterType).ToArray(), SetText_Cache);
                }
                float v2 = ModernNative.GetValue<float>("_textScale");
                bool v3 = ModernNative.GetValue<bool>("_isLarge");
                if (Terraria.VersionChecks.Using_Modern_UITextPanelUsingGeneric)
                    ModernNative.Invoke("SetText", value, v2, v3);
                else
                    ModernNative.Invoke("SetText", (value as string), v2, v3);
            }
        }

        /// <summary> Sets the text colour of the given element. </summary>
        public Color TextColour {
            get {
                if (IsLegacy) return (LegacyNative as LegacyUIText).StartColor;
                else {
                    if (Terraria.VersionChecks.Using_Modern_UITextColour) return ModernNative.GetValue<Color>("_color");
                    else return Color.Transparent;
                }
            }
            set {
                if (IsLegacy) {
                    // if they are the same change both
                    if ((LegacyNative as LegacyUIText).StartColor == (LegacyNative as LegacyUIText).EndColor)
                        (LegacyNative as LegacyUIText).EndColor = value;
                    (LegacyNative as LegacyUIText).StartColor = value;
                }
                else {
                    if (Terraria.VersionChecks.Using_Modern_UITextColour)
                        ModernNative.SetValue<Color>("_color", value);
                }
            }
        }

        public cUITextPanel(T text, float scale, bool big, bool isButton = false) : base() {
            if (!IsLegacy) {
                native = UIUtils.ModernReferences.UITextPanel_Reference.New(text, scale, big);
                BackgroundColor = isButton ? Terraria.Colour.UIBackgroundTransparent : Terraria.Colour.UIBackgroundSolid;
                if (isButton) {
                    OnMouseOver += (e, o) => UIUtils.ModernReferences.DefaultButtonMouseEnter(o);
                    OnMouseOut += (e, o) => UIUtils.ModernReferences.DefaultButtonMouseExit(o);
                }
            } else {
                native = new LegacyUIText(text as string, big, scale, isButton ? scale + 0.1f : scale, Color.White, Color.White * (isButton ? 0.5f : 1f), true);
            }
        }
    }
    /// <summary>cModLoader specific UI wrapper. This wraps Terraria's UIState for modern UI and <see cref="LegacyUIState"/> for legacy UI.</summary>
    public class cUIState : cUIElement {
        public cUIState() {
            if (!IsLegacy)
                native = UIUtils.ModernReferences.UIState_Reference.New();
            else
                native = new LegacyUIState();

        }
    }
    /// <summary>cModLoader specific UI wrapper. This wraps Terraria's UIText for modern UI and <see cref="LegacyUIText"/> for legacy UI.</summary>
    public class cUIText : cUIElement {
        /// <summary> Uses a lot of reflection in modern UIs, use sparingly. </summary>
        public string Text {
            get {
                if (IsLegacy) return (LegacyNative as LegacyUIText).Text;
                // older versions don't make this accessible so we are just going to always use reflections
                return ModernNative.GetValue<string>("_text"); // could be string or LocalizedText, LocalizedText can be converted to string
            }
            set {
                if (IsLegacy) { (LegacyNative as LegacyUIText).Text = value; return; }
                float v2 = ModernNative.GetValue<float>("_textScale");
                bool v3 = ModernNative.GetValue<bool>("_isLarge");
                ModernNative.Invoke("SetText", value, v2, v3);
            }
        }
        /// <summary> Sets the text colour of the given element. </summary>
        public Color TextColour {
            get {
                if (IsLegacy) return (LegacyNative as LegacyUIText).StartColor;
                else {
                    if (Terraria.VersionChecks.Using_Modern_UITextColour) return ModernNative.GetValue<Color>("_color");
                    else return Color.Transparent;
                }
            }
            set {
                if (IsLegacy) {
                    // if they are the same change both
                    if ((LegacyNative as LegacyUIText).StartColor == (LegacyNative as LegacyUIText).EndColor)
                        (LegacyNative as LegacyUIText).EndColor = value;
                    (LegacyNative as LegacyUIText).StartColor = value;
                }
                else {
                    if (Terraria.VersionChecks.Using_Modern_UITextColour)
                        ModernNative.SetValue<Color>("_color", value);
                }
            }
        }
        public cUIText(string text, float textScale = 1f, bool large = false) {
            native = IsLegacy ? new LegacyUIText(text, large, textScale, textScale, Color.White, Color.White, true) : UIUtils.ModernReferences.UIText_Reference.New(text, textScale, large);
            if (IsLegacy) (LegacyNative as LegacyUIText).FitToText = true;
        }
    }
    /// <summary>cModLoader specific UI wrapper. This wraps Terraria's UIImageButton for modern UI and <see cref="LegacyUIText"/> for legacy UI.</summary>
    public class cUIImageButton : cUIElement {
        public cUIImageButton(ReLogicAsset<Texture2D> texture) {
            if (!IsLegacy)
                native = UIUtils.ModernReferences.UIImageButton_Reference.New(texture);
            else throw new NotImplementedException();
        }
    }
    /// <summary>cModLoader specific UI wrapper. This wraps Terraria's UIPanel for modern UI and <see cref="LegacyUIContainer"/> for legacy UI. <br/>Not the same as Terraria's UITextPanel, this is specifically used for cModLoader as a wrapper </summary>
    public class cUIList : cUIElement {
        /// <summary>
        /// The <see cref="cUIList"/> wrapper children, this is not the native object's children, although you could obtain it through this.<br/>
        /// If the native objects children are changed manually this will not update.
        /// <para>
        /// This is modified through <see cref="Add(cUIElement)"/> and <see cref="Remove(cUIElement)"/>
        /// </para>
        /// </summary>
        public List<cUIElement> WrapperListItems = new List<cUIElement>();
        /// <summary> Modern UI: Modifies Terraria's UIElement.ListPadding<br/>Legacy UI: Does the same as modern but for <see cref="LegacyUIContainer"/></summary>
        public float ListPadding {
            get {
                if (IsLegacy) return (LegacyNative as LegacyUIScroll).ListPadding;
                return ModernNative.GetValue<float>("ListPadding");
            }
            set {
                if (IsLegacy) {
                    (LegacyNative as LegacyUIScroll).ListPadding = value;
                    return;
                }
                ModernNative.SetValue<float>("ListPadding", value);
            }
        }
        
        public void SetScrollbar(cUIScrollbar scrollBar) {
            if (IsLegacy) return;
            ModernNative.Invoke("SetScrollbar", scrollBar.ModernNative.Value);
        }

        public cUIList() {
            if (!IsLegacy) native = UIUtils.ModernReferences.UIList_Reference.New();
            else native = new LegacyUIScroll();
        }

        /// <summary> Adds an element to the scroll list. This is different from <see cref="cUIElement.Append(cUIElement)"/> as it does not add it as a normal child.</summary>
        public void Add(cUIElement element) {
            if (element == null) return;
            WrapperListItems.Add(element);
            if (IsLegacy) (LegacyNative as LegacyUIScroll).Add(element.LegacyNative);
            else {
                // manual calculation because we don't want UpdateOrder();
                ModernNative.GetValue<object>("_items").AsDynamic().Invoke("Add", element.ModernNative.Value);
                var _innerList = ModernNative.GetValue<object>("_innerList").AsDynamic();
                _innerList.Invoke("Append", element.ModernNative.Value);
                _innerList.Invoke("Recalculate");
            }
        }
        /// <summary> Removes an element to the scroll list. This is different from <see cref="cUIElement.RemoveChild(cUIElement)"/> as it does not remove it as a normal child.</summary>
        public bool Remove(cUIElement element) {
            WrapperListItems.Remove(element);
            if (IsLegacy) return (LegacyNative as LegacyUIScroll).Remove(element.LegacyNative);
            else {
                // manual calculation because we don't want UpdateOrder();
                ModernNative.GetValue<object>("_innerList").AsDynamic().Invoke("RemoveChild", element.ModernNative.Value);
                return ModernNative.GetValue<object>("_items").AsDynamic().Invoke<bool>("Remove", element.ModernNative.Value);
            }
        }

    }
    /// <summary>cModLoader specific UI wrapper. This wraps Terraria's cUIScrollbar for modern UI. This is not implemented and will create a placeholder <see cref="LegacyUIContainer"/> for legacy UI.</summary>
    public class cUIScrollbar : cUIElement {
        public void SetView(float viewSize, float maxViewSize) {
            if (IsLegacy) return;
            ModernNative.Invoke("SetView", viewSize, maxViewSize);
        }
        public float GetView() {
            if (IsLegacy) return 0f;
            return ModernNative.Invoke<float>("GetView");
        }
        public cUIScrollbar() {
            if (!IsLegacy) native = UIUtils.ModernReferences.UIScrollbar_Reference.New();
            else native = new LegacyUIContainer();
        }
    }
    /// <summary>cModLoader specific UI wrapper. This wraps Terraria's UIColoredSlider for modern UI above and including 1.4 and <see cref="LegacyUIContainer"/> for legacy versions.<br/>Throws an error on version 1.3 to 1.3.5.3</summary>
    public class cUIColoredSlider : cUIElement {
        private float _value;
        /// <summary> Gets or sets the slider value.</summary>
        public float Value {
            get => GetSliderValue();
            set => SetSliderValue(value);
        }
        public Color startColour;
        public Color endColour;
        /// <summary> Called when the value changes. </summary>
        public Action<float> OnValueChange;
        /// <summary> Returns the colour of the point percentage on the slider. By default this just lerps between <see cref="startColour"/> and <see cref="endColour"/><br/>Not used for legacy UI.</summary>
        public Func<float, Color> SliderColour;

        /// <summary> <paramref name="startColour"/> and <paramref name="endColour"/> are ignored for legacy UI.</summary>
        public cUIColoredSlider(Color startColour, Color endColour, float value) {
            native = IsLegacy ? new LegacyUINumericUpDown(GetSliderValue, SetSliderValue) : (Terraria.VersionChecks.Using_Modern_UISliders ? UIUtils.ModernReferences.UIColoredSlider_Reference.New(GetSliderValue, SetSliderValue, _SetSliderGamepad, _SliderColour) : throw new Exception("Non UIColoredSlider class For Modern UI not implemented."));
            Value = value;
            this.startColour = startColour;
            this.endColour = endColour;
            SliderColour = DefaultSliderColour;
        }
        private Color DefaultSliderColour(float position) => Color.Lerp(startColour, endColour, position);
        internal Color _SliderColour(float position) => SliderColour == null ? startColour : SliderColour.Invoke(position);
        internal void _SetSliderGamepad() { }
        /// <summary> Gets the slider value, this is internal used for Terraria slider class. </summary>
        public virtual float GetSliderValue() => _value;
        /// <summary> Sets the slider value, this is internal used for Terraria slider class. </summary>
        public virtual void SetSliderValue(float newValue) {
            _value = newValue;
            OnValueChange?.Invoke(newValue);
        }
       
    }
}
