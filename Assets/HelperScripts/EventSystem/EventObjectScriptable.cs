using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HelperScripts.EventSystem
{
    [CreateAssetMenu(fileName = "New Event", menuName = "UnityHelperScripts/Event system/Event Object Variable", order = 0)]
    public class EventObjectScriptable : ScriptableObject
    {
        private void CallEventNull()
        {
            Call(null);
        }

        [TextArea]
        [SerializeField] private string description = "";
        private List<EventObjectListener> callbacks = new List<EventObjectListener>();
        private Action<object> actionCallbacks = null;
        public void Call(object obj)
        {
            for(int i = 0; i< callbacks.Count; i++)
            {
                if(callbacks[i] != null)
                {
                    callbacks[i].Raise(obj);
                }
            }
            actionCallbacks?.Invoke(obj);
        }

        public void RemoveAllListeners()
        {
            callbacks = new List<EventObjectListener>();
        }

        public void AddListener(EventObjectListener eventListener)
        {
            callbacks.Add(eventListener);
        }

        public void RemoveListener(EventObjectListener eventListener)
        {
            callbacks.Remove(eventListener);
        }

        public void AddListener(Action<object> action)
        {
            actionCallbacks += action;
        }
        
        public void RemoveListener(Action<object> action)
        {
            actionCallbacks -= action;
        }
    }
}
