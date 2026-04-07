using UnityEngine;

/// <summary>
/// A simple bounce pad. Put this on a GameObject with a Collider2D (not trigger).
/// </summary>
public class BouncePad : MonoBehaviour
{
    public float bounceForce = 16f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        var rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Reset vertical velocity so bounces are consistent.
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound("Jump");
    }
}
