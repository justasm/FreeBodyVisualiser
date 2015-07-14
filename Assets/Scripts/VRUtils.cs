using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class VRUtils : MonoBehaviour {

    private Cardboard cardboard;

    void Awake()
    {
        cardboard = GetComponent<Cardboard>();
    }

    void Update()
    {
        // Exit VR mode on four-finger tap
        if (cardboard.VRModeEnabled && Input.touchCount >= 4)
        {
            cardboard.VRModeEnabled = false;
        }
    }

    public void EnableVR()
    {
        cardboard.VRModeEnabled = true;
    }

}
