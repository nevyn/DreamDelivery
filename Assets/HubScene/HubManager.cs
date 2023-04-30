using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class HubManager : MonoBehaviour
{
    public SC_FPSController singletonController;
    
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
            SC_FPSController.singleton = singletonController;
        }
        else
        {
            Debug.Log("Configuring for AR UX");
            // AR, use AR camera by loading it from a scene
            SC_FPSController.FPSControlsEnabled = false;
        }

        // Setup sub-scenes
//        SceneManager.LoadSceneAsync("")
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
