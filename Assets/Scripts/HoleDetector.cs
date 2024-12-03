using UnityEngine;
using System.Collections;

public class HoleDetector : MonoBehaviour
{
    public TMPro.TextMeshProUGUI ballInHoleText; // Reference to the "Ball In Hole" UI Text

    private void OnTriggerEnter(Collider other)
    {
        // Check if the golf ball entered the hole
        if (other.CompareTag("GolfBall"))
        {
            Debug.Log("Ball is in the hole!");

            // Show the "Ball In Hole" UI Text
            if (ballInHoleText != null)
            {
                ballInHoleText.gameObject.SetActive(true);
            }

            // Stop the ball
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true; // Lock the ball in place
            }

            // Call CompleteHole() from HoleManager after a short delay
            StartCoroutine(CompleteHoleAfterDelay(2f));
        }
    }

    private IEnumerator CompleteHoleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Find the HoleManager and call CompleteHole()
        HoleManager holeManager = FindObjectOfType<HoleManager>();
        if (holeManager != null)
        {
            holeManager.CompleteHole();
        }
        else
        {
            Debug.LogError("HoleManager not found in the scene!");
        }
    }
}
