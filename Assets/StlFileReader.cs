using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

public class StlFileReader {
    private const int vertexLimit = 65529; // technically 65534 but not divisible by 3 or 9!
    private const string asciiFileHeaderStart = "solid";
    private const string asciiFileFooter = "endsolid";
    private static char[] asciiVertexSeparator = {' '};

    public delegate Vector3 ProcessVertexDelegate(float x, float y, float z);

    public static Mesh[] LoadStlFile(string filePath, ProcessVertexDelegate processVertexDelegate)
    {
        List<Mesh> meshes = new List<Mesh>();

        using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath), Encoding.Default))
        {
            uint vertexCount;
            Vector3[] vertices;
            int[] triangles;

            char[] start = reader.ReadChars(asciiFileHeaderStart.Length);
            if (new String(start).Equals(asciiFileHeaderStart))
            {
                reader.ReadLine(); // drop rest of name

                // TODO note that this importer expects a stricter file format than is allowed by the format
                // e.g. it is expected that the file does not have unnecessary whitespace / newlines
                Debug.Log("Reading ASCII STL.");
                List<Vector3> _vertices = new List<Vector3>();
                List<int> _triangles = new List<int>();
                vertexCount = 0;

                string line = reader.ReadLine();
                while (!line.Equals(asciiFileFooter))
                {
                    reader.ReadLine(); // outer loop

                    for (int i = 0; i < 3; i++)
                    {
                        String[] vs = reader.ReadLine().Split(asciiVertexSeparator);
                        _vertices.Add(processVertexDelegate(float.Parse(vs[1]), float.Parse(vs[2]), float.Parse(vs[3])));
                        _triangles.Add((int) vertexCount);
                        ++vertexCount;
                    }

                    reader.ReadLine(); // endloop
                    reader.ReadLine(); // endfacet

                    line = reader.ReadLine(); // start of next facet, or endsolid
                }

                vertices = _vertices.ToArray();
                triangles = _triangles.ToArray();

            }
            else
            {
                // binary format, please
                reader.ReadBytes(80 - asciiFileHeaderStart.Length); // drop rest of header
                uint triangleCount = reader.ReadUInt32();
                vertexCount = triangleCount * 3;

                Debug.Log("Reading " + triangleCount + " triangles from binary STL.");

                vertices = new Vector3[vertexCount];
                triangles = new int[triangleCount * 3];

                for (int i = 0; i < triangleCount; i++)
                {
                    for (int j = 0; j < 3; j++) reader.ReadSingle(); // drop normal, will calculate self

                    for (int j = 0; j < 3; j++)
                    {
                        vertices[3 * i + j] = processVertexDelegate(
                            reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                        triangles[3 * i + j] = 3 * i + j;
                    }
                    reader.ReadUInt16(); // drop attribute byte count
                }
            }

            if (vertexCount > vertexLimit)
            {
                uint latestProcessedVertex = 0;

                while (latestProcessedVertex < vertexCount)
                {
                    uint verticesToProcess = Math.Min(vertexLimit, vertexCount - latestProcessedVertex);

                    Vector3[] partVertices = new Vector3[verticesToProcess];
                    int[] partTriangles = new int[verticesToProcess];
                    Array.Copy(vertices, latestProcessedVertex, partVertices, 0, verticesToProcess);
                    Array.Copy(triangles, latestProcessedVertex, partTriangles, 0, verticesToProcess);
                    for (int i = 0; i < verticesToProcess; i++)
                    {
                        partTriangles[i] -= (int) latestProcessedVertex;
                    }
                    meshes.Add(CreateMesh(partVertices, partTriangles));

                    latestProcessedVertex += verticesToProcess;
                }
            }
            else
            {
                meshes.Add(CreateMesh(vertices, triangles));
            }

            reader.Close();
        }

        return meshes.ToArray();
    }

    private static Mesh CreateMesh(Vector3[] vertices, int[] triangles)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.normals = normals;

        mesh.Optimize();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        Debug.Log("Optimised mesh has " + mesh.vertexCount + " vertices and " + (mesh.triangles.Length / 3) + " triangles.");
        return mesh;
    }
}
