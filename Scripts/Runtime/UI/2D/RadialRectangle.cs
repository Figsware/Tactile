using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tactile.UI
{
    /// <summary>
    /// Draws a rectangle with a cutout that extends radially. Useful for masking.
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class RadialRectangle : MaskableGraphic
    {
        public float startAngle
        {
            get => _startAngle;
            set
            {
                _startAngle = value;
                SetVerticesDirty();
            }
        }

        [Range(0, 360f)]
        public float angle;

        private float _startAngle;
        private float _angle;
        private Vector2[] rectangleCorners;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            CalculateRectangleCorners();

            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            Vector2 pivot = rectTransform.pivot * rectTransform.rect.size;

            // Create center vert.
            AddVertWithUV(vh, new Vector2(width / 2f, height / 2f));

            float normalizedAngle = angle / 360f;
            float remainingAngle = normalizedAngle;

            AddVertWithUV(vh, new Vector2(width, height));
            AddVertWithUV(vh, new Vector2(0, height));
            
            vh.AddTriangle(0, 1, 2);
        }

        protected void AddVertWithUV(VertexHelper vh, Vector2 point)
        {
            Vector2 pivot = rectTransform.pivot * rectTransform.rect.size;
            vh.AddVert(point - pivot, color, point / rectTransform.rect.size);
        }

        protected void CalculateRectangleCorners()
        {
            var rect = rectTransform.rect;
            float halfWidth = rect.width / 2f;
            float halfHeight = rect.height / 2f;
            Vector2 center = rect.center;

            rectangleCorners = new[]
            {
                center + new Vector2(halfWidth, halfHeight),
                center + new Vector2(-halfWidth, halfHeight),
                center + new Vector2(-halfWidth, -halfHeight),
                center + new Vector2(halfWidth, -halfHeight)
            };
        }

        /// <summary>
        /// Gets the point on a RectTransform from a ray at a specified angle.
        /// </summary>
        /// <param name="angle">Angle of ray</param>
        /// <returns>Point on rect</returns>
        protected Vector3 PointOnRectTransformWithAngle(float angle)
        {
            for (int i = 0; i < rectangleCorners.Length; i++)
            {
                Vector2 start = rectangleCorners[i];
                Vector2 end = rectangleCorners[(i + 1) % rectangleCorners.Length];
                
                
            }

            return Vector3.zero;
        }
    }

}

