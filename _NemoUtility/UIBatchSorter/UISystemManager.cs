using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

namespace NemoUtility
{
    public class UISystemManager : MonoBehaviour
    {
        public static UISystemManager Instance;

        [Header("Hedef Klasörler")]
        public RectTransform imageFolder;
        public RectTransform textFolder;

        [Header("Taşınacak Objeler (Beyaz Liste)")]
        [Tooltip("Sadece bu listelere koyduğunuz objeler klasörlere taşınır ve optimize edilir.")]
        public List<GameObject> imagesToBatch = new List<GameObject>();
        public List<GameObject> textsToBatch = new List<GameObject>();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            // Oyun başladığında sadece listelediklerini organize et
            OrganizeExplicitUI();
        }

        [ContextMenu("Listelenenleri Organize Et")]
        public void OrganizeExplicitUI()
        {
            if (imageFolder == null || textFolder == null)
            {
                Debug.LogError("HATA: Hedef klasörler atanmamış!");
                return;
            }

            // 1. Sadece listedeki görselleri taşı
            foreach (var img in imagesToBatch)
            {
                if (img != null) RegisterUI(img, imageFolder);
            }

            // 2. Sadece listedeki metinleri taşı
            foreach (var txt in textsToBatch)
            {
                if (txt != null) RegisterUI(txt, textFolder);
            }

            Debug.Log($"UI Optimizasyonu Tamamlandı! Sadece seçtiğin {imagesToBatch.Count + textsToBatch.Count} obje optimize edildi.");
        }

        // Çalışma anında (Runtime) yeni bir obje eklemek istersen bu fonksiyonu çağırabilirsin
        public void RegisterUI(GameObject obj, RectTransform targetFolder)
        {
            if (obj == null) return;

            Transform oldParent = obj.transform.parent;
            bool wasActiveInHierarchy = obj.activeInHierarchy;

            // Obje zaten o klasörde değilse işlemi yap
            if (oldParent != null && oldParent != targetFolder)
            {
                // Eski sırasını kaydet
                int originalSiblingIndex = obj.transform.GetSiblingIndex();

                // Gölge Obje (Proxy) Oluştur
                string proxyName = "_Proxy_" + obj.name;
                Transform existingProxy = oldParent.Find(proxyName);
                UIHierarchyProxy proxy;

                if (existingProxy == null)
                {
                    // Proxy'yi UI objesi gibi oluştur ve eski sıraya koy
                    GameObject proxyGo = new GameObject(proxyName, typeof(RectTransform));
                    proxyGo.transform.SetParent(oldParent, false);
                    proxyGo.transform.SetSiblingIndex(originalSiblingIndex);

                    // Ekranda yer kaplamaması için boyutunu sıfırla
                    RectTransform prt = proxyGo.GetComponent<RectTransform>();
                    prt.sizeDelta = Vector2.zero;

                    proxy = proxyGo.AddComponent<UIHierarchyProxy>();
                }
                else
                {
                    proxy = existingProxy.GetComponent<UIHierarchyProxy>();
                }

                proxy.targetElement = obj;

                // Eğer objenin eski yeri kapalıysa, kendisini de kapalı tut
                obj.SetActive(wasActiveInHierarchy);
            }

            // Objeyi yeni klasöre taşı (Dünya koordinatlarını koruyarak)
            obj.transform.SetParent(targetFolder, true);

            // Z-Order ve Scale hatalarını önlemek için değerleri sabitle
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            Vector3 lp = rt.localPosition;
            lp.z = 0;
            rt.localPosition = lp;
        }
    }
}