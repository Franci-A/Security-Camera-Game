using HelperScripts.EventSystem;
using UnityEngine;

public class CameraHackHandler : MonoBehaviour, IInteractable
{
    public bool isHacked = false;
    [SerializeField] private EventObjectScriptable cameraHackStartEvent;
    [SerializeField] private EventObjectScriptable cameraHackEndEvent;
    private string code;

    public string GetCode => code;

    private void Awake()
    {
        code = Random.Range(1000, 9999).ToString();
    }

    public void OnInteract()
    {
        if (isHacked)
            return;

        //start QTE
        cameraHackStartEvent.Call(this);
        cameraHackEndEvent.AddListener(OnSuccesfullHack);

    }

    public void OnSuccesfullHack(object obj)
    {
        if((CameraHackHandler)obj == this)
        isHacked = true;
        //onHackCompletCommand
        cameraHackEndEvent.RemoveListener(OnSuccesfullHack);

    }
}
