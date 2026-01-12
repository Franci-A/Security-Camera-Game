using HelperScripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLoadCallEvent : MonoBehaviour
{
    [SerializeField] private EventScriptable eventScriptable = null;

    private void OnEnable()
    {
        eventScriptable.Call();
    }
}
