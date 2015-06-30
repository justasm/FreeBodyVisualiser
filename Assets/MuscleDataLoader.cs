using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

public class MuscleDataLoader {

    public Vector3[][] LoadMusclePaths()
    {
        List<List<Vector3>> musclePathsPerFrame = new List<List<Vector3>>();

        // 162 distinct muscles
        for (int i = 0; i < 162; i++)
        {
            string path =
                "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\FreeBody App\\example"
                + "\\1037_C14\\walking6\\Outputs\\Muscle_geometry\\"
                + String.Format("1037_walking6_c14_new_muscle_path{0}.csv", i);

            //Debug.Log("Reading file " + path);
            Vector3[][] musclePath = ReadMuscleDataFile(path);
            for (int j = 0; j < musclePath.Length; j++)
            {
                if (musclePathsPerFrame.Count <= j)
                {
                    musclePathsPerFrame.Add(new List<Vector3>());
                }

                // break up chains into singular line segments
                List<Vector3> segmentedLines = new List<Vector3>();
                for (int k = 0; k < musclePath[j].Length; k++)
                {
                    if (k >= 2) segmentedLines.Add(musclePath[j][k - 1]);
                    segmentedLines.Add(musclePath[j][k]);
                }

                musclePathsPerFrame[j].AddRange(segmentedLines);
            }
        }

        Vector3[][] musclePathsPerFrameFinal = new Vector3[musclePathsPerFrame.Count][];
        Debug.Log("Found " + musclePathsPerFrameFinal.Length + " frames.");
        for (int i = 0; i < musclePathsPerFrameFinal.Length; i++)
        {
            musclePathsPerFrameFinal[i] = musclePathsPerFrame[i].ToArray();
        }

        return musclePathsPerFrameFinal;
    }

    private Vector3[][] ReadMuscleDataFile(string fileName)
    {
        List<Vector3[]> frames = new List<Vector3[]>();

        string line;
        using (StreamReader reader = new StreamReader(fileName, Encoding.Default))
        {
            line = reader.ReadLine();
            while (null != line)
            {
                float[] values = Array.ConvertAll(line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                    new Converter<string, float>(float.Parse));

                Vector3[] vertices = new Vector3[values.Length / 3];
                for (int i = 0; i < values.Length; i += 3)
                {
                    vertices[i / 3] = new Vector3(values[i + 0], values[i + 1], values[i + 2]);
                }
                frames.Add(vertices);

                line = reader.ReadLine();
            }

            reader.Close();
        }

        return frames.ToArray();
    }
}
