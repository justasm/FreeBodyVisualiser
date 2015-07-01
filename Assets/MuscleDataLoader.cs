using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

public class MuscleDataLoader {

    private const string outputPath =
                "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\FreeBody App\\example"
                + "\\1037_C14\\walking6\\Outputs\\";
    private const int numMuscleElements = 163; // distinct muscles

    public Vector3[][] LoadMusclePaths(out int[] vertexToMuscle)
    {
        List<List<Vector3>> musclePathsPerFrame = new List<List<Vector3>>();
        List<int> vertexToMuscleList = new List<int>();

        for (int i = 0; i < numMuscleElements; i++)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            string path = outputPath + "Muscle_geometry\\"
                + String.Format("1037_walking6_c14_new_muscle_path{0}.csv", i);
            Vector3[][] musclePath = ReadMuscleDataFile(path);
#else
            string path = String.Format("c14_demo_{0}", i);
            Vector3[][] musclePath = ReadMuscleDataFileResources(path);
#endif

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

                for (int k = 0; k < segmentedLines.Count; k++)
                {
                    vertexToMuscleList.Add(i);
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

        vertexToMuscle = vertexToMuscleList.ToArray();
        return musclePathsPerFrameFinal;
    }

    private Vector3[][] ReadMuscleDataFile(string fileName)
    {
        List<Vector3[]> frames = new List<Vector3[]>();

        using (StreamReader reader = new StreamReader(fileName, Encoding.Default))
        {
            ReadVertices(reader, frames);
            reader.Close();
        }

        return frames.ToArray();
    }

    private Vector3[][] ReadMuscleDataFileResources(string filename)
    {
        List<Vector3[]> frames = new List<Vector3[]>();

        TextAsset file = Resources.Load(filename) as TextAsset;

        using (StringReader reader = new StringReader(file.text))
        {
            ReadVertices(reader, frames);
            reader.Close();
        }

        return frames.ToArray();
    }

    private void ReadVertices(TextReader reader, List<Vector3[]> frames)
    {
        string line = reader.ReadLine();
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
    }

    public float[][] LoadMuscleActivations()
    {
        List<float[]> activations = new List<float[]>();

        string fileName = outputPath + "Optimisation\\" + "1037_walking6_c14_new_force_gcs.csv";
        using (StreamReader reader = new StreamReader(fileName, Encoding.Default))
        {
            string line = reader.ReadLine();
            while (null != line)
            {
                float[] values = Array.ConvertAll(line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                    new Converter<string, float>(float.Parse));

                float[] rawActivations = new float[numMuscleElements];
                Array.Copy(values, rawActivations, numMuscleElements);
                activations.Add(rawActivations);

                line = reader.ReadLine();
            }

            reader.Close();
        }

        // TODO figure out what's up dog.. why are 2 frames missing??..
        activations.Add(new float[numMuscleElements]);
        activations.Add(new float[numMuscleElements]);

        fileName = outputPath + "Optimisation\\" + "1037_walking6_c14_new_force_ub.csv";
        int frame = -1;
        float maxActivationValue = float.MinValue;
        float minActivationValue = float.MaxValue;
        using (StreamReader reader = new StreamReader(fileName, Encoding.Default))
        {
            string line = reader.ReadLine();
            while (null != line)
            {
                ++frame;
                double[] values = Array.ConvertAll(line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                    new Converter<string, double>(double.Parse));

                for (int i = 0; i < numMuscleElements; i++)
                {
                    if (values[i] != 0) activations[frame][i] = (float)(activations[frame][i] / values[i]);
                    maxActivationValue = Math.Max(maxActivationValue, activations[frame][i]);
                    minActivationValue = Math.Min(minActivationValue, activations[frame][i]);

                    activations[frame][i] = Math.Min(activations[frame][i], 1f); // cap at 1
                }

                line = reader.ReadLine();
            }
            reader.Close();
        }

        Debug.Log("Activation values loaded min " + minActivationValue + " max " + maxActivationValue + ".");
        if(maxActivationValue > 1) Debug.Log("Activation values capped to 1.");

        return activations.ToArray();
    }
}
