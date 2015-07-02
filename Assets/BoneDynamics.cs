using UnityEngine;
using System.Collections;

public class BoneDynamics : MonoBehaviour {

    // TODO enum
    // foot = 0; tibia = 1; femur = 2; pelvis = 3; patella = 4;
    public int boneIndex = 0;

    private Vector3[] positionOrigins;
    private Vector3[][] positions;
    private Quaternion[] rotationOrigins;
    private Quaternion[][] rotations;

    private int frame;
    private float accumulator;
    private const float secondsPerAnimationFrame = 0.016f;

    void Awake()
    {
        
    }

    void Start()
    {
        BoneDataLoader.LoadBoneRotations(out rotationOrigins, out rotations);
        BoneDataLoader.LoadBonePositions(out positionOrigins, out positions);
        frame = 0;
    }

    void Update()
    {
        accumulator += Time.deltaTime;
        while (accumulator >= secondsPerAnimationFrame)
        {
            frame = (frame + 1) % rotations.Length;
            accumulator -= secondsPerAnimationFrame;
        }

        transform.rotation = rotationOrigins[boneIndex] * rotations[frame][boneIndex];
        transform.position = positionOrigins[boneIndex] + positions[frame][boneIndex];
    }
}
