using UnityEngine;

public class ButtonForDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorHandler connectedDoor;

    public void OnInteract()
    {
        connectedDoor.OpenDoor();
    }
}
