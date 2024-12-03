using UnityEngine;
using TMPro;
using System;

public class GolfBallController : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public int strokeCount = 0;

    [Header("UI Components")]
    public TextMeshProUGUI strokeCounterText;

    public Action OnBallStopped; // Event triggered when the ball stops

    private Rigidbody rb;
    private bool hasStopped = false;
    private bool isFirstStop = true; // Tracks the first stop at the start of the scene

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateStrokeCounterUI();
    }

    public void HitBall(Vector3 direction, float power)
    {
        strokeCount++;
        UpdateStrokeCounterUI();

        if (rb != null)
        {
            Debug.Log($"Applying Force: Direction = {direction}, Power = {power}");
            rb.velocity = Vector3.zero; // Reset velocity
            rb.angularVelocity = Vector3.zero; // Reset angular velocity
            rb.AddForce(direction * power, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody reference is missing on GolfBall!");
        }

        hasStopped = false;
        isFirstStop = false; // Disable the first stop logic after the first hit
    }

    void Update()
    {
        if (!hasStopped)
        {
            if (rb.velocity.magnitude < 0.05f && rb.angularVelocity.magnitude < 0.05f)
            {
                hasStopped = true;

                if (!isFirstStop) // Ignore the first stop
                {
                    Debug.Log("Ball has stopped moving.");
                    OnBallStopped?.Invoke();
                }
            }
        }
        else
        {
            // Reset stop detection if the ball starts moving again
            if (rb.velocity.magnitude > 0.1f || rb.angularVelocity.magnitude > 0.1f)
            {
                hasStopped = false;
            }
        }
    }

    private void UpdateStrokeCounterUI()
    {
        if (strokeCounterText != null)
        {
            strokeCounterText.text = $"Strokes: {strokeCount}";
        }
    }
}
