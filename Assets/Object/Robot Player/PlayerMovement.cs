using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector3 direction;

    [Range(0,1)]
    [SerializeField] private float controllzeDeadzone = 0.15f;
    private Transform cameraTransform;

    [SerializeField] private Transform visual;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravityPull = 1;
    [SerializeField] private float targetHeight = 1f;

    private bool isActive = true;
    public bool IsActive { get => isActive; set => isActive = value; }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5, groundMask);
        transform.position = new Vector3(transform.position.x, hit.point.y + targetHeight, transform.position.z);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        if(value.magnitude < controllzeDeadzone)
            value = Vector2.zero;

        if (!isActive)
            return;

        Vector3 camFoward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        
        camFoward.y = 0;
        camRight.y = 0;

        Vector3 forwardRelative = value.y * camFoward.normalized;
        Vector3 rightRelative = value.x * camRight.normalized;

        direction = (forwardRelative + rightRelative);
        if (value.magnitude > controllzeDeadzone)
            visual.forward = direction;
    }

    private void FixedUpdate()
    {
        if (!isActive)
            return;

        Vector3 targetPos = transform.position;
        targetPos += direction * speed * Time.deltaTime;
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5, groundMask);
        if (hit.collider != null)
        {
            if (Mathf.Abs(hit.point.y - transform.position.y) > targetHeight + .05f || Mathf.Abs(hit.point.y - transform.position.y) < targetHeight - .05f)
            {
               targetPos += new Vector3(0, gravityPull * Time.fixedDeltaTime * Mathf.Sign(hit.point.y - transform.position.y + targetHeight));
            }
        }
        transform.position = targetPos;
    }
}
