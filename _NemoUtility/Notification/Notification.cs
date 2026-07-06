using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NemoUtility
{
    public class Notification : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _bgImage;
        [SerializeField] private AudioSource _audioSource;

        public void Init(string text, Color textColor, Color bgColor, AudioClip audioClip)
        {
            _text.text = text;
            _text.color = textColor;
            _bgImage.color = bgColor;
            if (audioClip != null)
            {
                _audioSource.clip = audioClip;
                _audioSource.Play();
            }
        }
    }
}