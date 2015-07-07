using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MuscleVisibilityUi : MonoBehaviour {

    public GameObject toggle;
    public Transform contentPanel;
    public MuscleMesh muscleMesh;

    private List<MuscleGroup> groupList;

	void Start () {
        groupList = MuscleGroup.groups;

        for(int i = 0; i < groupList.Count; i++){
            MuscleGroup group = groupList[i];
            GameObject newToggle = Instantiate(toggle) as GameObject;
            
            MuscleGroupToggle muscleToggle = newToggle.GetComponent<MuscleGroupToggle>();
            muscleToggle.label.text = group.name;
            muscleToggle.toggle.isOn = muscleMesh.visibility[group.index];
            muscleToggle.toggle.onValueChanged.AddListener((enabled) => muscleMesh.SetVisibility(group, enabled));

            newToggle.transform.SetParent(contentPanel);
        }
	}
}
