using UnityEngine;
using UnityEngine.UI;

namespace NemoUtility
{
    public class FullScreenShowButton : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => { FullScreenShow(); });
        }
        public void FullScreenShow()
        {
            PlatformManager.Instance.FullScreenShow(() => { Time.timeScale = 1f; });
        }
    }
}