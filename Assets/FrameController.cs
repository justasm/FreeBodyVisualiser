using UnityEngine;
using System.Collections;

public class FrameController : MonoBehaviour {

    public int frame { get; private set; }
    private float accumulator;
    [Range(0.016f, 1f)]
    public float secondsPerFrame = 0.016f;
    [Range(-1f, 1f)]
    public float speedAlpha = 1f;

    public int frameCount = 0;

	void Start () {
        frame = 0;
	}
	
	void Update () {
        speedAlpha += Input.GetAxis("Horizontal") * Time.deltaTime;
        if (speedAlpha < -1) speedAlpha = -1;
        if (speedAlpha > 1) speedAlpha = 1;

        accumulator += speedAlpha * Time.deltaTime;
        while (Mathf.Abs(accumulator) >= secondsPerFrame)
        {
            frame = (frameCount + frame + (int)Mathf.Sign(accumulator)) % frameCount;
            accumulator += -Mathf.Sign(accumulator) * secondsPerFrame;
        }
	}
}
