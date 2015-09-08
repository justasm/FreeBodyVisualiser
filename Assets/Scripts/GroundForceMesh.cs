using UnityEngine;
using System.Collections;

// TODO consolidate such that this and JointForceMesh extend same class; most logic is shared
public class GroundForceMesh : MonoBehaviour {

    private const float arrowBodyScale = .02f;
    private const float arrowTipScale = .08f;
    private const float groundForceScale = 1 / 10000f;
    private float sizeMultiplier = 1f;

    private Mesh coneMesh;
    private Mesh cylinderMesh;
    private Material blueMaterial;

    public Shader shader;

    public FrameController controller;

    Vector3[] positions;
    Vector3[] forces;

	void Awake () {
        coneMesh = new Mesh();
        PrimitiveUtils.GenerateCone(coneMesh);
        cylinderMesh = new Mesh();
        PrimitiveUtils.GenerateCone(cylinderMesh, topRadius: .5f);

        blueMaterial = new Material(shader);
        blueMaterial.color = new Color(0f, 0f, 1f, .7f);
	}

    public void SetSizeMultiplier(float multiplier)
    {
        sizeMultiplier = multiplier;
    }

    public void ReloadGroundForces()
    {
        GroundForceDataLoader.LoadGroundForces(out positions, out forces);
    }
	
	void Update () {
        // TODO visualise positions idependent of contact forces
        if (null == forces) return;
        // draw ground contact forces for which data is available
        
        Vector3 v = Vector3.Lerp(forces[controller.frame],
            forces[controller.nextFrame], controller.frameAlpha);
        float mag = v.magnitude;
        if (0 == mag) return;
        Vector3 n = v.normalized;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, n);
        Vector3 pos = Vector3.Lerp(positions[controller.frame],
            positions[controller.nextFrame], controller.frameAlpha);

        Graphics.DrawMesh(cylinderMesh,
                    Matrix4x4.TRS(
                        pos + n * arrowTipScale * sizeMultiplier,
                        rot,
                        new Vector3(
                            arrowBodyScale,
                            mag * groundForceScale,
                            arrowBodyScale) * sizeMultiplier
                    ),
                    blueMaterial, 0, null, 0, null, false, false);

        Graphics.DrawMesh(cylinderMesh,
                    Matrix4x4.TRS(
                        pos - n * arrowTipScale * sizeMultiplier,
                        rot,
                        new Vector3(
                            arrowBodyScale,
                            -mag * groundForceScale,
                            arrowBodyScale) * sizeMultiplier
                    ),
                    blueMaterial, 0, null, 0, null, false, false);

        Graphics.DrawMesh(coneMesh,
                    Matrix4x4.TRS(
                        pos + n * arrowTipScale * sizeMultiplier,
                        rot,
                        new Vector3(1, -1, 1) * arrowTipScale * sizeMultiplier),
                    blueMaterial, 0, null, 0, null, false, false);

        Graphics.DrawMesh(coneMesh,
                    Matrix4x4.TRS(
                        pos - n * arrowTipScale * sizeMultiplier,
                        rot,
                        Vector3.one * arrowTipScale * sizeMultiplier),
                    blueMaterial, 0, null, 0, null, false, false);
	}

    public int GetFrameCount()
    {
        return positions.Length;
    }
}
