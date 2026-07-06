using System.Collections.Generic;
using NemoUtility;
using UnityEngine;

public enum SoundType { Main, Effect, Music, Ambiance, Spell }

public class SoundManager : MonoBehaviour, IService
{
    public int AudioSourceCount = 15;
    public Queue<AudioSource> AudioSources = new Queue<AudioSource>();

    private Dictionary<AudioSource, float> _sourceBaseVolumes = new Dictionary<AudioSource, float>();
    private Dictionary<AudioSource, SoundType> _sourceTypes = new Dictionary<AudioSource, SoundType>();

    private void Awake()
    {
        for (int i = 0; i < AudioSourceCount; i++)
        {
            var g = new GameObject("Sound");
            AudioSource gameobjectAudioSource = g.AddComponent<AudioSource>();
            AudioSources.Enqueue(gameobjectAudioSource);
            _sourceBaseVolumes[gameobjectAudioSource] = 1f;
            _sourceTypes[gameobjectAudioSource] = SoundType.Effect;
        }
    }

    private void Start()
    {
        DataManager.Instance.SetDataEvent += OnDataChanged;
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
        if (id == "S_EFFECT_VOLUME" ||
            id == "S_MAIN_VOLUME" ||
            id == "S_MUSIC_VOLUME" ||
            id == "S_AMBIANCE_VOLUME" ||
            id == "S_SPELL_VOLUME")
        {
            ApplyVolumeToAll();
        }
    }

    private void ApplyVolumeToAll()
    {
        var mainVolumeFactor = DataManager.Instance.GetInt("S_MAIN_VOLUME") / 100f;
        var effectVolumeFactor = DataManager.Instance.GetInt("S_EFFECT_VOLUME") / 100f;
        var musicVolumeFactor = DataManager.Instance.GetInt("S_MUSIC_VOLUME") / 100f;
        var ambianceVolumeFactor = DataManager.Instance.GetInt("S_AMBIANCE_VOLUME") / 100f;
        var spellVolumeFactor = DataManager.Instance.GetInt("S_SPELL_VOLUME") / 100f;

        foreach (var source in AudioSources)
        {
            if (_sourceBaseVolumes.TryGetValue(source, out float baseVolume))
            {
                float typeFactor = 1f;
                if (_sourceTypes.TryGetValue(source, out SoundType type))
                {
                    switch (type)
                    {
                        case SoundType.Effect: typeFactor = effectVolumeFactor; break;
                        case SoundType.Music: typeFactor = musicVolumeFactor; break;
                        case SoundType.Ambiance: typeFactor = ambianceVolumeFactor; break;
                        case SoundType.Spell: typeFactor = spellVolumeFactor; break;
                        case SoundType.Main: typeFactor = 1f; break;
                    }
                }

                source.volume = baseVolume * typeFactor * mainVolumeFactor;
            }
        }
    }

    public AudioSource PlaySound(AudioClip audioClip, Vector3 position, float distance, float volume, float spatialBlend, SoundType type = SoundType.Effect)
    {
        var gameobjectAudioSource = AudioSources.Peek();
        AudioSources.Dequeue();
        AudioSources.Enqueue(gameobjectAudioSource);

        _sourceBaseVolumes[gameobjectAudioSource] = volume;
        _sourceTypes[gameobjectAudioSource] = type;

        var mainVolumeFactor = DataManager.Instance.GetInt("S_MAIN_VOLUME") / 100f;
        float typeFactor = 1f;
        switch (type)
        {
            case SoundType.Effect: typeFactor = DataManager.Instance.GetInt("S_EFFECT_VOLUME") / 100f; break;
            case SoundType.Music: typeFactor = DataManager.Instance.GetInt("S_MUSIC_VOLUME") / 100f; break;
            case SoundType.Ambiance: typeFactor = DataManager.Instance.GetInt("S_AMBIANCE_VOLUME") / 100f; break;
            case SoundType.Spell: typeFactor = DataManager.Instance.GetInt("S_SPELL_VOLUME") / 100f; break;
            case SoundType.Main: typeFactor = 1f; break;
        }

        gameobjectAudioSource.transform.SetParent(null);
        gameobjectAudioSource.transform.position = position;
        gameobjectAudioSource.maxDistance = distance;
        gameobjectAudioSource.spatialBlend = spatialBlend;
        gameobjectAudioSource.clip = audioClip;
        gameobjectAudioSource.volume = volume * typeFactor * mainVolumeFactor;
        gameobjectAudioSource.pitch = 1;
        gameobjectAudioSource.rolloffMode = AudioRolloffMode.Linear;
        gameobjectAudioSource.Play();
        return gameobjectAudioSource;
    }
}