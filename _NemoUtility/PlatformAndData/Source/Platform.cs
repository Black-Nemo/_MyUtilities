using System;

namespace NemoUtility
{
    public abstract class Platform
    {
        public Action<string> SwitchLangEvent;

        public abstract void OnEnable();
        public abstract void OnDisable();

        public abstract void FullScreenShow(Action finishAction);
        public abstract void Rewarded(Action rewardComplateAction);
        public abstract Data GetAllData();
        public abstract object GetData(string id);
        public abstract void SetData(string id, object value);
        public abstract void SetLeaderBoardValue(string id, double value);
        public abstract string GetLanguage();
        public abstract void SetLanguage(string lang);
        public abstract void ResetData();

        protected bool FindId(Data data, string id)
        {
            return data.Datas.ContainsKey(id);
        }
    }
}