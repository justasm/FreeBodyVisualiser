using UnityEngine;
using System.Collections;

/*
 * Adapted from sample by Dave Hampson
 * http://wiki.unity3d.com/index.php?title=FramesPerSecond
 */
public class FPSDisplay : MonoBehaviour {

    float deltaTime = 0.0f;
    int w, h;
    GUIStyle style;
    Rect rect;

    void Start()
    {
        w = Screen.width;
        h = Screen.height;

        style = new GUIStyle();

        rect = new Rect(0, 0, w, h * 2 / 100);
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
