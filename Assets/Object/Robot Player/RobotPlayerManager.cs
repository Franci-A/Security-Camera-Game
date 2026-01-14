using HelperScripts.EventSystem;
using UnityEngine;

public class RobotPlayerManager : MonoBehaviour
{
    private PlayerInteract playerInteract;
    private PlayerMovement playerMovement;
    private RobotPlayerState currentState = RobotPlayerState.Idle;
    [SerializeField] private EventObjectScriptable cameraHackStartEvent;
    [SerializeField] private EventObjectScriptable cameraHackEndEvent;

    private void Awake()
    {
        playerInteract = GetComponent<PlayerInteract>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        ChangeState(RobotPlayerState.Moving);
        cameraHackStartEvent.AddListener(StartCameraHack);
        cameraHackEndEvent.AddListener(EndCameraHack);
    }

    public void ChangeState( RobotPlayerState newState)
    {

        currentState = newState;
        switch (currentState)
        {
            case RobotPlayerState.Idle:
                playerInteract.IsActive = false;
                playerMovement.IsActive = false;

                break;
            case RobotPlayerState.Moving:
                playerInteract.IsActive = true;
                playerMovement.IsActive = true;

                break;
            case RobotPlayerState.Interacting:
                playerInteract.IsActive = false;
                playerMovement.IsActive = false;

                break;
            default:
                break;
        }
    }

    public void StartCameraHack(object obj)
    {
        ChangeState(RobotPlayerState.Interacting);
    }
   
    
    public void EndCameraHack(object obj)
    {
        ChangeState(RobotPlayerState.Moving);
    }

    private void OnDestroy()
    {
        cameraHackStartEvent.RemoveListener(StartCameraHack);
        cameraHackEndEvent.RemoveListener(EndCameraHack);
    }
}

public enum RobotPlayerState
{
    Idle,
    Moving,
    Interacting
}
