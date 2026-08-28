using cModLoader.Patching;
using cModLoader.Window;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.CompilerServices;
using cModLoader.Utils;


namespace cModLoader.UI
{
    /// <summary> Utils for UI related stuff. If you are modding older versions make sure you use its respective system for UI </summary>
    public static class UIUtils {

        /// <summary> Draws bounds of elements using legacy UI, this can be used to testing or whatever. This only works on <see cref="LegacyUIContainer"/> and overriding classes. </summary>
        public static bool Test_DrawBoundsLegacyUI = false;

        private static Dynamic TerrariaUtils = null;
        //private static Func<SpriteBatch, string, Vector2, Color, float, float, float, int, Vector2> DrawBorderString_Cache = null;
        //private static Func<SpriteBatch, string, Vector2, Color, float, float, float, int, Vector2> DrawBorderStringBig_Cache = null;
        /// <summary> Calls <see cref="DrawString(SpriteBatch, string, Vector2, Color, float, Vector2, bool)"/> </summary>
        public static void DrawString(SpriteBatch sb, string str, Vector2 pos, Color color, float scale, bool big = false) => DrawString(sb, str, pos, color, scale, new Vector2(0, 0), big);
        /// <summary> Draws a string using Terraria's string drawing method.<br/>This will NOT check if the font system is initialized, use <see cref="DrawSafeString(GameReference, string, Vector2, Color, float, Vector2)"/> for that. </summary>
        public static void DrawString(SpriteBatch sb, string str, Vector2 pos, Color color, float scale, Vector2 origin, bool big = false) {
            if (Terraria.VersionChecks.Using_UtilsTextDrawing) {
                if (TerrariaUtils == null) {
                    TerrariaUtils = new Dynamic(Terraria.GetType("Terraria.Utils"));
                }
                // could return void or Vector2, 1.4.5.7 changed it to void
                TerrariaUtils.Invoke(!big ? "DrawBorderString" : "DrawBorderStringBig", sb, str, pos, color, scale, 0f, 0f, -1);
            } else {
                var f = !big ? LegacyFontSystem.getFontMouseText() : LegacyFontSystem.getFontDeathText();
                sb.DrawString(f, str, new Vector2(pos.X + -2, pos.Y + 0), Color.Black, 0, origin, scale, SpriteEffects.None, 0);
                sb.DrawString(f, str, new Vector2(pos.X + 2, pos.Y + 0), Color.Black, 0, origin, scale, SpriteEffects.None, 0);
                sb.DrawString(f, str, new Vector2(pos.X + 0, pos.Y + 2), Color.Black, 0, origin, scale, SpriteEffects.None, 0);
                sb.DrawString(f, str, new Vector2(pos.X + 0, pos.Y + -2), Color.Black, 0, origin, scale, SpriteEffects.None, 0);
                sb.DrawString(f, str, new Vector2(pos.X, pos.Y), color, 0, origin, scale, SpriteEffects.None, 0);
            }
        }

        private static object _safeFontMouseText = null;
        private static MethodInfo DrawString_Cache = null;
        /// <summary> Safely draws a string by loading the fonts instead of relying on Terraria, draws like normal in versions using the legacy font system and draws using the ReLogic.dll with the modern font system.<br/>Used to draw before fonts are loaded.</summary>
        public static void DrawSafeString(GameReference game, string str, Vector2 pos, Color color, float scale, Vector2 origin) {
            if (_safeFontMouseText == null) {
                if (Terraria.VersionChecks.Using_LegacyFontSystem) {
                    _safeFontMouseText = game.game.Content.Load<SpriteFont>("Fonts\\Mouse_Text");
                    if (_safeFontMouseText == null) throw new Exception("Failed to load SpriteFont object \"Fonts\\Mouse_Text\".");
                } else {
                    _safeFontMouseText = game.game.Content.Load<object>("Fonts\\Mouse_Text");
                    if (_safeFontMouseText == null) throw new Exception("Failed to load DynamicSpriteFont object \"Fonts\\Mouse_Text\".");
                    DrawString_Cache = cModLoaderInitializer.LoadedAssembilies["ReLogic"].GetType("ReLogic.Graphics.DynamicSpriteFontExtensionMethods").GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => {
                        if (m.Name != "DrawString") return false;
                        var p = m.GetParameters();
                        return p.Length == 10 && p[1].ParameterType.Name == "DynamicSpriteFont" && p[2].ParameterType == typeof(string) && p[3].ParameterType.Name == "Vector2" && p[4].ParameterType.Name == "Color" && p[5].ParameterType == typeof(float) && p[6].ParameterType.Name == "Vector2" && p[7].ParameterType.Name == "Vector2" && p[8].ParameterType.Name == "SpriteEffects" && p[9].ParameterType == typeof(float);
                    });
                    if (DrawString_Cache == null) throw new Exception("Failed to find method \"ReLogic.Graphics.DynamicSpriteFontExtensionMethods.DrawString\".");
                }
            }
            var sb = game.spriteBatch;
            if (Terraria.VersionChecks.Using_LegacyFontSystem) {
                var sf = (SpriteFont)_safeFontMouseText;
                sb.DrawString(sf, str, new Vector2(pos.X + -2, pos.Y + 0), Color.Black, 0, origin, scale, SpriteEffects.None, 0);
                sb.DrawString(sf, str, new Vector2(pos.X + 2, pos.Y + 0), Color.Black, 0, origin, scale, SpriteEffects.None, 0);
                sb.DrawString(sf, str, new Vector2(pos.X + 0, pos.Y + 2), Color.Black, 0, origin, scale, SpriteEffects.None, 0);
                sb.DrawString(sf, str, new Vector2(pos.X + 0, pos.Y + -2), Color.Black, 0, origin, scale, SpriteEffects.None, 0);
                sb.DrawString(sf, str, new Vector2(pos.X, pos.Y), color, 0, origin, scale, SpriteEffects.None, 0);
            } else {
                DrawString_Cache.Invoke(null, new object[] { sb, _safeFontMouseText, str, pos, color, 0f, origin, new Vector2(scale, scale), SpriteEffects.None, 0 });
            }
        }

        /// <summary> Used for version using legacy font system.</summary>
        public class LegacyFontSystem {
            private static SpriteFont _fontMouseText = null;
            private static SpriteFont _fontDeathText = null;
            public static SpriteFont getFontMouseText() => _fontMouseText ?? (_fontMouseText = (SpriteFont)Terraria._Main.Get("fontMouseText"));
            public static SpriteFont getFontDeathText() => _fontDeathText ?? (_fontDeathText = (SpriteFont)Terraria._Main.Get("fontDeathText"));
        }
        /// <summary> Used for version using modern font system.</summary>
        public class ModernFontSystem {
            private static object _fontMouseText = null;
            private static object _fontDeathText = null;
            public static object getFontMouseText() => _fontMouseText ?? (_fontMouseText = new ReLogicAsset<object>(Terraria.TerrariaAsm.GetType("Terraria.GameContent.FontAssets").GetField("MouseText").GetValue(null)).Value);
            public static object getFontDeathText() => _fontDeathText ?? (_fontDeathText = new ReLogicAsset<object>(Terraria.TerrariaAsm.GetType("Terraria.GameContent.FontAssets").GetField("DeathText").GetValue(null)).Value);
        }

        /// <summary> Used to reference modern UI, for versions 1.3 and up. See <see cref="Using_LegacyUISystem"/> </summary>
        public class ModernReferences {
            // had help from AI (google could have given me the same stuff if it would stop giving me the most abysmal results that have nothing to do with what i asked for)
            public static class InstanceCreator {
                private static AssemblyBuilder _asmBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("cModLoaderDynamicAsm"), AssemblyBuilderAccess.Run);
                private static ModuleBuilder _moduleBuilder = _asmBuilder.DefineDynamicModule("DynamicModule");
                private static Dictionary<string, Type> _cachedTypes = new Dictionary<string, Type>();

                /// <summary>
                /// <para>
                /// <see langword="⚠ Only works on public classes"/>
                /// </para>
                /// Returns an instance of the type you provided but with a field called "DrawSelfCallback" which is called by the original DrawSelf function, use <see cref="cUIElement.DrawSelf"/> to set this.<br/>
                /// <paramref name="baseType"/> must inherit Terraria's UIElement.<br/>
                /// This is used for <see cref="ModListItem"/> to draw stuff.
                /// <para>
                /// Types are cached so calling this multiple times on the same type will return the same type (as a new instance).<br/>
                /// Technically DrawSelf will call <see cref="cUIElement.DrawSelf_Intermediate(object, SpriteBatch)"/> which will then call "DrawSelfCallback" (or whatever <see cref="cUIElement.DrawSelf"/> is).
                /// </para>
                /// </summary>
                public static object CreateUIElementWithOverrides(Type baseType) {
                    var typeName = $"cModLoaderCustom_{baseType.Name}";
                    if (_cachedTypes.TryGetValue(typeName, out Type t)) {
                        return Activator.CreateInstance(t);
                    }
                    // create type
                    var typeBuilder = _moduleBuilder.DefineType(typeName, TypeAttributes.Public, baseType);
                    // create method
                    var baseMethod = baseType.GetMethod("DrawSelf", BindingFlags.NonPublic | BindingFlags.Instance);
                    var param = baseMethod.GetParameters().Select(p => p.ParameterType).ToArray();
                    var methodBuilder = typeBuilder.DefineMethod(baseMethod.Name + "_", MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig, baseMethod.ReturnType, param);
                    // create event
                    var callbackField = typeBuilder.DefineField("DrawSelfCallback", typeof(Action<SpriteBatch>), FieldAttributes.Public);
                    
                    var callMethod = typeof(cUIElementOverride).GetMethod(nameof(cUIElementOverride.DrawSelf_Intermediate), BindingFlags.NonPublic | BindingFlags.Static);
                    
                    var il = methodBuilder.GetILGenerator();
                    // call original
                    // il.Emit(OpCodes.Ldarg_0);
                    // il.Emit(OpCodes.Ldarg_1);
                    // il.Emit(OpCodes.Call, baseMethod);
                    // modify IL to call our function
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Call, callMethod);
                    il.Emit(OpCodes.Ret);

                    // set function as override
                    typeBuilder.DefineMethodOverride(methodBuilder, baseMethod);

                    // actually create type
                    var result = typeBuilder.CreateType();
                    _cachedTypes[typeName] = result;
                    return Activator.CreateInstance(result);
                }
            }
            public static class UIElement_Reference {
                private const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
                /// <summary> Cache type. </summary>
                private static Type Type_Cache = null;
                /// <summary> Gets the native Terraria type name. </summary>
                public static string GetNativeName() => "Terraria.UI.UIElement";
                /// <summary> Gets the native Terraria type. </summary>
                public static Type GetNativeType() => Type_Cache == null ? (Type_Cache = Terraria.TerrariaAsm.GetType(GetNativeName())) : Type_Cache;
                /// <summary> Create a new instance of the native Terraria type. </summary>
                public static object New() => Activator.CreateInstance(GetNativeType());
                /// <summary> Adds a given function to an event, returns <see langword="false"/> if failed or <see langword="true"/> if success</summary>
                public static bool AddMouseEvent(cUIElement UIElement, string eventName, MethodInfo method) {
                    if (UIElement == null) { Output.Error("Failed to AddMouseEvent, UIElement was null."); return false; }
                    var eventInfo = (UIElement.ModernNative.Value).GetType().GetEvent(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    if (eventInfo == null) { Output.Error("Failed to AddMouseEvent, eventInfo was null."); return false; }
                    var handlerType = eventInfo.EventHandlerType;
                    if (handlerType == null) { Output.Error("Failed to AddMouseEvent, handlerType was null."); return false; }
                    if (method == null) { Output.Error("Failed to AddMouseEvent, method was null."); return false; }
                    var addMouseEvent = Delegate.CreateDelegate(handlerType, UIElement, method);
                    if (addMouseEvent == null) { Output.Error("Failed to AddMouseEvent, addMouseEvent was null."); return false; }
                    eventInfo.AddEventHandler(UIElement.ModernNative.Value, addMouseEvent);
                    return true;
                }
                /// <summary> Removes a given function to an event, returns <see langword="false"/> if failed or <see langword="true"/> if success</summary>
                public static bool RemoveMouseEvent(cUIElement UIElement, string eventName, MethodInfo method) {
                    if (UIElement == null) { Output.Error("Failed to RemoveMouseEvent, UIElement was null."); return false; }
                    var eventInfo = (UIElement.ModernNative.Value).GetType().GetEvent(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    if (eventInfo == null) { Output.Error("Failed to RemoveMouseEvent, eventInfo was null."); return false; }
                    var handlerType = eventInfo.EventHandlerType;
                    if (handlerType == null) { Output.Error("Failed to RemoveMouseEvent, handlerType was null."); return false; }
                    if (method == null) { Output.Error("Failed to RemoveMouseEvent, method was null."); return false; }
                    var removeMouseEvent = Delegate.CreateDelegate(handlerType, UIElement, method);
                    if (removeMouseEvent == null) { Output.Error("Failed to RemoveMouseEvent, removeMouseEvent was null."); return false; }
                    eventInfo.RemoveEventHandler(UIElement.ModernNative.Value, removeMouseEvent);
                    return true;
                }
                /// <summary> Adds a given event to an event, returns <see langword="false"/> if failed or <see langword="true"/> if success</summary>
                public static bool AddMouseEvent(cUIElement UIElement, string eventName, Delegate handler) {
                    if (UIElement == null) { Output.Error("Failed to AddMouseEvent, UIElement was null."); return false; }
                    var eventInfo = (UIElement.ModernNative.Value).GetType().GetEvent(eventName, flags);
                    if (eventInfo == null) { Output.Error("Failed to AddMouseEvent, eventInfo was null."); return false; }
                    var handlerType = eventInfo.EventHandlerType;
                    if (handlerType == null) { Output.Error("Failed to AddMouseEvent, handlerType was null."); return false; }
                    if (handler == null) { Output.Error("Failed to AddMouseEvent, handler was null."); return false;}
                    var addMouseEvent = Delegate.CreateDelegate(handlerType, handler.Target, handler.Method);
                    if (addMouseEvent == null) { Output.Error("Failed to AddMouseEvent, addMouseEvent was null."); return false; }
                    eventInfo.AddEventHandler(UIElement.ModernNative.Value, addMouseEvent);
                    return true;
                }
                /// <summary> Removes a given event to an event, returns <see langword="false"/> if failed or <see langword="true"/> if success</summary>
                public static bool RemoveMouseEvent(cUIElement UIElement, string eventName, Delegate handler) {
                    if (UIElement == null) { Output.Error("Failed to RemoveMouseEvent, UIElement was null."); return false; }
                    var eventInfo = (UIElement.ModernNative.Value).GetType().GetEvent(eventName, flags);
                    if (eventInfo == null) { Output.Error("Failed to RemoveMouseEvent, eventInfo was null."); return false; }
                    var handlerType = eventInfo.EventHandlerType;
                    if (handlerType == null) { Output.Error("Failed to RemoveMouseEvent, handlerType was null."); return false; }
                    if (handler == null) { Output.Error("Failed to RemoveMouseEvent, handler was null."); return false; }
                    var removeMouseEvent = Delegate.CreateDelegate(handlerType, handler.Target, handler.Method);
                    if (removeMouseEvent == null) { Output.Error("Failed to RemoveMouseEvent, removeMouseEvent was null."); return false; }
                    eventInfo.RemoveEventHandler(UIElement.ModernNative.Value, removeMouseEvent);
                    return true;
                }
            }
            public static class UIPanel_Reference {
                /// <summary> Cache type. </summary>
                private static Type Type_Cache = null;
                /// <summary> Gets the native Terraria type name. </summary>
                public static string GetNativeName() => "Terraria.GameContent.UI.Elements.UIPanel";
                /// <summary> Gets the native Terraria type. </summary>
                public static Type GetNativeType() => Type_Cache == null ? (Type_Cache = Terraria.TerrariaAsm.GetType(GetNativeName())) : Type_Cache;
                /// <summary> Create a new instance of the native Terraria type. </summary>
                public static object New() => Activator.CreateInstance(GetNativeType());
            }
            public static class UIText_Reference {
                /// <summary> Cache type. </summary>
                private static Type Type_Cache = null;
                /// <summary> Gets the native Terraria type name. </summary>
                public static string GetNativeName() => "Terraria.GameContent.UI.Elements.UIText";
                /// <summary> Gets the native Terraria type. </summary>
                public static Type GetNativeType() => Type_Cache == null ? (Type_Cache = Terraria.TerrariaAsm.GetType(GetNativeName())) : Type_Cache;
                /// <summary> Create a new instance of the native Terraria type. </summary>
                public static object New(string text, float textScale = 1f, bool large = false) => Activator.CreateInstance(GetNativeType(), new object[] { text, textScale, large });
            }
            public static class UserInterface_Reference {
                /// <summary> Cache type. </summary>
                private static Type Type_Cache = null;
                /// <summary> Gets the native Terraria type name. </summary>
                public static string GetNativeName() => "Terraria.UI.UserInterface";
                /// <summary> Gets the native Terraria type. </summary>
                public static Type GetNativeType() => Type_Cache == null ? (Type_Cache = Terraria.TerrariaAsm.GetType(GetNativeName())) : Type_Cache;
                /// <summary> Create a new instance of the native Terraria type. </summary>
                public static object New() => Activator.CreateInstance(GetNativeType());
            }
            public static class UIState_Reference {
                /// <summary> Cache type. </summary>
                private static Type Type_Cache = null;
                /// <summary> Gets the native Terraria type name. </summary>
                public static string GetNativeName() => "Terraria.UI.UIState";
                /// <summary> Gets the native Terraria type. </summary>
                public static Type GetNativeType() => Type_Cache == null ? (Type_Cache = Terraria.TerrariaAsm.GetType(GetNativeName())) : Type_Cache;
                /// <summary> Create a new instance of the native Terraria type. </summary>
                public static object New() => Activator.CreateInstance(GetNativeType());
            }
            public static class UITextPanel_Reference {
                /// <summary> Cache type. </summary>
                private static Type Type_Cache = null;
                /// <summary> Gets weather or not the native Terraria type is using generic. </summary>
                public static bool IsGeneric => GetNativeType().FullName.Contains('`');
                /// <summary> Gets the native Terraria type name. </summary>
                public static string GetNativeName(bool genericVersion) => "Terraria.GameContent.UI.Elements.UITextPanel" + (genericVersion ? "`1" : "");
                /// <summary> Gets the native Terraria type. </summary>
                public static Type GetNativeType() => Type_Cache == null ? (Type_Cache = (Terraria.TerrariaAsm.GetType(GetNativeName(true)) ?? Terraria.TerrariaAsm.GetType(GetNativeName(false)))) : Type_Cache;
                /// <summary> Creates a new instance of UITextPanel.<para>T (<typeparamref name="T"/>) must be a <see cref="string"/> in versions not using <see cref="Using_Modern_UITextPanelUsingGeneric"/> (before 1.3.4) </para></summary>
                public static object New<T>(T value, float textScale, bool large) => IsGeneric ? Activator.CreateInstance(GetNativeType().MakeGenericType(typeof(T)), value, textScale, large) : Activator.CreateInstance(GetNativeType(), value, textScale, large);
            }
            public static class UIList_Reference {
                /// <summary> Cache type. </summary>
                private static Type Type_Cache = null;
                /// <summary> Gets the native Terraria type name. </summary>
                public static string GetNativeName() => "Terraria.GameContent.UI.Elements.UIList";
                /// <summary> Gets the native Terraria type. </summary>
                public static Type GetNativeType() => Type_Cache == null ? (Type_Cache = Terraria.TerrariaAsm.GetType(GetNativeName())) : Type_Cache;
                /// <summary> Create a new instance of the native Terraria type. </summary>
                public static object New() => Activator.CreateInstance(GetNativeType());
            }
            public static class UIScrollbar_Reference {
                /// <summary> Cache type. </summary>
                private static Type Type_Cache = null;
                /// <summary> Gets the native Terraria type name. </summary>
                public static string GetNativeName() => "Terraria.GameContent.UI.Elements.UIScrollbar";
                /// <summary> Gets the native Terraria type. </summary>
                public static Type GetNativeType() => Type_Cache == null ? (Type_Cache = Terraria.TerrariaAsm.GetType(GetNativeName())) : Type_Cache;
                /// <summary> Create a new instance of the native Terraria type. </summary>
                public static object New() => Terraria.VersionChecks.Using_Modern_UIScrollbarUsesEnum ? Activator.CreateInstance(GetNativeType(), new object[] { Enum.ToObject(Terraria.TerrariaAsm.GetType(GetNativeName() + "+ColorTheme"), 0) }) : Activator.CreateInstance(GetNativeType());
            }
            public static class UIImageButton_Reference {
                /// <summary> Cache type. </summary>
                private static Type Type_Cache = null;
                /// <summary> Gets the native Terraria type name. </summary>
                public static string GetNativeName() => "Terraria.GameContent.UI.Elements.UIImageButton";
                /// <summary> Gets the native Terraria type. </summary>
                public static Type GetNativeType() => Type_Cache == null ? (Type_Cache = Terraria.TerrariaAsm.GetType(GetNativeName())) : Type_Cache;
                /// <summary> Create a new instance of the native Terraria type.</summary>
                public static object New(ReLogicAsset<Texture2D> texture) => Terraria.VersionChecks.Using_Modern_UIImageButtonRectParams
                    ? Activator.CreateInstance(GetNativeType(), texture.Asset, (Rectangle?)null)
                    : Activator.CreateInstance(GetNativeType(), texture.Asset);
            }
            public static class UIColoredSlider_Reference {
                /// <summary> Cache type. </summary>
                private static Type Type_Cache = null;
                /// <summary> Gets the native Terraria type name. </summary>
                public static string GetNativeName() => "Terraria.GameContent.UI.Elements.UIColoredSlider";
                /// <summary> Gets the native Terraria type. </summary>
                public static Type GetNativeType() => Type_Cache == null ? (Type_Cache = Terraria.TerrariaAsm.GetType(GetNativeName())) : Type_Cache;
                /// <summary> Create a new instance of the native Terraria type. </summary>
                public static object New(Func<float> getStatus, Action<float> setStatusKeyboard, Action setStatusGamepad, Func<float, Color> blipColorFunction)
                    // parameters "color" and "_textKey" are ignored because they are private and not used, this could becomes an issue in later Terraria versions.
                    => Activator.CreateInstance(GetNativeType(), null, getStatus, setStatusKeyboard, setStatusGamepad, blipColorFunction, Color.White);
            }


            /// <summary> Developer note: we can not put instance functions in here that modify values because the getters and setters in <see cref="cUIElement"/> wont work.</summary>
            public struct StyleDimension_Reference {
                /// <summary> Cache type. </summary>
                private static Type Type_Cache = null;
                /// <summary> Gets the native Terraria type name. </summary>
                public static string GetNativeName() => "Terraria.UI.StyleDimension";
                /// <summary> Gets the native Terraria type. </summary>
                public static Type GetNativeType() => Type_Cache == null ? (Type_Cache = Terraria.TerrariaAsm.GetType(GetNativeName())) : Type_Cache;
                /// <summary> Create a new instance of the native Terraria type. </summary>
                public static object New(StyleDimension_Reference data) => Activator.CreateInstance(GetNativeType(), data.Pixels, data.Precent);
                /// <summary> Create a new instance of the native Terraria type. </summary>
                public static object New(Positioning data) => Activator.CreateInstance(GetNativeType(), data.Pixels, data.Precent);
                /// <summary> Create a new instance of the native Terraria type. </summary>
                public static object New(float pixels, float precent) => Activator.CreateInstance(GetNativeType(), pixels, precent);

                public float Pixels;
                public float Precent;
                public StyleDimension_Reference(float pixels, float precent) {
                    Pixels = pixels;
                    Precent = precent;
                }
                public object ToNative() => New(Pixels, Precent);
                public Positioning ToPositioning() => new Positioning(Pixels, Precent);
                public override string ToString() {
                    return $"{{{Pixels},{Precent}}}";
                }

            }

            /// <summary> Plays a sound and changed the colours of a given UIElement (<paramref name="native"/> is the native element, not the cModLoader wrappers). </summary>
            public static void DefaultButtonMouseEnter(object native) {
                Terraria.Audio.PlaySound(12);
                var _native = new Dynamic(native);
                _native.SetValue("BackgroundColor", Terraria.Colour.UIBackgroundSolid);
                _native.SetValue("BorderColor", Terraria.Colour.FancyUIFatButtonMouseOver);
            }
            /// <summary> Changed the colours of a given UIElement </summary>
            public static void DefaultButtonMouseExit(object native) {
                var _native = new Dynamic(native);
                _native.SetValue("BackgroundColor", Terraria.Colour.UIBackgroundTransparent);
                _native.SetValue("BorderColor", Color.Black);
            }
        }


    }


}