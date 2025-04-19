using UnityEngine;

namespace NemoUtility
{
    public class AutoSetactiveChange : MonoBehaviour
    {
        [SerializeField] private GameObject _object;
        [SerializeField] private string Key;

        private void OnDisable()
        {
            DataManager.Instance.SetDataEvent -= SetData;
        }


        public void Start()
        {
            DataManager.Instance.SetDataEvent += SetData;
            try
            {
                var x = DataManager.Instance.GetBool(Key);
                SetData(Key, x);
                Debug.Log(x);
            }
            catch (Exception404 ex)
            {
                Debug.Log(ex.Message);
            }

        }

        public void SetData(string id, object value)
        {
            if (id == Key)
            {
                _object.SetActive((bool)value);
            }
        }
    }
}