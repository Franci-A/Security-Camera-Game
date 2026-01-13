using HelperScripts.EventSystem;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHackHandler : MonoBehaviour
{
    [SerializeField] private EventObjectScriptable cameraHackStartEvent;
    [SerializeField] private EventObjectScriptable cameraHackEndEvent;
    [SerializeField] private EventObjectScriptable onHackCompletCommand;
    [SerializeField] private HackPopupHandler hackPopupPrefab;
    private HackPopupHandler hackPopupInstance;
    bool pressed = false;

    private CameraHackHandler currentCameraHacked = null;
    List<ArrowNumber> sequence;
    private int currentCodeIndex = 0;
    private void Start()
    {
        cameraHackStartEvent.AddListener(StartHack);
    }

    private void StartHack(object obj)
    {
        currentCameraHacked = obj as CameraHackHandler;
        currentCodeIndex = 0;

        hackPopupInstance = Instantiate<HackPopupHandler>(hackPopupPrefab);
        sequence = CreateHackSequence(4);
        hackPopupInstance.InitPopup(sequence);
    }

    public void OnInputCode(InputAction.CallbackContext context)
    {
        if (pressed && context.canceled)
        {
            pressed = false;
            return;
        }
        else if (!context.performed)
            return;

        pressed = true;


        Vector2 input = context.ReadValue<Vector2>();
        if(input.x >0 && input.x > Mathf.Abs(input.y))
        {
            // Right
            CheckInput(ArrowNumber.RIGHT);
        }
        else if(input.x < 0 && Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            // Left
            CheckInput(ArrowNumber.LEFT);
        }
        else if(input.y >0 && input.y > Mathf.Abs(input.x))
        {
            // Up
            CheckInput(ArrowNumber.UP);
        }
        else if(input.y <0 && Mathf.Abs(input.y) > Mathf.Abs(input.x))
        {
            // Down
            CheckInput(ArrowNumber.DOWN);
        }
    }

    private void CheckInput(ArrowNumber input)
    {
        if(currentCameraHacked == null)
            return;
        if(input == sequence[currentCodeIndex])
        {
            currentCodeIndex++;
            if(currentCodeIndex >= sequence.Count)
            {
                // Hack complete
                HackComplet();
            }
            else
            {
                hackPopupInstance.UpdatePopup(currentCodeIndex);
            }
        }
        else
        {
            currentCodeIndex = 0;
            hackPopupInstance.UpdatePopup(currentCodeIndex);

        }
    }
    private void HackComplet()
    {
        cameraHackEndEvent.Call(currentCameraHacked);
        onHackCompletCommand.Call(new Command("Camera Hack", true, "Hack completed successfully. \n Camera " + currentCameraHacked.GetCode + " unlocked"));
        
        currentCameraHacked = null;
        Destroy(hackPopupInstance.gameObject);
    }

    private List<ArrowNumber> CreateHackSequence(int length)
    {
        List<ArrowNumber> sequence = new List<ArrowNumber>();
        for (int i = 0; i < length; i++)
        {
            sequence.Add(Random.Range(0, 4) switch
            {
                0 => ArrowNumber.UP,
                1 => ArrowNumber.RIGHT,
                2 => ArrowNumber.DOWN,
                3 => ArrowNumber.LEFT,
                _ => ArrowNumber.UP
            });
        }

        return sequence;
    }

    private void OnDestroy()
    {
        cameraHackStartEvent.RemoveListener(StartHack);
    }

}
