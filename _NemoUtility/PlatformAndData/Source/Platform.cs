using System;

namespace NemoUtility
{
    public abstract class Platform
    {
        public abstract void FullScreenShow();
        public abstract void Rewarded(Action rewardComplateAction);
        public abstract Data GetAllData();
        public abstract object GetData(string id);
        public abstract void SetData(string id, object value);
        public abstract void SetLeaderBoardValue(string id, double value);

        protected bool FindId(Data data, string id)
        {
            return data.Datas.ContainsKey(id);
        }
    }
}