using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tactile.UI.Builder
{
    public struct BoxParameters: ISizeable<BoxParameters>, IViewParameters
    {
        public float? topLeftRadius;
        public float? topRightRadius;
        public float? bottomLeftRadius;
        public float? bottomRightRadius;
        public Color? color;
        
        public void PopulateWithDefaultValues()
        {
            topLeftRadius = 0;
            topRightRadius = 0;
            bottomLeftRadius = 0;
            bottomRightRadius = 0;
            color = Color.clear;
            XSize = SizeType.Fill;
            YSize = SizeType.Fill;
        }

        public SizeType XSize { get; set; }
        public SizeType YSize { get; set; }
    }
    
    public static class Box
    {
        public static View<TMessage, BoxParameters> FillColor<TMessage>(this View<TMessage, BoxParameters> view,
                    Color color) where TMessage : Message
        {
            view.ViewParameters.color = color;
            return view;
        }

        public static View<TMessage, BoxParameters> Radius<TMessage>(this View<TMessage, BoxParameters> view,
            float tl, float tr, float bl, float br) where TMessage : Message
        {
            view.ViewParameters.topLeftRadius = tl;
            view.ViewParameters.topRightRadius = tr;
            view.ViewParameters.bottomLeftRadius = bl;
            view.ViewParameters.bottomRightRadius = br;
            
            return view;
        }

        public static View<TMessage, BoxParameters> Radius<TMessage>(this View<TMessage, BoxParameters> view,
            float radius) where TMessage : Message
        {
            return Radius(view, radius, radius, radius, radius);
        }
    }
    
    public class Box<TMessage>: View<TMessage, BoxParameters> where TMessage: Message
    {
        public Box(BoxParameters parameters, params View<TMessage>[][] children ) : base(parameters, children) { }
        public Box(params View<TMessage>[][] children) : base(children) { }

        public override void Build(Action<TMessage> update)
        {
            var rectangle = GetComponent<Rectangle>();
            Rectangle.CornerSizes sizes = new Rectangle.CornerSizes
            {
                topLeft = ViewParameters.topLeftRadius.GetValueOrDefault(),
                topRight = ViewParameters.topRightRadius.GetValueOrDefault(),
                bottomLeft = ViewParameters.bottomLeftRadius.GetValueOrDefault(),
                bottomRight = ViewParameters.bottomRightRadius.GetValueOrDefault()
            };
            rectangle.Corners = sizes;
            rectangle.color = ViewParameters.color.GetValueOrDefault();
        }
    }
}