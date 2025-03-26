using UnityEngine;
using UnityEngine.UI;

namespace NemoUtility
{
    [RequireComponent(typeof(Button))]
    public class OpenUrlButton : MonoBehaviour
    {
        [Space]
        [SerializeField] private string url;

        private void Awake()
        {

            GetComponent<Button>().onClick.AddListener(() => { Application.OpenURL(url); });
        }
    }
}