using TMPro;
using UnityEngine;

namespace NemoUtility
{
    public class VersionText : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<TextMeshProUGUI>().text = Application.version;
        }
    }

}