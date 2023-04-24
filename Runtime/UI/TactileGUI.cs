using UnityEngine;
using System.Linq;

namespace Tactile.UI
{
    public static class TactileGUI
    {
        public enum SplitDirection
        {
            Vertical,
            Horizontal
        }
        
        public static Rect[] SplitRects(this Rect parentRect, SplitDirection direction, float spacing, params float[] sizes)
        {
            Rect[] contentRects = new Rect[sizes.Length];
            float totalFlex = sizes.Where(f => f > 0).Sum();
            float fixedSize = sizes.Where(f => f < 0).Select(Mathf.Abs).Sum();
            float parentSize = direction == SplitDirection.Horizontal ? parentRect.width : parentRect.height;
            float startPos = direction == SplitDirection.Horizontal ? parentRect.x : parentRect.y;
            float contentSize = parentSize - spacing * (contentRects.Length - 1) - fixedSize;
            for (int i = 0; i < contentRects.Length; i++)
            {
                float flex = sizes[i];
                float size;
                if (flex > 0)
                {
                    size = flex / totalFlex * contentSize;
                }
                else
                {
                    size = Mathf.Abs(flex);
                }
            
                Rect rect;
                if (direction == SplitDirection.Horizontal)
                {
                    rect = new Rect(startPos, parentRect.y, size, parentRect.height);
                }
                else
                {
                    
                    rect = new Rect(parentRect.x, startPos, parentRect.width, size);
                }
                
                contentRects[i] = rect;
                startPos += size + spacing;
            }

            return contentRects;
        }
    }
}