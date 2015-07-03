using UnityEngine;
using System.Collections;
using System;

public class DataPathUtils {
    private const string outputPathTemplate =
                "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\FreeBody App\\example"
                + "\\{0}\\{1}\\Outputs\\";
    private const string geometryOutputSubpath = "Muscle_geometry\\";
    private const string optimisationOutputSubpath = "Optimisation\\";

    private const string boneModelPath =
                "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\Some Matlab Code\\2015-05-08 C014 R bones\\";
    private const string femurModelSubpath = "ZHAN0303-C014_R_Femur.stl";
    private const string pelvisModelSubpath = "ZHAN0303-C014_Pelvis 87_001.stl";

    // Change below to easily configure which model is loaded
    private const string subject = "1037_C14";
    private const string trial = "walking6";
    private const string studyName = "1037_walking6_c14_new";

    private static string outputPath;

    public static string MuscleActivationValueFile { get; private set; }
    public static string MuscleActivationMaxFile { get; private set; }

    public static string BoneRotationOriginFile { get; private set; }
    public static string BoneRotationFile { get; private set; }

    public static string BonePositionOriginFile { get; private set; }
    public static string BonePositionFile { get; private set; }

    static DataPathUtils()
    {
        outputPath = String.Format(outputPathTemplate, subject, trial);

        MuscleActivationValueFile = outputPath + optimisationOutputSubpath + studyName + "_force_gcs.csv";
        MuscleActivationMaxFile = outputPath + optimisationOutputSubpath + studyName + "_force_ub.csv";

        BoneRotationOriginFile = outputPath + geometryOutputSubpath + studyName + "_anatomy_model_orientation.csv";
        BoneRotationFile = outputPath + optimisationOutputSubpath + studyName + "_lcs_quaternion.csv";
        // not _rotations.csv?

        BonePositionOriginFile = outputPath + geometryOutputSubpath + studyName + "_anatomy_model_origin.csv";
        BonePositionFile = outputPath + geometryOutputSubpath + studyName + "_origins.csv";
    }

    public static string GetMusclePositionFile(int muscleIndex)
    {
        return outputPath + geometryOutputSubpath +
            studyName + String.Format("_muscle_path{0}.csv", muscleIndex);
    }

    public static string GetFemurModelFile()
    {
        return boneModelPath + femurModelSubpath;
    }

    public static string GetPelvisModelFile()
    {
        return boneModelPath + pelvisModelSubpath;
    }
}
