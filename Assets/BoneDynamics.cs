using UnityEngine;
using System.Collections;

public class BoneDynamics : MonoBehaviour {

    // TODO enum
    // foot = 0; tibia = 1; femur = 2; pelvis = 3; patella = 4;
    public int boneIndex = 0;

    public FrameController controller;

    private Vector3[] positionOrigins;
    private Vector3[][] positions;
    private Quaternion[] rotationOrigins;
    private Quaternion[][] rotations;

    void Start()
    {
        //BoneDataLoader.LoadBoneRotations(out rotationOrigins, out rotations);
        //BoneDataLoader.LoadBonePositions(out positionOrigins, out positions);

    }

    void Update()
    {
        //transform.localRotation = rotations[controller.frame][boneIndex] * Quaternion.Euler(-90, 0, 0);

        //transform.localRotation = rotationOrigins[boneIndex];
        //transform.rotation = Quaternion.Inverse(rotations[controller.frame][boneIndex]);
        //transform.position = positions[controller.frame][boneIndex] - positionOrigins[boneIndex];
    }
}
