using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

    private CardboardHead head;
    public float distanceMeters = 2;
    public float zOffset = 0.2f;
    private MuscleMesh muscleMesh;
    private MarkerMesh markerMesh;

    void Awake()
    {
        muscleMesh = FindObjectOfType<MuscleMesh>();
        markerMesh = FindObjectOfType<MarkerMesh>();
    }

    void Start()
    {
        head = Camera.main.GetComponent<CardboardHead>();
    }

    void Update()
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

        // Move Cardboard head appropriately
        head.transform.position = target - (distanceMeters * head.Gaze.direction);
        head.transform.Translate(Vector3.up * zOffset);

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
    }

}
