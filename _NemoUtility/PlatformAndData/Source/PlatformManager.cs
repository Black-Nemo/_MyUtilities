using System;
using UnityEngine;

namespace NemoUtility
{
    public class PlatformManager : MonoBehaviour
    {
        public PlatformTypes PlatformTypes;

        private Platform _currentPlatform;



        public static PlatformManager Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                SetPlatform(PlatformTypes);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void SetPlatform(PlatformTypes platformTypes)
        {
            switch (platformTypes)
            {
                case PlatformTypes.Local:
                    break;
                case PlatformTypes.YandexGames:
#if YANDEX_SDK
                    _currentPlatform = new YandexGamesPlatform();
#endif
                    break;
                case PlatformTypes.CrazyGames:
                    break;

            }
        }

        public void FullScreenShow()
        {
            _currentPlatform.FullScreenShow();
        }
        public void Rewarded(Action rewardComplateAction)
        {
            _currentPlatform.Rewarded(rewardComplateAction);
        }
        public Data GetAllData()
        {
            return _currentPlatform.GetAllData();
        }
        public object GetData(string id)
        {
            return _currentPlatform.GetData(id);
        }
        public void SetData(string id, object value)
        {
            _currentPlatform.SetData(id, value);
        }

        public void SetLeaderBoardValue(string id, double value)
        {
            _currentPlatform.SetLeaderBoardValue(id, value);
        }
    }
}