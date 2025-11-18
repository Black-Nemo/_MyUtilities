using UnityEngine;
using UnityEngine.UI;

namespace NemoUtility
{
    [RequireComponent(typeof(Button))]
    public class BackPanelButton : MonoBehaviour
    {
        private Button _button;
        private void Awake()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(() =>
            {
                PanelManager.Instance.OpenBackPanel();
            });
        }
    }
}