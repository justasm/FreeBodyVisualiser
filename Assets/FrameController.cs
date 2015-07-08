using UnityEngine;
using System.Collections;

public class FrameController : MonoBehaviour {

    public delegate void FrameChange(int frame);
    public event FrameChange OnFrameChanged;

    public int frame { get; private set; }
    public float frameAlpha { get; private set; }
    public int nextFrame { get; private set; }
    private float accumulator;
    [Range(0.016f, 1f)]
    public float secondsPerFrame = 0.016f;
    [Range(-1f, 1f)]
    public float speedAlpha = 1f;

    public int frameCount = 0;

	void Start () {
        frame = 0;
        frameAlpha = 0;
        nextFrame = 1;
	}

    public void UpdateFrameCount(FreeBodyModel model)
    {
        frameCount = model.endFrame - model.startFrame + 1;
        frame = 0;
    }
	
	void Update () {
        if (0 == frameCount) return;

        speedAlpha += Input.GetAxis("Horizontal") * Time.deltaTime;
        if (speedAlpha < -1) speedAlpha = -1;
        if (speedAlpha > 1) speedAlpha = 1;

        accumulator += speedAlpha * Time.deltaTime;
        while (Mathf.Abs(accumulator) >= secondsPerFrame)
        {
            frame = (frameCount + frame + (int)Mathf.Sign(accumulator)) % frameCount;
            accumulator += -Mathf.Sign(accumulator) * secondsPerFrame;

            if (null != OnFrameChanged) OnFrameChanged(frame);
        }
        nextFrame = (frameCount + frame + (int)Mathf.Sign(accumulator)) % frameCount;

        if (Mathf.Abs(nextFrame - frame) > 1)
        {
            // don't lerp if frames are not sequential, e.g. last frame -> first frame
            frameAlpha = 0;
        }
        else
        {
            frameAlpha = Mathf.Abs(accumulator) / secondsPerFrame;
        }
	}
}
