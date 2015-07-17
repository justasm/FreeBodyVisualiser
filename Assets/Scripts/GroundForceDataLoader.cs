using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GroundForceDataLoader {

    public static void LoadGroundForces(out Vector3[] positions, out Vector3[] forces)
    {
        List<Vector3> _positions = new List<Vector3>();
        List<Vector3> _forces = new List<Vector3>();

        FloatCsvFileReader.ReadLines(DataPathUtils.GroundContactForceFile,
            (floats) =>
            {
                Vector3[] vecs = FloatCsvFileReader.FloatsRhsToLhsVectors(floats);
                _positions.Add(vecs[0]);
                _forces.Add(vecs[1]);
            });

        positions = _positions.ToArray();
        forces = _forces.ToArray();
    }
}
