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

    public Text logField;
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

        logField.gameObject.SetActive(false);

        if (parameterLoadButton && parameterFilenameField)
        {
            //parameterFilenameField.text = "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\" +
            //    "FreeBody App\\example\\1176_C12\\1176_walking5_C12 - Justas.xml";
            parameterFilenameField.text = "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\" +
                "FreeBody App\\example\\1037_C14\\1037_walking6_C14 - Justas.xml";
            parameterLoadButton.onClick.AddListener(() =>
            {
                if (0 == parameterFilenameField.text.Length) return;
                StartCoroutine(PreventSpamClick());
                StartCoroutine(LoadAndVisualiseModel(parameterFilenameField.text));
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

    IEnumerator PreventSpamClick()
    {
        parameterLoadButton.enabled = false;
        yield return new WaitForSeconds(1);
        parameterLoadButton.enabled = true;
    }

    IEnumerator LoadAndVisualiseModel(string parameterFilePath)
    {
        // TODO cancel any existing load
        // TODO separate bone, joint force and muscle force loading

        logField.text = "Loading XML data";
        logField.gameObject.SetActive(true);
        yield return 0;

        try
        {
            ModelParameterLoader.LoadModel(parameterFilePath, out activeModel);
            logField.text = logField.text + ".. Done.";
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.LogWarning(e);
            logField.text = logField.text + "\n<color=red>Failed to find directory.</color>";
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e);
            logField.text = logField.text + "\n<color=red>Failed to find XML file.</color>";
        }
        catch (IOException e)
        {
            Debug.LogWarning(e);
            logField.text = logField.text + "\n<color=red>Failed to load XML file.</color>";
        }
        DataPathUtils.UpdatePaths(activeModel);
        frameController.UpdateFrameData(activeModel);

        logField.text = logField.text + "\nLoading muscle force data";
        yield return 0;
        muscleMesh.Reload();

        logField.text = logField.text + ".. Done.\nLoading contact force data";
        yield return 0;
        jointForceMesh.Reload();

        logField.text = logField.text + ".. Done.\nLoading bone data";
        yield return 0;
        try
        {
            boneData.Reload();
            logField.text = logField.text + ".. Done.";
        }
        catch (IOException e)
        {
            Debug.LogWarning(e);
            logField.text = logField.text + ".. <color=red>Failed.</color>";
        }

        for (int i = 0; i < boneMeshes.Length; i++)
        {
            logField.text = logField.text + "\nLoading bone " + boneMeshes[i].bone;
            yield return 0;
            try
            {
                boneMeshes[i].Reload();
                logField.text = logField.text + ".. Done.";
            }
            catch (DirectoryNotFoundException e)
            {
                Debug.LogWarning(e);
                logField.text = logField.text + ".. <color=red>Failed, directory missing.</color>";
                break;
            }
            catch (IOException e)
            {
                Debug.LogWarning(e);
                logField.text = logField.text + ".. <color=red>Failed.</color>";
            }
        }

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

        logField.text = logField.text + "\n<color=green>Complete.</color>";
        yield return new WaitForSeconds(3);
        /*float fadeOutDuration = 1f;
        for (float timer = 0; timer < fadeOutDuration; timer += Time.deltaTime)
        {
            logField.materialForRendering.color = new Color(1f, 1f, 1f, 1f - (timer / fadeOutDuration));
            yield return 0;
        }*/
        logField.gameObject.SetActive(false);
        //logField.materialForRendering.color = Color.white;
    }
}