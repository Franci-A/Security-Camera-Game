using Unity.Cinemachine;
using UnityEngine;

public class SecurityCameraHandler : MonoBehaviour
{
    public bool startCamera = false;
    public CameraHackHandler cameraHackHandler;
    public CameraController cameraController;
    private CinemachineCamera cam;

    private void Awake()
    {
        cam = GetComponent<CinemachineCamera>();
    }

    public void SetActiveCamera()
    {
        cam.Priority = 10;
    }
    
    public void DeactivateCamera()
    {
        cam.Priority = 1;
    }
}
