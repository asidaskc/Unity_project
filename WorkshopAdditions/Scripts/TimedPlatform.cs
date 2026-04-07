using UnityEngine;
using System.Collections;

/// <summary>
/// Platform that toggles collider + renderer on a timer.
/// Put this on a GameObject with a Collider2D and a SpriteRenderer.
/// </summary>
public class TimedPlatform : MonoBehaviour
{
    public float onTime = 2f;
    public float offTime = 2f;

    private Collider2D col;
    private SpriteRenderer sr;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(Cycle());
    }

    private IEnumerator Cycle()
    {
        while (true)
        {
            SetEnabled(true);
            yield return new WaitForSeconds(onTime);
            SetEnabled(false);
            yield return new WaitForSeconds(offTime);
        }
    }

    private void SetEnabled(bool enabled)
    {
        if (col != null) col.enabled = enabled;
        if (sr != null) sr.enabled = enabled;
    }
}
