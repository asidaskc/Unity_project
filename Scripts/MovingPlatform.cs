using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Movement speed of the platform
    public float speed = 2f;

    // Distance the platform will move from its starting position
    public float moveDistance = 5f;

    // Store the starting position of the platform
    private Vector3 startPosition;

    // Update is called once per frame
    void Start()
    {
        startPosition = transform.position; // Initialize the starting position
    }

    void Update()
    {
        // Move the platform back and forth based on the direction
        transform.position = new Vector3(startPosition.x + Mathf.PingPong(Time.time * speed, moveDistance), transform.position.y, transform.position.z);
    }
}
