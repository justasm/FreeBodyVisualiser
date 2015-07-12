﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;
using UnityEngine.UI;
using System.IO.IsolatedStorage;
using System;

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
    public Toggle markerToggle;
    public Toggle boneGhostToggle;
    public Text markerSizeField;
    public Slider markerSizeSlider;
    public Toggle dynamicMarkerToggle;
    public Toggle staticMarkerToggle;

    public GameObject bonesGroup;

    public GameObject muscleVisibilityToggle;
    public Transform muscleVisibilityContentPanel;
    public GameObject boneVisibilityToggle;
    public Transform boneVisibilityContentPanel;

    private FrameController frameController;
    private BoneData boneData;
    private BoneMesh[] boneMeshes;
    private JointForceMesh jointForceMesh;
    private MuscleMesh muscleMesh;
    private MarkerMesh markerMesh;

    void Awake()
    {
        frameController = GetComponent<FrameController>();
        boneData = GetComponent<BoneData>();
        boneMeshes = FindObjectsOfType<BoneMesh>();
        jointForceMesh = FindObjectOfType<JointForceMesh>();
        muscleMesh = FindObjectOfType<MuscleMesh>();
        markerMesh = FindObjectOfType<MarkerMesh>();
    }

	void Start () {

        logField.gameObject.SetActive(false);

        parameterLoadButton.onClick.AddListener(() =>
        {
            if (0 == parameterFilenameField.text.Length) return;
            StartCoroutine(PreventSpamClick());
            StartCoroutine(LoadAndVisualiseModel(parameterFilenameField.text));
        });

        muscleToggle.onValueChanged.AddListener((on) => muscleMesh.gameObject.SetActive(on));
        forceToggle.onValueChanged.AddListener((on) => jointForceMesh.gameObject.SetActive(on));
        boneToggle.onValueChanged.AddListener((on) => bonesGroup.SetActive(on));
        markerToggle.onValueChanged.AddListener((on) => markerMesh.gameObject.SetActive(on));
        boneGhostToggle.onValueChanged.AddListener(
            (on) =>
            {
                foreach (BoneMesh boneMesh in boneMeshes) boneMesh.RenderGhost = on;
            });

        markerSizeSlider.onValueChanged.AddListener(
            (value) =>
            {
                markerMesh.SetSizeMultiplier(value);
                markerSizeField.text = (int)(100 * value) + "%";
            });

        dynamicMarkerToggle.isOn = markerMesh.ShowDynamicMarkers;
        staticMarkerToggle.isOn = markerMesh.ShowStaticMarkers;
        dynamicMarkerToggle.onValueChanged.AddListener((on) => markerMesh.ShowDynamicMarkers = on);
        staticMarkerToggle.onValueChanged.AddListener((on) => markerMesh.ShowStaticMarkers = on);

        for (int i = 0; i < MuscleGroup.groups.Count; i++)
        {
            MuscleGroup group = MuscleGroup.groups[i];
            GameObject newMuscleGroupToggle = Instantiate(muscleVisibilityToggle) as GameObject;

            ToggleWrapper muscleGroupToggle = newMuscleGroupToggle.GetComponent<ToggleWrapper>();
            muscleGroupToggle.label.text = group.name;
            muscleGroupToggle.toggle.isOn = muscleMesh.visibility[group.index];
            muscleGroupToggle.toggle.onValueChanged.AddListener((enabled) => muscleMesh.SetVisibility(group, enabled));

            newMuscleGroupToggle.transform.SetParent(muscleVisibilityContentPanel);
        }

#if UNITY_EDITOR
        //string defaultPath = "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\" +
        //        "FreeBody App\\example\\1176_C12\\1176_walking5_C12 - Justas.xml";
        string defaultPath = "C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\" +
                "FreeBody App\\example\\1037_C14\\1037_walking6_C14 - Justas.xml";
#else
        string defaultPath = "";
#endif
        string autoloadPath = "";

        string[] args = System.Environment.GetCommandLineArgs();
        if (null != args)
        {
            int i = 1; // first argument will always be program
            while (i < args.Length)
            {
                switch (args[i])
                {
                    case "-fb-default-xml-path":
                        if (i + 1 < args.Length)
                        {
                            ++i;
                            defaultPath = args[i];
                        }
                        else
                        {
                            Debug.LogError("Argument " + args[i] + " needs a parameter.");
                        }
                        break;
                    case "-fb-autoload-xml-path":
                        if (i + 1 < args.Length)
                        {
                            ++i;
                            autoloadPath = args[i];
                        }
                        else
                        {
                            Debug.LogError("Argument " + args[i] + " needs a parameter.");
                        }
                        break;
                    default:
                        if (i > 1) break;
                        defaultPath = args[i];
                        autoloadPath = args[i];
                        break;
                }
                ++i;
            }
        }

        parameterFilenameField.text = defaultPath;
        if (autoloadPath.Length > 0) StartCoroutine(LoadAndVisualiseModel(autoloadPath));
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

        logField.text = "Loading study XML file";
        logField.gameObject.SetActive(true);
        yield return 0;

        #region load xml parameters
        LoadCatchErrors(() => ModelParameterLoader.LoadModel(parameterFilePath, out activeModel));
        DataPathUtils.UpdatePaths(activeModel);
        frameController.UpdateFrameData(activeModel);
        #endregion

        #region load muscle positions
        appendToLog("\nLoading muscle positions");
        yield return 0;
        bool musclePathsLoaded = false;
        LoadCatchErrors(
            () =>
            {
                muscleMesh.ReloadMusclePaths();
                musclePathsLoaded = true;
            });
        #endregion

        if (musclePathsLoaded && muscleMesh.GetFrameCount() != frameController.frameCount)
        {
            appendToLog("\n<color=blue>Muscle frame count (" + muscleMesh.GetFrameCount() +
                ") does not match frame count specified in XML file (" + frameController.frameCount + ").</color>");
            frameController.SetFrameCount(Mathf.Min(muscleMesh.GetFrameCount(), frameController.frameCount));
        }

        #region load muscle activations
        if (!musclePathsLoaded)
        {
            appendToLog("\n<color=blue>Muscle positions are missing, will not load activations.</color>");
        }
        else
        {
            appendToLog("\nLoading muscle activations");
            yield return 0;
            LoadCatchErrors(muscleMesh.ReloadMuscleActivations);
        }
        #endregion

        #region load joint positions
        appendToLog("\nLoading joint positions");
        yield return 0;
        bool jointPositionsLoaded = false;
        LoadCatchErrors(
            () =>
            {
                jointForceMesh.ReloadJointPositions();
                jointPositionsLoaded = true;
            });
        #endregion

        if (jointPositionsLoaded && jointForceMesh.GetFrameCount() != frameController.frameCount)
        {
            appendToLog("\n<color=blue>Joint frame count (" + jointForceMesh.GetFrameCount() +
                ") does not match frame count specified in XML file (" + frameController.frameCount + ").</color>");
            frameController.SetFrameCount(Mathf.Min(jointForceMesh.GetFrameCount(), frameController.frameCount));
        }

        #region load joint contact forces
        if (!jointPositionsLoaded)
        {
            appendToLog("\n<color=blue>Joint positions are missing, will not load contact forces.</color>");
        }
        else
        {
            appendToLog("\nLoading joint contact forces");
            yield return 0;
            LoadCatchErrors(jointForceMesh.ReloadJointContactForces);
        }
        #endregion

        #region load marker positions
        appendToLog("\nLoading marker positions");
        yield return 0;
        bool markerPositionsLoaded = false;
        LoadCatchErrors(
            () =>
            {
                markerMesh.ReloadMarkers(activeModel);
                markerPositionsLoaded = true;
            });
        #endregion

        if (markerPositionsLoaded && markerMesh.GetFrameCount() != frameController.frameCount)
        {
            appendToLog("\n<color=blue>Marker frame count (" + markerMesh.GetFrameCount() +
                ") does not match frame count specified in XML file (" + frameController.frameCount + ").</color>");
            frameController.SetFrameCount(Mathf.Min(markerMesh.GetFrameCount(), frameController.frameCount));
        }

        #region load bone dynamics
        appendToLog("\nLoading bone data");
        yield return 0;
        bool boneDynamicsLoaded = false;
        LoadCatchErrors(
            () =>
            {
                boneData.Reload();
                boneDynamicsLoaded = true;
            });
        #endregion

        #region load bone geometry
        if (!boneDynamicsLoaded)
        {
            appendToLog("\n<color=blue>Bone data is missing, will not load bone geometry.</color>");
        }
        else
        {
            for (int i = 0; i < boneMeshes.Length; i++)
            {
                appendToLog("\nLoading bone " + boneMeshes[i].SelectedBone);
                yield return 0;
                LoadCatchErrors(boneMeshes[i].Reload);
            }
        }
        #endregion

        UpdateUiAfterLoad();

        appendToLog("\n<color=green>Complete.</color>");
        yield return new WaitForSeconds(4);
        /*float fadeOutDuration = 1f;
        for (float timer = 0; timer < fadeOutDuration; timer += Time.deltaTime)
        {
            logField.materialForRendering.color = new Color(1f, 1f, 1f, 1f - (timer / fadeOutDuration));
            yield return 0;
        }*/
        logField.gameObject.SetActive(false);
        //logField.materialForRendering.color = Color.white;
    }

    void appendToLog(string text)
    {
        logField.text = logField.text + text;
    }

    delegate void LoadStuffDelegate();
    void LoadCatchErrors(LoadStuffDelegate loadStuff)
    {
        try
        {
            loadStuff();
            appendToLog(".. Done.");
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.LogWarning(e);
            appendToLog(".. <color=red>Failed, directory missing.</color>");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning(e);
            appendToLog(".. <color=red>Failed, file missing.</color>");
        }
        catch (IOException e)
        {
            OnGenericFailure(e);
        }
        catch (IsolatedStorageException e)
        {
            OnGenericFailure(e);
        }
        catch (FrameMismatchException e)
        {
            Debug.LogWarning(e);
            appendToLog(".. <color=red>Failed, frame count does not match (" +
                e.frames1 + " vs " + e.frames2 + ").</color>");
        }
        catch (Exception e)
        {
            OnGenericFailure(e);
        }
    }

    void OnGenericFailure(Exception e)
    {
        Debug.LogWarning(e);
        appendToLog(".. <color=red>Failed.</color>");
    }

    void UpdateUiAfterLoad()
    {
        studyNameField.text = activeModel.studyName;
        studySubjectField.text = activeModel.framesPerSecond + "Hz  |  " + activeModel.sex + " " +
                activeModel.height + "m " + activeModel.mass + "kg";

        foreach (Transform child in boneVisibilityContentPanel)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < boneMeshes.Length; i++)
        {
            BoneMesh bone = boneMeshes[i];
            if (boneMeshes[i].LoadedSuccessfully)
            {
                GameObject newBoneToggle = Instantiate(boneVisibilityToggle) as GameObject;

                ToggleWrapper boneToggle = newBoneToggle.GetComponent<ToggleWrapper>();
                boneToggle.label.text = bone.SelectedBone.ToString();
                boneToggle.toggle.isOn = bone.gameObject.activeSelf;
                boneToggle.toggle.onValueChanged.AddListener((enabled) => bone.gameObject.SetActive(enabled));

                newBoneToggle.transform.SetParent(boneVisibilityContentPanel);
            }
        }
    }
}