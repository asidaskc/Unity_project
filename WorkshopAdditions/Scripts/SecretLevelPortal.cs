using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A simple trigger portal that loads a secret scene when the player enters.
/// It identifies the player by looking for ShrinkAndFade (present on the Player prefab in this project),
/// so it won't be triggered by platforms or hazards.
/// </summary>
public class SecretLevelPortal : MonoBehaviour
{
    [Tooltip("Scene name as it appears in Assets/Scenes (and Build Settings).")]
    public string sceneName = "Level7";

    [Tooltip("Optional delay before loading (lets one-liners finish).")]
    public float loadDelay = 0.15f;

    private bool loading;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (loading) return;
        if (other == null) return;

        // Player prefab contains ShrinkAndFade; use that as a robust identifier.
        if (other.GetComponent<ShrinkAndFade>() == null && (other.attachedRigidbody == null || other.attachedRigidbody.GetComponent<ShrinkAndFade>() == null))
            return;

        loading = true;
        if (loadDelay <= 0f)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Invoke(nameof(LoadNow), loadDelay);
        }
    }

    private void LoadNow()
    {
        SceneManager.LoadScene(sceneName);
    }
}
