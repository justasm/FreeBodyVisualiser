using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

    private CardboardHead head;
    public float distanceMeters = 2;
    public float distanceMetersMin = 0.4f;
    public float distanceMetersMax = 10;
    public float scrollSensitivity = 0.25f;

    public float zOffset = 0.2f;
    public MuscleMesh muscleMesh;
    public MarkerMesh markerMesh;

    public bool ScrollWheelEnabled { get; set; }

    void Start()
    {
        head = Camera.main.GetComponent<CardboardHead>();
    }

    void LateUpdate()
    {
        Vector3 target = new Vector3();
        if (muscleMesh.Centroid.sqrMagnitude != 0)
        {
            target = muscleMesh.Centroid;
        }
        else if (markerMesh.Centroid.sqrMagnitude != 0)
        {
            target = markerMesh.Centroid;
        }

        //transform.rotation = Quaternion.Inverse(head.transform.rotation);
        //Vector3 eulers = head.transform.rotation.eulerAngles;
        //eulers.x = 0;
        //eulers.y = -eulers.y;
        //eulers.z = 0;
        //head.transform.rotation = Quaternion.Euler(eulers);

        if (Input.touchCount >= 2)
        {
            Vector2 current0 = Input.GetTouch(0).position;
            Vector2 prev0 = current0 - Input.GetTouch(0).deltaPosition;
            Vector2 current1 = Input.GetTouch(1).position;
            Vector2 prev1 = current1 - Input.GetTouch(1).deltaPosition;
            float currentDistance = Vector2.Distance(current0, current1);
            float prevDistance = Vector2.Distance(prev0, prev1);

            distanceMeters *= (prevDistance / currentDistance);
        }

        if (ScrollWheelEnabled)
        {
            distanceMeters -= Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
        }
        
        distanceMeters = Mathf.Clamp(distanceMeters, distanceMetersMin, distanceMetersMax);
        
        // Move Cardboard head appropriately
        head.transform.position = target - (distanceMeters * head.Gaze.direction);
        head.transform.Translate(Vector3.up * zOffset);
    }

}
