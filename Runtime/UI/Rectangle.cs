using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Tactile.UI
{

    /// <summary>
    /// A rectangle that can have each of its corners curved. A number of corner vertices can be specified to increase
    /// the resolution of the corners.
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    [ExecuteInEditMode]
    public class Rectangle : Graphic
    {
        [SerializeField, Min(2)] private int cornerVertices = 3;
        [SerializeField] private CornerRadii corners;

        public CornerRadii Corners
        {
            get => corners;
            set
            {
                corners = value;
                SetVerticesDirty();
            }
        }
        
        protected readonly static Dictionary<int, (Vector2[], int[])> CornerCache = new Dictionary<int, (Vector2[], int[])>();
        protected static readonly (bool, bool) TopLeftCorner = (false, true);
        protected static readonly (bool, bool) TopRightCorner = (true, true);
        protected static readonly (bool, bool) BottomLeftCorner = (false, false);
        protected static readonly (bool, bool) BottomRightCorner = (true, false);
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            
            var (tl_l, tl_o, tl_r) = CreateCorner(vh, TopLeftCorner);
            var (tr_l, tr_o, tr_r) = CreateCorner(vh, TopRightCorner);
            var (bl_l, bl_o, bl_r) = CreateCorner(vh, BottomLeftCorner);
            var (br_l, br_o, br_r) = CreateCorner(vh, BottomRightCorner);
            
            // Fill edges
            StitchEdges(vh, tl_r, tr_l, tl_o, tr_o);
            StitchEdges(vh, tr_o, tr_r, br_o, br_r);
            StitchEdges(vh, bl_o, br_o, bl_r, br_l);
            StitchEdges(vh, tl_l, tl_o, bl_l, bl_o);
            
            // Fill center
            StitchEdges(vh, tl_o, tr_o, bl_o, br_o);
        }

        protected void StitchEdges(VertexHelper vh, int topLeft, int topRight, int bottomLeft, int bottomRight)
        {
            vh.AddTriangle(bottomLeft, topLeft, topRight);
            vh.AddTriangle(bottomLeft, topRight, bottomRight);
        }

        protected UIVertex GetUICorner((bool, bool) corner)
        {
            UIVertex uiv = UIVertex.simpleVert;
            uiv.color = color;
            uiv.position = GetCorner(corner);
            
            return uiv;
        }
        
        protected Vector2 GetCorner((bool, bool) corner)
        {
            Rect rect = rectTransform.rect;
            Vector2 pivot = Vector2.Scale(rectTransform.pivot, rect.size);
            float rectWidth = corner.Item1 ? rect.width : 0;
            float rectHeight = corner.Item2 ? rect.height : 0;
            Vector2 origin = new Vector2(rectWidth, rectHeight) - pivot;
            return origin;
        }

        protected (int, int, int) CreateCorner(VertexHelper vh, (bool, bool) corner)
        {
            float cornerSize = corners[corner];
            if (cornerSize > 0)
            {
                Rect rect = rectTransform.rect;
                float width = Mathf.Min(cornerSize, rect.width / 2f) * (corner.Item1 ? 1 : -1);
                float height = Mathf.Min(cornerSize, rect.height / 2f) * (corner.Item2 ? 1 : -1);
                Vector2 origin = GetCorner(corner) - new Vector2(width, height);
                return CreateCorner(vh, width, height, origin);
            }
            else
            {
                int vertexIndex = vh.currentVertCount;
                vh.AddVert(GetUICorner(corner));
                return (vertexIndex, vertexIndex, vertexIndex);
            }
           
        }

        protected (int, int, int) CreateCorner(VertexHelper vh, float width, float height, Vector2 origin)
        {
            if (!CornerCache.ContainsKey(cornerVertices))
            {
                CornerCache.Add(cornerVertices, BuildCorner(cornerVertices));
            }

            var (verts, triangles) = CornerCache[cornerVertices];
            Vector2 scaleVector = new Vector2(width, height);
            int triangleOffset = vh.currentVertCount;
            
            // Add verts
            for (int i = 0; i < verts.Length; i++)
            {
                UIVertex uiv = UIVertex.simpleVert;
                uiv.position = Vector2.Scale(verts[i], scaleVector) + origin;
                uiv.color = color;
                vh.AddVert(uiv);
            }
            
            // Add triangles
            bool heightFlip = height < 0;
            bool widthFlip = width < 0;
            bool needFlip = heightFlip != widthFlip;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int t1 = triangles[i] + triangleOffset;
                int t2 = triangles[i + 1] + triangleOffset;
                int t3 = triangles[i + 2] + triangleOffset;
                if (needFlip)
                {
                    vh.AddTriangle(t1, t3, t2);    
                }
                else
                {
                    vh.AddTriangle(t1, t2, t3);
                }
            }


            int leftVert = verts.Length - 1 + triangleOffset;
            int originVert = triangleOffset;
            int rightVert = triangleOffset + 1;

            return (widthFlip ? rightVert : leftVert, originVert, widthFlip ? leftVert : rightVert);
        }

        protected static (Vector2[], int[]) BuildCorner(int numCornerVertices)
        {
            if (numCornerVertices < 2)
            {
                throw new ArgumentException("Cannot make a corner with less than two corner vertices.");
            }
            
            int totalTriangleIndices = 3 * (numCornerVertices - 1);
            int totalVerts = numCornerVertices + 1;
            Vector2[] verts = new Vector2[totalVerts];
            int[] triangles = new int[totalTriangleIndices];
            const float halfPi = Mathf.PI / 2f;
            verts[0] = Vector2.zero;
            verts[1] = new Vector2(1f, 0f);
            for (int i = 2; i <= numCornerVertices; i++)
            {
                float angle = ((float)(i - 1) / (numCornerVertices - 1)) * halfPi;
                Vector2 v = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                verts[i] = v;
                
                // Create origin, line segment, prev line segment triangle.
                int ti = 3 * (i - 2);
                triangles[ti] = 0;
                triangles[ti + 1] = i;
                triangles[ti + 2] = i - 1;
            }

            return (verts, triangles);
        }

        [Serializable]
        public struct CornerRadii
        {
            public float topLeft;
            public float topRight;
            public float bottomLeft;
            public float bottomRight;

            public float this[(bool, bool) corner] => GetCornerSize(corner);

            public float GetCornerSize((bool, bool) corner)
            {
                return corner switch
                {
                    (false, false) => bottomLeft,
                    (false, true) => topLeft,
                    (true, false) => bottomRight,
                    (true, true) => topRight,
                };
            }
        }
    }
}