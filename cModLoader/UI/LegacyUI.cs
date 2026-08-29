using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Microsoft.Xna.Framework.GamerServices;
using cModLoader.Patching;
using System.Reflection;
using System.Windows.Forms;
using cModLoader.Utils;

namespace cModLoader.UI {

    /// <summary> Used as output data from drawing. </summary>
    public struct DrawnDetails {
        /// <summary> The calculated bounds of the element drawn </summary>
        public RectangleF bounds;
        /// <summary> The calculated bounds of the element drawn including margin and padding </summary>
        public RectangleF outerBounds;
        /// <summary> Is the mouse over the element </summary>
        public bool hovering;
        /// <summary> Did the mouse just go over the element </summary>
        public bool hovered;
        /// <summary> Did the mouse just leave the element </summary>
        public bool unhovered;
        /// <summary> Was the element clicked </summary>
        public bool clicked;
    }

    /// <summary> Substitute for Terraria's UserInterface for legacy UI. </summary>
    public class LegacyUserInterface {
        public LegacyUIState CurrentState;
        /// <summary> Draw (which also updates) the current state.</summary>
        public void Draw(GameReference game) {
            CurrentState?.Draw(game);
        }

    }

    /// <summary> Basically Terraria's UIElement but for legacy UI. </summary>
    public class LegacyUIContainer {

        /// <summary> IDK what this is for but its what Terraria uses for clipping so i guess its important.</summary>
        private static RasterizerState _overflowHiddenRasterizerState;
        public LegacyUIContainer() {
            if (_overflowHiddenRasterizerState == null) {
                _overflowHiddenRasterizerState = new RasterizerState {
                    CullMode = CullMode.None,
                    ScissorTestEnable = true
                };
            }
        }

        internal static Texture2D backgroundImage;

        protected bool wasHover = false;
        protected bool mouseLeftRelease;

        public LegacyUIContainer Parent;
        protected List<LegacyUIContainer> Children = new List<LegacyUIContainer>();

        public Positioning Top;
        public Positioning Left;
        public Positioning Width;
        public Positioning Height;
        public Positioning MaxWidth = Positioning.Fill;
        public Positioning MaxHeight = Positioning.Fill;
        public Positioning MinWidth = Positioning.Empty;
        public Positioning MinHeight = Positioning.Empty;
        public float PaddingTop = 0f;
        public float PaddingLeft = 0f;
        public float PaddingRight = 0f;
        public float PaddingBottom = 0f;
        public float MarginTop = 0f;
        public float MarginLeft = 0f;
        public float MarginRight = 0f;
        public float MarginBottom = 0f;
        public float HAlign = 0f;
        public float VAlign = 0f;

        public bool OverflowHidden = false;

        /// <summary> Simulates a click. </summary>
        public void DoClick() => OnClick.Invoke(null, this);
        public event EventReference OnClick;
        public event EventReference OnMouseExit;
        public event EventReference OnMouseEnter;

        /// <summary> Dimension not including margin or padding </summary>
        private RectangleF _dimensions;
        /// <summary> Dimension including margin and padding </summary>
        private RectangleF _innerDimensions;

        /// <summary> Used as cache to not create an instance every draw call </summary>
        protected DrawnDetails Info_Cache = new DrawnDetails();

        /// <summary> Gets the dimension not including margin or padding </summary>
        public RectangleF GetInnerDimensions() => _innerDimensions;
        /// <summary> Gets the dimension including margin and padding </summary>
        public RectangleF GetDimensions() => _dimensions;
        /// <summary> Does the element contain <paramref name="point"/>. </summary>
        public bool ContainsPoint(Vector2 point) {
            if (point.X > _dimensions.X && point.Y > _dimensions.Y && point.X < _dimensions.X + _dimensions.Width) {
                return point.Y < _dimensions.Y + _dimensions.Height;
            }
            return false;
        }
        /// <summary> Sets all side's padding to <paramref name="pixels"/>. </summary>
        public void SetPadding(float pixels) {
            PaddingBottom = pixels;
            PaddingLeft = pixels;
            PaddingRight = pixels;
            PaddingTop = pixels;
        }

        /// <summary> Overrides the size of the parent or default size.</summary>
        public RectangleF? BaseParentSizeOverride = null;

        /// <summary> Copied from Terraria. (from 1.3.0.1 but added margin from newer versions) </summary>
        public virtual void Recalculate() {
            RectangleF parentDimensions = BaseParentSizeOverride ?? ((Parent == null) ? new RectangleF(0, 0, ModHelper.ScreenWidth, ModHelper.ScreenHeight) : Parent.GetInnerDimensions());
            // newer Terraria versions compress some of this into GetDimensionsBasedOnParentDimensions() but its not needed
            RectangleF calculatedStyle = default(RectangleF);
            calculatedStyle.X = Left.GetValue(parentDimensions.Width) + parentDimensions.X;
            calculatedStyle.Y = Top.GetValue(parentDimensions.Height) + parentDimensions.Y;
            float value = MinWidth.GetValue(parentDimensions.Width);
            float value2 = MaxWidth.GetValue(parentDimensions.Width);
            float value3 = MinHeight.GetValue(parentDimensions.Height);
            float value4 = MaxHeight.GetValue(parentDimensions.Height);
            calculatedStyle.Width = MathHelper.Clamp(Width.GetValue(parentDimensions.Width), value, value2);
            calculatedStyle.Height = MathHelper.Clamp(Height.GetValue(parentDimensions.Height), value3, value4);
            calculatedStyle.X += parentDimensions.Width * HAlign - calculatedStyle.Width * HAlign;
            calculatedStyle.Y += parentDimensions.Height * VAlign - calculatedStyle.Height * VAlign;

            _dimensions = calculatedStyle;
            calculatedStyle.X += MarginLeft;
            calculatedStyle.Y += MarginTop;
            calculatedStyle.Width -= MarginLeft + MarginRight;
            calculatedStyle.Height -= MarginTop + MarginBottom;
            calculatedStyle.X += PaddingLeft;
            calculatedStyle.Y += PaddingTop;
            calculatedStyle.Width -= PaddingLeft + PaddingRight;
            calculatedStyle.Height -= PaddingTop + PaddingBottom;
            _innerDimensions = calculatedStyle;
            Info_Cache.bounds = _innerDimensions;
            Info_Cache.outerBounds = _dimensions;
            RecalculateChildren();
        }
        /// <summary> Copied from Terraria. </summary>
        public virtual void RecalculateChildren() {
            foreach (var element in Children) element.Recalculate();
        }

        protected static FieldInfo spriteSortMode_Cache;
        protected static FieldInfo blendState_Cache;
        protected static FieldInfo samplerState_Cache;
        protected static FieldInfo depthStencilState_Cache;

        /// <summary> Runs in <see cref="DrawSelf(GameReference)"/> before anything is draw but after calculations.</summary>
        public event Action<GameReference> DrawSelfExtension;

        /// <summary> Draws element and child elements. </summary>
        public virtual void Draw(GameReference game) {
            DrawSelf(game);
            if (OverflowHidden) {
                // cache reflections
                // TODO: switch to Dynamic
                if (spriteSortMode_Cache == null) {
                    spriteSortMode_Cache = typeof(SpriteBatch).GetField("spriteSortMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    blendState_Cache = typeof(SpriteBatch).GetField("blendState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    samplerState_Cache = typeof(SpriteBatch).GetField("samplerState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    depthStencilState_Cache = typeof(SpriteBatch).GetField("depthStencilState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                }
                // get old states data
                RasterizerState rasterizerState = game.spriteBatch.GraphicsDevice.RasterizerState;
                Rectangle scissorRectangle = game.spriteBatch.GraphicsDevice.ScissorRectangle;
                var oldSort = (SpriteSortMode)spriteSortMode_Cache.GetValue(game.spriteBatch);
                var oldBlend = (BlendState)blendState_Cache.GetValue(game.spriteBatch);
                var oldSample = (SamplerState)samplerState_Cache.GetValue(game.spriteBatch);
                var oldDepth = (DepthStencilState)depthStencilState_Cache.GetValue(game.spriteBatch);
                // set new data
                game.spriteBatch.End();
                Rectangle scissorRectangle2 = new Rectangle((int)_dimensions.X, (int)_dimensions.Y, (int)_dimensions.Width, (int)_dimensions.Height);
                int width = game.spriteBatch.GraphicsDevice.Viewport.Width;
                int height = game.spriteBatch.GraphicsDevice.Viewport.Height;
                scissorRectangle2.X = (int)MathHelper.Clamp(scissorRectangle2.X, 0, width);
                scissorRectangle2.Y = (int)MathHelper.Clamp(scissorRectangle2.Y, 0, height);
                scissorRectangle2.Width = (int)MathHelper.Clamp(scissorRectangle2.Width, 0, width - scissorRectangle2.X);
                scissorRectangle2.Height = (int)MathHelper.Clamp(scissorRectangle2.Height, 0, height - scissorRectangle2.Y);
                game.spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle2;
                // terraria assumes these values but its safer to use what it was.
                game.spriteBatch.Begin(oldSort, oldBlend, oldSample, oldDepth, _overflowHiddenRasterizerState);
                // draw children
                foreach (var child in Children) {
                    child.Draw(game);
                }
                // set back 
                game.spriteBatch.End();
                game.spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
                // terraria assumes these values but its safer to use what it was.
                game.spriteBatch.Begin(oldSort, oldBlend, oldSample, oldDepth, rasterizerState);
            }
            else {
                foreach (var child in Children) {
                    child.Draw(game);
                }
            }
        }
        protected virtual void DrawSelf(GameReference game) {
            Recalculate();
            var mouse = Mouse.GetState();
            int width = ModHelper.ScreenWidth;
            int height = ModHelper.ScreenHeight;
            var h = _dimensions.ToRectangle().Contains(mouse.X, mouse.Y);
            if (!Info_Cache.hovering && h) {
                Info_Cache.hovered = true;
                OnMouseEnter?.Invoke(null, this);
            }
            if (Info_Cache.hovering && !h) OnMouseExit?.Invoke(null, this);
            Info_Cache.hovering = h;
            var press = InputHelper.WasLeftMouseDown && InputHelper.IsLeftMouseUp;
            if (press && Info_Cache.hovering) {
                Info_Cache.clicked = true;
                OnClick?.Invoke(null, this);
            }
            else Info_Cache.clicked = false;
            
            DrawSelfExtension?.Invoke(game);

            if (UIUtils.Test_DrawBoundsLegacyUI) {
                if (backgroundImage == null) {
                    backgroundImage = game.game.Content.Load<Texture2D>("Images\\Inventory_Back");
                }
                game.spriteBatch.Draw(backgroundImage, Info_Cache.bounds.ToRectangle(), new Rectangle(8, 8, 40, 40), new Color(0.25f, 0.25f, 0.25f, 0.1f));
            }
        }

        public void RemoveChild(LegacyUIContainer child) {
            Children.Remove(child);
            child.Parent = null;
        }
        public void RemoveFromParent() {
            if (Parent != null) Parent.RemoveChild(this);
        }
        public void Append(LegacyUIContainer element) {
            element.RemoveFromParent();
            element.Parent = this;
            Children.Add(element);
            element.Recalculate();
        }

        /// <summary> Gets the list of children, normally Terraria does not allow for this. </summary>
        public List<LegacyUIContainer> GetChildren() {
            return Children;
        }

        public DrawnDetails GetDrawnDetails() => Info_Cache;

    }
    /// <summary> Substitute for Terraria's UIText for legacy UI. </summary>
    public class LegacyUIText : LegacyUIContainer {

        /// <summary> Pixel offset for text drawing. This is not a thing in vanilla Terraria's modern UI. </summary>
        public Vector2 TextPixelOffset = new Vector2(0, 0);

        /// <summary> Weather or not to use the text size as the element size. If <see langword="true"/> <see cref="LegacyUIContainer.Width"/> and <see cref="LegacyUIContainer.Height"/> will be overriden. </summary>
        public bool FitToText = false;

        public string Text;
        public bool Big;
        public float StartSize;
        public float EndSize;
        public Color StartColor;
        public Color EndColor;
        public bool Outline;
        public LegacyUIText() : this("Empty", true, 1f, 1f, Color.White, Color.White, true) { }
        public LegacyUIText(string text) : this(text, true, 1f, 1f, Color.White, Color.White, true) { }
        public LegacyUIText(string text, bool big, float startSize, float endSize, Color startColor, Color endColor, bool outline) {
            Text = text;
            Big = big;
            StartSize = startSize;
            EndSize = endSize;
            StartColor = startColor;
            EndColor = endColor;
            Outline = outline;
        }
        public float buttonScale = 0f;

        /// <summary> Returns either a <see cref="SpriteFont"/> or Terraria's custom DynamicSpriteFont based off of <see cref="Terraria.VersionChecks.Using_LegacyFontSystem"/></summary>
        public Dynamic GetFont() {
            Dynamic font;
            if (Terraria.VersionChecks.Using_LegacyFontSystem) {
                font = new Dynamic(!Big ? UIUtils.LegacyFontSystem.getFontMouseText() : UIUtils.LegacyFontSystem.getFontDeathText());
            }
            else {
                font = new Dynamic(!Big ? UIUtils.ModernFontSystem.getFontMouseText() : UIUtils.ModernFontSystem.getFontDeathText());
            }
            return font;
        }

        public override void Recalculate() {
            if (FitToText) {
                Vector2 size = GetFont().Invoke<Vector2>("MeasureString", Text) * StartSize;
                Width = new Positioning(size.X, 0f);
                Height = new Positioning(size.Y, 0f);
            }
            base.Recalculate();
        }

        protected override void DrawSelf(GameReference game) {
            base.DrawSelf(game);

            var speed = 0.08f;
            buttonScale += Info_Cache.hovering ? speed : -speed;
            if (buttonScale >= 1) buttonScale = 1f;
            if (buttonScale <= 0) buttonScale = 0f;

            var scale = MathHelper.Lerp(StartSize, EndSize, buttonScale); //StartSize + ((EndSize - StartSize) * buttonScale);
            var colour = Color.Lerp(EndColor, StartColor, buttonScale);

            Vector2 size = GetFont().Invoke<Vector2>("MeasureString", Text);
            Vector2 pos = (Info_Cache.bounds.Center() + TextPixelOffset) - ((size * scale) / 2);

            UIUtils.DrawString(game.spriteBatch, Text, pos, colour, scale, Big);
        }
    }
    /// <summary> Substitute for Terraria's UIState for legacy UI. </summary>
    public class LegacyUIState : LegacyUIContainer {
        public LegacyUIState() {
            Width = new Positioning(0f, 1f);
            Height = new Positioning(0f, 1f);
            Recalculate();
        }
    }
    /// <summary> Substitute for Terraria's UIList for legacy UI. </summary>
    public class LegacyUIScroll : LegacyUIContainer {
        protected List<LegacyUIContainer> _items = new List<LegacyUIContainer>();
        /// <summary> Used as container from scrolling, needed or else the top level element will have all children, including scroll bar, act as a scrolling object.</summary>
        protected LegacyUIContainer _innerList = new LegacyUIContainer();

        public static float ScrollMult = 0.25f;

        public float ListPadding = 5f;
        public float ScrollPosition = 0f;
        private float previousScroll = 0f;

        public LegacyUIScroll() {
            _innerList.Width = new Positioning(0f, 2f);
            _innerList.Height = new Positioning(0f, 1f);
            Append(_innerList);
        }

        public virtual void Add(LegacyUIContainer item) {
            _items.Add(item);
            _innerList.Append(item);
        }

        public virtual bool Remove(LegacyUIContainer item) {
            _innerList.RemoveChild(item);
            return _items.Remove(item);
        }

        public override void Draw(GameReference game) {
            MouseState state = Mouse.GetState();
            if (previousScroll != state.ScrollWheelValue) {
                ScrollPosition += (state.ScrollWheelValue - previousScroll) * ScrollMult;
                previousScroll = state.ScrollWheelValue;
            }
            float totalHeight = 0;
            foreach (var item in _items) {
                totalHeight += item.GetDimensions().Height + ListPadding;
            }
            var h = GetDimensions().Height;
            if (totalHeight > h) {
                var max = totalHeight - h;
                if (ScrollPosition > 0) ScrollPosition = 0;
                else if (ScrollPosition < -max) ScrollPosition = -max;
            } else ScrollPosition = 0;
            var y = ScrollPosition;
            foreach (var item in _items) {
                item.Width = new Positioning(500f, 2f);
                item.Top = new Positioning((int)y, 0f);
                y += item.GetDimensions().Height + ListPadding;
            }
            base.Draw(game);
        }
    }
    /// <summary> Substitute for Terraria's UIColoredSlider for legacy UI. </summary>
    public class LegacyUINumericUpDown : LegacyUIContainer {
        public static int pressCounterMax = 20;
        public float Increment;
        public Action<float> OnSetValue;
        public Func<float> OnGetValue;
        public int pressCounter = 0;

        public LegacyUIText decrease;
        public LegacyUIText increase;
        public LegacyUIText number;

        public LegacyUINumericUpDown(Func<float> getValue, Action<float> setValue, float increment = 0.01f) {
            OnSetValue = setValue;
            OnGetValue = getValue;
            Increment = increment;
            
            number = new LegacyUIText("0.00", false, 1f, 1f, Color.White, Color.White * 0.5f, true);
            number.VAlign = 0.5f;
            number.HAlign = 0.5f;
            number.Width = new Positioning(0, 1f);
            number.Height = new Positioning(0f, 1f);

            var s = 0.6f;
            decrease = new LegacyUIText("-", true, s, s, Color.White, Color.White * 0.5f, true);
            decrease.FitToText = false;
            decrease.VAlign = 0.5f;
            decrease.HAlign = 1f;
            decrease.Width = new Positioning(20f, 0f);
            decrease.Height = new Positioning(20f, 0f);
            decrease.OnClick += (o, n) => {
                OnSetValue?.Invoke(MathHelper.Clamp(OnGetValue() - Increment, 0f, 1f));
            };
            Append(decrease);

            increase = new LegacyUIText("+", true, s, s, Color.White, Color.White * 0.5f, true);
            increase.FitToText = false;
            increase.VAlign = 0.5f;
            increase.HAlign = 0f;
            increase.Width = new Positioning(20f, 0f);
            increase.Height = new Positioning(20f, 0f);
            increase.OnClick += (o, n) => {
                OnSetValue?.Invoke(MathHelper.Clamp(OnGetValue() + Increment, 0f, 1f));
            };
            Append(increase);

            Append(number);
        }
        public override void Draw(GameReference game) {

            var pos = InputHelper.MousePos;
            var overInc = increase.ContainsPoint(pos);
            var overDec = decrease.ContainsPoint(pos);
            if (overInc || overDec) {
                if (InputHelper.IsLeftMouseDown) {
                    pressCounter++;
                    if (pressCounter >= pressCounterMax) {
                        pressCounter = pressCounterMax;
                        if (overInc) increase.DoClick();
                        if (overDec) decrease.DoClick();
                    }
                }
                else pressCounter = 0;
            }
            else pressCounter = 0;
            
            number.Text = OnGetValue().ToString("0.00");

            base.Draw(game);
        }
    }

}
