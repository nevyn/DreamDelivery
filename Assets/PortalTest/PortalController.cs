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
        // setup local state not dependent on configuration.
        localCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        localMaterial = new Material(shader);
        localMaterial.mainTexture = localCamera.targetTexture;
    }
    void LateStart()
    {
        // setup local state dependent on configuration and remote portal
        playerCamera = playerRoot.GetComponentInChildren<Camera>(false).transform;
        teleporter.player = playerRoot;

        // things to fetch from the remote portal. Must be done after the other portal has done Start().
        remoteCamera = remotePortal.localCamera;
        renderPlane.material = remotePortal.localMaterial;
        teleporter.reciever = remotePortal.teleporter.transform;
    }

    void Update()
    {
        // If we're not configured with a remote portal, wait until we are
        if(remotePortal == null || playerRoot == null) { return; }
        // if we are but local state is missing, set it up now.
        if(remoteCamera == null) { LateStart(); }
    }

    void LateUpdate()
    {
        // wait for lazy initialization
        if(remotePortal == null || playerRoot == null || remoteCamera == null) { return; }

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
