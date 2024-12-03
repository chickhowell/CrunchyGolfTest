using UnityEngine;

public class BallStopper : MonoBehaviour
{
    public float stopThreshold = 0.005f; // Minimum velocity magnitude to stop the ball
    public float stopAngularThreshold = 0.01f; // Minimum angular velocity magnitude to stop rotation

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to this GameObject
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Check if linear and angular velocities are below thresholds
        if (rb.velocity.sqrMagnitude < stopThreshold * stopThreshold && rb.angularVelocity.sqrMagnitude < stopAngularThreshold * stopAngularThreshold)
        {
            // If velocity is very low but still visible, let it continue rolling slightly
            if (rb.velocity.magnitude > 0.001f) return;

            // Stop the ball completely
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Put the Rigidbody to sleep to save performance
            rb.Sleep();
        }
    }
}
