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

        public UnityEvent OnCollisionEnterEvent;
        public UnityEvent OnCollisionExitEvent;
        public UnityEvent OnTriggerEnterEvent;
        public UnityEvent OnTriggerExitEvent;

        [Header("EnableEvents")]
        public UnityEvent OnEnableEvent;
        public UnityEvent OnDisableEvent;
        private void OnEnable()
        {
            OnEnableEvent?.Invoke();
        }
        private void OnDisable()
        {
            OnDisableEvent?.Invoke();
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
            if (ActiveTag == "" || ActiveTag == other.gameObject.tag) { OnCollisionEnterEvent?.Invoke(); }
            if (component != null && other.gameObject.TryGetComponent(out Component _component)) { OnCollisionEnterEvent?.Invoke(); }
        }
        private void OnCollisionExit(Collision other)
        {
            if (ActiveTag == "" || ActiveTag == other.gameObject.tag) { OnCollisionExitEvent?.Invoke(); }
            if (component != null && other.gameObject.TryGetComponent(out Component _component)) { OnCollisionExitEvent?.Invoke(); }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (ActiveTag == "" || ActiveTag == other.gameObject.tag) { OnTriggerEnterEvent?.Invoke(); }
            if (component != null && other.gameObject.TryGetComponent(out Component _component)) { OnTriggerEnterEvent?.Invoke(); }
        }
        private void OnTriggerExit(Collider other)
        {
            if (ActiveTag == "" || ActiveTag == other.gameObject.tag) { OnTriggerExitEvent?.Invoke(); }
            if (component != null && other.gameObject.TryGetComponent(out Component _component)) { OnTriggerExitEvent?.Invoke(); }
        }
    }
    [System.Serializable]
    public class KeyEvents
    {
        public KeyCode KeyCode;
        public UnityEvent KeyDownEvent;
    }
}
