using UnityEngine;

public class PlayerRotationAroundBall : MonoBehaviour
{
    [Header("References")]
    public Transform ballTransform; // Assign the ball here in the Inspector
    public float rotationSpeed = 50f; // Speed of rotation

    void Update()
    {
        if (ballTransform == null)
        {
            Debug.LogWarning("Ball Transform is not assigned!");
            return;
        }

        // Check for left/right input
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrows

        if (horizontalInput != 0f)
        {
            // Rotate the player around the ball on the Y-axis
            transform.RotateAround(ballTransform.position, Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);

            // Look at the ball to keep the player facing it
            transform.LookAt(ballTransform.position);
        }
    }
}
