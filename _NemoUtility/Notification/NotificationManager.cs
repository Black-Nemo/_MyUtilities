using UnityEngine;

namespace NemoUtility
{
    public class NotificationManager : MonoBehaviour
    {
        [SerializeField] private Notification _notificationPrefab;

        [SerializeField] private Transform _notificationSpawnTransform;


        public static NotificationManager Instance;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void ShowNotification(string text, Color textColor, Color bgColor, AudioClip audioClip = null)
        {
            var notification = Instantiate(_notificationPrefab, _notificationSpawnTransform);
            RectTransform rectTransform = notification.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + rectTransform.sizeDelta.y / 2);
            notification.Init(text, textColor, bgColor, audioClip);
            Destroy(notification.gameObject, 5f);
        }
    }
}