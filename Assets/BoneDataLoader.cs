using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

public class BoneDataLoader {
    //private const string modelPath =
    //            "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\Some Matlab Code\\2015-05-08 C014 R bones\\";
    private const string outputPath =
                "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\FreeBody App\\example"
                + "\\1037_C14\\walking6\\Outputs\\";

    public static void LoadBoneRotations(out Quaternion[] originRotations, out Quaternion[][] frameRotations)
    {
        List<Quaternion[]> _frameRotations = new List<Quaternion[]>();
        Quaternion[] _originRotations = new Quaternion[0];

        string originRotsFileName = outputPath + "Muscle_geometry\\" + "1037_walking6_c14_new_anatomy_model_orientation.csv";
        FloatCsvFileReader.ReadLines(originRotsFileName,
            (floats) =>
            {
                _originRotations = FloatCsvFileReader.FloatsToQuats(floats);
            });

        string fileName = outputPath + "Optimisation\\" + "1037_walking6_c14_new_rotations.csv";
        //                                                  "1037_walking6_c14_new_lcs_quaternion.csv";
        FloatCsvFileReader.ReadLines(fileName,
            (floats) =>
            {
                _frameRotations.Add(FloatCsvFileReader.FloatsToQuats(floats));
            });

        originRotations = _originRotations;
        frameRotations = _frameRotations.ToArray();
    }

    public static void LoadBonePositions(out Vector3[] originPositions, out Vector3[][] framePositions)
    {
        List<Vector3[]> _framePositions = new List<Vector3[]>();
        Vector3[] _originPositions = new Vector3[0];
        
        string originsFileName = outputPath + "Muscle_geometry\\" + "1037_walking6_c14_new_anatomy_model_origin.csv";
        FloatCsvFileReader.ReadLines(originsFileName,
            (floats) =>
            {
                _originPositions = FloatCsvFileReader.FloatsToVectors(floats);
            });

        string positionsFileName = outputPath + "Muscle_geometry\\" + "1037_walking6_c14_new_origins.csv";
        FloatCsvFileReader.ReadLines(positionsFileName,
            (floats) =>
            {
                _framePositions.Add(FloatCsvFileReader.FloatsToVectors(floats));
            });


        originPositions = _originPositions;
        framePositions = _framePositions.ToArray();
    }
}
