using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class BoneMesh : MonoBehaviour {

    private MeshFilter meshFilter;
    public Shader shader;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(shader);
    }

    void Start()
    {
        Mesh mesh = StlFileReader.LoadStlFile(DataPathUtils.GetFemurModelFile());
        mesh.name = "Bone";
        meshFilter.mesh = mesh;

        transform.localScale = 0.001f * Vector3.one;
    }

    void Update()
    {
        
    }
}
