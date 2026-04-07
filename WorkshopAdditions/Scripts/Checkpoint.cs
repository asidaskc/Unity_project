using UnityEngine;

/// <summary>
/// Simple checkpoint: when the Player enters this trigger, it updates the RespawnManager's respawnPoint position.
/// Add a Collider2D set to IsTrigger = true on the same GameObject.
/// </summary>
public class Checkpoint : MonoBehaviour
{
    [Tooltip("If not set, the script will search the scene for a RespawnManager.")]
    public RespawnManager respawnManager;

    [Tooltip("Optional sound name to play via SoundManager (leave blank for none).")]
    public string checkpointSoundName = "";

    private void Awake()
    {
        if (respawnManager == null)
            respawnManager = FindObjectOfType<RespawnManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (respawnManager == null || respawnManager.respawnPoint == null) return;

        respawnManager.SetRespawnPosition(transform.position);

        if (!string.IsNullOrEmpty(checkpointSoundName) && SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(checkpointSoundName);
    }
}
