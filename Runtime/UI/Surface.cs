using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tactile.UI
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
        [SerializeField] private Parameters parameters;
        [SerializeField] private int cornerVertices = 3;

        private Mesh _surfaceMesh;
        private MeshFilter _meshFilter;

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();

            BuildSurface();
        }

        private void BuildSurface()
        {
            Debug.Log("Create surface mesh!");
            _surfaceMesh = new Mesh();
            _surfaceMesh.name = "Surface Mesh";

            var (verts, triangles) = BuildCorner(cornerVertices);
            _surfaceMesh.vertices = verts;
            _surfaceMesh.triangles = triangles;
            _surfaceMesh.Optimize();
            _meshFilter.mesh = _surfaceMesh;
        }

        /// <summary>
        /// Builds a corner of the surface.
        /// </summary>
        /// <param name="cornerSubdivisions">The number of times to subdivide the corner</param>
        /// <returns>A tuple of the corner's vertices and triangles</returns>
        private static (Vector3[], int[]) BuildCorner(int cornerSubdivisions)
        {
            // Create the corner vertices that are unique to each of the three corner "faces"
            float oneOverVerts = 1f / cornerSubdivisions;
            int numFaceVertices = cornerSubdivisions * cornerSubdivisions;
            int totalFaceVertices = 3 * numFaceVertices;
            int numEdgeVertices = cornerSubdivisions;
            int totalVerts = totalFaceVertices + 3 * numEdgeVertices + 1;
            Vector3[] cornerVertices = new Vector3[totalVerts];
            
            // Create the vertices that are unique to each "face" of the corner.
            for (int y = 0; y < cornerSubdivisions; y++)
            {
                for (int x = 0; x < cornerSubdivisions; x++)
                {
                    int i = y * cornerSubdivisions + x;

                    // We create a curved corner by placing vertices along the faces of a cube, then normalizing
                    // these vectors so that they conform to the surface of a sphere.
                    Vector3 edgeVertX = new Vector3(1f, y * oneOverVerts, x * oneOverVerts).normalized;
                    Vector3 edgeVertY = new Vector3(x * oneOverVerts, 1f, y * oneOverVerts).normalized;
                    Vector3 edgeVertZ = new Vector3(y * oneOverVerts, x * oneOverVerts, 1f).normalized;

                    cornerVertices[i] = edgeVertX;
                    cornerVertices[i + numFaceVertices] = edgeVertY;
                    cornerVertices[i + 2 * numFaceVertices] = edgeVertZ;
                }
            }

            // Create the edges that will "stitch" the different corner faces together into a larger corner.
            for (int i = 0; i < cornerSubdivisions; i++)
            {
                Vector3 edgeVertXY = new Vector3(1f, 1f, i * oneOverVerts).normalized;
                Vector3 edgeVertYZ = new Vector3(i * oneOverVerts, 1f, 1f).normalized;
                Vector3 edgeVertXZ = new Vector3(1f, i * oneOverVerts, 1f).normalized;

                cornerVertices[i + totalFaceVertices] = edgeVertXY;
                cornerVertices[i + totalFaceVertices + numEdgeVertices] = edgeVertYZ;
                cornerVertices[i + totalFaceVertices + 2 * numEdgeVertices] = edgeVertXZ;
            }
            
            // Add center vert
            cornerVertices[cornerVertices.Length - 1] = Vector3.one.normalized;
            
            // Create triangles
            List<int> triangles = new List<int>();
            triangles.Capacity = 6 * cornerSubdivisions;
            int gridQuads = cornerSubdivisions - 1;
            int centerVert = totalVerts - 1;
            
            for (int z = 0; z < 3; z++)
            {
                for (int y = 0; y < gridQuads + 1; y++)
                {
                    for (int x = 0; x < gridQuads + 1; x++)
                    {
                        int offset = z * (int)Mathf.Pow(gridQuads + 1, 2);
                        int i = offset + y * (gridQuads + 1) + x;
                        int i2 = offset + (gridQuads - y) * (gridQuads + 1) + (gridQuads - x);
                        int edgeVertX = totalFaceVertices + ((z + 2) * cornerSubdivisions + y) % (cornerSubdivisions * 3);
                        int edgeVertY = totalFaceVertices + (z * cornerSubdivisions + x) % (cornerSubdivisions * 3);

                        // Center Vert
                        if (x == gridQuads && y == gridQuads) 
                        {
                            triangles.AddRange(new[] { i, edgeVertY, edgeVertX });
                            triangles.AddRange(new[] { edgeVertY, centerVert, edgeVertX });
                        }
                        // X Edge
                        else if (x == gridQuads)
                        {
                            triangles.AddRange(new[] { i, i + cornerSubdivisions, edgeVertX });
                            triangles.AddRange(new[] { edgeVertX + 1, edgeVertX, i + cornerSubdivisions });
                        }
                        // Y Edge
                        else if (y == gridQuads)
                        {
                            triangles.AddRange(new[] { i, edgeVertY, i + 1 });
                            triangles.AddRange(new[] { edgeVertY + 1, i + 1, edgeVertY });
                        }
                        else
                        {
                            triangles.AddRange(new[] { i, i + gridQuads + 1, i + 1 });
                            triangles.AddRange(new[] { i2, i2 - (gridQuads + 1), i2 - 1 });
                        }
                    }
                }
            }

            return (cornerVertices, triangles.ToArray());
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.zero, parameters.Size);
        }

        [Serializable]
        public struct Parameters
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
            
            public Vector3 Size;
            public float topLeftRadius;
            public float topRightRadius;
            public float bottomLeftRadius;
            public float bottomRightRadius;
            public float frontDepth;
            public float dackDepth;

            public float TLTREdge => 0;
            public float TLBLEdge => 0;
            public float TRBREdge => 0;
            public float BLBREdge => 0;
        }
    }
}