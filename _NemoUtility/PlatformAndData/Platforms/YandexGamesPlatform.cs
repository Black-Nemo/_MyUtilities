namespace NemoUtility
{
#if YANDEX_SDK
    public class YandexGamesPlatform : Platform
    {
        public override void FullScreenShow()
        {
            YG.YandexGame.FullscreenShow();
        }

        public override void Rewarded(Action rewardComplateAction)
        {
            throw new NotImplementedException();
        }

        public override Data GetAllData()
        {
            throw new NotImplementedException();
        }

        public override object GetData(string id)
        {
            if (YG.YandexGame.savesData.Data.Datas.TryGetValue(id, out object value))
            { 
                return value;
            }
            return null;
        }

        public override void SetData(string id, object value)
        {
            if (FindId(YG.YandexGame.savesData.Data, id))
            {
                YG.YandexGame.savesData.Data.Datas[id] = value;
            }
            else
            {
                YG.YandexGame.savesData.Data.Datas.Add(id, value);
            }
            YG.YandexGame.SaveProgress();
        }

        public override void SetLeaderBoardValue(string id, double value)
        {
            YG.YandexGame.NewLeaderboardScores(id, (long)value);
        }
    }
#endif
}