using HelperScripts.EventSystem;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private SecurityCameraHandler[] allCameras;
    private SecurityCameraHandler activeCamera;
    private ConsolePlayerManager playerManager;
    [SerializeField] private EventObjectScriptable onValidateCommandeEvent;


    void Start()
    {
        playerManager = GetComponent<ConsolePlayerManager>();
        //get all cameras
        allCameras = FindObjectsByType<SecurityCameraHandler>(FindObjectsSortMode.None);
        for (int i = 0; i < allCameras.Length; i++)
        {
            if (allCameras[i].startCamera)
            {
                activeCamera = allCameras[i];
                activeCamera.SetActiveCamera();
                playerManager.SetActiveCameraController(activeCamera.cameraController);
                onValidateCommandeEvent.Call(new Command("camera " + activeCamera.cameraHackHandler.GetCode, true, "Starting camera " + activeCamera.cameraHackHandler.GetCode));
            }
            else
            {
                allCameras[i].DeactivateCamera();
            }
        }
    }

    public bool OnSwitchCamera(string code)
    {
        for (int i = 0; i < allCameras.Length; i++)
        {
            if(allCameras[i].cameraHackHandler.GetCode == code)
            {
                if(activeCamera == allCameras[i])
                    return false;
                activeCamera.DeactivateCamera();

                activeCamera = allCameras[i];
                activeCamera.SetActiveCamera();
                playerManager.SetActiveCameraController(activeCamera.cameraController);
                return true;
            }
        }

        return false;
    }
}
