using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

    private CardboardHead head;
    public float distanceMeters = 2;
    public float distanceMetersMin = 0.4f;
    public float distanceMetersMax = 10;
    public float scrollSensitivity = 0.35f;

    public float zOffset = 0.2f;
    public MuscleMesh muscleMesh;
    public MarkerMesh markerMesh;

    public bool ScrollWheelEnabled { get; set; }
    public bool MouseDragEnabled { get; set; }

    public float horizontalMouseSpeed = 20f;
    public float verticalMouseSpeed = 15f;

    private float xRotationOffset = 0;
    private float yRotationOffset = 0;

    void Start()
    {
        head = Camera.main.GetComponent<CardboardHead>();
        ScrollWheelEnabled = true;
        MouseDragEnabled = true;
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

        // scaling / zooming
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

        // panning
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        // don't support this yet, too sporradic
#else
        if (MouseDragEnabled && Input.GetMouseButton(0))
        {
            float xDelta = Input.GetAxis("Mouse X") * horizontalMouseSpeed;
            float yDelta = -Input.GetAxis("Mouse Y") * verticalMouseSpeed;

            xRotationOffset += xDelta;
            yRotationOffset += yDelta;
        }
#endif

        yRotationOffset = Mathf.Clamp(yRotationOffset, -40, 80);

        Quaternion rotationOffset = Quaternion.Euler(yRotationOffset, xRotationOffset, 0);
        
        // move Cardboard head appropriately
        head.transform.position = target -
            head.transform.rotation * rotationOffset * Vector3.forward * distanceMeters;
        head.transform.Translate(Vector3.up * zOffset);
        head.transform.rotation *= rotationOffset;
    }

}
