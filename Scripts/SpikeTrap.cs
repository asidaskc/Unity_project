using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Tooltip("If not set, the script will search the scene for a RespawnManager.")]
    public RespawnManager respawnManager;

    private void Awake()
    {
        if (respawnManager == null)
            respawnManager = FindObjectOfType<RespawnManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // If the player has the break-into-pieces component, let it handle death + respawn.
        var breakFx = collision.gameObject.GetComponent<BreakIntoPiecesOnDeath>();
        if (breakFx != null)
        {
            breakFx.TriggerBreakIntoPieces();
            return;
        }

        // Otherwise: simple disable + respawn.
        collision.gameObject.SetActive(false);

        if (respawnManager != null)
            respawnManager.RespawnPlayer();
    }
}
