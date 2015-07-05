using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public class FloatCsvFileReader {

    public delegate void ReadLineFloatsDelegate(float[] floats);

    public static void ReadLines(string filePath, ReadLineFloatsDelegate readLineFloatsDelegate)
    {
        using (StreamReader reader = new StreamReader(filePath, Encoding.Default))
        {
            string line = reader.ReadLine();
            while (null != line)
            {
                readLineFloatsDelegate(Array.ConvertAll(
                    line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                    new Converter<string, float>(float.Parse)));
                line = reader.ReadLine();
            }

            reader.Close();
        }
    }

    public static Vector3[] FloatsToVectors(float[] floats)
    {
        Vector3[] vectors = new Vector3[floats.Length / 3];
        for (int i = 0; i < floats.Length; i += 3)
        {
            vectors[i / 3] = new Vector3(floats[i + 0], floats[i + 1], floats[i + 2]);
        }
        return vectors;
    }

    public static Quaternion[] FloatsWxyzToQuats(float[] floats)
    {
        Quaternion[] quats = new Quaternion[floats.Length / 4];
        for (int i = 0; i < floats.Length; i += 4)
        {
            // note the order of components
            quats[i / 4] = new Quaternion(floats[i + 1], floats[i + 2], floats[i + 3], floats[i + 0]);
        }
        return quats;
    }
}
