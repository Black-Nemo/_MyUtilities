using System;
using System.IO;
using NemoUtility;
using UnityEngine.Device;

namespace NemoUtility
{
    public class LocalPlatform : Platform
    {
        private string _filePath = "";
        private Data _data;

        public override void OnEnable()
        {
            _filePath = Path.Combine(Application.persistentDataPath, "Datas", "datas.json");

            // Eğer dosya yüklenemezse, yeni bir tane oluştur
            _data = MyJsonUtility<Data>.Load(_filePath);
            if (_data == null)
            {
                _data = new Data();
                MyJsonUtility<Data>.SaveData(_filePath, _data);
            }
        }

        public override void FullScreenShow(Action finishAction)
        {
            throw new NotImplementedException();
        }

        public override Data GetAllData()
        {
            return MyJsonUtility<Data>.Load(_filePath);
        }

        public override object GetData(string id)
        {
            if (_data.Datas.TryGetValue(id, out object value))
            {
                return value;
            }
            return null;
        }

        public override string GetLanguage()
        {
            throw new NotImplementedException();
        }

        public override void OnDisable()
        {
        }

        public override void ResetData()
        {
            throw new NotImplementedException();
        }

        public override void Rewarded(Action rewardComplateAction)
        {
            throw new NotImplementedException();
        }

        public override void SetData(string id, object value)
        {
            if (FindId(_data, id))
            {
                _data.Datas[id] = value;
            }
            else
            {
                _data.Datas.Add(id, value);
            }
            MyJsonUtility<Data>.SaveData(_filePath, _data);
        }

        public override void SetLanguage(string lang)
        {
            throw new NotImplementedException();
        }

        public override void SetLeaderBoardValue(string id, double value)
        {
            throw new NotImplementedException();
        }
    }

}