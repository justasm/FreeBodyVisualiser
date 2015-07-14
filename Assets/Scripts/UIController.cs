using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class UIController : MonoBehaviour {

    public Toggle muscleControlToggle;
    public Toggle forceControlToggle;
    public Toggle boneControlToggle;
    public Toggle markerControlToggle;
    private Toggle[] controlToggles;
    private ToggleAction[] toggleActions;

    public GameObject mobileUi;

    public Button loadMobileButton;
    public Button enableVrMobileButton;

    public GameObject desktopUi;

    public GameObject rightPanel;
    public GameObject markerControlPanel;
    public GameObject boneControlPanel;
    public GameObject muscleActivationControlPanel;
    public GameObject muscleControlPanel;
    public GameObject forceControlPanel;

    public Transform muscleVisibilityContentPanel;
    public Button muscleVisibilityAllOn;
    public Button muscleVisibilityAllOff;
    public InputField muscleGroupSearch;

    public Transform boneVisibilityContentPanel;
    public Button boneVisibilityAllOn;
    public Button boneVisibilityAllOff;

    private class ToggleAction
    {
        public delegate void OnToggleValueChangedDelegate(Toggle toggle, bool on);
        private Toggle toggle;
        private OnToggleValueChangedDelegate deleg;

        public ToggleAction(Toggle toggle, OnToggleValueChangedDelegate deleg)
        {
            this.toggle = toggle;
            this.deleg = deleg;
        }

        public void OnValueChanged(bool on)
        {
            deleg(toggle, on);
        }
    }

    void Awake()
    {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        desktopUi.SetActive(false);
        mobileUi.SetActive(true);

        VRUtils vrUtils = GetComponent<VRUtils>();
        loadMobileButton.onClick.AddListener(() => LaunchMobileFilePicker());
        enableVrMobileButton.onClick.AddListener(() => vrUtils.EnableVR());
#else
        desktopUi.SetActive(true);
        mobileUi.SetActive(false);
#endif
    }

    void LaunchMobileFilePicker()
    {
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("launchFilePicker");
#else
        Debug.LogWarning("File picker not implemented for this platform.");
#endif
    }

    void Start()
    {

        controlToggles = new Toggle[]{muscleControlToggle, forceControlToggle, boneControlToggle, markerControlToggle};
        toggleActions = new ToggleAction[controlToggles.Length];
        for (int i = 0; i < controlToggles.Length; i++)
        {
            toggleActions[i] = new ToggleAction(controlToggles[i], OnControlToggleChange);
            controlToggles[i].onValueChanged.AddListener(toggleActions[i].OnValueChanged);
        }

        MuscleMesh muscleMesh = FindObjectOfType<MuscleMesh>();
        muscleControlToggle.onValueChanged.AddListener(
            (on) =>
            {
                muscleControlPanel.SetActive(on);
                muscleActivationControlPanel.SetActive(muscleMesh.LoadedActivations && on);
            });
        forceControlToggle.onValueChanged.AddListener((on) => forceControlPanel.SetActive(on));
        markerControlToggle.onValueChanged.AddListener((on) => markerControlPanel.SetActive(on));
        boneControlToggle.onValueChanged.AddListener((on) => boneControlPanel.SetActive(on));

        muscleVisibilityAllOn.onClick.AddListener(
            () =>
            {
                for (int i = 0; i < muscleVisibilityContentPanel.childCount; i++)
                {
                    muscleVisibilityContentPanel.GetChild(i).GetComponent<ToggleWrapper>().toggle.isOn = true;
                }
            });
        muscleVisibilityAllOff.onClick.AddListener(
            () =>
            {
                for (int i = 0; i < muscleVisibilityContentPanel.childCount; i++)
                {
                    muscleVisibilityContentPanel.GetChild(i).GetComponent<ToggleWrapper>().toggle.isOn = false;
                }
            });
        muscleGroupSearch.onValueChange.AddListener(
            (query) =>
            {
                foreach (Transform group in muscleVisibilityContentPanel)
                {
                    ToggleWrapper groupToggle = group.GetComponent<ToggleWrapper>();
                    group.gameObject.SetActive(
                        groupToggle.label.text.ToLowerInvariant().Contains(query.ToLowerInvariant()));
                }
            });

        boneVisibilityAllOn.onClick.AddListener(
            () =>
            {
                for (int i = 0; i < boneVisibilityContentPanel.childCount; i++)
                {
                    boneVisibilityContentPanel.GetChild(i).GetComponent<ToggleWrapper>().toggle.isOn = true;
                }
            });
        boneVisibilityAllOff.onClick.AddListener(
            () =>
            {
                for (int i = 0; i < boneVisibilityContentPanel.childCount; i++)
                {
                    boneVisibilityContentPanel.GetChild(i).GetComponent<ToggleWrapper>().toggle.isOn = false;
                }
            });
    }

    void OnControlToggleChange(Toggle toggle, bool on)
    {
        if (on)
        {
            for (int i = 0; i < controlToggles.Length; i++)
            {
                Toggle otherToggle = controlToggles[i];
                if (otherToggle == toggle) continue;
                otherToggle.onValueChanged.RemoveListener(toggleActions[i].OnValueChanged);
                otherToggle.isOn = false;
                otherToggle.onValueChanged.AddListener(toggleActions[i].OnValueChanged);
            }
        }

        rightPanel.gameObject.SetActive(on);
        //rightPanel.gameObject.GetComponent<CanvasRenderer>().SetAlpha(on ? 1f : 0f);
    }

}
