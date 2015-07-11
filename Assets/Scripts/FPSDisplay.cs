using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Adapted from sample by Dave Hampson
 * http://wiki.unity3d.com/index.php?title=FramesPerSecond
 * CC BY-SA 3.0 http://creativecommons.org/licenses/by-sa/3.0/
 */
public class FPSDisplay : MonoBehaviour {

    private float deltaTime = 0.0f;
    public Text textField;

    void Update()
    {
        // low-pass IIR filter with smoothing factor of .1
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        if (textField)
        {
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            textField.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        }
    }
}
