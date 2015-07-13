using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrameController : MonoBehaviour {

    public int frame { get; private set; }
    public float frameAlpha { get; private set; }
    public int nextFrame { get; private set; }
    private float accumulator;
    [Range(0.016f, 1f)]
    public float secondsPerFrame = 0.016f;
    [Range(-1f, 1f)]
    public float speedAlpha = 0.5f;
    private float prevSpeedAlpha = 0f;

    private int startFrame = 0;
    public int frameCount = 0;

    public Toggle advanceAutomaticallyToggle;
    public Text timeScaleField;
    public Slider timeScaleSlider;
    public Text frameValueField;
    public Slider frameSlider;

    public bool AdvanceAutomatically { get; set; }

	void Start () {
        frame = 0;
        frameAlpha = 0;
        nextFrame = 1;

        AdvanceAutomatically = true;
        timeScaleSlider.onValueChanged.AddListener((alpha) => speedAlpha = alpha);
        advanceAutomaticallyToggle.isOn = AdvanceAutomatically;
        advanceAutomaticallyToggle.onValueChanged.AddListener((on) => AdvanceAutomatically = on);
	}

    void OnFrameChanged()
    {
        frameValueField.text = (startFrame + frame) + " / " + (startFrame + frameCount);
        frameSlider.value = startFrame + frame;
    }

    void OnSpeedChanged()
    {
        timeScaleField.text = (int)(100 * speedAlpha) + "%";
        timeScaleSlider.value = speedAlpha;
    }

    public void UpdateFrameData(FreeBodyModel model)
    {
        secondsPerFrame = 1f / model.framesPerSecond;
        startFrame = model.startFrame - 1;
        SetFrameCount(model.endFrame - model.startFrame + 1);
    }

    public void SetFrameCount(int newFrameCount)
    {
        if (frameCount == newFrameCount) return;
        frameCount = newFrameCount;
        frame = 0;

        frameSlider.minValue = startFrame;
        frameSlider.maxValue = startFrame + frameCount;
    }
	
	void Update () {

        float alpha;
        if (AdvanceAutomatically)
        {
            speedAlpha += Input.GetAxis("Horizontal") * Time.deltaTime;
            if (speedAlpha < -1) speedAlpha = -1;
            if (speedAlpha > 1) speedAlpha = 1;
            alpha = speedAlpha;
        }
        else
        {
            alpha = Input.GetAxis("Horizontal");
        }

        if (speedAlpha != prevSpeedAlpha)
        {
            OnSpeedChanged();
        }
        prevSpeedAlpha = speedAlpha;

        if (0 == frameCount) return;

        accumulator += alpha * Time.deltaTime;
        while (Mathf.Abs(accumulator) >= secondsPerFrame)
        {
            frame = (frameCount + frame + (int)Mathf.Sign(accumulator)) % frameCount;
            accumulator += -Mathf.Sign(accumulator) * secondsPerFrame;

            OnFrameChanged();
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
