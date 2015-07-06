using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MuscleMesh : MonoBehaviour {

    private Mesh mesh;
    private MeshFilter meshFilter;
    public Shader shader;

    private Vector3[][] lines;
    private float[][] lineWeights;
    private Color[][] lineColors;

    public Vector3 Centroid { get; private set; }

    public FrameController controller;
    private const float minLineWidth = 0.001f;
    private const float maxLineWidth = 0.02f;

    void Awake()
    {
        mesh = new Mesh();
        mesh.name = "Muscles";
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(shader);
    }

    void Start()
    {
        int[] vertexToMuscle;
        float[][] muscleForce;
        MuscleDataLoader.LoadMusclePaths(out lines, out vertexToMuscle);
        MuscleDataLoader.LoadMuscleActivations(out muscleForce);
        lineWeights = CalculateLineWeights(vertexToMuscle, muscleForce);
        lineColors = CalculateLineColors(vertexToMuscle, muscleForce);

        AddLines(lines[controller.frame], lineWeights[controller.frame], lineColors[controller.frame]);

        Centroid = new Vector3();
        int count = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                ++count;
                Centroid += lines[i][j];
            }
        }
        Centroid /= count;
    }
	
    void Update()
    {
        UpdateLines(
            Lerp(lines[controller.frame], lines[controller.nextFrame], controller.frameAlpha),
            Lerp(lineWeights[controller.frame], lineWeights[controller.nextFrame], controller.frameAlpha),
            Lerp(lineColors[controller.frame], lineColors[controller.nextFrame], controller.frameAlpha));
	}

    Vector3[] Lerp(Vector3[] v1, Vector3[] v2, float t)
    {
        Vector3[] v = new Vector3[v1.Length];
        for (int i = 0; i < v.Length; i++)
        {
            v[i] = Vector3.Lerp(v1[i], v2[i], t);
        }
        return v;
    }

    float[] Lerp(float[] f1, float[] f2, float t)
    {
        float[] f = new float[f1.Length];
        for (int i = 0; i < f.Length; i++)
        {
            f[i] = f1[i] + (f2[i] - f1[i]) * t;
        }
        return f;
    }

    Color[] Lerp(Color[] c1, Color[] c2, float t)
    {
        Color[] c = new Color[c1.Length];
        for (int i = 0; i < c.Length; i++)
        {
            c[i] = Color.Lerp(c1[i], c2[i], t);
        }
        return c;
    }

    // lines == line endpoints, not continuous, size = 2 x #lines
    void AddLines(Vector3[] lines, float[] lineWeights, Color[] lineColors)
    {
        Debug.Log("Adding " + lines.Length / 2 + " lines.");

        Vector3[] vertices = new Vector3[lines.Length * 2];
        int[] triangles = new int[lines.Length * 3];
        Color[] colors = new Color[lines.Length * 2];

        for (int i = 0; i < lines.Length; i += 2)
        {
            int vi = (i / 2) * 4; // vertex start index for line quad
            UpdateQuad(vertices, vi, lines[i], lines[i + 1], lineWeights[i]);
            for (int j = 0; j < 4; j++)
            {
                colors[vi + j] = lineColors[i];
            }
            System.Array.Copy(new int[] {
                vi + 0, vi + 1, vi + 2,
                vi + 1, vi + 3, vi + 2 },
                0, triangles, (i / 2) * 6, 6);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateBounds();
    }

    void UpdateLines(Vector3[] lines, float[] lineWeights, Color[] lineColors)
    {
        Vector3[] vertices = mesh.vertices;
        Color[] colors = mesh.colors;

        for (int i = 0; i < lines.Length; i += 2)
        {
            int vi = (i / 2) * 4; // vertex start index for line quad
            UpdateQuad(vertices, vi, lines[i], lines[i + 1], lineWeights[i]);
            for (int j = 0; j < 4; j++)
            {
                colors[vi + j] = lineColors[i];
            }
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
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

    // per frame per vertex
    static float[][] CalculateLineWeights(int[] vertexToMuscle, float[][] muscleForce)
    {
        float[][] lineWeights = new float[muscleForce.Length][];

        for (int i = 0; i < lineWeights.Length; i++)
        {
            lineWeights[i] = new float[vertexToMuscle.Length];
            for (int j = 0; j < vertexToMuscle.Length; j++)
            {
                lineWeights[i][j] = minLineWidth + muscleForce[i][vertexToMuscle[j]] * (maxLineWidth - minLineWidth);
                //lineWeights[i][j] = Mathf.Max(minLineWidth, Mathf.Min(lineWeights[i][j], maxLineWidth));
            }
        }

        return lineWeights;
    }

    // per frame per vertex
    static Color[][] CalculateLineColors(int[] vertexToMuscle, float[][] muscleForce)
    {
        Color[][] lineColors = new Color[muscleForce.Length][];

        for (int i = 0; i < lineColors.Length; i++)
        {
            lineColors[i] = new Color[vertexToMuscle.Length];
            for (int j = 0; j < vertexToMuscle.Length; j++)
            {
                lineColors[i][j] = //muscleForce[i][vertexToMuscle[j]] > 0.005f ? Color.red : Color.gray;
                                    Color.Lerp(Color.black, Color.red, muscleForce[i][vertexToMuscle[j]]);
            }
        }

        return lineColors;
    }
}
