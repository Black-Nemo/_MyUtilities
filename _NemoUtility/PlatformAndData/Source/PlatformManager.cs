using System;
using UnityEngine;

namespace NemoUtility
{
    public class PlatformManager : MonoBehaviour
    {
        public PlatformTypes PlatformTypes;

        private Platform _currentPlatform;


        private void OnEnable()
        {
            if (_currentPlatform != null)
            {
                _currentPlatform.OnEnable();
            }
        }
        private void OnDisable()
        {
            if (_currentPlatform != null)
            {
                _currentPlatform.OnDisable();
            }
        }


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
                    _currentPlatform = new LocalPlatform();
                    break;
                case PlatformTypes.YandexGames:
                    _currentPlatform = new YandexGamesPlatform();
                    break;
                case PlatformTypes.CrazyGames:
                    break;

            }
        }

        public void FullScreenShow(Action finishAction)
        {
            _currentPlatform.FullScreenShow(finishAction);
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

        public string GetLanguage()
        {
            return _currentPlatform.GetLanguage();
        }
        public void SetLanguage(string lang)
        {
            _currentPlatform.SetLanguage(lang);
        }

        public void ResetData()
        {
            _currentPlatform.ResetData();
        }

        public Action<string> GetSwitchLangEvent()
        {
            return _currentPlatform.SwitchLangEvent;
        }
    }
}