using UnityEngine;
using System.Collections;

public class MarkerMesh : MonoBehaviour {

    private Mesh sphereMesh;
    private Material materialStaticMarkers, materialDynamicMarkers;

    public Shader shader;
    public FrameController controller;
    public bool ShowDynamicMarkers { get; set; }
    public bool ShowStaticMarkers { get; set; }

    private Vector3[][] dynamicMarkerPositions;
    private Vector3[][] staticMarkerPositions;

    public Vector3 Centroid { get; private set; }

    private float markerScale = 1 / 100f; // by default, if present uses that defined in XML parameter file
    private float sizeMultiplier = 1f;

    void Awake()
    {
        sphereMesh = new Mesh();
        PrimitiveUtils.GenerateSphere(sphereMesh);

        materialStaticMarkers = new Material(shader);
        materialStaticMarkers.color = new Color(0f, 1f, 0f, .7f);

        materialDynamicMarkers = new Material(shader);
        materialDynamicMarkers.color = new Color(.8f, .1f, .7f, .7f);

        ShowStaticMarkers = true;
        ShowDynamicMarkers = false;
    }

    public void ReloadMarkers(FreeBodyModel model)
    {
        MarkerDataLoader.LoadMarkerPositions(out dynamicMarkerPositions, out staticMarkerPositions);
        if(0 != model.markerRadiusMetres) markerScale = model.markerRadiusMetres * 2;

        Centroid = new Vector3();
        int count = 0;
        for (int i = 0; i < staticMarkerPositions.Length; i++)
        {
            for (int j = 0; j < staticMarkerPositions[i].Length; j++)
            {
                ++count;
                Centroid += staticMarkerPositions[i][j];
            }
        }
        Centroid /= count;
    }

    public void SetSizeMultiplier(float multiplier)
    {
        sizeMultiplier = multiplier;
    }

    void Update()
    {
        if (ShowDynamicMarkers && null != dynamicMarkerPositions)
        {
            for (int i = 0; i < dynamicMarkerPositions[controller.frame].Length; i++)
            {
                Vector3 v = Vector3.Lerp(dynamicMarkerPositions[controller.frame][i],
                    dynamicMarkerPositions[controller.nextFrame][i], controller.frameAlpha);
                Graphics.DrawMesh(sphereMesh,
                            Matrix4x4.TRS(v, Quaternion.identity, Vector3.one * markerScale * sizeMultiplier),
                            materialDynamicMarkers, 0, null, 0, null, false, false);
            }
        }

        if (ShowStaticMarkers && null != staticMarkerPositions)
        {
            for (int i = 0; i < staticMarkerPositions[controller.frame].Length; i++)
            {
                Vector3 v = Vector3.Lerp(staticMarkerPositions[controller.frame][i],
                    staticMarkerPositions[controller.nextFrame][i], controller.frameAlpha);
                Graphics.DrawMesh(sphereMesh,
                            Matrix4x4.TRS(v, Quaternion.identity, Vector3.one * markerScale * sizeMultiplier),
                            materialStaticMarkers, 0, null, 0, null, false, false);
            }
        }
    }

    public int GetFrameCount()
    {
        return dynamicMarkerPositions.Length; // arbitrarily pick one, assume other is same
    }
}
