using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    // set these externally
    public PortalController remotePortal;
    public Transform playerRoot;

    // prefab ivars
    public MeshRenderer renderPlane;
    public Camera localCamera;
    public Shader shader;
    public PortalTeleporter teleporter;

    // code-only ivars
    Transform playerCamera;
    Camera remoteCamera;
    [HideInInspector] public Material localMaterial;

    void Start()
    {
        // setup local portal state
        playerCamera = playerRoot.GetComponentInChildren<Camera>(false).transform;
        localMaterial = new Material(shader);
        localCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        localMaterial.mainTexture = localCamera.targetTexture;
        teleporter.player = playerRoot;
    }
    void LateStart()
    {
        // things to fetch from the remote portal. Must be done after the other portal has done Start().
        remoteCamera = remotePortal.localCamera;
        renderPlane.material = remotePortal.localMaterial;
        teleporter.reciever = remotePortal.teleporter.transform;
    }

    void Update()
    {
        if(remoteCamera == null) { LateStart(); }
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
