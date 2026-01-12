using System;
using System.Collections.Generic;
using UnityEngine;

namespace HelperScripts.EventSystem
{
    [CreateAssetMenu(fileName = "New Event", menuName = "UnityHelperScripts/Event system/Event Scriptable")]
    public class EventScriptable : ScriptableObject
    {
        private void CallEvent()
        {
            Call();
        }

        private List<EventListener> callbacks = new List<EventListener>();
        private Action actionCallbacks = null;

        [TextArea]
        [SerializeField] private string description= "";
        public void Call()
        {
            for(int i = 0; i < callbacks.Count; i++)
            {
                if(i < callbacks.Count)
                {
                    callbacks[i].Raise();
                }
            }
            actionCallbacks?.Invoke();
        }

        public void AddListener(EventListener eventListener)
        {
            callbacks.Add(eventListener);
        }

        public void RemoveListener(EventListener eventListener)
        {
            callbacks.Remove(eventListener);
        }
        public void AddListener(Action action)
        {
            actionCallbacks += action;
        }

        public void RemoveListener(Action action)
        {
            actionCallbacks -= action;
        }
    }
}
