using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    public AudioSource musicSource;  // Reference to the AudioSource component

    // Array of scene names or indices where the music should be disabled
    public string[] scenesToDisableMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Preserve this GameObject when loading new scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate GameObjects if this one already exists
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to the sceneLoaded event
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Unsubscribe to prevent memory leaks
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the current scene name is in the list of scenes to disable music
        foreach (string sceneName in scenesToDisableMusic)
        {
            if (scene.name == sceneName)
            {
                if (musicSource != null)
                {
                    musicSource.Stop();  // Stop the music
                }
                return;
            }
        }

        // If the scene does not match, ensure the music is playing
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
}
