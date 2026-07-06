using UnityEngine;
using System.Collections.Generic;

namespace NemoUtility
{
    public class UIHierarchyLink : MonoBehaviour
    {
        public List<GameObject> detachedElements = new List<GameObject>();
        private bool lastState = true;

        // Gölge obje hala hiyerarşide olduğu için activeInHierarchy 
        // en üstteki objeye kadar tüm zinciri kontrol eder.
        private void Update()
        {
            bool currentState = gameObject.activeInHierarchy;

            if (currentState != lastState)
            {
                SyncActive(currentState);
                lastState = currentState;
            }
        }

        private void OnDestroy()
        {
            foreach (var el in detachedElements)
                if (el != null) Destroy(el);
        }

        private void SyncActive(bool state)
        {
            foreach (var el in detachedElements)
                if (el != null) el.SetActive(state);
        }
    }
}