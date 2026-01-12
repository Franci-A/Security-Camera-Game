using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] private float speed = 5f;
    private Vector3 direction;
    private Vector3 inputValue;
    [Range(0,1)]
    [SerializeField] private float controllzeDeadzone = 0.15f;
    private Transform cameraTransform;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    public void OnMove(InputValue input)
    {
        Vector2 value = input.Get<Vector2>();
        if(value.magnitude < controllzeDeadzone)
            value = Vector2.zero;
        inputValue = new Vector3(value.x, 0, value.y);

        Vector3 camFoward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        
        camFoward.y = 0;
        camRight.y = 0;

        Vector3 forwardRelative = value.y * camFoward.normalized;
        Vector3 rightRelative = value.x * camRight.normalized;

        direction = (forwardRelative + rightRelative).normalized;
    }

    private void FixedUpdate()
    {
        characterController.Move(direction * speed * Time.deltaTime);
    }
}
