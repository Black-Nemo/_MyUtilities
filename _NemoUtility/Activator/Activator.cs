using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NemoUtility
{
    public class Activator : MonoBehaviour
    {
        [SerializeField] List<KeyEvents> keyEvents = new List<KeyEvents>();

        [SerializeField] private string ActiveTag;
        [SerializeField] private Component component;

        public UnityEvent OnCollisionEnterUnityEvent;
        public UnityEvent OnCollisionExitUnityEvent;
        public UnityEvent OnTriggerEnterUnityEvent;
        public UnityEvent OnTriggerExitUnityEvent;

        public Action<GameObject> OnCollisionEnterEvent;
        public Action<GameObject> OnCollisionExitEvent;
        public Action<GameObject> OnTriggerEnterEvent;
        public Action<GameObject> OnTriggerExitEvent;

        [Header("EnableEvents")]
        public UnityEvent OnEnableUnityEvent;
        public UnityEvent OnDisableUnityEvent;

        private void OnEnable()
        {
            OnEnableUnityEvent.Invoke();
        }
        private void OnDisable()
        {
            OnDisableUnityEvent?.Invoke();
        }
        private void Update()
        {
            foreach (var item in keyEvents)
            {
                if (Input.GetKeyDown(item.KeyCode))
                {
                    item.KeyDownEvent?.Invoke();
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (ActiveTag == "" || ActiveTag == other.gameObject.tag) { OnCollisionEnterEvent?.Invoke(other.gameObject); OnCollisionEnterUnityEvent?.Invoke(); }
            if (component != null && other.gameObject.TryGetComponent(out Component _component)) { OnCollisionEnterEvent?.Invoke(other.gameObject); OnCollisionEnterUnityEvent?.Invoke(); }
        }
        private void OnCollisionExit(Collision other)
        {
            if (ActiveTag == "" || ActiveTag == other.gameObject.tag) { OnCollisionExitEvent?.Invoke(other.gameObject); OnCollisionExitUnityEvent?.Invoke(); }
            if (component != null && other.gameObject.TryGetComponent(out Component _component)) { OnCollisionExitEvent?.Invoke(other.gameObject); OnCollisionExitUnityEvent?.Invoke(); }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (ActiveTag == "" || ActiveTag == other.gameObject.tag) { OnTriggerEnterEvent?.Invoke(other.gameObject); OnTriggerEnterUnityEvent?.Invoke(); }
            if (component != null && other.gameObject.TryGetComponent(out Component _component)) { OnTriggerEnterEvent?.Invoke(other.gameObject); OnTriggerEnterUnityEvent?.Invoke(); }
        }
        private void OnTriggerExit(Collider other)
        {
            if (ActiveTag == "" || ActiveTag == other.gameObject.tag) { OnTriggerExitEvent?.Invoke(other.gameObject); OnTriggerExitUnityEvent?.Invoke(); }
            if (component != null && other.gameObject.TryGetComponent(out Component _component)) { OnTriggerExitEvent?.Invoke(other.gameObject); OnTriggerExitUnityEvent?.Invoke(); }
        }
    }
    [System.Serializable]
    public class KeyEvents
    {
        public KeyCode KeyCode;
        public UnityEvent KeyDownEvent;
    }
}
