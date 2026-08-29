using cModLoader.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows.Forms;

namespace cModLoader.ModComponents {

    /// <summary> Default base class for config elements, this does not store an element its just a base for displaying stuff.<br/>This is not UI although is used in creating the UI. </summary>
    public abstract class ConfigElement {

        internal static cUIElement CreateDefaultLeftRightElement(string leftText, cUIElement rightElement) {
            var baseElement = new cUIPanel() {
                Width = new Positioning(0f, 1f),
                Height = new Positioning(42f, 0f)
            };
            if (!baseElement.IsLegacy) {
                baseElement.SetPadding(6f);
            }
            else {
                baseElement.Height = new Positioning(30f, 0f);
            }
            var toggleText = new cUIText(leftText) {
                Left = new Positioning(6f, 0f),
                Top = new Positioning(4f, 0f)
            };
            if (toggleText.IsLegacy && toggleText.LegacyNative is LegacyUIText legText1) {
                legText1.FitToText = true;
            }
            baseElement.Append(toggleText);

            rightElement.Top = new Positioning(4f, 0f);
            rightElement.Left = new Positioning(-6f, 0f);
            rightElement.Alignment = new Vector2(1f, 0f);
            if (rightElement.IsLegacy && rightElement.LegacyNative is LegacyUIText legText2) {
                legText2.EndColor = Color.White * 0.5f;
                legText2.FitToText = true;
            }

            baseElement.Append(rightElement);

            return baseElement;
        }

        /// <summary> Internal name used for identification </summary>
        public string InternalName;
        /// <summary> External name displayed, this is accessed through <see cref="GetDisplayText"/>, this can be overridden to dynamical change text. </summary>
        public string DisplayText;

        /// <summary><see langword="⚠ Not Implemented"/>, this was an idea but im too lazy to implement it.<br/>If you really want encryption you can override the <see cref="ToConfigString"/> and <see cref="FromConfigString(string[])"/> functions.<br/>Would have been used to encrypt output data in config file so nobody tampers with it.<para>This can easily be bypassed if someone created another mod to read or change the value while the game is running.<br/>Or they could read the process memory and take the value.<br/>This is not meant to be any advanced algorithm, its probably fairly easy to decipher<br/>Its mainly just so unexperienced noobs don't try messing with your config</para> </summary>
        public bool EncryptOutput = false;

        public ConfigElement(string internalText, string displayText) {
            InternalName = internalText;
            DisplayText = displayText;
        }

        /// <summary> Gets the display text for the config element. </summary>
        public virtual string GetDisplayText() {
            return DisplayText;
        }
    
        /// <summary> Gets the UI element associated with this config element. New instance created every call to reduce memory usage. </summary>
        public virtual cUIElement GetUIElement() {
            var elm = new cUITextPanel<string>(DisplayText, 1f, false);
            if (elm.IsLegacy) (elm.LegacyNative as LegacyUIText).FitToText = true;
            return elm;
        }

        /// <summary> Should return weather or not this config option should be saved to the config file. </summary>
        protected virtual bool ShouldSaveConfig() => false;
        /// <summary> Should return a string (ideally a single line for clarity) to be placed in a config file </summary>
        protected virtual string SaveConfigValue() => $"{DisplayText}";
        /// <summary> Should do whatever to parse config data. <paramref name="stringData"/> is the raw saved data.</summary>
        protected virtual void ReadConfigValue(string stringData) => DisplayText = stringData;
        /// <summary> Should do whatever when the config value is not found, probably just set as a default value or something.</summary>
        protected virtual void MissingConfigValueFallback() { }

        /// <summary> Calls <see cref="ShouldSaveConfig()"/> but wraps it in a mod context.<br/><paramref name="mod"/> can be <see langword="null"/>, or any mod. </summary>
        public static bool ShouldSaveConfig(Mod mod, ConfigElement _ref) => ModLoader.ModContext.RunUnderModContext(mod, () => _ref.ShouldSaveConfig());
        /// <summary> Calls <see cref="ShouldSaveConfig()"/> but wraps it in a mod context.<br/><paramref name="mod"/> can be <see langword="null"/>, or any mod. </summary>
        public static string SaveConfigValue(Mod mod, ConfigElement _ref) => ModLoader.ModContext.RunUnderModContext(mod, () => _ref.SaveConfigValue());
        /// <summary> Calls <see cref="ReadConfigValue(string)"/> but wraps it in a mod context.<br/><paramref name="mod"/> can be <see langword="null"/>, or any mod. </summary>
        public static void ReadConfigValue(Mod mod, ConfigElement _ref, string stringData) => ModLoader.ModContext.RunUnderModContext(mod, () => _ref.ReadConfigValue(stringData));
        /// <summary> Calls <see cref="MissingConfigValueFallback()"/> but wraps it in a mod context.<br/><paramref name="mod"/> can be <see langword="null"/>, or any mod. </summary>
        public static void MissingConfigValueFallback(Mod mod, ConfigElement _ref) => ModLoader.ModContext.RunUnderModContext(mod, () => _ref.MissingConfigValueFallback());

    }
    public abstract class BaseConfigItem : ConfigElement {
        /// <summary> Should return the config value. </summary>
        protected abstract object GetRawValue();
        /// <summary> Should return the config value and set the config value. </summary>
        protected abstract object SetRawValue(object newValue);
        public BaseConfigItem(string internalText, string displayText) : base(internalText, displayText) {

        }
        //public static T GetConfigValue<T>(Mod mod, ConfigItem<T> _ref) => (T)ModLoader.ModContext.RunUnderModContext(mod, () => _ref.GetRawValue());
        //public static T SetConfigValue<T>(Mod mod, ConfigItem<T> _ref, T value) => (T)ModLoader.ModContext.RunUnderModContext(mod, () => _ref.SetRawValue(value));
    }
    /// <summary> Base typed config item, used as a base class for config element that store a value such as <see cref="ConfigBoolean"/>. </summary>
    public abstract class ConfigItem<T> : BaseConfigItem {
        protected T value;

        /// <summary> Delegate used when the config value is set.<br/><paramref name="newValue"/> and <paramref name="oldValue"/> may be the same. </summary>
        public delegate void ValueSetEvent(T oldValue, T newValue);
        /// <summary> Delegate used when the config value is gotten.</summary>
        public delegate void ValueGetEvent(ref T value);
        /// <summary> This is invoked when <see cref="SetValue(T)"/> is called.<br/>New and old values may be the same.<br/>This never modifies the value. </summary>
        public ValueSetEvent OnSetValue;
        /// <summary> This is invoked when <see cref="GetValue()"/> is called.<br/>Can be used to change the value, this will NOT not the value.<br/>This never modifies the value. </summary>
        public ValueGetEvent OnGetValue;

        public ConfigItem(string internalText, string displayText) : base(internalText, displayText) {

        }

        /// <summary> Should return the config value. </summary>
        protected override object GetRawValue() => value;
        /// <summary> Should return the config value and set the config value. </summary>
        protected override object SetRawValue(object newValue) => value = (T)newValue;

        /// <summary> Can be overridden to change default value set functionality. This is used when setting <see cref="value"/>.<br/>Should return <paramref name="newValue"/> or whatever new value you want.<br/>By default this invokes <see cref="OnSetValue"/>.</summary>
        public virtual T SetValue(T newValue) {
            if (OnSetValue != null) OnSetValue(value, newValue);
            value = newValue;
            return newValue;
        }
        /// <summary> Can be overridden to change default value get functionality. This is used when getting <see cref="value"/>.<br/>Should return the value or whatever you want the value to be.<br/>By default this invokes <see cref="OnGetValue"/>.</summary>
        public virtual T GetValue() {
            T val = value;
            if (OnGetValue != null) OnGetValue(ref val);
            return val;
        }

        protected override bool ShouldSaveConfig() => true;
        protected override string SaveConfigValue() => throw new Exception($"SaveConfigValue Error, No Save code for type: {nameof(T)}");
        protected override void ReadConfigValue(string str) => throw new Exception($"ReadConfigValue Error, No Save code for type: {nameof(T)}");
        protected override void MissingConfigValueFallback() => SetValue(default);
    }

    /// <summary> Title text config element, can be used to separate config sections. </summary>
    public class ConfigText : ConfigElement {
        public ConfigText(string internalText, string displayText) : base(internalText, displayText) { }
        public override cUIElement GetUIElement() {
            var textElm = new cUIText(DisplayText, 0.6f, true);
            textElm.Width = new Positioning(0f, 1f);
            textElm.Height = new Positioning(35f, 0f);
            return textElm;
        }
    }
    /// <summary> Config element with left text and a toggleable element on the right. Use <see cref="ConfigItem{T}.OnSetValue"/> to do stuff when the value changes (when the element is toggled). </summary>
    public class ConfigBoolean : ConfigItem<bool> {
        bool defaultValue;
        public string trueValue = "true";
        public string falseValue = "false";
        public ConfigBoolean(string internalText, string displayText, bool defaultValue, string trueDisplayValue = "true", string falseDisplayValue = "false") : base(internalText, displayText) {
            this.defaultValue = defaultValue;
            value = defaultValue;
            trueValue = trueDisplayValue;
            falseValue = falseDisplayValue;
        }
        public override cUIElement GetUIElement() {
            var toggleBtn = new cUIText($"[{(GetValue() ? trueValue : falseValue)}]");
            toggleBtn.OnMouseOut += (o, e) => {
                if (!toggleBtn.IsLegacy) {
                    toggleBtn.TextColour = Color.White;
                }
            };
            toggleBtn.OnMouseOver += (o, e) => {
                Terraria.Audio.PlaySound(12);
                if (!toggleBtn.IsLegacy) {
                    toggleBtn.TextColour = Terraria.Colour.FancyUIFatButtonMouseOver;
                }
            };
            OnSetValue += (_old, _new) => toggleBtn.Text = $"[{(_new ? trueValue : falseValue)}]";
            toggleBtn.OnClick += (e, o) => {
                SetValue(!GetValue());
                Terraria.Audio.PlaySound(12);
            };
            return ConfigElement.CreateDefaultLeftRightElement(DisplayText, toggleBtn);
        }
        protected override string SaveConfigValue() => value.ToString();
        protected override void ReadConfigValue(string stringData) => SetValue(bool.Parse(stringData));
        protected override void MissingConfigValueFallback() => SetValue(defaultValue);
    }
    /// <summary> Config element with left text and a toggleable element on the right. Use <see cref="ConfigItem{T}.OnSetValue"/> to do stuff when the value changes (when the element is toggled). </summary>
    public class ConfigSelectToggle : ConfigItem<int> {
        int defaultIndex;
        int maxValues;
        string[] optionTexts = null;
        
        public ConfigSelectToggle(string internalText, string displayText, int defaultIndex, string[] optionTexts) : base(internalText, displayText) {
            this.defaultIndex = defaultIndex;
            value = defaultIndex;
            this.optionTexts = optionTexts;
            this.maxValues = optionTexts.Length;
        }

        public override cUIElement GetUIElement() {
            var toggleBtn = new cUIText($"[{optionTexts[GetValue()]}]");
            toggleBtn.OnMouseOut += (o, e) => {
                if (!toggleBtn.IsLegacy) {
                    toggleBtn.TextColour = Color.White;
                }
            };
            toggleBtn.OnMouseOver += (o, e) => {
                Terraria.Audio.PlaySound(12);
                if (!toggleBtn.IsLegacy) {
                    toggleBtn.TextColour = Terraria.Colour.FancyUIFatButtonMouseOver;
                }
            };
            OnSetValue += (_old, _new) => toggleBtn.Text = $"[{optionTexts[_new]}]"; ;

            toggleBtn.OnClick += (e, o) => {
                SetValue((GetValue() + 1) % maxValues);
                Terraria.Audio.PlaySound(12);
            };
            return ConfigElement.CreateDefaultLeftRightElement(DisplayText, toggleBtn);
        }
        protected override string SaveConfigValue() => value.ToString();
        protected override void ReadConfigValue(string stringData) => SetValue(int.Parse(stringData));
        protected override void MissingConfigValueFallback() => SetValue(defaultIndex);
    }
    /// <summary> Config element with left text and a button on the right. Use <see cref="ConfigButton.OnButtonPress"/> to do stuff when the button is pressed. </summary>
    public class ConfigButton : ConfigElement {
        public string buttonText;
        public Action OnButtonPress;
        public ConfigButton(string internalText, string displayText, string buttonText) : base(internalText, displayText) {
            InternalName = internalText;
            DisplayText = displayText;
            this.buttonText = buttonText;
        }
        public override cUIElement GetUIElement() {
            var toggleBtn = new cUITextPanel<string>(buttonText, 1f, false, true);
            toggleBtn.OnMouseOut += (o, e) => {
                if (!toggleBtn.IsLegacy) {
                    toggleBtn.TextColour = Color.White;
                }
            };
            toggleBtn.OnMouseOver += (o, e) => {
                Terraria.Audio.PlaySound(12);
                if (!toggleBtn.IsLegacy) {
                    toggleBtn.TextColour = Terraria.Colour.FancyUIFatButtonMouseOver;
                }
            };
            toggleBtn.OnClick += (e, o) => {
                if (OnButtonPress != null) OnButtonPress();
                Terraria.Audio.PlaySound(12);
            };
            var elm = ConfigElement.CreateDefaultLeftRightElement(DisplayText, toggleBtn);
            if (!elm.IsLegacy) {
                elm.Height = new Positioning(48f, 0f);
                toggleBtn.Top = new Positioning(-2f, 0f);
                toggleBtn.Left = new Positioning(0f, 0f);
            }
            return elm;
        }
    }
    /// <summary> Config element with left text and a slider element on the right. In legacy UI versions this is a + and - button, not a slider. </summary>
    public class ConfigSlider : ConfigItem<float> {
        float defaultValue;

        public ConfigSlider(string internalText, string displayText, float defaultValue) : base(internalText, displayText) {
            this.defaultValue = defaultValue;
        }
        public override cUIElement GetUIElement() {
            var slider = new cUIColoredSlider(Color.White, Color.Black, GetValue());
            if (slider.IsLegacy) {
                slider.Width = new Positioning(75f, 0f);
            }
            slider.Height = new Positioning(0f, 1f);
            slider.OnValueChange = (val) => SetValue(val);
            var elm = ConfigElement.CreateDefaultLeftRightElement(DisplayText, slider);
            if (!slider.IsLegacy) {
                slider.Margin = new Vector4(0, 0, 0, 0);
                slider.Left = new Positioning(6f, 0f);
                slider.Top = new Positioning(-4f, 0f);
            }
            return elm;
        }

        protected override string SaveConfigValue() => value.ToString();
        protected override void ReadConfigValue(string stringData) => SetValue(float.Parse(stringData));
        protected override void MissingConfigValueFallback() => SetValue(defaultValue);
    }

    /// <summary> Configuration settings for your mod. </summary>
    public class Config {
        /// <summary> Config elements. </summary>
        public List<ConfigElement> Items = new List<ConfigElement>();
        /// <summary> Gets number of config items. </summary>
        public int Count => Items.Count;
        /// <summary> Gets a given config element. Returns <see langword="null"/> if no elements are found. </summary>
        public ConfigElement this[int i] => i >= 0 && i < Items.Count ? Items[i] : null;
        /// <summary> Gets a given config element using its internal name. Returns <see langword="null"/> if no elements are found. </summary>
        public ConfigElement this[string internalConfigName] => Items.Exists(x => x.InternalName == internalConfigName) ? Items.Find(x => x.InternalName == internalConfigName) : throw new Exception($"No config elements with the internal name \"{internalConfigName}\"");

        private Mod modContext = null;
        public Config() { }
        public Config(Mod mod) {
            if (mod == null) {
                modContext = mod;
                // if config folder was not found, still do a config but dont try saving or reading
                if (!ModLoader.ConfigFolderFound) return;
                if (!File.Exists(modContext.GetConfigFile())) {
                    Output.Error("Failed to find file \"" + modContext.GetConfigFile() + "\"");
                    Output.Error("  Can not load config for mod " + mod.ModName + ".");
                }
            }
        }
        /// <summary> Saves config data. By default this is called when pressing the "Back and Save" button in the config menu.</summary>
        public void SaveConfig(string configPath, bool overrideOld = true) {
            // config folder should always exist
            if (!Directory.Exists(Path.GoBack(configPath))) {
                Output.Print($"Can not find folder \"{Path.GoBack(configPath)}\".");
                return;
            }
            if (!overrideOld) return;
            var lines = Items.Where(ix => ConfigElement.ShouldSaveConfig(modContext, ix)).Select(x => x.InternalName + ":" + ConfigElement.SaveConfigValue(modContext, x));
            File.WriteAllLines(configPath, lines);
        }
        /// <summary> Loads config data. By default this is called when and instance of this config is created (once per game session).<br/>Feel free to call it yourself, use <see cref="Mod.GetConfigFile()"/> to get the path.</summary><returns>If the config was loaded.</returns>
        public void LoadConfig(string configPath) {
            var path = configPath;
            if (!File.Exists(path)) {
                Output.Error($"Config file \"{configPath}\" does not exist.");
                return;
            }
            var lines = File.ReadAllLines(path);
            foreach (var elm in Items) {
                bool found = false;
                foreach (var line in lines) {
                    if (line.StartsWith(elm.InternalName + ":")) {
                        var s = (elm.InternalName + ":").Length;
                        var str = line.Substring(s, line.Length - s);
                        ConfigElement.ReadConfigValue(modContext, elm, str);
                        found = true;
                    }
                }
                if (!found) ConfigElement.MissingConfigValueFallback(modContext, elm);
            }
            return;
        }
        /// <summary> Adds a config element to the config. </summary>
        public void Add(ConfigElement element) {
            Items.Add(element);
        }

    }
}
