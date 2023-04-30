using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class HubManager : MonoBehaviour
{
    public SC_FPSController singletonFPS;
    public PortalController dreamPortal;

    Transform playerRoot;
    
    IEnumerator Start()
    {
        // Configure AR
        if ((ARSession.state == ARSessionState.None ) ||
            (ARSession.state == ARSessionState.CheckingAvailability))
        {
            yield return ARSession.CheckAvailability();
        }

        if (ARSession.state == ARSessionState.Unsupported || ARSession.state == ARSessionState.NeedsInstall)
        {
            Debug.Log("Configuring for flat UX");
            // No AR, use FPS controls (and only the instance in this scene, to destroy
            // the debug cameras in sub-scenes)
            SC_FPSController.singleton = singletonFPS;
            playerRoot = singletonFPS.transform;
        }
        else
        {
            Debug.Log("Configuring for AR UX");
            // AR, use AR camera by loading it from a scene
            SC_FPSController.FPSControlsEnabled = false;
            // TODO: Set playerRoot
        }

        // Setup sub-scenes: Dream scene
        AsyncOperation dreamLoad = SceneManager.LoadSceneAsync("Art Scenes/Dream Scene/Dream Scene", LoadSceneMode.Additive);
        yield return new WaitUntil(() => dreamLoad.isDone);
        PortalController dreamPortalInner = GameObject.Find("DreamPortalInner").GetComponent<PortalController>();
        dreamPortalInner.playerRoot = playerRoot;
        dreamPortalInner.remotePortal = dreamPortal;
        dreamPortal.remotePortal = dreamPortalInner;
        dreamPortal.playerRoot = playerRoot;
        yield return new WaitUntil(() => dreamLoad.isDone);
        Debug.Log("Dream scene loaded and configured");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
