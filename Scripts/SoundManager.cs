using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string soundName;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop = false;
    }

    public List<Sound> sounds;

    private Dictionary<string, AudioSource> audioSources;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSources = new Dictionary<string, AudioSource>();
        foreach (var sound in sounds)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.loop = sound.loop;
            audioSources[sound.soundName] = audioSource;
        }
    }

    public void PlaySound(string soundName)
    {
        if (audioSources.ContainsKey(soundName))
        {
            audioSources[soundName].Play();
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    public void StopSound(string soundName)
    {
        if (audioSources.ContainsKey(soundName))
        {
            audioSources[soundName].Stop();
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    public void StopAllSounds()
    {
        foreach (var audioSource in audioSources.Values)
        {
            audioSource.Stop();
        }
    }
}
