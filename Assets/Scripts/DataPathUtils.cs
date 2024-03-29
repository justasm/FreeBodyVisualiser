﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public class DataPathUtils {
    private static string boneModelPath;
    private const string anatomyFileTypeSuffix = ".xml";
    private const string anatomyFileSuffix = "_dataset" + anatomyFileTypeSuffix;

    private const string femurModelSuffix = "_Femur.stl";
    private const string pelvisModelSuffix = "_Pelvis.stl";
    private const string fibulaModelSuffix = "_Fibula.stl";
    private const string patellaModelSuffix = "_Patella.stl";
    private const string tibiaModelSuffix = "_Tibia.stl";
    private const string footModelSuffix = "_Foot.stl";

    private static Dictionary<BoneMesh.Bone, string> boneToFilename;

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

    public static string GroundContactForceFile { get; private set; }

    public static string MarkerDynamicFile { get; private set; }
    public static string MarkerVirtualStaticFile { get; private set; }

    public static void UpdatePaths(FreeBodyModel model)
    {
        if (!Path.IsPathRooted(model.geometryOutputPath))
        {
            geoPathPrefix = Path.Combine(model.rootPath, model.geometryOutputPath) +
                Path.DirectorySeparatorChar + model.studyName;
        }
        else
        {
            geoPathPrefix = model.geometryOutputPath + Path.DirectorySeparatorChar + model.studyName;
        }

        string optPathPrefix;
        if (!Path.IsPathRooted(model.optimisationOutputPath))
        {
            optPathPrefix = Path.Combine(model.rootPath, model.optimisationOutputPath) +
                Path.DirectorySeparatorChar + model.studyName;
        }
        else
        {
            optPathPrefix = model.optimisationOutputPath + Path.DirectorySeparatorChar + model.studyName;
        }

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

        GroundContactForceFile = geoPathPrefix + "_external_forces.csv";

        MarkerDynamicFile = geoPathPrefix + "_dynamic_marker.csv";
        MarkerVirtualStaticFile = geoPathPrefix + "_virtual_static_marker.csv";

        string boneFilePrefix = model.anatomyDatasetFileName;
        // TODO more rigorous error checking
        if (boneFilePrefix.EndsWith(anatomyFileSuffix))
        {
            boneFilePrefix = boneFilePrefix.Remove(boneFilePrefix.Length - anatomyFileSuffix.Length);
        }
        else if (boneFilePrefix.EndsWith(anatomyFileTypeSuffix))
        {
            boneFilePrefix = boneFilePrefix.Remove(boneFilePrefix.Length - anatomyFileTypeSuffix.Length);
        }
        if (!Path.IsPathRooted(model.anatomyDatasetPath))
        {
            boneModelPath = Path.Combine(model.rootPath, model.anatomyDatasetPath) +
                Path.DirectorySeparatorChar + boneFilePrefix + "_bones" + Path.DirectorySeparatorChar;
        }
        else
        {
            boneModelPath = model.anatomyDatasetPath +
                Path.DirectorySeparatorChar + boneFilePrefix + "_bones" + Path.DirectorySeparatorChar;
        }

        boneToFilename = new Dictionary<BoneMesh.Bone, string>();
        boneToFilename[BoneMesh.Bone.Foot] = boneModelPath + boneFilePrefix + footModelSuffix;
        boneToFilename[BoneMesh.Bone.Tibia] = boneModelPath + boneFilePrefix + tibiaModelSuffix;
        boneToFilename[BoneMesh.Bone.Fibula] = boneModelPath + boneFilePrefix + fibulaModelSuffix;
        boneToFilename[BoneMesh.Bone.Patella] = boneModelPath + boneFilePrefix + patellaModelSuffix;
        boneToFilename[BoneMesh.Bone.Femur] = boneModelPath + boneFilePrefix + femurModelSuffix;
        boneToFilename[BoneMesh.Bone.Pelvis] = boneModelPath + boneFilePrefix + pelvisModelSuffix;
    }

    public static string GetMusclePositionFile(int muscleIndex)
    {
        return geoPathPrefix + String.Format("_muscle_path{0}.csv", muscleIndex);
    }

    public static string getBoneModelFile(BoneMesh.Bone bone)
    {
        return boneToFilename[bone];
    }
}
