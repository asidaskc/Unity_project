using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShrinkAndFade : MonoBehaviour
{
    public string targetTag = "Shrinkable"; // Tag of the object that causes the effect
    public float shrinkDuration = 1f; // Duration to shrink to a small size
    public GameObject fadeInObject; // Assign the fade-in object in the Inspector
    public GameObject fadeObject; // Assign the fade object in the Inspector

    [Header("Scene Loading")]
    [Tooltip("If set, this scene name will be loaded instead of the next build index.")]
    public string explicitSceneName = "";

    private bool isShrinking = false;
    private bool hasPlayedSound = false;

    private void Start()
    {
        // Enable the fade-in object at the start
        if (fadeInObject != null)
        {
            fadeInObject.SetActive(true);
            Debug.Log("Fade-in object enabled.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the specified tag, is not already shrinking, and the sound hasn't been played
        if (collision.gameObject.CompareTag(targetTag) && !isShrinking && !hasPlayedSound)
        {
            Debug.Log($"Collision with {collision.gameObject.name} detected. Starting shrink and fade.");
            StartCoroutine(ShrinkAndTurnWhite());
            SoundManager.Instance.PlaySound("LevelComplete"); // Ensure this matches your sound name
            hasPlayedSound = true;  // Set the flag to true to prevent the sound from playing again
        }
    }

    private IEnumerator ShrinkAndTurnWhite()
    {
        isShrinking = true;

        // Get the original scale
        Vector3 originalScale = transform.localScale;

        // Set color to white immediately
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
            Debug.Log("Color changed to white.");
        }

        // Start shrinking
        float elapsedTime = 0f;
        while (elapsedTime < shrinkDuration)
        {
            float t = elapsedTime / shrinkDuration;
            transform.localScale = Vector3.Lerp(originalScale, new Vector3(0.01f, 0.01f, 0.01f), t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set the scale to a very small value instead of disabling
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        Debug.Log("Object has shrunk to a small scale, but remains active.");

        // Enable the fade object for transition
        if (fadeObject != null)
        {
            fadeObject.SetActive(true);
            Debug.Log("Fade object enabled for transition.");
            StartCoroutine(FadeOut()); // Start fading out
        }

        // Wait before loading the next scene
        yield return new WaitForSeconds(2f);
        Debug.Log("Waiting for 2 seconds before loading the next scene.");

        LoadNextScene();
    }

    private IEnumerator FadeOut()
    {
        // Assuming the fadeObject has a CanvasGroup component for fading
        CanvasGroup canvasGroup = fadeObject.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            float fadeDuration = 1f; // Duration for fading out
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 1f; // Ensure it's fully opaque
        }
    }

    private void LoadNextScene()
    {
        // If explicitly requested, load that scene by name.
        if (!string.IsNullOrWhiteSpace(explicitSceneName))
        {
            SceneManager.LoadScene(explicitSceneName);
            Debug.Log($"Loading explicit scene: {explicitSceneName}");
            return;
        }

        // Keep Credits as the final scene while still allowing a secret Level7 in Build Settings.
        // This prevents Level6 from automatically progressing into Level7.
        string currentName = SceneManager.GetActiveScene().name;
        if (currentName == "Level6")
        {
            SceneManager.LoadScene("Credits");
            Debug.Log("Level6 completed. Skipping secret level and loading Credits.");
            return;
        }

        // Default behavior: load next build index.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
            Debug.Log($"Loading next scene: {currentSceneIndex + 1}");
        }
        else
        {
            Debug.LogWarning("No next scene available to load.");
        }
    }
}
