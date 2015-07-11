using UnityEngine;
using System.Collections;

public class ToggleUtils : MonoBehaviour {

    public void ToggleActive(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
