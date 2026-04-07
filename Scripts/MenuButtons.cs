using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButtons : MonoBehaviour
{
    public GameObject objectToEnable; // Reference to the GameObject to enable (e.g., fade-out screen)
    public float delayBeforeSceneLoad = 2f; // Duration to wait before loading the next scene

    // Method to start the game with fade-out transition and delay
    public void StartGame()
    {
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
        StartCoroutine(LoadNextSceneAfterDelay(delayBeforeSceneLoad));
    }

    // Coroutine to load the next scene after a delay
    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes to load!");
        }
    }

    // Method to exit the game
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Method to return to the main menu with fade-out transition and delay
    public void ReturnToMainMenu()
    {
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
        StartCoroutine(LoadMainMenuAfterDelay(delayBeforeSceneLoad));
    }

    // Coroutine to load the main menu scene after a delay
    private IEnumerator LoadMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Load the main menu scene by index (assuming it's at build index 0)
        SceneManager.LoadScene(0);
    }
}
