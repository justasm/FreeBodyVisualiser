using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class BoneMesh : MonoBehaviour
{

    private const float modelScale = 1000f;

    private Mesh mesh;
    private Material material;

    private MeshFilter meshFilter;
    public Shader shader;

    public enum Bone { Foot = 0, Tibia = 1, Femur = 2, Pelvis = 3, Patella = 4 };
    public Bone bone;
    public BoneData boneData;
    public FrameController controller;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(shader);

        material = new Material(shader);
        material.color = new Color(1f, 1f, 1f, 0.1f);
    }

    void Start()
    {
        mesh = StlFileReader.LoadStlFile(DataPathUtils.getBoneModelFile(bone), processVertex);
        mesh.name = "Bone";
        meshFilter.mesh = mesh;

        transform.localScale = Vector3.one / modelScale;
    }

    Vector3 processVertex(float x, float y, float z)
    {
        Vector3 v = new Vector3(-y, z, -x);
        v = Quaternion.Inverse(boneData.rotationOrigins[(int)bone]) * (v - modelScale * boneData.positionOrigins[(int)bone]);
        return v;
    }

    void Update()
    {
        transform.rotation = boneData.rotations[controller.frame][(int)bone];
        transform.position = boneData.positions[controller.frame][(int)bone];
        for (int i = 0; i < controller.frameCount; i += 38)
        {
            Graphics.DrawMesh(mesh,
                Matrix4x4.TRS(boneData.positions[i][(int)bone], boneData.rotations[i][(int)bone], Vector3.one / modelScale),
                material, 0);
        }
    }
}
