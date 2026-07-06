using UnityEngine;
using System.Collections.Generic;

namespace NemoUtility
{
    public class NemoLODManager : MonoBehaviour
    {
        public static NemoLODManager Instance { get; private set; }

        [Header("Settings")]
        [Tooltip("LOD kontrollerinin kaç saniyede bir yapılacağı")]
        public float checkInterval = 0.5f;

        private List<NemoLODObject> lodObjects = new List<NemoLODObject>();
        private float timer;
        private Transform cameraTransform;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // Sahne değişimlerinde manager'ın yok olmasını istemiyorsanız DontDestroyOnLoad ekleyebilirsiniz:
                // DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            FindCamera();
        }

        private void FindCamera()
        {
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        public void Register(NemoLODObject obj)
        {
            if (!lodObjects.Contains(obj))
            {
                lodObjects.Add(obj);
            }
        }

        public void Unregister(NemoLODObject obj)
        {
            if (lodObjects.Contains(obj))
            {
                lodObjects.Remove(obj);
            }
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= checkInterval)
            {
                timer = 0f;
                PerformLODCheck();
            }
        }

        private void PerformLODCheck()
        {
            if (cameraTransform == null)
            {
                FindCamera();
                // Hala kamera bulunamadıysa işlemi iptal et
                if (cameraTransform == null) return;
            }

            Vector3 camPos = cameraTransform.position;

            for (int i = lodObjects.Count - 1; i >= 0; i--)
            {
                var obj = lodObjects[i];
                // Obje sahnede silinmişse listemizden de temizliyoruz
                if (obj == null)
                {
                    lodObjects.RemoveAt(i);
                    continue;
                }

                obj.CheckLOD(camPos);
            }
        }
    }
}
