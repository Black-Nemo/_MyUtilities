using UnityEngine;

namespace NemoUtility
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceVolumeLinker : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private string _volumeKey;
        [SerializeField] private string _mainVolumeKey;
        [SerializeField] private float _baseVolume = 1f;

        private void Start()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }

            if (DataManager.Instance != null)
            {
                DataManager.Instance.SetDataEvent += OnDataChanged;
            }
            ApplyVolume();
        }

        private void OnDestroy()
        {
            if (DataManager.Instance != null)
            {
                DataManager.Instance.SetDataEvent -= OnDataChanged;
            }
        }

        private void OnDataChanged(string id, object value)
        {
            if (id == _volumeKey || id == _mainVolumeKey)
            {
                ApplyVolume();
            }
        }

        private void ApplyVolume()
        {
            if (_audioSource == null || DataManager.Instance == null) return;

            try
            {
                float volumeFactor = DataManager.Instance.GetInt(_volumeKey, true) / 100f;
                float mainVolumeFactor = DataManager.Instance.GetInt(_mainVolumeKey, true) / 100f;

                _audioSource.volume = _baseVolume * volumeFactor * mainVolumeFactor;
            }
            catch (System.Exception)
            {
                // DataManager'da anahtar yoksa veya bir sorun oluşursa baseVolume'u kullan
                _audioSource.volume = _baseVolume;
            }
        }
    }
}
