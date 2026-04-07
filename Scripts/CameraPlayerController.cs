using UnityEngine;
using System.Collections;

public class CameraPlayerController : MonoBehaviour
{
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

        // Make the Rigidbody2D not affected by external forces while still detecting collisions
        rb.isKinematic = false; // Set to true if you want to manage all movements manually
    }

    void Update()
    {
        Move();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
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
    // Check if there are any contacts before accessing them
    if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5)
    {
        isGrounded = false;
    }
}

}
