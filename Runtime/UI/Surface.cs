using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tactile.UI.Menu
{
    /// <summary>
    /// A surface allows you to create a rounded rectangular prism with specified corner sizes. You can specify both
    /// the radi and the depth of the corners, as well as the resolution of the corners by adjusting the number
    /// of corner vertices.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [ExecuteInEditMode]
    public class Surface : MonoBehaviour
    {
        /* 
             *                 +-=-=-=-=-=-=-=-=-=-=-=-=+
             *               /                        / |
             *             /       TOP VIEW         /   |
             *           /                        /     |
             *    TL   + =-=-=-=- TLTR -=-=-=-=-=+  TR  |
             *         |                        |       |
             *                                          |
             *         T                        T       |
             *         L         FRONT          R       |
             *         B         VIEW           B       +  
             *         L                        R     /
             *                                      /
             *         |                        | /
             *    BL   +-=-=-=-=- BLBR -=-=-=-=-=+  BR
             *    
             */

        public Vector3 Size = Vector3.one;
        public Rectangle.CornerRadii corners;
        public float frontDepth = 0.25f;
        public float backDepth = 0.25f;
        public BoxCollider boxCollider;

        public float TLTREdge => 0;
        public float TLBLEdge => 0;
        public float TRBREdge => 0;
        public float BLBREdge => 0;
        [SerializeField] private int cornerVertices = 3;

        private Mesh _surfaceMesh;
        private MeshFilter _meshFilter;

        private readonly Vector3 FrontTopLeftCornerNormal = new Vector3(1, 1, 1);
        private readonly Vector3 FrontTopRightCornerNormal = new Vector3(-1, 1, 1);
        private readonly Vector3 FrontBottomLeftCornerNormal = new Vector3(1, -1, 1);
        private readonly Vector3 FrontBottomRightCornerNormal = new Vector3(-1, -1, 1);
        private readonly Vector3 BackTopLeftCornerNormal = new Vector3(-1, 1, -1);
        private readonly Vector3 BackTopRightCornerNormal = new Vector3(1, 1, -1);
        private readonly Vector3 BackBottomLeftCornerNormal = new Vector3(-1, -1, -1);
        private readonly Vector3 BackBottomRightCornerNormal = new Vector3(1, -1, -1);

        private float HalfSizeX => Size.x / 2f;
        private float HalfSizeY => Size.y / 2f;
        private float HalfSizeZ => Size.z / 2f;

        private int _verticesPerCorner;

        private Vector3 FrontTopLeftCornerScale =>
            Vector3.Scale(FrontTopLeftCornerNormal, MakeCornerScale(corners.topLeft, frontDepth));

        private Vector3 FrontTopRightCornerScale =>
            Vector3.Scale(FrontTopRightCornerNormal, MakeCornerScale(corners.topRight, frontDepth));

        private Vector3 FrontBottomLeftCornerScale =>
            Vector3.Scale(FrontBottomLeftCornerNormal, MakeCornerScale(corners.bottomLeft, frontDepth));

        private Vector3 FrontBottomRightCornerScale =>
            Vector3.Scale(FrontBottomRightCornerNormal, MakeCornerScale(corners.bottomRight, frontDepth));

        private Vector3 BackTopLeftCornerScale =>
            Vector3.Scale(BackTopLeftCornerNormal, MakeCornerScale(corners.topRight, backDepth));

        private Vector3 BackTopRightCornerScale =>
            Vector3.Scale(BackTopRightCornerNormal, MakeCornerScale(corners.topLeft, backDepth));

        private Vector3 BackBottomLeftCornerScale =>
            Vector3.Scale(BackBottomLeftCornerNormal, MakeCornerScale(corners.bottomRight, backDepth));

        private Vector3 BackBottomRightCornerScale =>
            Vector3.Scale(BackBottomRightCornerNormal, MakeCornerScale(corners.bottomLeft, backDepth));

        private Vector3 FrontTopLeftCornerOrigin =>
            Vector3.Scale(MakeOriginVector(corners.topLeft, frontDepth), FrontTopLeftCornerNormal);

        private Vector3 FrontTopRightCornerOrigin =>
            Vector3.Scale(MakeOriginVector(corners.topRight, frontDepth), FrontTopRightCornerNormal);

        private Vector3 FrontBottomLeftCornerOrigin => Vector3.Scale(MakeOriginVector(corners.bottomLeft, frontDepth),
            FrontBottomLeftCornerNormal);

        private Vector3 FrontBottomRightCornerOrigin => Vector3.Scale(MakeOriginVector(corners.bottomRight, frontDepth),
            FrontBottomRightCornerNormal);

        private Vector3 BackTopLeftCornerOrigin =>
            Vector3.Scale(MakeOriginVector(corners.topRight, backDepth), BackTopLeftCornerNormal);

        private Vector3 BackTopRightCornerOrigin =>
            Vector3.Scale(MakeOriginVector(corners.topLeft, backDepth), BackTopRightCornerNormal);

        private Vector3 BackBottomLeftCornerOrigin =>
            Vector3.Scale(MakeOriginVector(corners.bottomRight, backDepth), BackBottomLeftCornerNormal);

        private Vector3 BackBottomRightCornerOrigin => Vector3.Scale(MakeOriginVector(corners.bottomLeft, backDepth),
            BackBottomRightCornerNormal);

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();

            BuildSurface();
        }

        private Vector3 MakeOriginVector(float radius, float depth)
        {
            return new Vector3(Mathf.Max(HalfSizeX - radius, 0),
                Mathf.Max(HalfSizeY - radius, 0), Mathf.Max(HalfSizeZ - depth, 0));
        }

        private void OnValidate()
        {
            BuildSurface();
        }

        private void BuildSurface()
        {
            _surfaceMesh = new Mesh();
            _surfaceMesh.subMeshCount = 6;
            _surfaceMesh.name = "Surface Mesh";

            var corner = BuildCorner(cornerVertices);
            _verticesPerCorner = corner.VertexCount;

            int numCornerTriangles = 8 * corner.TriangleCount;
            int numFaceTriangles = 6 * 2 * 3;
            int numEdgeTriangles = 12 * 2 * cornerVertices * 2 * 3;
            int totalSurfaceTriangles = numCornerTriangles + numFaceTriangles + numEdgeTriangles;

            MeshPart surfaceMeshPart = new MeshPart(corner.VertexCount * 8, totalSurfaceTriangles);
            MeshPart[] slices = new MeshPart[8];

            // Create slices
            for (int i = 0; i < slices.Length; i++)
            {
                int triangleOffset = i * corner.VertexCount;

                MeshPart slice = surfaceMeshPart.Slice(corner.VertexCount, corner.TriangleCount);
                slice.CopyFrom(corner);
                slice.TransformTriangles(triangleOffset, i % 2 == 1);
                slices[i] = slice;
            }

            int j = 0;
            var frontTopLeftCorner = slices[j++];
            var frontTopRightCorner = slices[j++];
            var frontBottomRightCorner = slices[j++];
            var frontBottomLeftCorner = slices[j++];
            var backTopLeftCorner = slices[j++];
            var backTopRightCorner = slices[j++];
            var backBottomRightCorner = slices[j++];
            var backBottomLeftCorner = slices[j];

            // Position and scale each of the corners of the surface
            frontTopLeftCorner.TransformVertices(FrontTopLeftCornerScale, FrontTopLeftCornerOrigin);
            frontTopRightCorner.TransformVertices(FrontTopRightCornerScale, FrontTopRightCornerOrigin);
            frontBottomLeftCorner.TransformVertices(FrontBottomLeftCornerScale, FrontBottomLeftCornerOrigin);
            frontBottomRightCorner.TransformVertices(FrontBottomRightCornerScale, FrontBottomRightCornerOrigin);
            backTopLeftCorner.TransformVertices(BackTopLeftCornerScale, BackTopLeftCornerOrigin);
            backTopRightCorner.TransformVertices(BackTopRightCornerScale, BackTopRightCornerOrigin);
            backBottomRightCorner.TransformVertices(BackBottomRightCornerScale, BackBottomRightCornerOrigin);
            backBottomLeftCorner.TransformVertices(BackBottomLeftCornerScale, BackBottomLeftCornerOrigin);

            // Left Face
            surfaceMeshPart.CreateRectangleFace(
                GetCornerEdgePoint(Corner.BackTopRight, XYZ.X),
                GetCornerEdgePoint(Corner.FrontTopLeft, XYZ.X),
                GetCornerEdgePoint(Corner.BackBottomRight, XYZ.X),
                GetCornerEdgePoint(Corner.FrontBottomLeft, XYZ.X)
            );

            // Right Face
            surfaceMeshPart.CreateRectangleFace(
                GetCornerEdgePoint(Corner.FrontTopRight, XYZ.X),
                GetCornerEdgePoint(Corner.BackTopLeft, XYZ.X),
                GetCornerEdgePoint(Corner.FrontBottomRight, XYZ.X),
                GetCornerEdgePoint(Corner.BackBottomLeft, XYZ.X)
            );

            // Top Face
            surfaceMeshPart.CreateRectangleFace(
                GetCornerEdgePoint(Corner.BackTopRight, XYZ.Y),
                GetCornerEdgePoint(Corner.BackTopLeft, XYZ.Y),
                GetCornerEdgePoint(Corner.FrontTopLeft, XYZ.Y),
                GetCornerEdgePoint(Corner.FrontTopRight, XYZ.Y)
            );

            // Bottom Face
            surfaceMeshPart.CreateRectangleFace(
                GetCornerEdgePoint(Corner.FrontBottomLeft, XYZ.Y),
                GetCornerEdgePoint(Corner.FrontBottomRight, XYZ.Y),
                GetCornerEdgePoint(Corner.BackBottomRight, XYZ.Y),
                GetCornerEdgePoint(Corner.BackBottomLeft, XYZ.Y)
            );

            // Front Face
            surfaceMeshPart.CreateRectangleFace(
                GetCornerEdgePoint(Corner.FrontTopLeft, XYZ.Z),
                GetCornerEdgePoint(Corner.FrontTopRight, XYZ.Z),
                GetCornerEdgePoint(Corner.FrontBottomLeft, XYZ.Z),
                GetCornerEdgePoint(Corner.FrontBottomRight, XYZ.Z)
            );

            // Back Face
            surfaceMeshPart.CreateRectangleFace(
                GetCornerEdgePoint(Corner.BackTopLeft, XYZ.Z),
                GetCornerEdgePoint(Corner.BackTopRight, XYZ.Z),
                GetCornerEdgePoint(Corner.BackBottomLeft, XYZ.Z),
                GetCornerEdgePoint(Corner.BackBottomRight, XYZ.Z)
            );

            // Front Edges
            JoinCornerEdges(surfaceMeshPart, Corner.FrontTopLeft, Corner.FrontTopRight, Face.YZ);
            JoinCornerEdges(surfaceMeshPart, Corner.FrontTopRight, Corner.FrontBottomRight, Face.XZ);
            JoinCornerEdges(surfaceMeshPart, Corner.FrontBottomRight, Corner.FrontBottomLeft, Face.YZ);
            JoinCornerEdges(surfaceMeshPart, Corner.FrontBottomLeft, Corner.FrontTopLeft, Face.XZ);
            
            // Back Edges
            JoinCornerEdges(surfaceMeshPart, Corner.BackTopLeft, Corner.BackTopRight, Face.YZ);
            JoinCornerEdges(surfaceMeshPart, Corner.BackTopRight, Corner.BackBottomRight, Face.XZ);
            JoinCornerEdges(surfaceMeshPart, Corner.BackBottomRight, Corner.BackBottomLeft, Face.YZ);
            JoinCornerEdges(surfaceMeshPart, Corner.BackBottomLeft, Corner.BackTopLeft, Face.XZ);
            
            // Side Edges
            JoinCornerEdges(surfaceMeshPart, Corner.BackTopRight, Corner.FrontTopLeft, Face.XY);
            JoinCornerEdges(surfaceMeshPart, Corner.FrontBottomLeft, Corner.BackBottomRight, Face.XY);
            JoinCornerEdges(surfaceMeshPart, Corner.FrontTopRight, Corner.BackTopLeft, Face.XY);
            JoinCornerEdges(surfaceMeshPart, Corner.BackBottomLeft, Corner.FrontBottomRight, Face.XY);
            
            // Front / Back UV Face
            CreateUVFace(frontTopLeftCorner, frontTopRightCorner, frontBottomLeftCorner, frontBottomRightCorner, Face.XY, new Rect(0.25f, 0.375f, 0.25f, 0.25f));
            CreateUVFace(backTopLeftCorner, backTopRightCorner, backBottomLeftCorner, backBottomRightCorner, Face.XY, new Rect(0.75f, 0.375f, 0.25f, 0.25f));
            
            // Left / Right UV Face
            CreateUVFace(backTopRightCorner, frontTopLeftCorner, backBottomRightCorner, frontBottomLeftCorner, Face.YZ, new Rect(0f, 0.375f, 0.25f, 0.25f));
            CreateUVFace(frontTopRightCorner, backTopLeftCorner, frontBottomRightCorner, backBottomLeftCorner, Face.YZ, new Rect(0.5f, 0.375f, 0.25f, 0.25f));
            
            // Top / Bottom UV Face
            CreateUVFace(backTopRightCorner, backTopLeftCorner, frontTopLeftCorner, frontTopRightCorner, Face.XZ, new Rect(0.25f, 0.625f, 0.25f, 0.25f));
            CreateUVFace(frontBottomLeftCorner, frontBottomRightCorner, backBottomRightCorner, backBottomLeftCorner, Face.XZ, new Rect(0.25f, 0.125f, 0.25f, 0.25f));

            _surfaceMesh.vertices = surfaceMeshPart.vertices;
            _surfaceMesh.triangles = surfaceMeshPart.triangles;
            _surfaceMesh.uv = surfaceMeshPart.uv;
            _surfaceMesh.RecalculateNormals();
            _surfaceMesh.RecalculateTangents();
            
            GetComponent<MeshFilter>().mesh = _surfaceMesh;
            
            if (boxCollider)
            {
                boxCollider.size = Size;
            }
        }

        /// <summary>
        /// Builds a corner of the surface.
        /// </summary>
        /// <param name="cornerSubdivisions">The number of times to subdivide the corner</param>
        /// <returns>A tuple of the corner's vertices and triangles</returns>
        private static MeshPart BuildCorner(int cornerSubdivisions)
        {
            // Create the corner vertices that are unique to each of the three corner "faces"
            float oneOverVerts = 1f / (cornerSubdivisions - 1);
            int numFaceVertices = cornerSubdivisions * cornerSubdivisions;
            int totalFaceVertices = 3 * numFaceVertices;
            int numEdgeVertices = cornerSubdivisions;
            int totalVerts = totalFaceVertices + 3 * numEdgeVertices + 1;
            Vector3[] cornerVertices = new Vector3[totalVerts];
            Vector2[] uv = new Vector2[totalVerts];
            List<int> triangles = new List<int>();

            void AddTriangles(int start)
            {
                triangles.AddRange(new [] { start, start + cornerSubdivisions, start + 1 });
                triangles.AddRange(new [] { start + cornerSubdivisions, start + cornerSubdivisions + 1, start + 1 });
            }

            // Create the vertices that are unique to each "face" of the corner.
            for (int y = 0; y < cornerSubdivisions; y++)
            {
                for (int x = 0; x < cornerSubdivisions; x++)
                {
                    int i = y * cornerSubdivisions + x;
                    int iX = i;
                    int iY = i + numFaceVertices;
                    int iZ = i + 2 * numFaceVertices;

                    // We create a curved corner by placing vertices along the faces of a cube, then normalizing
                    // these vectors so that they conform to the surface of a sphere.
                    Vector3 edgeVertX = new Vector3(1f, y * oneOverVerts, x * oneOverVerts).normalized;
                    Vector3 edgeVertY = new Vector3(x * oneOverVerts, 1f, y * oneOverVerts).normalized;
                    Vector3 edgeVertZ = new Vector3(y * oneOverVerts, x * oneOverVerts, 1f).normalized;

                    cornerVertices[iX] = edgeVertX;
                    cornerVertices[iY] = edgeVertY;
                    cornerVertices[iZ] = edgeVertZ;

                    if (y != cornerSubdivisions - 1 && x != cornerSubdivisions - 1)
                    {
                        AddTriangles(iX);
                        AddTriangles(iY);
                        AddTriangles(iZ);
                    }
                }
            }

            return new MeshPart(cornerVertices, uv, triangles.ToArray());
        }

        enum Corner
        {
            FrontTopLeft,
            FrontTopRight,
            FrontBottomLeft,
            FrontBottomRight,
            BackTopLeft,
            BackTopRight,
            BackBottomLeft,
            BackBottomRight
        }

        enum Face
        {
            XY,
            YZ,
            XZ
        }

        private void CreateUVFace(MeshPart topLeft, MeshPart topRight, MeshPart bottomLeft, MeshPart bottomRight, Face face, Rect rect)
        {
            var topLeftPart = SliceCornerFace(topLeft, face);
            var topRightPart = SliceCornerFace(topRight, face);
            var bottomLeftPart = SliceCornerFace(bottomLeft, face);
            var bottomRightPart = SliceCornerFace(bottomRight, face);
            float oneOverCornerVertices = 1f / (cornerVertices - 1);
            float cornerSquareScale = 0.1f;
            float gap = 1f - cornerSquareScale;
            
            Vector2 FitToRect(Vector2 vec)
            {
                return Vector2.Scale(new Vector2(rect.xMax - rect.xMin, rect.yMax - rect.yMin), vec) +
                       new Vector2(rect.xMin, rect.yMin);
            }
            
            for (int y = 0; y < cornerVertices; y++)
            {
                for (int x = 0; x < cornerVertices; x++)
                {
                    int i = y * cornerVertices + x;
                    Vector2 uv = new Vector2(x * oneOverCornerVertices, y * oneOverCornerVertices);
                    
                    // Rotate initial UV based on face
                    uv = face switch
                    {
                        Face.XY => uv,
                        Face.YZ => new Vector2(uv.y, uv.x),
                        Face.XZ => new Vector2(uv.y, uv.x),
                        _ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
                    };

                    Vector2 topLeftUv = FitToRect(cornerSquareScale * new Vector2(1 - uv.y, uv.x) + gap * new Vector2(0, 1));
                    topLeftPart.SetUV(i, topLeftUv);

                    Vector2 topRightUv = FitToRect(cornerSquareScale * new Vector2(uv.y, uv.x) + gap * new Vector2(1, 1));
                    topRightPart.SetUV(i, topRightUv);

                    Vector2 bottomLeftUv = FitToRect(cornerSquareScale * new Vector2(1 - uv.y, 1 - uv.x));
                    bottomLeftPart.SetUV(i, bottomLeftUv);

                    Vector2 bottomRightUv = FitToRect(cornerSquareScale * new Vector2(uv.y, 1 - uv.x) + gap * new Vector2(1, 0));
                    bottomRightPart.SetUV(i, bottomRightUv);
                }
            }
        }

        private MeshPart SliceCornerFace(MeshPart corner, Face face)
        {
            int faceVertices = cornerVertices * cornerVertices;
            int numTriangles = 6 * faceVertices;
            
            MeshPart part = face switch
            {
                Face.YZ => corner.Slice(0, faceVertices, 0, numTriangles),
                Face.XZ => corner.Slice(faceVertices, faceVertices, numTriangles, numTriangles),
                Face.XY => corner.Slice(2 * faceVertices, faceVertices, 3 * numTriangles, numTriangles),
                _ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
            };

            return part;
        }

        private void JoinCornerEdges(MeshPart surfaceMesh, Corner left, Corner right, Face face)
        {
            int leftCornerOffset = GetCornerOffset(left);
            int rightCornerOffset = GetCornerOffset(right);

            var (edgeAOffset, edgeBOffset) = face switch
            {
                Face.XY => (cornerVertices * cornerVertices, 0),
                Face.YZ => (cornerVertices * cornerVertices, 2 * cornerVertices * cornerVertices),
                Face.XZ => (0, 2 * cornerVertices * cornerVertices),
                _ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
            };

            var (edgeAStep, edgeBStep) = face switch
            {
                Face.XY => (1, cornerVertices),
                Face.YZ => (cornerVertices, 1),
                Face.XZ => (1, cornerVertices),
                _ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
            };
            
            for (int i = 0; i < cornerVertices - 1; i++)
            {
                surfaceMesh.CreateRectangleFace(
                    leftCornerOffset + edgeAOffset + edgeAStep * i,
                    rightCornerOffset + edgeAOffset + edgeAStep * i,
                    leftCornerOffset + edgeAOffset + edgeAStep * (i + 1),
                    rightCornerOffset + edgeAOffset + edgeAStep * (i + 1)
                );

                surfaceMesh.CreateRectangleFace(
                    rightCornerOffset + edgeBOffset + edgeBStep * i,
                    leftCornerOffset + edgeBOffset + edgeBStep * i,
                    rightCornerOffset + edgeBOffset + edgeBStep * (i + 1),
                    leftCornerOffset + edgeBOffset + edgeBStep * (i + 1)
                );
            }
        }

        enum XYZ
        {
            X,
            Y,
            Z
        }

        private int GetCornerEdgePoint(Corner corner, XYZ face)
        {
            int index = face switch
            {
                XYZ.X => 0,
                XYZ.Y => cornerVertices * cornerVertices,
                XYZ.Z => 2 * cornerVertices * cornerVertices,
                _ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
            };

            index += GetCornerOffset(corner);

            return index;
        }

        private int GetCornerOffset(Corner corner)
        {
            return _verticesPerCorner * corner switch
            {
                Corner.FrontTopLeft => 0,
                Corner.FrontTopRight => 1,
                Corner.FrontBottomRight => 2,
                Corner.FrontBottomLeft => 3,
                Corner.BackTopLeft => 4,
                Corner.BackTopRight => 5,
                Corner.BackBottomLeft => 7,
                Corner.BackBottomRight => 6,
                _ => throw new ArgumentOutOfRangeException(nameof(corner), corner, null)
            };
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.zero, Size);
        }

        private Vector3 MakeCornerScale(float size, float depth)
        {
            return new Vector3(Mathf.Min(HalfSizeX, size), Mathf.Min(HalfSizeY, size), Mathf.Min(HalfSizeZ, depth));
        }

        private class MeshPart
        {
            public Vector3[] vertices;
            public int[] triangles;
            public Vector2[] uv;

            public int VertexCount => _verticesLength;
            public int TriangleCount => _trianglesLength;

            private int _vertexIndex = 0;
            private int _triangleIndex = 0;
            private readonly int _verticesOffset;
            private readonly int _verticesLength;
            private readonly int _trianglesOffset;
            private readonly int _trianglesLength;

            public MeshPart(int numVertices, int numTriangles)
            {
                vertices = new Vector3[numVertices];
                triangles = new int[numTriangles];
                uv = new Vector2[numVertices];
                _verticesOffset = 0;
                _verticesLength = numVertices;
                _trianglesOffset = 0;
                _trianglesLength = numTriangles;
            }


            public MeshPart(Vector3[] vertices, Vector2[] uv, int[] triangles)
            {
                this.vertices = vertices;
                this.triangles = triangles;
                this.uv = uv;
                _verticesOffset = 0;
                _verticesLength = vertices.Length;
                _trianglesOffset = 0;
                _trianglesLength = triangles.Length;
            }

            public MeshPart(Vector3[] vertices, int[] triangles, Vector2[] uv, int verticesOffset, int verticesLength,
                int trianglesOffset, int trianglesLength)
            {
                this.vertices = vertices;
                this.triangles = triangles;
                this.uv = uv;
                _verticesOffset = verticesOffset;
                _verticesLength = verticesLength;
                _trianglesOffset = trianglesOffset;
                _trianglesLength = trianglesLength;
            }

            public void CreateRectangleFace(int topLeftIndex, int topRightIndex, int bottomLeftIndex,
                int bottomRightIndex)
            {
                CreateRectangleFace(_triangleIndex, topLeftIndex, topRightIndex, bottomLeftIndex, bottomRightIndex);
                _triangleIndex += 6;
            }

            public void CreateRectangleFace(int offset, int topLeftIndex, int topRightIndex, int bottomLeftIndex,
                int bottomRightIndex)
            {
                // TL --- TR
                // |  \ /  |
                // |  / \  |
                // BL --- BR

                CreateTriangleFace(offset, topLeftIndex, topRightIndex, bottomLeftIndex);
                CreateTriangleFace(offset + 3, bottomLeftIndex, topRightIndex, bottomRightIndex);
            }

            public void CreateTriangleFace(int a, int b, int c)
            {
                CreateTriangleFace(_triangleIndex, a, b, c);
                _triangleIndex += 3;
            }

            public void CreateTriangleFace(int offset, int a, int b, int c)
            {
                triangles[offset] = a;
                triangles[offset + 1] = b;
                triangles[offset + 2] = c;
            }

            public void CopyFrom(MeshPart meshPart)
            {
                for (int i = 0; i < meshPart.VertexCount; i++)
                {
                    var vert = meshPart.GetVertex(i);
                    var uvVec = meshPart.GetUV(i);
                    SetVertex(i, vert);
                    SetUV(i, uvVec);
                }

                for (int i = 0; i < meshPart.TriangleCount; i++)
                {
                    var triangle = meshPart.GetTriangle(i);
                    SetTriangle(i, triangle);
                }
            }

            public void TransformVertices(Vector3 scale, Vector3 offset)
            {
                for (int i = 0; i < VertexCount; i++)
                {
                    var vert = GetVertex(i);
                    SetVertex(i, Vector3.Scale(vert, scale) + offset);
                }
            }

            public void TransformTriangles(int offset, bool reverse)
            {
                // No need to transform if the offset is zero and we're not reversing.
                if (offset == 0 && !reverse)
                    return;

                for (int i = 0; i < TriangleCount; i += 3)
                {
                    var triangle = (GetTriangle(i) + offset, GetTriangle(i + 1) + offset, GetTriangle(i + 2) + offset);

                    if (reverse)
                    {
                        var (a, b, c) = triangle;
                        triangle = (c, b, a);
                    }

                    SetTriangle(i, triangle.Item1);
                    SetTriangle(i + 1, triangle.Item2);
                    SetTriangle(i + 2, triangle.Item3);
                }
            }

            public void TransformUV(Vector2 scale, Vector2 offset)
            {
                for (int i = 0; i < VertexCount; i++)
                {
                    var uvVert = GetUV(i);
                    SetUV(i, Vector2.Scale(uvVert, scale) + offset);
                }
            }

            public MeshPart Slice(int numVertices, int numTriangles)
            {
                MeshPart slice = Slice(_vertexIndex, numVertices, _triangleIndex, numTriangles);
                _vertexIndex += numVertices;
                _triangleIndex += numTriangles;

                return slice;
            }

            public MeshPart Slice(int startVertex, int numVertices, int startTriangle, int numTriangles)
            {
                startVertex += _verticesOffset;
                startTriangle += _trianglesOffset;
                
                MeshPart slicedMeshPart = new MeshPart(vertices, triangles, uv, startVertex, numVertices, startTriangle,
                    numTriangles);

                return slicedMeshPart;
            }

            public int GetVertexIndex(int i) => i + _verticesOffset;

            public Vector3 GetVertex(int i) => vertices[i + _verticesOffset];

            public Vector3 GetUV(int i) => uv[i + _verticesOffset];

            public void SetVertex(int i, Vector3 vertex) => vertices[i + _verticesOffset] = vertex;

            public void SetUV(int i, Vector2 vec) => uv[i + _verticesOffset] = vec;

            public int GetTriangle(int i) => triangles[i + _trianglesOffset];

            public void SetTriangle(int i, int triangle)
            {
                triangles[i + _trianglesOffset] = triangle;
            }
        }
    }
}