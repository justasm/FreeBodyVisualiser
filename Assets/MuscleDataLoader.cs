using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

public class MuscleDataLoader {

    private const int numMuscleElements = 163; // distinct muscles

    public static void LoadMusclePaths(out Vector3[][] frameMusclePaths, out int[] vertexToMuscle)
    {
        List<List<Vector3>> musclePathsPerFrame = new List<List<Vector3>>();
        List<int> _vertexToMuscle = new List<int>();

        for (int i = 0; i < numMuscleElements; i++)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            string path = DataPathUtils.GetMusclePositionFile(i);
            Vector3[][] musclePath = ReadMuscleDataFile(path);
#else
            string path = String.Format("c14_demo_{0}", i);
            Vector3[][] musclePath = ReadMuscleDataFileResources(path);
#endif

            for (int j = 0; j < musclePath.Length; j++)
            {
                if (musclePathsPerFrame.Count <= j)
                {
                    // no muscles found for this frame yet
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

                if (0 == j)
                {
                    // assumes each subsequent frame has same number of vertices
                    for (int k = 0; k < segmentedLines.Count; k++)
                    {
                        _vertexToMuscle.Add(i);
                    }
                }
            }
        }

        Debug.Log("Found " + musclePathsPerFrame.Count + " frames, " +
            musclePathsPerFrame[0].Count + " line vertices per frame.");
        frameMusclePaths = new Vector3[musclePathsPerFrame.Count][];
        for (int i = 0; i < frameMusclePaths.Length; i++)
        {
            frameMusclePaths[i] = musclePathsPerFrame[i].ToArray();
        }

        vertexToMuscle = _vertexToMuscle.ToArray();
    }

    private static Vector3[][] ReadMuscleDataFile(string fileName)
    {
        List<Vector3[]> frames = new List<Vector3[]>();

        using (StreamReader reader = new StreamReader(fileName, Encoding.Default))
        {
            ReadVertices(reader, frames);
            reader.Close();
        }

        return frames.ToArray();
    }

    private static Vector3[][] ReadMuscleDataFileResources(string filename)
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

    private static void ReadVertices(TextReader reader, List<Vector3[]> frames)
    {
        string line = reader.ReadLine();
        while (null != line)
        {
            float[] values = Array.ConvertAll(line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                new Converter<string, float>(float.Parse));

            frames.Add(FloatCsvFileReader.FloatsToVectors(values));

            line = reader.ReadLine();
        }
    }

    public static void LoadMuscleActivations(out float[][] activations)
    {
        List<float[]> _activations = new List<float[]>();

        FloatCsvFileReader.ReadLines(DataPathUtils.MuscleActivationValueFile,
            (values) =>
            {
                float[] rawActivations = new float[numMuscleElements];
                Array.Copy(values, rawActivations, numMuscleElements);
                _activations.Add(rawActivations);
            });

        // TODO figure out what's up dog.. why are 2 frames missing??..
        _activations.Add(new float[numMuscleElements]);
        _activations.Add(new float[numMuscleElements]);

        int frame = -1;
        float maxActivationValue = float.MinValue;
        float minActivationValue = float.MaxValue;
        using (StreamReader reader = new StreamReader(DataPathUtils.MuscleActivationMaxFile, Encoding.Default))
        {
            string line = reader.ReadLine();
            while (null != line)
            {
                ++frame;
                double[] values = Array.ConvertAll(line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                    new Converter<string, double>(double.Parse));

                for (int i = 0; i < numMuscleElements; i++)
                {
                    if (values[i] != 0) _activations[frame][i] = (float)(_activations[frame][i] / values[i]);
                    maxActivationValue = Math.Max(maxActivationValue, _activations[frame][i]);
                    minActivationValue = Math.Min(minActivationValue, _activations[frame][i]);

                    _activations[frame][i] = Math.Min(_activations[frame][i], 1f); // cap at 1
                }

                line = reader.ReadLine();
            }
            reader.Close();
        }

        Debug.Log("Activation values loaded, " + (frame + 1) + " frames, min " +
            minActivationValue + " max " + maxActivationValue + ".");
        if(maxActivationValue > 1) Debug.Log("Activation values capped to 1.");

        activations = _activations.ToArray();
    }
}
