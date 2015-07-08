using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;

public class ModelController : MonoBehaviour {

    private FreeBodyModel activeModel;

    FrameController frameController;
    BoneData boneData;
    BoneMesh[] boneMeshes;
    JointForceMesh jointForceMesh;
    MuscleMesh muscleMesh;

    void Awake()
    {
        frameController = GetComponent<FrameController>();
        boneData = GetComponent<BoneData>();
        boneMeshes = FindObjectsOfType<BoneMesh>();
        jointForceMesh = FindObjectOfType<JointForceMesh>();
        muscleMesh = FindObjectOfType<MuscleMesh>();
    }

	void Start () {
        LoadAndVisualiseModel("C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\FreeBody App\\example\\1037_C14\\1037_walking6_C14 - Justas.xml");
	}

    void LoadAndVisualiseModel(string parameterFilePath)
    {
        // TODO cancel any existing load
        // TODO thread / yield loading routines
        // TODO display loading UI
        // TODO separate bone, joint force and muscle force loading

        ModelParameterLoader.LoadModel(parameterFilePath, out activeModel);

        DataPathUtils.UpdatePaths(activeModel);
        frameController.UpdateFrameCount(activeModel);

        boneData.Reload();
        for (int i = 0; i < boneMeshes.Length; i++)
        {
            boneMeshes[i].Reload();
        }

        jointForceMesh.Reload();

        muscleMesh.Reload();

        Debug.Log("Study: " + activeModel.studyName + " by " + activeModel.responsiblePerson);
        Debug.Log("Geometry: " + activeModel.geometryOutputPath);
        Debug.Log("Optimisation: " + activeModel.optimisationOutputPath);
        Debug.Log("Frames " + activeModel.startFrame + " to " + activeModel.endFrame + " FPS: " + activeModel.framesPerSecond);
        Debug.Log(activeModel.sex + " subject, " + activeModel.height + "m, " + activeModel.mass + "kg");
        Debug.Log("Anatomy path: " + activeModel.anatomyDatasetPath);
        Debug.Log("Anatomy file: " + activeModel.anatomyDatasetFileName);
    }
}