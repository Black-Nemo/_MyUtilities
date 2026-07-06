using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NemoUtility
{
    public class AutoTextChangeData : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI Text;
        [SerializeField] private string Key;

        [SerializeField] private RectTransform _parentContentSizeFildterTargetRectTransform;

        private void OnEnable()
        {
            DataManager.Instance.SetDataEvent += SetData;
            SetData(Key, DataManager.Instance.GetInt(Key, true));
        }

        private void OnDisable()
        {
            DataManager.Instance.SetDataEvent -= SetData;
        }

        public void Start()
        {
            SetData(Key, DataManager.Instance.GetInt(Key, true));
        }

        public void SetData(string id, object value)
        {
            if (id == Key)
            {
                Text.text = value.ToString();
                if (_parentContentSizeFildterTargetRectTransform != null) { StartCoroutine(UpdateLayoutNextFrame(_parentContentSizeFildterTargetRectTransform)); }
            }
        }

        public IEnumerator UpdateLayoutNextFrame(RectTransform rectTransform)
        {
            // O anki frame'in bitmesini bekle
            yield return new WaitForEndOfFrame();

            // Layout'u güncelle
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}