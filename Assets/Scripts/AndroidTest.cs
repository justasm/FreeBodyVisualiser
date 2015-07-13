using UnityEngine;
using System.IO;
using System;
using System.Collections;

public class AndroidTest : MonoBehaviour {

    void Start()
    {
        Debug.Log("persistent path " + Application.persistentDataPath);
        string path = Application.persistentDataPath +
            System.IO.Path.DirectorySeparatorChar + "test.txt";
        Debug.Log("reading from path " + path);
        try
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string line = reader.ReadLine();
                while (null != line)
                {
                    Debug.Log(line);
                    line = reader.ReadLine();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        Debug.Log("reading END - - - - -");

        Debug.Log("writing to path " + path);
        try
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("I'm a test.");
                writer.WriteLine("You're a test.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        Debug.Log("writing END - - - - -");
    }

    public void OnPathReceived(string path)
    {
        Debug.Log("Unity received path " + path);
        using (StreamReader reader = new StreamReader(path))
        {
            string line = reader.ReadLine();
            while (null != line)
            {
                Debug.Log(line);
                line = reader.ReadLine();
            }
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("performFileSearch");
        }
    }
}
