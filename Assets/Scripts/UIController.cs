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
        markerControlToggle.onValueChanged.AddListener((on) => markerControlPanel.SetActive(on));
        boneControlToggle.onValueChanged.AddListener((on) => boneControlPanel.SetActive(on));
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
