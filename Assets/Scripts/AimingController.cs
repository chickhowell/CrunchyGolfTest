using UnityEngine;

public class AimingController : MonoBehaviour
{
    public RectTransform arrowReticle; // The arrow in the UI (set as a RectTransform)
    public Transform golfBall;        // Reference to the golf ball
    public Transform greenTarget;     // Reference to the green or hole target
    public float rotationSpeed = 100f; // Speed of the aiming arrow's rotation
    public float maxAngle = 45f;      // Maximum angle the arrow can rotate left or right

    private float currentAngle = 0f;  // Tracks the current rotation angle of the arrow

    void Update()
    {
        if (golfBall == null || arrowReticle == null) return;

        // Get user input for arrow rotation
        float horizontalInput = Input.GetAxis("Horizontal"); // Arrow keys or joystick
        currentAngle += horizontalInput * rotationSpeed * Time.deltaTime;
        currentAngle = Mathf.Clamp(currentAngle, -maxAngle, maxAngle);

        // Rotate the arrow reticle in the UI
        arrowReticle.localRotation = Quaternion.Euler(0f, 0f, -currentAngle);
    }

    public Vector3 GetAimDirection()
    {
        // Calculate the aiming direction based on the arrow's angle
        Quaternion rotation = Quaternion.Euler(0f, currentAngle, 0f);
        return rotation * (greenTarget.position - golfBall.position).normalized;
    }

    public void ResetArrow()
    {
        // Reset the arrow position for a new shot
        currentAngle = 0f;
        if (arrowReticle != null)
        {
            arrowReticle.localRotation = Quaternion.identity;
        }
    }

    public void ShowReticle()
    {
        if (arrowReticle != null)
        {
            arrowReticle.gameObject.SetActive(true);
        }
    }

    public void HideReticle()
    {
        if (arrowReticle != null)
        {
            arrowReticle.gameObject.SetActive(false);
        }
    }
}
