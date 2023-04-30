using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    // set these externally
    public PortalController remotePortal;
    public Transform playerCamera;

    // prefab ivars
    public MeshRenderer renderPlane;
    public Camera localCamera;
    public Shader shader;
    public PortalTeleporter teleporter;

    // code-only ivars
    Camera remoteCamera;
    [HideInInspector] public Material localMaterial;

    void Start()
    {
        // setup local portal state
        localMaterial = new Material(shader);
        localMaterial.name = "RenderMaterial";
        localCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        localMaterial.mainTexture = localCamera.targetTexture;
        teleporter.player = playerCamera;
        

        // things to fetch from the remote portal
        remoteCamera = remotePortal.localCamera;
        renderPlane.material = remotePortal.localMaterial;
        teleporter.reciever = remotePortal.teleporter.transform;
    }

    void LateUpdate()
    {
        Transform portal = transform;
        Transform otherPortal = remotePortal.transform;

        Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
		localCamera.transform.position = portal.position + playerOffsetFromPortal;

		float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

		Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
		Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
		localCamera.transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
