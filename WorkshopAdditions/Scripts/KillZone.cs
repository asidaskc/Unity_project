using UnityEngine;

/// <summary>
/// A trigger volume that kills the player (pit, laser, etc.).
/// Add a Collider2D set to IsTrigger = true on the same GameObject.
/// </summary>
public class KillZone : MonoBehaviour
{
    [Tooltip("If not set, the script will search the scene for a RespawnManager.")]
    public RespawnManager respawnManager;

    private void Awake()
    {
        if (respawnManager == null)
            respawnManager = FindObjectOfType<RespawnManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Prefer break effect if available (more 'Thomas Was Alone' vibe).
        var breakFx = other.GetComponent<BreakIntoPiecesOnDeath>();
        if (breakFx != null)
        {
            breakFx.TriggerBreakIntoPieces();
            return;
        }

        other.gameObject.SetActive(false);

        if (respawnManager != null)
            respawnManager.RespawnPlayer();
    }
}
