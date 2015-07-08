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

    public void Reload()
    {
        // TODO clean up existing meshes?
        meshes = StlFileReader.LoadStlFile(DataPathUtils.getBoneModelFile(bone), ProcessVertex);
        meshes[0].name = bone.ToString();
        meshFilter.mesh = meshes[0];

        transform.localScale = Vector3.one / modelScale;
    }

    Vector3 ProcessVertex(float x, float y, float z)
    {
        Vector3 v = new Vector3(-y, z, -x);
        v = Quaternion.Inverse(boneData.rotationOrigins[(int)bone]) * (v - modelScale * boneData.positionOrigins[(int)bone]);
        return v;
    }

    void Update()
    {
        if (null == meshes || 0 == meshes.Length || null == boneData.positions) return;

        transform.rotation = Quaternion.Lerp(boneData.rotations[controller.frame][(int)bone],
            boneData.rotations[controller.nextFrame][(int)bone], controller.frameAlpha);
        transform.position = Vector3.Lerp(boneData.positions[controller.frame][(int)bone],
            boneData.positions[controller.nextFrame][(int)bone], controller.frameAlpha);

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
