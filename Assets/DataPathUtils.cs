using UnityEngine;
using System.Collections;
using System;

public class DataPathUtils {
    private const string boneModelPath =
                "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\Some Matlab Code\\2015-05-08 C014 R bones\\";
    private const string femurModelSubpath = "ZHAN0303-C014_R_Femur.stl";
    private const string pelvisModelSubpath = "ZHAN0303-C014_Pelvis 87_001.stl";
    private const string fibulaModelSubpath = "ZHAN0303-C014_R_Fibula.stl";
    private const string patellaModelSubpath = "ZHAN0303-C014_R_Patella.stl";
    private const string tibiaModelSubpath = "ZHAN0303-C014_R_Tibia.stl";

    private static string geoPathPrefix;

    public static string MuscleJointContactForceFile { get; private set; }
    public static string MuscleActivationMaxFile { get; private set; }

    public static string BoneRotationOriginFile { get; private set; }
    public static string BoneRotationFile { get; private set; }

    public static string BonePositionOriginFile { get; private set; }
    public static string BonePositionFile { get; private set; }

    public static string BoneScaleFile { get; private set; }

    public static string JointCenterFile { get; private set; }
    public static string JointTFContactFile { get; private set; }

    public static void UpdatePaths(FreeBodyModel model)
    {
        geoPathPrefix = model.geometryOutputPath + "\\" + model.studyName;
        string optPathPrefix = model.optimisationOutputPath + "\\" + model.studyName;

        MuscleJointContactForceFile = optPathPrefix + "_force_gcs.csv";
        MuscleActivationMaxFile = optPathPrefix + "_force_ub.csv";

        BoneRotationOriginFile = geoPathPrefix + "_anatomy_model_orientation.csv";
        BoneRotationFile = optPathPrefix + "_lcs_quaternion.csv";
        // not _rotations.csv?

        BonePositionOriginFile = geoPathPrefix + "_anatomy_model_origin.csv";
        BonePositionFile = geoPathPrefix + "_origins.csv";

        BoneScaleFile = geoPathPrefix + "_scaling_factors.csv";

        JointCenterFile = geoPathPrefix + "_rot_centres_gcs.csv";
        JointTFContactFile = geoPathPrefix + "_tf_contact_gcs.csv";
    }

    public static string GetMusclePositionFile(int muscleIndex)
    {
        return geoPathPrefix + String.Format("_muscle_path{0}.csv", muscleIndex);
    }

    public static string getBoneModelFile(BoneMesh.Bone bone)
    {
        switch (bone)
        {
            case BoneMesh.Bone.Tibia:
                return boneModelPath + tibiaModelSubpath;
            case BoneMesh.Bone.Pelvis:
                return boneModelPath + pelvisModelSubpath;
            case BoneMesh.Bone.Femur:
                return boneModelPath + femurModelSubpath;
            case BoneMesh.Bone.Patella:
                return boneModelPath + patellaModelSubpath;
            default:
                throw new System.NotImplementedException("No file for " + bone + " model data.");
        }
    }
}
