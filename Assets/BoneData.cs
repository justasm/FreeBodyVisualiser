using UnityEngine;
using System.Collections;

public class BoneData : MonoBehaviour {

    public Quaternion[] rotationOrigins { get; private set; }
    public Quaternion[][] rotations { get; private set; }
    public Vector3[] positionOrigins { get; private set; }
    public Vector3[][] positions { get; private set; }
    public Vector3[] scalingFactors { get; private set; }

    public void Reload()
    {
        Quaternion[] _rotationOrigins;
        Vector3[] _positionOrigins;
        BoneDataLoader.LoadBoneOrigins(out _rotationOrigins, out _positionOrigins);
        rotationOrigins = _rotationOrigins;
        positionOrigins = _positionOrigins;

        Quaternion[][] _rotations;
        Vector3[][] _positions;
        BoneDataLoader.LoadBoneDynamics(out _rotations, out _positions);
        rotations = _rotations;
        positions = _positions;

        Vector3[] _scalingFactors;
        BoneDataLoader.LoadBoneScalingFactors(out _scalingFactors);
        scalingFactors = _scalingFactors;
    }
}
