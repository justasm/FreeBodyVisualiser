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
    public Shader transparentShader;

    public enum Bone { Foot = 0, Tibia = 1, Femur = 2, Pelvis = 3, Patella = 4, Fibula = 5 };
    static int BoneToIndex(Bone bone)
    {
        if (Bone.Fibula == bone) return (int) Bone.Tibia; // tibia and fibula share same transforms
        return (int) bone;
    }
    [SerializeField]
    private Bone bone;
    public Bone SelectedBone { get { return bone; } }
    private int boneIndex;
    public BoneData boneData;
    public FrameController controller;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(shader);

        material = new Material(transparentShader);
        material.color = new Color(1f, 1f, 1f, 0.1f);

        boneIndex = BoneToIndex(bone);
    }

    public void Reload()
    {
        // TODO clean up existing meshes?
        meshes = StlFileReader.LoadStlFile(DataPathUtils.getBoneModelFile(bone), ProcessVertex, GetTriangle);
        meshes[0].name = bone.ToString();
        meshFilter.mesh = meshes[0];

        transform.localScale = Vector3.one / modelScale;
    }

    Vector3 ProcessVertex(float x, float y, float z)
    {
        Vector3 v = new Vector3(-y, z, x);
        v = Quaternion.Inverse(boneData.rotationOrigins[boneIndex]) * (v - modelScale * boneData.positionOrigins[boneIndex]);
        return v;
    }

    int GetTriangle(int start, int i)
    {
        return start + 2 - i; // reverse default triangle order as ProcessVertex flips handedness
    }

    void Update()
    {
        if (null == meshes || 0 == meshes.Length || null == boneData.positions) return;

        transform.rotation = Quaternion.Lerp(boneData.rotations[controller.frame][boneIndex],
            boneData.rotations[controller.nextFrame][boneIndex], controller.frameAlpha);
        transform.position = Vector3.Lerp(boneData.positions[controller.frame][boneIndex],
            boneData.positions[controller.nextFrame][boneIndex], controller.frameAlpha);

        for (int i = 1; i < meshes.Length; i++)
        {
            Graphics.DrawMesh(meshes[i], Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale),
                meshRenderer.sharedMaterial, 0, null, 0, null, true, false);
        }

        for (int i = 0; i < controller.frameCount; i += (controller.frameCount - 1) / 2)
        {
            for (int j = 0; j < meshes.Length; j++)
            {
                Graphics.DrawMesh(meshes[j],
                    Matrix4x4.TRS(boneData.positions[i][boneIndex], boneData.rotations[i][boneIndex], Vector3.one / modelScale),
                    material, 0, null, 0, null, true, false);
            }
        }
    }
}
