using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    [Header("Setup")]
    public GameObject playerPrefab;  // Reference to the player prefab
    public Transform respawnPoint;   // The respawn point in the scene

    [Header("Behaviour")]
    [Tooltip("If true, a disabled player (SetActive(false)) will be reused instead of destroyed + re-instantiated.")]
    public bool reusePlayerIfDisabled = true;

    [Tooltip("Default delay before respawning (seconds).")]
    public float respawnDelay = 2f;

    [Header("Fun (Optional)")]
    [Tooltip("Show a little one-liner after respawn (loaded from Resources).")]
    public bool showRespawnQuips = true;

    [Tooltip("Resources path to a DialogueAsset that contains quips (without the Resources/ prefix).")]
    public string respawnQuipsResourceName = "Dialogue/Respawn_Quips";

    [Range(0f, 1f)]
    public float respawnQuipChance = 0.7f;

    public float respawnQuipDuration = 1.6f;
    public bool respawnQuipPausesGame = false;

    private GameObject currentPlayer;  // Track the current player instance
    private bool isRespawning = false; // Flag to prevent multiple respawns

    private void Awake()
    {
        // If the level already has a Player in the scene, track it so we can respawn it cleanly.
        if (currentPlayer == null)
        {
            var found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
                currentPlayer = found;
        }
    }

    /// <summary>
    /// Optional helper for checkpoints to update the respawn point position.
    /// </summary>
    public void SetRespawnPosition(Vector3 position)
    {
        if (respawnPoint != null)
            respawnPoint.position = position;
    }

    /// <summary>
    /// Respawn using the manager's default delay.
    /// </summary>
    public void RespawnPlayer()
    {
        RespawnPlayer(respawnDelay);
    }

    /// <summary>
    /// Respawn using a specific delay (useful for death effects that control timing).
    /// </summary>
    public void RespawnPlayer(float delay)
    {
        if (isRespawning)
        {
            Debug.LogWarning("Respawn already in progress, ignoring duplicate call.");
            return;
        }

        StartCoroutine(RespawnWithDelay(Mathf.Max(0f, delay)));
    }

    private IEnumerator RespawnWithDelay(float delay)
    {
        isRespawning = true;

        // Try to recover the player reference if it was never set
        if (currentPlayer == null)
        {
            var found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
                currentPlayer = found;
        }

        // Ensure the player isn't controllable while waiting
        if (currentPlayer != null && currentPlayer.activeInHierarchy)
            currentPlayer.SetActive(false);

        yield return new WaitForSeconds(delay);

        if (respawnPoint == null)
        {
            Debug.LogError("Respawn point is not assigned!");
            isRespawning = false;
            yield break;
        }

        // Smooth respawn: re-enable the same player if it was only disabled.
        // (If the player was destroyed, Unity's null check will evaluate to false.)
        if (reusePlayerIfDisabled && currentPlayer != null)
        {
            currentPlayer.transform.position = respawnPoint.position;

            Rigidbody2D rb = currentPlayer.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.WakeUp();
            }

            PlayerController pc = currentPlayer.GetComponent<PlayerController>();
            if (pc != null)
                pc.respawnManager = this;

            currentPlayer.SetActive(true);
        }
        else
        {
            // Hard respawn: destroy + reinstantiate (useful when the player object was destroyed, e.g. break effect)
            if (currentPlayer != null)
            {
                Destroy(currentPlayer.gameObject);
                currentPlayer = null;
                yield return new WaitForSeconds(0.05f);
            }

            if (playerPrefab == null)
            {
                Debug.LogError("Player prefab is not assigned!");
                isRespawning = false;
                yield break;
            }

            currentPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);

            PlayerController pc = currentPlayer.GetComponent<PlayerController>();
            if (pc != null)
                pc.respawnManager = this;

            Rigidbody2D rb = currentPlayer.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.WakeUp();
            }

            currentPlayer.SetActive(true);
        }

        TryShowRespawnQuip();

        isRespawning = false;

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound("Spawn"); // Ensure this matches your sound name
    }

    private void TryShowRespawnQuip()
    {
        if (!showRespawnQuips) return;
        if (string.IsNullOrEmpty(respawnQuipsResourceName)) return;
        if (Random.value > respawnQuipChance) return;

        DialogueAsset asset = Resources.Load<DialogueAsset>(respawnQuipsResourceName);
        if (asset == null || asset.lines == null || asset.lines.Length == 0) return;

        DialogueManager dm = DialogueManager.FindOrCreate();
        if (dm == null) return;

        dm.ShowRandomLine(asset, respawnQuipDuration, respawnQuipPausesGame);
    }
}
