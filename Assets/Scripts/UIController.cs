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

    void Start()
    {
        controlToggles = new Toggle[]{muscleControlToggle, forceControlToggle, boneControlToggle, markerControlToggle};
        toggleActions = new ToggleAction[controlToggles.Length];
        for (int i = 0; i < controlToggles.Length; i++)
        {
            toggleActions[i] = new ToggleAction(controlToggles[i], OnControlToggleChange);
            controlToggles[i].onValueChanged.AddListener(toggleActions[i].OnValueChanged);
        }

        muscleControlToggle.onValueChanged.AddListener(
            (on) =>
            {
                muscleControlPanel.SetActive(on);
                muscleActivationControlPanel.SetActive(on);
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
