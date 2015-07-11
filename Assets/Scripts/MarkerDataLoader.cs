using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarkerDataLoader {
    public static void LoadMarkerPositions(out Vector3[][] dynamicPositions, out Vector3[][] staticPositions)
    {
        List<Vector3[]> _dynamicPositions = new List<Vector3[]>();
        List<Vector3[]> _staticPositions = new List<Vector3[]>();

        FloatCsvFileReader.ReadLines(DataPathUtils.MarkerDynamicFile,
            (floats) =>
            {
                _dynamicPositions.Add(FloatCsvFileReader.FloatsRhsToLhsVectors(floats));
            });
        FloatCsvFileReader.ReadLines(DataPathUtils.MarkerVirtualStaticFile,
            (floats) =>
            {
                _staticPositions.Add(FloatCsvFileReader.FloatsRhsToLhsVectors(floats));
            });

        dynamicPositions = _dynamicPositions.ToArray();
        staticPositions = _staticPositions.ToArray();
    }
}
