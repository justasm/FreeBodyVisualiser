using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

public class StlFileReader {
    private const int vertexLimit = 65529; // technically 65534 but not divisible by 3 or 9!
    private const string asciiFileHeaderStart = "solid";

    public delegate Vector3 ProcessVertexDelegate(float x, float y, float z);

    public static Mesh[] LoadStlFile(string filePath, ProcessVertexDelegate processVertexDelegate)
    {
        List<Mesh> meshes = new List<Mesh>();

        using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath), Encoding.Default))
        {
            char[] start = reader.ReadChars(asciiFileHeaderStart.Length);
            if (start.ToString().Equals(asciiFileHeaderStart))
            {
                throw new IOException("ASCII .STL files are not supported.");
            }

            reader.ReadBytes(80 - asciiFileHeaderStart.Length); // drop rest of header
            uint triangleCount = reader.ReadUInt32();
            uint vertexCount = triangleCount * 3;

            Debug.Log("Reading " + triangleCount + " triangles.");

            //Vector3[] normals = new Vector3[vertexCount];
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[triangleCount * 3];

            for (int i = 0; i < triangleCount; i++)
            {
                //Vector3 normal = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                for (int j = 0; j < 3; j++) reader.ReadSingle(); // drop normal, will calculate self

                for (int j = 0; j < 3; j++)
                {
                    vertices[3 * i + j] = processVertexDelegate(
                        reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    triangles[3 * i + j] = 3 * i + j;
                    //normals[3 * i + j] = normal;
                }
                reader.ReadUInt16(); // drop attribute byte count
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
