using UnityEngine;

namespace NemoUtility
{
    public class AnimationAudioSourceAutoPlay : MonoBehaviour
    {
        [SerializeField] private AnimationEvent _animationEvent;
        [SerializeField] private int _index;
        [SerializeField] private bool _noParentAndAutoDestroy = false;

        private AudioSource _audioSource;

        private SoundManager _soundManager;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            _soundManager = ServiceLocator<SoundManager>.GetService();
        }

        private void Start()
        {
            _animationEvent.OnAnimationEvent += (a) =>
            {
                if (a == _index)
                {
                    if (!_noParentAndAutoDestroy)
                    {
                        _audioSource.Play();
                    }
                    else
                    {
                        _soundManager.PlaySound(_audioSource.clip, transform.position, _audioSource.maxDistance, _audioSource.volume, _audioSource.spatialBlend);
                    }
                }
            };
        }
    }

}
