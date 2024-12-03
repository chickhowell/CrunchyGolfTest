using UnityEngine;

public class PlayerAlignmentController : MonoBehaviour
{
    [Header("Target Objects")]
    public GolfBallController golfBallController; // GolfBallController to subscribe to
    public Transform golfBall;                    // The actual golf ball in the scene

    [Header("Character Offsets")]
    public Vector3 positionOffset;     // Additive offset relative to ball position
    public float yRotationOffset;      // Additive rotation relative to camera orientation

    private bool isTeleporting = false;           // Prevent conflicts during teleportation

    void Start()
    {
        AlignToBall(); // Align player to the ball initially

        if (golfBallController != null)
        {
            golfBallController.OnBallStopped += TeleportToBall;
            Debug.Log("Subscribed to OnBallStopped event.");
        }
        else
        {
            Debug.LogError("GolfBallController reference is missing!");
        }
    }

    void Update()
    {
        if (!isTeleporting)
        {
            float angleDelta = Camera.main.transform.eulerAngles.y - transform.eulerAngles.y;
            RotateAroundBall(angleDelta + yRotationOffset);
        }
    }

    private void RotateAroundBall(float angle)
    {
        if (golfBall == null) return;
    
        // Rotate around the ball's position
        transform.RotateAround(golfBall.position, Vector3.up, angle);
    
        // Reapply rotation adjustments
        // AlignRotation();
        Debug.Log($"Rotating Banana Man around ball. Angle: {angle}");
    }

    private void AlignToBall()
    {
        if (golfBall == null) return;

        // Calculate the position relative to the ball + offsets
		Vector3 adjustedPosition = golfBall.position + positionOffset;

        // Apply manual position offsets
        // adjustedPosition += new Vector3(positionOffset.x, 0, positionOffset.z);

        // Adjust position to the ground
        adjustedPosition = AdjustHeightToGround(adjustedPosition);

        // Set Banana Man's position
        transform.position = adjustedPosition;

        // Fix rotation to face the ball and apply manual rotation offset
        AlignRotation();
        Debug.Log($"Banana Man aligned to ball. Position: {transform.position}");

    }

    private void AlignRotation()
    {
		if (golfBall == null) return;

        // Calculate the direction to the ball
        Vector3 directionToBall = (golfBall.position - transform.position).normalized;
        directionToBall.y = 0; // Ignore vertical alignment

        // Create a rotation to face the ball
        Quaternion targetRotation = Quaternion.LookRotation(directionToBall, Vector3.up);

        // Apply the rotation to Banana Man
        transform.rotation = targetRotation;

        Debug.Log($"Banana Man rotation aligned. Rotation: {transform.rotation.eulerAngles}");
    }
    
    private void TeleportToBall()
    {
        Debug.Log("Teleporting Banana Man...");
        isTeleporting = true; // Prevent conflicts during teleportation

        AlignToBall();

        isTeleporting = false; // Allow updates after teleportation
        Debug.Log($"Banana Man teleported to the ball at position: {transform.position}");
    }

    private Vector3 AdjustHeightToGround(Vector3 position)
    {
        // Cast a ray downward to find the ground
        Ray ray = new Ray(position + Vector3.up * 5f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 10f))
        {
            position.y = hitInfo.point.y; // Adjust Y-coordinate to the ground
            Debug.Log($"Ground detected at height: {hitInfo.point.y}");
        }
        else
        {
            Debug.LogWarning("No ground detected under Banana Man! Defaulting to original position.");
        }

        return position;
    }

    void OnDestroy()
    {
        if (golfBallController != null)
        {
            golfBallController.OnBallStopped -= TeleportToBall;
        }
    }
}
