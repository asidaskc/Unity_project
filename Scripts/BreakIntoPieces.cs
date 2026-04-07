using UnityEngine;
using System.Collections;

public class BreakIntoPiecesOnDeath : MonoBehaviour
{
    public GameObject breakEffectPrefab;  // The new break effect prefab (assign this in the inspector)
    public float forceMagnitude = 5f;  // Magnitude of the force applied to the pieces (if you're using a Rigidbody2D in the break effect)
    public float respawnDelay = 2f;  // Delay before respawning the player
    public RespawnManager respawnManager; // Reference to the RespawnManager

    private Vector3 deathPosition;  // Store the death position for spawning the break effect

    void Start()
    {
        if (respawnManager == null)
        {
            respawnManager = FindObjectOfType<RespawnManager>();
        }
    }

    // This function can be called to trigger the "break into pieces" effect
    public void TriggerBreakIntoPieces()
    {
        // Adjust the death position to be slightly lower to match actual player death
        deathPosition = transform.position + new Vector3(2.0f, -3.5f, 0);

        // Spawn the break effect at the adjusted death position
        SpawnBreakEffect();
        SoundManager.Instance.PlaySound("Death"); // Ensure this matches your sound name


        // Destroy the player object
        Destroy(gameObject);

        // Ask the RespawnManager to respawn after the configured delay
        if (respawnManager != null)
        {
            respawnManager.RespawnPlayer(respawnDelay);
        }
        else
        {
            Debug.LogError("RespawnManager not found!");
        }
    }

    private void SpawnBreakEffect()
    {
        // Instantiate the break effect prefab at the adjusted death position
        if (breakEffectPrefab != null)
        {
            GameObject breakEffect = Instantiate(breakEffectPrefab, deathPosition, Quaternion.identity);

            // Apply force to each piece if they have Rigidbody2D
            Rigidbody2D[] pieces = breakEffect.GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D piece in pieces)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                piece.AddForce(randomDirection * forceMagnitude, ForceMode2D.Impulse);
            }

            // Destroy the break effect after 1.5 seconds
            Destroy(breakEffect, 1.5f);
        }
        else
        {
            Debug.LogError("BreakEffectPrefab is not assigned.");
        }
    }

    // This is an example of how to detect player death on collision (e.g., with a spike trap)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If player collides with a spiketrap
        if (collision.gameObject.CompareTag("SpikeTrap"))
        {
            TriggerBreakIntoPieces();
        }
    }
}