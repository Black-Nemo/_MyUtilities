using TMPro;
using UnityEngine;

namespace NemoUtility
{
    public class AutoTextChangeData : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI Text;
        [SerializeField] private string Key;

        private void OnDisable()
        {
            DataManager.Instance.SetDataEvent -= SetData;
        }

        public void Start()
        {
            DataManager.Instance.SetDataEvent += SetData;
            SetData(Key, DataManager.Instance.GetInt(Key, true));
        }

        public void SetData(string id, object value)
        {
            if (id == Key)
            {
                Text.text = value.ToString();
            }
        }
    }
}