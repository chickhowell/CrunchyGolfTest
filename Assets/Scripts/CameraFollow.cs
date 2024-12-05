using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target and Hole References")]
    public Transform target; // Reference to the GolfBall
    public Transform hole;   // Reference to the final Hole

    [Header("Camera Settings")]
    public float startDistance = 8f;         // Distance behind the ball
    public float height = 3f;                // Height above the ball
    public float rotationSpeed = 50f;        // Speed of camera rotation
    public float followSmoothSpeed = 0.125f; // Smooth follow speed for the ball

    private bool isLocked = true;            // Is the camera locked for aiming?
    private float currentAngle = 0f;         // Current rotation angle of the camera

    void Start()
    {
        Debug.Log("Starting CameraFollow...");

        // Dynamically assign the GolfBall as target if not assigned
        if (target == null)
        {
            GameObject golfBall = GameObject.Find("GolfBall");
            if (golfBall != null)
            {
                target = golfBall.transform;
                Debug.Log("Target dynamically assigned to GolfBall.");
            }
            else
            {
                Debug.LogError("GolfBall not found in the scene! Camera will not follow the ball.");
            }
        }

        // Warn if the hole reference is missing
        if (hole == null)
        {
            Debug.LogWarning("Hole reference is missing! Camera alignment to the hole may not work.");
        }

        AlignCameraToHole();
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (isLocked)
        {
            HandleCameraRotation();
        }
        else
        {
            FollowBall();
        }
    }

    private void AlignCameraToHole()
    {
        if (target == null)
        {
            Debug.LogError("Target is missing! Camera alignment skipped.");
            return;
        }

        if (hole == null)
        {
            Debug.LogWarning("Hole is missing! Camera will align to the ball only.");
        }

        // Calculate direction to the hole (or default forward if missing)
        Vector3 directionToHole = (hole != null) ? (hole.position - target.position).normalized : Vector3.forward;

        // Set the camera's position relative to the ball and hole
        Vector3 cameraPosition = target.position - directionToHole * startDistance;
        cameraPosition.y += height;

        transform.position = cameraPosition;
        transform.LookAt(target.position);

        currentAngle = Quaternion.LookRotation(directionToHole, Vector3.up).eulerAngles.y;

        Debug.Log($"Camera aligned to target. Position: {transform.position}, Rotation: {transform.rotation.eulerAngles}");
    }

    private void HandleCameraRotation()
    {
        // Rotate the camera based on player input
        float horizontalInput = Input.GetAxis("Horizontal");
        currentAngle += horizontalInput * rotationSpeed * Time.deltaTime;

        Quaternion rotation = Quaternion.Euler(0f, currentAngle, 0f);
        Vector3 offset = rotation * Vector3.back * startDistance + Vector3.up * height;

        transform.position = target.position + offset;
        transform.LookAt(target.position);
    }

    private void FollowBall()
    {
        // Smoothly follow the ball after it is hit
        Vector3 followPosition = target.position - target.forward * startDistance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, followPosition, followSmoothSpeed * Time.deltaTime);
        transform.LookAt(target.position);
    }

    public void AlignToTarget()
    {
        if (target == null)
        {
            Debug.LogError("Target is missing! Cannot align camera.");
            return;
        }

        Vector3 desiredPosition = target.position - Vector3.forward * startDistance + Vector3.up * height;
        transform.position = desiredPosition;
        transform.LookAt(target.position);

        Debug.Log($"Camera aligned to target. Position: {transform.position}, Rotation: {transform.rotation.eulerAngles}");
    }

    public Vector3 GetShotDirection()
    {
        return transform.forward.normalized;
    }

    public void UnlockCamera()
    {
        isLocked = false;
        Debug.Log("Camera unlocked.");
    }

    public void LockCamera()
    {
        isLocked = true;
        Debug.Log("Camera locked.");
    }
}
