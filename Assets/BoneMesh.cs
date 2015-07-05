using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class BoneMesh : MonoBehaviour {

    private MeshFilter meshFilter;
    public Shader shader;

    // TODO remove
    public enum Bone { Foot = 0, Tibia = 1, Femur = 2, Pelvis = 3, Patella = 4 };
    public Bone bone;
    private Vector3[] positionOrigins;
    private Vector3[][] positions;
    private Quaternion[] rotationOrigins;
    private Quaternion[][] rotations;
    public FrameController controller;
    [Range(0, 1f)]
    public float alpha;


    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(shader);
    }

    void Start()
    {
        // TODO remove
        BoneDataLoader.LoadBoneRotations(out rotationOrigins, out rotations);
        BoneDataLoader.LoadBonePositions(out positionOrigins, out positions);

        //Quaternion rot = Quaternion.Inverse(rotationOrigins[boneIndex]);
        //for (int i = 0; i < positions.Length; i++)
        //{
        //    positions[i][boneIndex] -= positionOrigins[boneIndex];
        //    positions[i][boneIndex] = rot * positions[i][boneIndex];

        //    v = rotations[i][boneIndex] * v + positions[i][boneIndex];
        //}

        Mesh mesh = StlFileReader.LoadStlFile(DataPathUtils.getBoneModelFile(bone));
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(-vertices[i].y, vertices[i].z, -vertices[i].x);
            vertices[i] = Quaternion.Inverse(rotationOrigins[(int)bone]) * (vertices[i] - 1000 * positionOrigins[(int)bone]);
        }

        mesh.vertices = vertices;
        mesh.name = "Bone";
        meshFilter.mesh = mesh;

        transform.localScale = 0.001f * Vector3.one;
    }

    void Update()
    {
        transform.rotation = rotations[controller.frame][(int)bone];
        transform.position = positions[controller.frame][(int)bone];
    }
}
