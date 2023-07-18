using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

namespace Tactile.UI
{
    /// <summary>
    /// A customizable UI circle that can have its radius, segments, and inner diameter
    /// configured.
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class Polygon : Graphic
    {
        [FormerlySerializedAs("segments")] [SerializeField, Min(3)]
        private int sides = 3;

        [SerializeField, Range(0, 1)] private float innerRadius;
        [SerializeField, Range(0, 360)] private float offset = 0f;
        [SerializeField, Range(0, 360)] private float startAngle = 0f;
        [SerializeField, Range(0, 360)] private float endAngle = 360f;
        [SerializeField] private bool fillToSquare = true;

        private Dictionary<int, (Vector2 offset, float maxDimension)> _sideOffsetAndScales = new();
        private float _anglePerSide = 0f;
        private Vector2 _scale = Vector2.one;
        private Vector2 _pivot = Vector2.one;

        protected override void OnValidate()
        {
            base.OnValidate();
            RecalculatePolygonParameters();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            RecalculatePolygonParameters();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            float minAngle = Mathf.Min(startAngle, endAngle) * Mathf.Deg2Rad;
            float maxAngle = Mathf.Max(startAngle, endAngle) * Mathf.Deg2Rad;

            float segmentMinAngle = GetSegmentStart(minAngle);
            float angleRange = maxAngle - minAngle;
            int segments = Mathf.CeilToInt(angleRange / _anglePerSide);

            Vector2[] segmentVectors = new Vector2[segments + 1];
            
            for (int i = 0; i < segmentVectors.Length; i++)
            {
                float segmentAngle = Mathf.Min(maxAngle, Mathf.Max(minAngle, segmentMinAngle + i * _anglePerSide));
                segmentVectors[i] = AngleToUnitVector(segmentAngle);
            }

            for (int i = 0; i < segments; i++)
            {
                var startOutVec = segmentVectors[i];
                var endOutVec = segmentVectors[i+1];
                var endInVec = innerRadius * endOutVec;
                var startInVec = innerRadius * startOutVec;

                var startOutVert = UnitVectorToUIVert(startOutVec);
                var endOutVert = UnitVectorToUIVert(endOutVec);
                var endInVert = UnitVectorToUIVert(endInVec);
                var startInVert = UnitVectorToUIVert(startInVec);

                var triangleOffset = i * 4;
                var startOutIndex = triangleOffset;
                var endOutIndex = triangleOffset + 1;
                var endInIndex = triangleOffset + 2;
                var startInIndex = triangleOffset + 3;

                vh.AddVert(startOutVert);
                vh.AddVert(endOutVert);
                vh.AddVert(endInVert);
                vh.AddVert(startInVert);

                vh.AddTriangle(startOutIndex, endOutIndex, startInIndex);
                vh.AddTriangle(startInIndex, endOutIndex, endInIndex);
            }
        }

        /// <summary>
        /// Gets the start angle of the segment containing the specified angle.
        /// </summary>
        /// <param name="angle">An angle within the polygon.</param>
        /// <returns>The start angle of the segment containing the specified angle</returns>
        private float GetSegmentStart(float angle)
        {
            var segmentStart = Mathf.Floor(angle / _anglePerSide) * _anglePerSide;

            return segmentStart;
        }

        /// <summary>
        /// Gets the end angle of the segment containing the specified angle.
        /// </summary>
        /// <param name="angle">An angle within the polygon.</param>
        /// <returns>The end angle of the segment containing the specified angle</returns>
        private float GetSegmentEnd(float angle)
        {
            var segmentEnd = Mathf.Ceil(angle / _anglePerSide) * _anglePerSide;

            return segmentEnd;
        }

        /// <summary>
        /// Converts an angle to a vector along the perimeter of the polygon.
        /// </summary>
        /// <param name="angle">The angle to get the vector for.</param>
        /// <returns>A vector along the perimeter of the polygon.</returns>
        private Vector2 AngleToUnitVector(float angle)
        {
            float mod = angle % _anglePerSide;
            Vector2 unitVec;

            if (mod > 0)
            {
                var segmentStart = GetSegmentStart(angle);
                var segmentEnd = GetSegmentEnd(angle);
                var startVec = Vector2.up.Rotate(segmentStart);
                var endVec = Vector2.up.Rotate(segmentEnd);
                unitVec = Vector2.Lerp(startVec, endVec, mod / _anglePerSide);
            }
            else
            {
                unitVec = Vector2.up.Rotate(angle);
            }
            
            return unitVec;
        }

        private UIVertex UnitVectorToUIVert(Vector2 vec)
        {
            Vector2 shapeVec = vec;
            
            if (fillToSquare)
            {
                var (offset, maxDimension) = GetSideOffsetAndScale();

                // Undo offset
                shapeVec -= offset;
            
                // Fill unit square
                shapeVec /= maxDimension * 0.5f; 
            }

            shapeVec = shapeVec.Rotate(offset * Mathf.Deg2Rad);
            
            // Scale based on rect
            shapeVec = Vector2.Scale(shapeVec, _scale);
            
            // Offset through pivot
            shapeVec -= _pivot;

            var vert = shapeVec.ToSimpleVert(color);
            vert.uv0 = 0.5f * (vec + Vector2.one);

            return vert;
        }

        private (Vector2 offset, float maxDimension) GetSideOffsetAndScale()
        {
            if (!_sideOffsetAndScales.ContainsKey(sides))
            {
                Vector2[] segmentVectors = new Vector2[sides];
                float minX = float.PositiveInfinity;
                float minY = float.PositiveInfinity;
                float maxX = float.NegativeInfinity;
                float maxY = float.NegativeInfinity;

                for (int i = 0; i < sides; i++)                                                           
                {                                                                                                         
                    float segmentAngle = i * _anglePerSide;
                    var vec = Vector2.up.Rotate(segmentAngle);

                    minX = Mathf.Min(minX, vec.x);
                    minY = Mathf.Min(minY, vec.y);
                    maxX = Mathf.Max(maxX, vec.x);
                    maxY = Mathf.Max(maxY, vec.y);
                    
                    segmentVectors[i] = vec;
                }

                var center = new Vector2((minX + maxX) / 2f, (minY + maxY) / 2f);
                float maxDimension = Mathf.Max(maxX - minX, maxY - minY);

                _sideOffsetAndScales[sides] = (center, maxDimension);
            }
            
            return _sideOffsetAndScales[sides];
        }

        private void RecalculatePolygonParameters()
        {
            _anglePerSide = 2 * Mathf.PI / sides;
            var rect = rectTransform.rect;
            _scale = 0.5f * new Vector2(rect.width, rect.height);
            _pivot = Vector2.Scale(rectTransform.pivot, rect.size) - rect.size / 2f;
        }
    }
}