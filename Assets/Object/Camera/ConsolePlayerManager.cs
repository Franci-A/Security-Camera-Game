using HelperScripts.EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsolePlayerManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField controlPanelInput;
    [SerializeField] private CameraController activeCameraController;
    [SerializeField] private EventObjectScriptable onValidateCommandeEvent;

    void Start()
    {
        controlPanelInput.ActivateInputField();
    }

    public void OnValidateCommande(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (controlPanelInput.text.Length == 0)
            return;

        string value = controlPanelInput.text.ToLower().Trim();
        controlPanelInput.text = "";

        string[] inputs = value.Split(' ');

        Command command = new Command(value, false, "Unknown command");

        if (inputs[0].CompareTo("rotate") == 0)
        {
            command = Rotate(value, inputs);
        }
        else if (inputs[0].CompareTo("zoom") == 0)
        {
            command = Zoom(value, inputs);
        }

        onValidateCommandeEvent.Call(command);
        controlPanelInput.ActivateInputField();

    }

    private Command Rotate(string value, string[] inputs)
    {
        if (inputs.Length < 2)
            return new Command(value, false, "Rotation not specified") ;

        if (inputs[1].CompareTo("left") == 0)
        {
            if (inputs.Length == 3 && float.TryParse(inputs[1], out float angle) && angle > 0)
            {
                activeCameraController.RotateCameraAngle(-angle);
                return new Command(value, true, "Rotation " + angle + "° left");
            }
            else if (inputs.Length == 2)
            {
                //rotate to max left angle
                activeCameraController.RotateCameraMax(true);
                return new Command(value, true, "Rotating to max left angle");
            }else
                return new Command(value, false, "Unknown rotation");
        }
        else if (inputs[1].CompareTo("right") == 0)
        {
            if (inputs.Length == 3 && float.TryParse(inputs[1], out float angle) && angle > 0)
            {
                activeCameraController.RotateCameraAngle(-angle);
                return new Command(value, true, "Rotation " + angle + "° right");
            }
            else if (inputs.Length == 2)
            {
                //rotate to max right angle
                activeCameraController.RotateCameraMax(false);
                return new Command(value, true, "Rotating to max right angle");
            }else
            return new Command(value, false, "Unknown rotation");

        }
        else
        {
            return new Command(value, false, "Unknown rotation");
        }
    }

    private Command Zoom(string value, string[] inputs)
    {
        if (inputs.Length != 2)
            return new Command(value, false, "Invalid zooming"); 
        if (inputs[1].CompareTo("in") == 0)
        {
            activeCameraController.ZoomCamera();
            return new Command(value, true, "Zooming in");
        }
        else if (inputs[1].CompareTo("out") == 0)
        {
            activeCameraController.ZoomCamera();
            return new Command(value, true, "Zooming out");
        }
        else
        {
            return new Command(value, false, "Invalid zooming");
        }
    }
}
