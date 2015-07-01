using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

public class BoneDataLoader {
    //private const string modelPath =
    //            "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\Some Matlab Code\\2015-05-08 C014 R bones\\";
    private const string outputPath =
                "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\FreeBody App\\example"
                + "\\1037_C14\\walking6\\Outputs\\";

    public Quaternion[][] loadBoneRotations()
    {
        List<Quaternion[]> rotations = new List<Quaternion[]>();

        string fileName = outputPath + "Optimisation\\" + "1037_walking6_c14_new_rotations.csv";
        
        using (StreamReader reader = new StreamReader(fileName, Encoding.Default))
        {
            string line = reader.ReadLine();
            while (null != line)
            {
                float[] values = Array.ConvertAll(line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                    new Converter<string, float>(float.Parse));

                Quaternion[] frameRotations = new Quaternion[values.Length / 4];
                for (int i = 0; i < values.Length; i += 4)
                {
                    frameRotations[i / 4] = new Quaternion(values[i + 0], values[i + 1], values[i + 2], values[i + 3]);
                }
                rotations.Add(frameRotations);

                line = reader.ReadLine();
            }

            reader.Close();
        }
        return rotations.ToArray();
    }
}
