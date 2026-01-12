using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    bool doorIsOpen = false;

    public void OpenDoor()
    {
        if (doorIsOpen) return;
        doorIsOpen = true;
        doorAnimator.SetTrigger("OpenDoor");
    }
}
