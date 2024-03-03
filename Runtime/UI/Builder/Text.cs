using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Tactile.UI.Menu.Builder
{
    public static class Text
    {
        public enum VerticalAlignment
        {
            Top, Center, Bottom
        }
        
        public enum HorizontalAlignment {
            Left, Center, Right
        }

        public enum FontStyle
        {
            None = 0,
            Bold = 1 << 0,
            Italic = 1 << 1,
            Underline = 1 << 2,
            Strikethrough = 1 << 6
        }
        
        public struct Parameters: ISizeable<Parameters>, IViewParameters, IRaycastTarget
        {
            public VerticalAlignment? verticalAlignment;
            public HorizontalAlignment? horizontalAlignment;
            public Color? color;
            public float? size;
            public FontStyle? fontStyle;

            public void PopulateWithDefaultValues()
            {
                verticalAlignment = VerticalAlignment.Top;
                horizontalAlignment = HorizontalAlignment.Left;
                color = Color.black;
                XSize = SizeType.Fill;
                YSize = SizeType.Fill;
                fontStyle = FontStyle.None;
                size = 36;
                RaycastEnabled = true;
            }

            public SizeType XSize { get; set; }
            public SizeType YSize { get; set; }
            public bool? RaycastEnabled { get; set; }
        }
        
        public static View<TMessage, Parameters> TextColor<TMessage>(this View<TMessage, Parameters>  text, Color textColor) where TMessage: Message
        {
            text.ViewParameters.color = textColor;
            return text;
        }

        public static View<TMessage, Parameters> VerticalAlign<TMessage>(this View<TMessage, Parameters> text, VerticalAlignment alignment) where TMessage: Message
        {
            text.ViewParameters.verticalAlignment = alignment;
            return text;
        }

        public static View<TMessage, Parameters> HorizontalAlign<TMessage>(this View<TMessage, Parameters>  text, HorizontalAlignment alignment) where TMessage: Message
        {
            text.ViewParameters.horizontalAlignment = alignment;
            return text;
        }
        
        public static View<TMessage, Parameters> CenterX<TMessage>(this View<TMessage, Parameters>  text) where TMessage: Message
        {
            text.ViewParameters.horizontalAlignment = HorizontalAlignment.Center;
            return text;
        }
        
        public static View<TMessage, Parameters> CenterY<TMessage>(this View<TMessage, Parameters>  text) where TMessage: Message
        {
            text.ViewParameters.verticalAlignment = VerticalAlignment.Center;
            return text;
        }
        
        public static View<TMessage, Parameters> CenterXY<TMessage>(this View<TMessage, Parameters>  text) where TMessage: Message
        {
            return text.CenterX().CenterY();
        }

        public static View<TMessage, Parameters> Font<TMessage>(this View<TMessage, Parameters> text, TMP_FontAsset font)
            where TMessage : Message
        {
            throw new NotImplementedException();
        }
        
        public static View<TMessage, Parameters> FontSize<TMessage>(this View<TMessage, Parameters> text, float size)
            where TMessage : Message
        {
            text.ViewParameters.size = size;
            return text;
        }
        
        public static View<TMessage, Parameters> Style<TMessage>(this View<TMessage, Parameters> text, FontStyle style)
            where TMessage : Message
        {
            text.ViewParameters.fontStyle = style;
            return text;
        }
        
        public static View<TMessage, Parameters> Bold<TMessage>(this View<TMessage, Parameters> text)
            where TMessage : Message
        {
            text.ViewParameters.fontStyle |= FontStyle.Bold;
            return text;
        }
        
        public static View<TMessage, Parameters> Italic<TMessage>(this View<TMessage, Parameters> text)
            where TMessage : Message
        {
            text.ViewParameters.fontStyle |= FontStyle.Italic;
            return text;
        }
        
        public static View<TMessage, Parameters> Underline<TMessage>(this View<TMessage, Parameters> text)
            where TMessage : Message
        {
            text.ViewParameters.fontStyle |= FontStyle.Underline;
            return text;
        }
        
        public static View<TMessage, Parameters> Strikethrough<TMessage>(this View<TMessage, Parameters> text)
            where TMessage : Message
        {
            text.ViewParameters.fontStyle |= FontStyle.Strikethrough;
            return text;
        }
    }
    
    public class Text<TMessage>: View<TMessage, Text.Parameters>, IEquatable<Text<TMessage>> where TMessage: Message
    {
        private readonly string text;
        
        public Text(Text.Parameters parameters, string text) : base(parameters)
        {
            this.text = text;
        }
        
        public Text(string text)
        {
            this.text = text;
        }
        
        public override void Build(Action<TMessage> update)
        {
            var tmp = GetComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.color = ViewParameters.color.GetValueOrDefault();

            switch (ViewParameters.verticalAlignment.GetValueOrDefault())
            {
                case Text.VerticalAlignment.Top:
                    tmp.verticalAlignment = VerticalAlignmentOptions.Top;
                    break;
                case Text.VerticalAlignment.Center:
                    tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
                    break;
                case Text.VerticalAlignment.Bottom:
                    tmp.verticalAlignment = VerticalAlignmentOptions.Bottom;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            switch (ViewParameters.horizontalAlignment.GetValueOrDefault())
            {
                case Text.HorizontalAlignment.Left:
                    tmp.horizontalAlignment = HorizontalAlignmentOptions.Left;
                    break;
                case Text.HorizontalAlignment.Center:
                    tmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
                    break;
                case Text.HorizontalAlignment.Right:
                    tmp.horizontalAlignment = HorizontalAlignmentOptions.Right;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            tmp.fontStyle = (FontStyles) ViewParameters.fontStyle.GetValueOrDefault();
            tmp.fontSize = ViewParameters.size.GetValueOrDefault();
            tmp.raycastTarget = ViewParameters.RaycastEnabled ?? true;
        }

        public bool Equals(Text<TMessage> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && text == other.text;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Text<TMessage>)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), text);
        }
    }
}