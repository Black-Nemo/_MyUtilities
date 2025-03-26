using System;
using NemoUtility;
using YG;

public class YandexGamesPlatform : Platform
{
    public override void FullScreenShow()
    {
        YandexGame.FullscreenShow();
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
        if (YandexGame.savesData.Data.Datas.TryGetValue(id, out object value))
        {
            return value;
        }
        return null;
    }

    public override void SetData(string id, object value)
    {
        if (FindId(YandexGame.savesData.Data, id))
        {
            YandexGame.savesData.Data.Datas[id] = value;
        }
        else
        {
            YandexGame.savesData.Data.Datas.Add(id, value);
        }

        YandexGame.SaveProgress();
    }

    public override void SetLeaderBoardValue(string id, double value)
    {
        YandexGame.NewLeaderboardScores(id, (long)value);
    }
}
