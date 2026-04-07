using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public RespawnManager respawnManager;  // Add this line to reference RespawnManager
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded;
    private Rigidbody2D rb;
    private Vector3 originalScale;
    private bool isSquishing;

    // Squish settings
    public float squishScaleX = 0.8f;  // Scale for squish on the X-axis
    public float squishScaleY = 0.7f;  // Scale for squish on the Y-axis
    public float squishDuration = 0.1f; // Duration of the squish animation

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;

        // Debug check to ensure Rigidbody2D is enabled and set up
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from the player!");
        }
        else if (!rb.isKinematic)
        {
            Debug.Log("Rigidbody2D is enabled and ready for physics interactions.");
        }

        // Ensure the player starts with an active Rigidbody2D
        rb.isKinematic = false;  // Ensure Rigidbody2D is not kinematic
    }

    void Update()
    {
        Move();

        // Jump only if grounded and the Jump button is pressed
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
            SoundManager.Instance.PlaySound("Jump"); // Ensure this matches your sound name
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        if (!isSquishing)
        {
            StartCoroutine(SquishAnimation());
        }
    }

    IEnumerator SquishAnimation()
    {
        isSquishing = true;

        // Squish down smoothly
        float elapsedTime = 0f;
        while (elapsedTime < squishDuration)
        {
            transform.localScale = new Vector3(
                Mathf.Lerp(originalScale.x, originalScale.x * squishScaleX, elapsedTime / squishDuration),
                Mathf.Lerp(originalScale.y, originalScale.y * squishScaleY, elapsedTime / squishDuration),
                originalScale.z
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return to original scale smoothly
        elapsedTime = 0f;
        while (elapsedTime < squishDuration)
        {
            transform.localScale = new Vector3(
                Mathf.Lerp(originalScale.x * squishScaleX, originalScale.x, elapsedTime / squishDuration),
                Mathf.Lerp(originalScale.y * squishScaleY, originalScale.y, elapsedTime / squishDuration),
                originalScale.z
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        isSquishing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is touching the ground
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player has left the ground
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5)
        {
            isGrounded = false;
        }
    }
}
