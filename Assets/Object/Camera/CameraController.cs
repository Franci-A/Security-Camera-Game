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


    private void Start()
    {
        cineCamera = GetComponent<CinemachineCamera>();
        maxZoomOut += Camera.main.fieldOfView;
        maxZoomIn += Camera.main.fieldOfView;
    }

    public void RotateCameraAngle(float angle)
    {
        if (angle == 0) return;

        if (DOTween.IsTweening(pivotPoint))
            DOTween.Kill(pivotPoint);

        float targetAngle = (pivotPoint.rotation.y * Mathf.Rad2Deg) + angle;
        targetAngle = Mathf.Clamp(targetAngle, maxAngleLeft, maxAngleRight);
        float duration = MathF.Abs(pivotPoint.rotation.y * Mathf.Rad2Deg - targetAngle) /moveSpeed;
        pivotPoint.DORotate(new Vector3(0, targetAngle,0), duration);

    }

    public void RotateCameraMax(bool isLeft)
    {
        if (DOTween.IsTweening(pivotPoint))
            DOTween.Kill(pivotPoint);
        float duration = 1;
        Vector3 targetAngle;

        //Debug.Log((pivotPoint.rotation.y * Mathf.Rad2Deg) + "  -  total : " + (pivotPoint.rotation.y * Mathf.Rad2Deg - maxAngleLeft));
        if (isLeft)
        {
            duration = (pivotPoint.rotation.y * Mathf.Rad2Deg - maxAngleLeft) / moveSpeed;
            targetAngle = new Vector3(0, maxAngleLeft);
        }
        else
        {
            duration = (pivotPoint.rotation.y * Mathf.Rad2Deg + maxAngleRight) / moveSpeed;
            targetAngle = new Vector3(0, maxAngleRight);
        }

        pivotPoint.DORotate(targetAngle, duration);

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


