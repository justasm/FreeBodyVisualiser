using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class VRController : MonoBehaviour {

    private Cardboard cardboard;

    public GameObject nonVRMobileUi;
    public GameObject VRMobileUi;

    void Awake()
    {
        cardboard = GetComponent<Cardboard>();
        
#if UNITY_IOS || UNITY_ANDROID
        nonVRMobileUi.SetActive(!cardboard.VRModeEnabled);
        VRMobileUi.SetActive(cardboard.VRModeEnabled);
#endif
    }

    void Update()
    {
        // Exit VR mode on four-finger tap
        if (cardboard.VRModeEnabled && Input.touchCount >= 4)
        {
            cardboard.VRModeEnabled = false;
            nonVRMobileUi.SetActive(!cardboard.VRModeEnabled);
            VRMobileUi.SetActive(cardboard.VRModeEnabled);
        }
    }

    public void EnableVR()
    {
        cardboard.VRModeEnabled = true;
        nonVRMobileUi.SetActive(!cardboard.VRModeEnabled);
        VRMobileUi.SetActive(cardboard.VRModeEnabled);
    }

}
