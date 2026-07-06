using UnityEngine;
using System.Collections.Generic;

namespace NemoUtility
{
    public class NemoLODObject : MonoBehaviour
    {
        [Header("LOD Settings")]
        [Tooltip("Nesnenin render edileceği maksimum mesafe")]
        public float renderDistance = 50f;

        [Tooltip("Eğer seçilirse alt objelerde Animator arar ve LOD durumuna göre açıp kapatır")]
        public bool handleAnimator = false;

        private Renderer[] renderers;
        private Animator animator;
        private bool isVisible = true;

        private void Start()
        {
            // Alt objelerdeki tüm rendererları alıp MeshRenderer veya SkinnedMeshRenderer olanları filtreliyoruz
            Renderer[] allRenderers = GetComponentsInChildren<Renderer>(true);
            List<Renderer> validRenderers = new List<Renderer>();

            foreach (var rnd in allRenderers)
            {
                if (rnd is MeshRenderer || rnd is SkinnedMeshRenderer)
                {
                    validRenderers.Add(rnd);
                }
            }
            renderers = validRenderers.ToArray();

            // Eğer animator kontrolü isteniyorsa ilk bulduğumuz animatorü alıyoruz
            if (handleAnimator)
            {
                animator = GetComponentInChildren<Animator>(true);
            }

            // Manager'a kayıt olma işlemi
            if (NemoLODManager.Instance != null)
            {
                NemoLODManager.Instance.Register(this);
            }
            else
            {
                Debug.LogWarning("NemoLODManager sahnede bulunamadı! Lütfen sisteme ekleyin.", this);
            }
        }

        private void OnDestroy()
        {
            // Yok olurken abonelikten çıkma
            if (NemoLODManager.Instance != null)
            {
                NemoLODManager.Instance.Unregister(this);
            }
        }

        public void CheckLOD(Vector3 cameraPosition)
        {
            // sqrMagnitude kullanarak performans için karekök alma işleminin önüne geçiyoruz
            float distanceSqr = (transform.position - cameraPosition).sqrMagnitude;
            bool shouldBeVisible = distanceSqr <= (renderDistance * renderDistance);

            if (shouldBeVisible != isVisible)
            {
                isVisible = shouldBeVisible;
                SetVisibility(isVisible);
            }
        }

        private void SetVisibility(bool state)
        {
            if (renderers != null)
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    if (renderers[i] != null)
                    {
                        renderers[i].enabled = state;
                    }
                }
            }

            if (handleAnimator && animator != null)
            {
                animator.enabled = state;
            }
        }
    }
}
