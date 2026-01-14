using DG.Tweening;
using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera cineCamera;
    [SerializeField] private Transform pivotPoint;
    [Header("Camera Movement Settings")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float maxAngleLeft = -20f;
    [SerializeField] private float maxAngleRight = 20f;
    [Header("Camera Zoom Settings")]
    [SerializeField] private float zoomSpeed = 4;
    [SerializeField] private float zoomAmount = 10;
    [SerializeField] private float maxZoomIn = -10f;
    [SerializeField] private float maxZoomOut = 10f;
    private float currentRotation = 0;
    float targetAngle = 0;

    private void Start()
    {
        cineCamera = GetComponent<CinemachineCamera>();
        maxZoomOut += Camera.main.fieldOfView;
        maxZoomIn += Camera.main.fieldOfView;
    }

    private void Update()
    {
        if (Mathf.Abs(currentRotation - targetAngle) > 1)
        {
            currentRotation += moveSpeed * Mathf.Sign(targetAngle - currentRotation) * Time.deltaTime;
            currentRotation = Mathf.Clamp(currentRotation, maxAngleLeft, maxAngleRight);

            pivotPoint.rotation = Quaternion.Euler(0, currentRotation, 0);
        }
    }

    public void RotateCameraAngle(float angle)
    {
        if (angle == 0) return;


        targetAngle = currentRotation + angle;
        targetAngle = Mathf.Clamp(targetAngle, maxAngleLeft, maxAngleRight);
        float duration = MathF.Abs(currentRotation - targetAngle) /moveSpeed;
    }

    public void RotateCameraMax(bool isLeft)
    {

        float duration = 1;

        if (isLeft)
        {
            duration = (currentRotation - maxAngleLeft) / moveSpeed;
            targetAngle = maxAngleLeft;
        }
        else
        {
            duration = (currentRotation + maxAngleRight) / moveSpeed;
            targetAngle = maxAngleRight;
        }
    }

    public void ZoomCamera()
    {
        if (DOTween.IsTweening(Camera.main))
            DOTween.Kill(Camera.main);

        float endValue = cineCamera.Lens.FieldOfView + zoomAmount;
        if (cineCamera.Lens.FieldOfView + zoomAmount > maxZoomOut)
            endValue = maxZoomOut;
        if (cineCamera.Lens.FieldOfView + zoomAmount < maxZoomIn)
            endValue = maxZoomIn;            
            
        DOTween.To(() => cineCamera.Lens.FieldOfView, x => cineCamera.Lens.FieldOfView = x, endValue, Mathf.Abs(cineCamera.Lens.FieldOfView - endValue) / zoomSpeed).SetEase(Ease.Linear);
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


