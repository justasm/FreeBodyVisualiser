using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Toggle))]
public class ToggleSprite : MonoBehaviour {

    public Image target;
    public Sprite stateOn;
    public Sprite stateOff;

    void Awake()
    {
        Toggle toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(
            (on) =>
            {
                target.sprite = on ? stateOn : stateOff;
            });
    }
}
