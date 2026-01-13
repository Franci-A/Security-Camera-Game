using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform playerForward;
    [SerializeField] private LayerMask interactableLayer;
    private bool isActive = true;

    public bool IsActive { get => isActive; set => isActive = value; }

    public void OnInteract(InputAction.CallbackContext context)
    {

        Debug.Log("Interact pressed");
        if(!isActive|| !context.performed)
            return;

        Physics.BoxCast(transform.position,new Vector3(.5f, 1, .5f), playerForward.forward, out RaycastHit hit, Quaternion.identity, 2f, interactableLayer);
        
        if (hit.collider != null)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.OnInteract();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 2, 1));
        Gizmos.DrawWireCube(transform.position + playerForward.forward, new Vector3(1, 2, 1));
    }
}
