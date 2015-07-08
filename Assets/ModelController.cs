using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class ModelController : MonoBehaviour {

    private FreeBodyModel activeModel;

    public Text studyNameField;
    public Text studySubjectField;
    public InputField parameterFilenameField;
    public Button parameterLoadButton;

    public Toggle muscleToggle;
    public Toggle forceToggle;
    public Toggle boneToggle;
    public Text timeScaleField;
    public Slider timeScaleSlider;
    public Text frameValueField;
    public Slider frameSlider;

    private FrameController frameController;
    private BoneData boneData;
    private BoneMesh[] boneMeshes;
    private JointForceMesh jointForceMesh;
    private MuscleMesh muscleMesh;

    void Awake()
    {
        frameController = GetComponent<FrameController>();
        boneData = GetComponent<BoneData>();
        boneMeshes = FindObjectsOfType<BoneMesh>();
        jointForceMesh = FindObjectOfType<JointForceMesh>();
        muscleMesh = FindObjectOfType<MuscleMesh>();
    }

	void Start () {

        if (parameterLoadButton && parameterFilenameField)
        {
            parameterFilenameField.text = "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\" +
                "FreeBody App\\example\\1176_C12\\1176_walking5_C12 - Justas.xml";
            parameterLoadButton.onClick.AddListener(() =>
            {
                if (0 == parameterFilenameField.text.Length) return;
                LoadAndVisualiseModel(parameterFilenameField.text);
                // TODO async above, more sane button behaviour
                parameterLoadButton.enabled = false;
                Invoke("EnableLoadButton", 0.5f);
            });
        }

        // TODO remove if checks, no need for ambiguity
        if (muscleToggle) muscleToggle.onValueChanged.AddListener((on) => muscleMesh.gameObject.SetActive(on));
        if (forceToggle) forceToggle.onValueChanged.AddListener((on) => jointForceMesh.gameObject.SetActive(on));
        if(boneToggle) boneToggle.onValueChanged.AddListener((on) => {
            foreach (BoneMesh bone in boneMeshes) bone.gameObject.SetActive(on);
            });

        frameController.OnFrameChanged +=
            (frame) =>
            {
                frameValueField.text = frame + " / " + frameController.frameCount;
                frameSlider.value = frame;
            }; // TODO unsubscribe

        timeScaleSlider.onValueChanged.AddListener((alpha) => frameController.speedAlpha = alpha);
        frameController.OnSpeedChanged +=
            (alpha) =>
            {
                timeScaleField.text = (int)(100 * alpha) + "%";
                timeScaleSlider.value = alpha;
            }; // TODO unsubscribe
	}

    void EnableLoadButton()
    {
        parameterLoadButton.enabled = true;
    }

    void LoadAndVisualiseModel(string parameterFilePath)
    {
        // TODO cancel any existing load
        // TODO thread / yield loading routines
        // TODO display loading UI
        // TODO separate bone, joint force and muscle force loading

        ModelParameterLoader.LoadModel(parameterFilePath, out activeModel);

        DataPathUtils.UpdatePaths(activeModel);
        frameController.UpdateFrameData(activeModel);

        boneData.Reload();
        for (int i = 0; i < boneMeshes.Length; i++)
        {
            boneMeshes[i].Reload();
        }

        jointForceMesh.Reload();

        muscleMesh.Reload();

        Debug.Log("Study: " + activeModel.studyName + " by " + activeModel.responsiblePerson);
        Debug.Log("Geometry: " + activeModel.geometryOutputPath);
        Debug.Log("Optimisation: " + activeModel.optimisationOutputPath);
        Debug.Log("Frames " + activeModel.startFrame + " to " + activeModel.endFrame + " FPS: " + activeModel.framesPerSecond);
        Debug.Log(activeModel.sex + " subject, " + activeModel.height + "m, " + activeModel.mass + "kg");
        Debug.Log("Anatomy path: " + activeModel.anatomyDatasetPath);
        Debug.Log("Anatomy file: " + activeModel.anatomyDatasetFileName);

        if (studyNameField) studyNameField.text = activeModel.studyName;
        if (studySubjectField)
        {
            studySubjectField.text = activeModel.framesPerSecond + "Hz  |  " + activeModel.sex + " " +
                activeModel.height + "m " + activeModel.mass + "kg";
        }
        if (frameSlider)
        {
            frameSlider.minValue = activeModel.startFrame - 1;
            frameSlider.maxValue = activeModel.endFrame;
        }
    }
}