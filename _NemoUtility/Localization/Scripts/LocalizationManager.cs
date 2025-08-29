using UnityEngine;

namespace NemoUtility
{
    [DefaultExecutionOrder(-1000)]
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance;

        [SerializeField] private Localization _localization;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

        }

        public Localization GetLocalization()
        {
            return _localization;
        }
    }
}