using UnityEngine;
using UnityEngine.UI;

namespace NemoUtility
{
    [RequireComponent(typeof(Button))]
    public class OpenPanelButton : MonoBehaviour
    {
        [SerializeField] private string _panelName;

        private Button _button;
        private void Awake()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(() =>
            {
                PanelManager.Instance.OpenPanel(_panelName);
            });
        }
    }
}