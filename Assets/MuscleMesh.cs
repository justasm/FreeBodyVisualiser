using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MuscleMesh : MonoBehaviour {

    private Mesh mesh;
    private MeshFilter meshFilter;
    public Shader shader;

    private MuscleDataLoader loader;
    private Vector3[][] lines;
    private float[][] lineWeights;
    private Color[][] lineColors;

    private int frame;
    private float accumulator;
    private const float secondsPerAnimationFrame = 0.016f;
    private const float minLineWidth = 0.002f;
    private const float maxLineWidth = 0.01f;

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
        int[] vertexToMuscle;
        lines = loader.LoadMusclePaths(out vertexToMuscle);
        float[][] muscleForce = loader.LoadMuscleActivations();
        lineWeights = CalculateLineWeights(vertexToMuscle, muscleForce);
        lineColors = CalculateLineColors(vertexToMuscle, muscleForce);

        AddLines(lines[0], lineWeights[0], lineColors[0]); // frame 0
        frame = 0;
    }
	
    void Update()
    {
        accumulator += Time.deltaTime;
        while (accumulator >= secondsPerAnimationFrame)
        {
            frame = (frame + 1) % lines.Length;
            accumulator -= secondsPerAnimationFrame;
        }
        UpdateLines(lines[frame], lineWeights[frame], lineColors[frame]);
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
        mesh.RecalculateBounds();
        mesh.colors = colors;
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
