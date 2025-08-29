using System;
using NemoUtility;

namespace NemoUtility
{
    public class YandexGamesPlatform : Platform
    {
        private Action CloseFullAdEvent;
        private Action RewardedEvent;

        public override void OnEnable()
        {
#if YG_PLUGIN_YANDEX_GAME
        YG.YandexGame.CloseFullAdEvent += CloseFullEvent;
        YandexGame.RewardVideoEvent += Rewarded;
        YandexGame.SwitchLangEvent += SwitchLang;
#endif
        }

        public override void OnDisable()
        {
#if YG_PLUGIN_YANDEX_GAME
        YandexGame.CloseFullAdEvent -= CloseFullEvent;
        YandexGame.RewardVideoEvent -= Rewarded;
        YandexGame.SwitchLangEvent -= SwitchLang;
#endif
        }

        private void Rewarded(int obj)
        {
#if YG_PLUGIN_YANDEX_GAME
        if (RewardedEvent != null)
        {
            RewardedEvent?.Invoke();
        }
        RewardedEvent = null;
#endif
        }

        private void CloseFullEvent()
        {
#if YG_PLUGIN_YANDEX_GAME
        if (CloseFullAdEvent != null)
        {
            CloseFullAdEvent?.Invoke();
        }
        CloseFullAdEvent = null;
#endif
        }

        public override void FullScreenShow(Action finishAction)
        {
#if YG_PLUGIN_YANDEX_GAME
        CloseFullAdEvent += finishAction;
        YandexGame.FullscreenShow();
#endif
        }

        public override void Rewarded(Action rewardComplateAction)
        {
#if YG_PLUGIN_YANDEX_GAME
        RewardedEvent += rewardComplateAction;
        YandexGame.RewVideoShow(1);
#endif
        }

        public override Data GetAllData()
        {
#if YG_PLUGIN_YANDEX_GAME
        
#endif
            throw new NotImplementedException();
        }

        public override object GetData(string id)
        {
#if YG_PLUGIN_YANDEX_GAME
        if (YandexGame.savesData.Data.Datas.TryGetValue(id, out object value))
        {
            return value;
        }
#endif
            return null;

        }

        public override void SetData(string id, object value)
        {
#if YG_PLUGIN_YANDEX_GAME
        if (FindId(YandexGame.savesData.Data, id))
        {
            YandexGame.savesData.Data.Datas[id] = value;
        }
        else
        {
            YandexGame.savesData.Data.Datas.Add(id, value);
        }

        YandexGame.SaveProgress();
#endif
        }

        public override void SetLeaderBoardValue(string id, double value)
        {
#if YG_PLUGIN_YANDEX_GAME
        YandexGame.NewLeaderboardScores(id, (long)value);
#endif
        }

        public override void ResetData()
        {
#if YG_PLUGIN_YANDEX_GAME
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
#endif
        }

        public override string GetLanguage()
        {
#if YG_PLUGIN_YANDEX_GAME
        return YandexGame.lang;
#endif
            return null;
        }
        public override void SetLanguage(string lang)
        {
#if YG_PLUGIN_YANDEX_GAME
        YandexGame.SwitchLanguage(lang);
#endif
        }
        private void SwitchLang(string obj)
        {
#if YG_PLUGIN_YANDEX_GAME
        SwitchLangEvent?.Invoke(obj);
#endif
        }
    }

}