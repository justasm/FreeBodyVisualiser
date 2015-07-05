using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

public class BoneDataLoader {

    public static void LoadBoneOrigins(out Quaternion[] originRotations, out Vector3[] originPositions)
    {
        Quaternion[] _originRotations = new Quaternion[0];
        Vector3[] _originPositions = new Vector3[0];

        FloatCsvFileReader.ReadLines(DataPathUtils.BoneRotationOriginFile,
            (floats) =>
            {
                _originRotations = FloatCsvFileReader.FloatsWxyzToQuats(floats);
            });
        FloatCsvFileReader.ReadLines(DataPathUtils.BonePositionOriginFile,
            (floats) =>
            {
                _originPositions = FloatCsvFileReader.FloatsToVectors(floats);
            });

        originRotations = _originRotations;
        originPositions = _originPositions;
    }

    public static void LoadBoneDynamics(out Quaternion[][] frameRotations, out Vector3[][] framePositions)
    {
        List<Quaternion[]> _frameRotations = new List<Quaternion[]>();
        List<Vector3[]> _framePositions = new List<Vector3[]>();

        FloatCsvFileReader.ReadLines(DataPathUtils.BoneRotationFile,
            (floats) =>
            {
                _frameRotations.Add(FloatCsvFileReader.FloatsWxyzToQuats(floats));
            });
        FloatCsvFileReader.ReadLines(DataPathUtils.BonePositionFile,
            (floats) =>
            {
                _framePositions.Add(FloatCsvFileReader.FloatsToVectors(floats));
            });

        frameRotations = _frameRotations.ToArray();
        framePositions = _framePositions.ToArray();
    }
}
