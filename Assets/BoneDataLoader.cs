using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

public class BoneDataLoader {

    public static void LoadBoneRotations(out Quaternion[] originRotations, out Quaternion[][] frameRotations)
    {
        List<Quaternion[]> _frameRotations = new List<Quaternion[]>();
        Quaternion[] _originRotations = new Quaternion[0];

        FloatCsvFileReader.ReadLines(DataPathUtils.BoneRotationOriginFile,
            (floats) =>
            {
                _originRotations = FloatCsvFileReader.FloatsWxyzToQuats(floats);
            });

        FloatCsvFileReader.ReadLines(DataPathUtils.BoneRotationFile,
            (floats) =>
            {
                _frameRotations.Add(FloatCsvFileReader.FloatsWxyzToQuats(floats));
            });

        originRotations = _originRotations;
        frameRotations = _frameRotations.ToArray();
    }

    public static void LoadBonePositions(out Vector3[] originPositions, out Vector3[][] framePositions)
    {
        List<Vector3[]> _framePositions = new List<Vector3[]>();
        Vector3[] _originPositions = new Vector3[0];
        
        FloatCsvFileReader.ReadLines(DataPathUtils.BonePositionOriginFile,
            (floats) =>
            {
                _originPositions = FloatCsvFileReader.FloatsToVectors(floats);
            });

        FloatCsvFileReader.ReadLines(DataPathUtils.BonePositionFile,
            (floats) =>
            {
                _framePositions.Add(FloatCsvFileReader.FloatsToVectors(floats));
            });


        originPositions = _originPositions;
        framePositions = _framePositions.ToArray();
    }
}
