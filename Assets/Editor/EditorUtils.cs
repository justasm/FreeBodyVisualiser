using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorUtils {

    [MenuItem("Edit/Reset Playerprefs")]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

}
