using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class JointForceDataLoader {

    // contact forces start after force magnitudes of muscles (163 elements) and patella tendon (1 element)
    // refer to FreeBody user guide "output variables in force_gcs.csv" for details
    private const int contactForceStartIndex = MuscleDataLoader.numMuscleElements + 1;

    public static void LoadJointPositions(out Vector3[] anklePositions, out Vector3[] kneePositions,
        out Vector3[] lateralTfPositions, out Vector3[] medialTfPositions, out Vector3[] hipPositions)
    {
        List<Vector3> _anklePositions = new List<Vector3>();
        List<Vector3> _kneePositions = new List<Vector3>();
        List<Vector3> _hipPositions = new List<Vector3>();

        List<Vector3> _lateralTfPositions = new List<Vector3>();
        List<Vector3> _medialTfPositions = new List<Vector3>();

        FloatCsvFileReader.ReadLines(DataPathUtils.JointCenterFile,
            (floats) =>
            {
                Vector3[] vecs = FloatCsvFileReader.FloatsRhsToLhsVectors(floats);
                _anklePositions.Add(vecs[0]);
                _kneePositions.Add(vecs[1]);
                _hipPositions.Add(vecs[2]);
            });
        FloatCsvFileReader.ReadLines(DataPathUtils.JointTFContactFile,
            (floats) =>
            {
                Vector3[] vecs = FloatCsvFileReader.FloatsRhsToLhsVectors(floats);
                _lateralTfPositions.Add(vecs[0]);
                _medialTfPositions.Add(vecs[1]);
            });

        anklePositions = _anklePositions.ToArray();
        kneePositions = _kneePositions.ToArray();
        lateralTfPositions = _lateralTfPositions.ToArray();
        medialTfPositions = _medialTfPositions.ToArray();
        hipPositions = _hipPositions.ToArray();
    }

    public static void LoadJointContactForces(out Vector3[] ankleContactForces, out Vector3[] lateralTfContactForces,
        out Vector3[] medialTfContactForces, out Vector3[] hipContactForces)
    {
        List<Vector3> _ankleContactForces = new List<Vector3>();
        List<Vector3> _lateralTfContactForces = new List<Vector3>();
        List<Vector3> _medialTfContactForces = new List<Vector3>();
        List<Vector3> _hipContactForces = new List<Vector3>();

        // ankle, tf lateral, tf medial and hip joint contact forces, 3 components each
        int numContactForceFloats = 4 * 3;
        float[] temp = new float[numContactForceFloats];
        FloatCsvFileReader.ReadLines(DataPathUtils.MuscleJointContactForceFile,
            (values) =>
            {
                Array.Copy(values, contactForceStartIndex, temp, 0, numContactForceFloats);
                Vector3[] vecs = FloatCsvFileReader.FloatsRhsToLhsVectors(temp);
                _ankleContactForces.Add(vecs[0]);
                _lateralTfContactForces.Add(vecs[1]);
                _medialTfContactForces.Add(vecs[2]);
                _hipContactForces.Add(vecs[3]);
            });

        // add two missing frames at end of cycle
        for (int i = 0; i < 2; i++)
        {
            _ankleContactForces.Add(Vector3.zero);
            _lateralTfContactForces.Add(Vector3.zero);
            _medialTfContactForces.Add(Vector3.zero);
            _hipContactForces.Add(Vector3.zero);
        }

        ankleContactForces = _ankleContactForces.ToArray();
        lateralTfContactForces = _lateralTfContactForces.ToArray();
        medialTfContactForces = _medialTfContactForces.ToArray();
        hipContactForces = _hipContactForces.ToArray();
    }
}
