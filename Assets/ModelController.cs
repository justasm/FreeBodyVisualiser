using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;

public class ModelController : MonoBehaviour {

    private FreeBodyModel activeModel;

	void Start () {
        // LoadAndVisualiseModel("C:\\Users\\Justas\\SkyDrive\\FreeBodyVis\\For Justas\\FreeBody App\\example\\1037_C14\\1037_walking7_C14 - Justas.xml");
	}

    void LoadAndVisualiseModel(string parameterFilePath)
    {
        ModelParameterLoader.LoadModel(parameterFilePath, out activeModel);

        Debug.Log("Study: " + activeModel.studyName + " by " + activeModel.responsiblePerson);
        Debug.Log("Geometry: " + activeModel.geometryOutputPath);
        Debug.Log("Optimisation: " + activeModel.optimisationOutputPath);
        Debug.Log("Frames " + activeModel.startFrame + " to " + activeModel.endFrame + " FPS: " + activeModel.framesPerSecond);
        Debug.Log(activeModel.sex + " subject, " + activeModel.height + "m, " + activeModel.mass + "kg");
        Debug.Log("Anatomy path: " + activeModel.anatomyDatasetPath);
        Debug.Log("Anatomy file: " + activeModel.anatomyDatasetFileName);
    }
}