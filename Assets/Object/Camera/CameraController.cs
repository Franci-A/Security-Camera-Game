using DG.Tweening;
using HelperScripts.EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private TMP_InputField controlPanelInput;
    [SerializeField] private Transform pivotPoint;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float zoomAmount = 10;
    [SerializeField] private float maxAngleLeft = -20f;
    [SerializeField] private float maxAngleRight = 20f;
    [SerializeField] private float maxZoomIn = -10f;
    [SerializeField] private float maxZoomOut = 10f;

    [SerializeField] private EventObjectScriptable onValidateCommandeEvent;

    private void Start()
    {
        maxZoomOut += Camera.main.fieldOfView;
        maxZoomIn += Camera.main.fieldOfView;
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

        Command command = null;

        if (inputs[0].CompareTo("rotate") == 0)
        {
            if (inputs.Length != 2)
                return;
            if (inputs[1].CompareTo("left") == 0)
            {
                //rotate to max left angle
                command = RotateCameraMax(true);
            }
            else if (inputs[1].CompareTo("right") == 0)
            {
                //rotate to max right angle
                command = RotateCameraMax(false);
            }
            else if (float.TryParse(inputs[1], out float angle))
            {
                command = RotateCameraAngle(angle);
            }
            else
            {
                command = new Command(value, false, "Unknown command");
            }
        }
        else if (inputs[0].CompareTo("zoom") == 0)
        {
            if (inputs.Length != 2)
                return;
            if (inputs[1].CompareTo("in") == 0)
            {
                ZoomCamera(-zoomAmount);
                command = new Command(value, true, "Zooming in");
            }
            else if (inputs[1].CompareTo("out") == 0)
            {
                ZoomCamera(zoomAmount);
                command = new Command(value, true, "Zooming out");
            }
            else
            {
                command = new Command(value, false, "Unknown command");
            }
        }
        else
        {
            command = new Command(value, false, "Unknown command");
        }

        onValidateCommandeEvent.Call(command);
    }

    private Command RotateCameraAngle(float angle)
    {
        if (angle == 0) return null;

        if (DOTween.IsTweening(pivotPoint))
            DOTween.Kill(pivotPoint);

        Vector3 targetAngle = pivotPoint.eulerAngles + new Vector3(0, angle, 0);
        targetAngle.y = Mathf.Clamp(targetAngle.y, maxAngleLeft, maxAngleRight);
        pivotPoint.DORotate(targetAngle, angle / moveSpeed);

        return new Command("rotate " + angle, true, "Rotating " + angle + " degres");
    }

    private Command RotateCameraMax(bool isLeft)
    {
        if (DOTween.IsTweening(pivotPoint))
            DOTween.Kill(pivotPoint);
        float duration = 1;
        Vector3 targetAngle;

        string commandName = isLeft ? "rotate left" : "rotate right";
        string commandResponse = isLeft ? "Rotating to max left angle" : "Rotating to max right angle";

        if (isLeft)
        {

            duration = (pivotPoint.eulerAngles.y - maxAngleLeft) / moveSpeed;
            targetAngle = new Vector3(0, maxAngleLeft);
        }
        else
        {
            duration = (pivotPoint.eulerAngles.y + maxAngleRight) / moveSpeed;
            targetAngle = new Vector3(0, maxAngleRight);
        }

        pivotPoint.DORotate(targetAngle, duration).SetEase(Ease.Linear);
        return new Command(commandName, true, commandResponse);

    }

    private void ZoomCamera(float distance)
    {
        if (Camera.main.fieldOfView + distance > maxZoomOut)
            DOTween.To(() => Camera.main.fieldOfView, x => Camera.main.fieldOfView = x, Camera.main.fieldOfView + maxZoomOut, Mathf.Abs(maxZoomOut) / moveSpeed).SetEase(Ease.Linear);


        if (Camera.main.fieldOfView + distance < maxZoomIn)
            DOTween.To(() => Camera.main.fieldOfView, x => Camera.main.fieldOfView = x, Camera.main.fieldOfView + maxZoomIn, Mathf.Abs(maxZoomIn) / moveSpeed).SetEase(Ease.Linear);


        else
            DOTween.To(() => Camera.main.fieldOfView, x => Camera.main.fieldOfView = x, Camera.main.fieldOfView + distance, Mathf.Abs(distance) / moveSpeed).SetEase(Ease.Linear);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        Gizmos.color = Color.red;
        Vector3 vec = transform.forward;
        vec = UnityEngine.Quaternion.Euler(0, maxAngleLeft, 0) * vec;
        Gizmos.DrawRay(new Ray(transform.position, vec));
        
        Gizmos.color = Color.darkOliveGreen;
        vec = transform.forward;
        vec = UnityEngine.Quaternion.Euler(0, maxAngleRight, 0) * vec;
        Gizmos.DrawRay(new Ray(transform.position, vec));
    }
}


