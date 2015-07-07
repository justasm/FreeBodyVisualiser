using UnityEngine;
using System.Collections;

public class JointForceMesh : MonoBehaviour {

    private const float arrowBodyScale = .03f;
    private const float arrowTipScale = .1f;
    private const float contactForceScale = 1 / 2000f;

    //private Mesh sphereMesh;
    private Mesh coneMesh;
    private Mesh cylinderMesh;
    private Material redMaterial;

    public Shader shader;

    public FrameController controller;

    Vector3[] anklePositions;
    Vector3[] kneePositions;
    Vector3[] lateralTfPositions;
    Vector3[] medialTfPositions;
    Vector3[] hipPositions;
    Vector3[][] jointPositions;

    Vector3[] ankleContactForces;
    Vector3[] lateralTfContactForces;
    Vector3[] medialTfContactForces;
    Vector3[] hipContactForces;
    Vector3[][] contactForces;

	void Start () {
        JointForceDataLoader.LoadJointPositions(out anklePositions, out kneePositions,
            out lateralTfPositions, out medialTfPositions, out hipPositions);
        jointPositions = new Vector3[][] { anklePositions, lateralTfPositions,
            medialTfPositions, hipPositions };

        JointForceDataLoader.LoadJointContactForces(out ankleContactForces,
            out lateralTfContactForces, out medialTfContactForces, out hipContactForces);
        contactForces = new Vector3[][] { ankleContactForces, lateralTfContactForces,
            medialTfContactForces, hipContactForces };

        //sphereMesh = new Mesh();
        //PrimitiveUtils.GenerateSphere(sphereMesh);
        coneMesh = new Mesh();
        PrimitiveUtils.GenerateCone(coneMesh);
        cylinderMesh = new Mesh();
        PrimitiveUtils.GenerateCone(cylinderMesh, topRadius: .5f);

        redMaterial = new Material(shader);
        redMaterial.color = new Color(1f, 0f, 0f, .7f);
	}
	
	void Update () {
        // draw contact forces for all joints for which data is available
        for (int i = 0; i < contactForces.Length; i++)
        {
            float mag = contactForces[i][controller.frame].magnitude;
            if (0 == mag) continue;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contactForces[i][controller.frame].normalized);

            Graphics.DrawMesh(cylinderMesh,
                        Matrix4x4.TRS(
                            jointPositions[i][controller.frame],
                            rot,
                            new Vector3(
                                arrowBodyScale,
                                mag * contactForceScale,
                                arrowBodyScale)
                        ),
                        redMaterial, 0);

            Graphics.DrawMesh(coneMesh,
                        Matrix4x4.TRS(
                            jointPositions[i][controller.frame] + contactForces[i][controller.frame] * contactForceScale,
                            rot,
                            Vector3.one * arrowTipScale),
                        redMaterial, 0);
        }

        //Graphics.DrawMesh(sphereMesh,
        //            Matrix4x4.TRS(anklePositions[controller.frame], Quaternion.identity, Vector3.one / 10f),
        //            redMaterial, 0);
	}
}
