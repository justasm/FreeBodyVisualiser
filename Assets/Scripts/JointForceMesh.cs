using UnityEngine;
using System.Collections;

public class JointForceMesh : MonoBehaviour {

    private const float arrowBodyScale = .02f;
    private const float arrowTipScale = .08f;
    private const float contactForceScale = 1 / 10000f;

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

	void Awake () {
        //sphereMesh = new Mesh();
        //PrimitiveUtils.GenerateSphere(sphereMesh);
        coneMesh = new Mesh();
        PrimitiveUtils.GenerateCone(coneMesh);
        cylinderMesh = new Mesh();
        PrimitiveUtils.GenerateCone(cylinderMesh, topRadius: .5f);

        redMaterial = new Material(shader);
        redMaterial.color = new Color(1f, 0f, 0f, .7f);
	}

    public void ReloadJointPositions()
    {
        JointForceDataLoader.LoadJointPositions(out anklePositions, out kneePositions,
            out lateralTfPositions, out medialTfPositions, out hipPositions);
        jointPositions = new Vector3[][] { anklePositions, lateralTfPositions,
            medialTfPositions, hipPositions };
    }

    // assumes jointPositions[][] is already populated
    public void ReloadJointContactForces()
    {
        JointForceDataLoader.LoadJointContactForces(out ankleContactForces,
            out lateralTfContactForces, out medialTfContactForces, out hipContactForces);
        if (anklePositions.Length != ankleContactForces.Length)
        {
            throw new FrameMismatchException(ankleContactForces.Length, anklePositions.Length);
        }
        contactForces = new Vector3[][] { ankleContactForces, lateralTfContactForces,
            medialTfContactForces, hipContactForces };
    }
	
	void Update () {
        // TODO visualise joint positions idependent of contact forces
        if (null == contactForces) return;
        // draw contact forces for all joints for which data is available
        for (int i = 0; i < contactForces.Length; i++)
        {
            Vector3 v = Vector3.Lerp(contactForces[i][controller.frame],
                contactForces[i][controller.nextFrame], controller.frameAlpha);
            float mag = v.magnitude;
            if (0 == mag) continue;
            Vector3 n = v.normalized;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, n);
            Vector3 pos = Vector3.Lerp(jointPositions[i][controller.frame],
                jointPositions[i][controller.nextFrame], controller.frameAlpha);

            Graphics.DrawMesh(cylinderMesh,
                        Matrix4x4.TRS(
                            pos + n * arrowTipScale,
                            rot,
                            new Vector3(
                                arrowBodyScale,
                                mag * contactForceScale,
                                arrowBodyScale)
                        ),
                        redMaterial, 0);

            Graphics.DrawMesh(cylinderMesh,
                        Matrix4x4.TRS(
                            pos - n * arrowTipScale,
                            rot,
                            new Vector3(
                                arrowBodyScale,
                                -mag * contactForceScale,
                                arrowBodyScale)
                        ),
                        redMaterial, 0);

            Graphics.DrawMesh(coneMesh,
                        Matrix4x4.TRS(
                            pos + n * arrowTipScale,
                            rot,
                            new Vector3(1, -1, 1) * arrowTipScale),
                        redMaterial, 0);

            Graphics.DrawMesh(coneMesh,
                        Matrix4x4.TRS(
                            pos - n * arrowTipScale,
                            rot,
                            Vector3.one * arrowTipScale),
                        redMaterial, 0);
        }

        //Graphics.DrawMesh(sphereMesh,
        //            Matrix4x4.TRS(anklePositions[controller.frame], Quaternion.identity, Vector3.one / 10f),
        //            redMaterial, 0);
	}

    public int GetFrameCount()
    {
        return anklePositions.Length; // arbitrarily picked one of the position sequences
    }
}
