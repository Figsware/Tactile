using UnityEngine;

namespace Tactile.UI.Menu.Builder
{
    public interface ISizeable<T> : ISizeable where T: struct { } 
    public interface ISizeable
    {
        public SizeType XSize { get; set; }
        public SizeType YSize { get; set; }
    }

    public abstract record SizeType
    {
        public record None : SizeType;
        public record Stretch(float min, float max) : SizeType;
        public record Position(float anchor, float pos, float size) : SizeType;

        public Position CenterPosition(float pos, float size) => new Position(0.5f, pos, size);

        public static readonly SizeType Fill = new Stretch(0, 0);

        public void GetAnchorAndOffset(ref Vector2 anchor, ref Vector2 offset)
        {
            switch (this)
            {
                case Stretch stretch:
                    anchor = new Vector2(0, 1);
                    offset = new Vector2(stretch.min, -stretch.max);
                    break;
                
                case Position position:
                {
                    anchor = position.anchor * Vector2.one;
                    float halfSize = position.size / 2f;
                    offset = new Vector2(position.pos - halfSize , position.pos + halfSize);
                    break;
                }
            }
        }
    }

    public static class SizeableUtil
    {
        public static View<M, P> Stretch<M, P>(this View<M, P> view, float size)
            where M : Message where P : struct, IViewParameters, ISizeable 
        {
            return view.Stretch(size, size, size, size);
        }
        
        public static View<M, P> Stretch<M, P>(this View<M, P> view, float top, float bottom, float left, float right)
            where M : Message where P : struct, IViewParameters, ISizeable
        {
            view.ViewParameters.XSize = new SizeType.Stretch(left, right);
            view.ViewParameters.YSize = new SizeType.Stretch(bottom, top);
            return view;
        }
    }
}