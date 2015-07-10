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
                _originRotations = FloatCsvFileReader.FloatsWxyzToLhsQuats(floats);
            });
        FloatCsvFileReader.ReadLines(DataPathUtils.BonePositionOriginFile,
            (floats) =>
            {
                _originPositions = FloatCsvFileReader.FloatsRhsToLhsVectors(floats);
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
                _frameRotations.Add(FloatCsvFileReader.FloatsWxyzToLhsQuats(floats));
            });
        FloatCsvFileReader.ReadLines(DataPathUtils.BonePositionFile,
            (floats) =>
            {
                _framePositions.Add(FloatCsvFileReader.FloatsRhsToLhsVectors(floats));
            });

        frameRotations = _frameRotations.ToArray();
        framePositions = _framePositions.ToArray();
    }

    public static void LoadBoneScalingFactors(out Vector3[] scalingFactors)
    {
        List<Vector3> _scalingFactors = new List<Vector3>();

        FloatCsvFileReader.ReadLines(DataPathUtils.BoneScaleFile,
            (floats) =>
            {
                _scalingFactors.Add(FloatCsvFileReader.FloatsToVectors(floats)[0]);
            });

        scalingFactors = _scalingFactors.ToArray();
    }
}
