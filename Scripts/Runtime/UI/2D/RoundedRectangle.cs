using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tactile.UI
{
    /// <summary>
    /// A customizable rounded rectangle for the Unity UI system.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(CanvasRenderer))]  
    public class RoundedRectangle : MaskableGraphic
    {
        #region Inspector Variables
        public Color borderColor;
        public int cornerSize;
        public float borderWidth;
        public float topLeftRadius;
        public float topRightRadius;
        public float bottomLeftRadius;
        public float bottomRightRadius;
        #endregion

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            Vector2 pivot = rectTransform.pivot * rectTransform.rect.size;

            vh.AddVert(new Vector2(0, 0) - pivot, color, new Vector2(0f, 0f));
            vh.AddVert(new Vector2(0, height) - pivot, color, new Vector2(0f, 1f));
            vh.AddVert(new Vector2(width, height) - pivot, color, new Vector2(1f, 1f));
            vh.AddVert(new Vector2(width, 0) - pivot, color, new Vector2(1f, 0f));

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }

        static void CreateUnitCorner(int cornerSize)
        {

        }


    }
}
