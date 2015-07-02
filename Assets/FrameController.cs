using UnityEngine;
using System.Collections;

public class FrameController : MonoBehaviour {

    public int frame { get; private set; }
    private float accumulator;
    [Range(0.016f, 1f)]
    public float secondsPerFrame = 0.016f;

    public int frameCount = 0;

	void Start () {
        frame = 0;
	}
	
	void Update () {
        accumulator += Time.deltaTime;
        while (accumulator >= secondsPerFrame)
        {
            frame = (frame + 1) % frameCount;
            accumulator -= secondsPerFrame;
        }
	}
}
