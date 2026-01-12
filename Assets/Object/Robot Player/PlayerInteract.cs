using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform playerForward;

    public void OnInteract(InputValue input)
    {
        Debug.Log("Interact pressed");
        Physics.BoxCast(transform.position, Vector3.one * 0.5f, playerForward.forward, out RaycastHit hit, Quaternion.identity, 2f);
        Debug.DrawLine(transform.position, transform.position + playerForward.forward * 2f, Color.red, 2f);
        if (hit.collider != null)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.OnInteract();
            }
        }
    }
}
