using HelperScripts.EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsolePlayerManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField controlPanelInput;
    private CameraController activeCameraController;
    [SerializeField] private EventObjectScriptable onValidateCommandeEvent;
    private CameraManager cameraManager;

    void Start()
    {
        controlPanelInput.ActivateInputField();
        cameraManager = GetComponent<CameraManager>();
    }

    public void SetActiveCameraController(CameraController cameraController)
    {
        activeCameraController = cameraController;
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
        }else if (inputs[0].CompareTo("camera") == 0) 
        {
            command = Camera(value, inputs);
        }else if (value.CompareTo("main menu") == 0 || value.CompareTo("menu")== 0) 
        {
            command = Menu(value, inputs);
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
            if (inputs.Length == 3 && float.TryParse(inputs[2], out float angle) && angle > 0)
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
            if (inputs.Length == 3 && float.TryParse(inputs[2], out float angle) && angle > 0)
            {
                activeCameraController.RotateCameraAngle(angle);
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

    private Command Camera(string value, string[] inputs)
    {
        if (inputs.Length != 2)
            return new Command(value, false, "Invalid camera command");
        //switch camera

        if (cameraManager.OnSwitchCamera(inputs[1]))
        {
            return new Command(value, true, "Switching to camera " + inputs[1]);

        }else 
            return new Command(value, false, "Camera not found");
    }

    private Command Menu(string value, string[] inputs)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        return new Command(value, true, "Loading Main Menu...");
    }
}
