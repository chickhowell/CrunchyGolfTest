using UnityEngine;

public class RaycastTest : MonoBehaviour
{
    public Transform ball; // Assign the ball in the Inspector

    void Update()
    {
        // Ensure the ball reference exists
        if (ball == null)
        {
            Debug.LogWarning("Ball is not assigned in the Inspector.");
            return;
        }

        Vector3 position = ball.position;

        // Perform a raycast downward from the ball's position
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 10f))
        {
            Debug.Log($"Hit Ground at: {hit.point}");
        }
        else
        {
            // Visualize the ray in the Scene View
            Debug.DrawRay(position, Vector3.down * 10f, Color.red);
            Debug.Log($"No Ground Hit from position: {position}");
        }
    }
}
