using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MuscleMesh : MonoBehaviour {

    private Mesh mesh;
    private MeshFilter meshFilter;
    public Shader shader;

    private MuscleDataLoader loader;
    private Vector3[][] line;

    private int frame;
    private float accumulator;
    private const float secondsPerAnimationFrame = 0.016f;
    private const float lineWidth = 0.01f;

    void Awake()
    {
        mesh = new Mesh();
        mesh.name = "Muscles";
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(shader);

        loader = new MuscleDataLoader();
    }

    void Start()
    {
        line = loader.LoadMusclePaths();

        /*
        lines = new Vector3[] {
            new Vector3(0, 0, -5), new Vector3(3, 5, -3),
            new Vector3(0, 0, -1), new Vector3(-3, 4, -3),
            new Vector3(-4, 2, -2), new Vector3(1, -1, -1)};
        */
        AddLines(line[0], lineWidth);
        frame = 0;
    }
	
    void Update()
    {
        accumulator += Time.deltaTime;
        while (accumulator >= secondsPerAnimationFrame)
        {
            frame = (frame + 1) % line.Length;
            accumulator -= secondsPerAnimationFrame;
        }
        UpdateLines(line[frame], lineWidth);
	}

    // lines == line endpoints, not continuous, size = 2 x #lines
    void AddLines(Vector3[] lines, float lineWidth)
    {
        Debug.Log("Adding " + lines.Length / 2 + " lines.");

        Vector3[] vertices = new Vector3[lines.Length * 2];
        int[] triangles = new int[lines.Length * 3];
        Color[] colors = new Color[lines.Length * 2];

        for (int i = 0; i < lines.Length; i += 2)
        {
            int vi = (i / 2) * 4; // vertex start index for line quad
            UpdateQuad(vertices, vi, lines[i], lines[i + 1], lineWidth);
            System.Array.Copy(new int[] {
                vi + 0, vi + 1, vi + 2,
                vi + 1, vi + 3, vi + 2 },
                0, triangles, (i / 2) * 6, 6);
            System.Array.Copy(new Color[] { Color.red, Color.red, Color.red, Color.red }, 0, colors, vi, 4);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateBounds();
    }

    void UpdateLines(Vector3[] lines, float lineWidth)
    {
        Vector3[] vertices = mesh.vertices;
        //int[] triangles = mesh.triangles;
        //Color[] colors = mesh.colors;

        for (int i = 0; i < lines.Length; i += 2)
        {
            int vi = (i / 2) * 4; // vertex start index for line quad
            UpdateQuad(vertices, vi, lines[i], lines[i + 1], lineWidth);
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }

    void UpdateQuad(Vector3[] vertices, int start, Vector3 p1, Vector3 p2, float width)
    {
        float halfWidth = width / 2;

        Vector3 n = -Camera.main.transform.forward; // Vector3.Cross(p1, p2);
        Vector3 l = Vector3.Cross(n, p2 - p1);
        l.Normalize();

        vertices[start + 0] = p1 + l * halfWidth;
        vertices[start + 1] = p1 - l * halfWidth;
        vertices[start + 2] = p2 + l * halfWidth;
        vertices[start + 3] = p2 - l * halfWidth;

        //vertex = transform.InverseTransformPoint(vertex); // unnecessary
    }
}
