using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // Golf ball reference
    public Transform hole;              // Hole reference
    public float startDistance = 8f;    // Distance behind the ball
    public float height = 3f;           // Height above the ball
    public float rotationSpeed = 50f;   // Speed of camera rotation
    public float followSmoothSpeed = 0.125f; // Smooth follow speed after the ball is hit
    private bool isLocked = true;       // Is the camera locked for aiming?
    private float currentAngle = 0f;    // Current rotation angle of the camera

    void Start()
    {
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
        if (target == null || hole == null)
        {
            Debug.LogError("Target or Hole reference is missing!");
            return;
        }

        // Calculate direction from ball to hole
        Vector3 directionToHole = (hole.position - target.position).normalized;

        // Set the camera's position relative to the ball and hole
        Vector3 cameraPosition = target.position - directionToHole * startDistance;
        cameraPosition.y += height;

        transform.position = cameraPosition;

        // Ensure the camera looks at the ball
        transform.LookAt(target.position);

        // Align the initial rotation angle to match the hole
        currentAngle = Quaternion.LookRotation(directionToHole, Vector3.up).eulerAngles.y;

        Debug.Log($"Camera aligned to hole. Position: {transform.position}, Rotation: {transform.rotation.eulerAngles}");
    }

    private void HandleCameraRotation()
    {
        // Rotate the camera based on player input
        float horizontalInput = Input.GetAxis("Horizontal");
        currentAngle += horizontalInput * rotationSpeed * Time.deltaTime;

        // Calculate the camera's position
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

    public Vector3 GetShotDirection()
    {
        // Return the camera's forward vector directly
        Vector3 direction = transform.forward.normalized;
        Debug.Log($"Shot Direction (Camera): {direction}");
        return direction;
    }

    public void UnlockCamera()
    {
        isLocked = false;
    }

    public void LockCamera()
    {
        isLocked = true;
    }
}
