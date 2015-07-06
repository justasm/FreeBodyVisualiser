using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class BoneMesh : MonoBehaviour
{

    private const float modelScale = 1000f;

    private Mesh[] meshes;
    private Material material;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    public Shader shader;

    public enum Bone { Foot = 0, Tibia = 1, Femur = 2, Pelvis = 3, Patella = 4 };
    public Bone bone;
    public BoneData boneData;
    public FrameController controller;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(shader);

        material = new Material(shader);
        material.color = new Color(1f, 1f, 1f, 0.1f);
    }

    void Start()
    {
        meshes = StlFileReader.LoadStlFile(DataPathUtils.getBoneModelFile(bone), processVertex);
        meshes[0].name = "Bone";
        meshFilter.mesh = meshes[0];

        transform.localScale = new Vector3(boneData.scalingFactors[(int)bone].x, boneData.scalingFactors[(int)bone].y, -boneData.scalingFactors[(int)bone].z) / modelScale;
    }

    Vector3 processVertex(float x, float y, float z)
    {
        Vector3 v = new Vector3(-y, z, -x);
        v = Quaternion.Inverse(boneData.rotationOrigins[(int)bone]) * (v - modelScale * boneData.positionOrigins[(int)bone]);
        return v;
    }

    void Update()
    {
        Quaternion q = boneData.rotations[controller.frame][(int)bone];
        Vector3 v = boneData.positions[controller.frame][(int)bone];
        transform.rotation = new Quaternion(q.x, q.y, -q.z, -q.w);
        transform.position = new Vector3(v.x, v.y, -v.z);

        for (int i = 1; i < meshes.Length; i++)
        {
            Graphics.DrawMesh(meshes[i], Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale),
                meshRenderer.sharedMaterial, 0);
        }

        for (int i = 0; i < controller.frameCount; i += (controller.frameCount - 1) / 2)
        {
            for (int j = 0; j < meshes.Length; j++)
            {
                Graphics.DrawMesh(meshes[j],
                    Matrix4x4.TRS(boneData.positions[i][(int)bone], boneData.rotations[i][(int)bone], Vector3.one / modelScale),
                    material, 0);
            }
        }
    }
}
