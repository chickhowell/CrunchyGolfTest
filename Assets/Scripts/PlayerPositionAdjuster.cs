using UnityEngine;

public class PlayerPositionAdjuster : MonoBehaviour
{
    [Header("Offset Configuration")]
    public Vector3 positionOffset;      // Manual position offsets (X, Z)
    public float rotationOffset = 0f;  // Manual rotation offset in degrees

    private Transform golfBall;        // Reference to the golf ball

    public void Initialize(Transform ball)
    {
        golfBall = ball;
    }

    public void AdjustPositionAndRotation()
    {
        if (golfBall == null)
        {
            Debug.LogError("Golf Ball reference is missing!");
            return;
        }

        // Calculate position behind the ball
        Vector3 directionToBall = -Camera.main.transform.forward;
        Vector3 adjustedPosition = golfBall.position + directionToBall.normalized * positionOffset.z;

        // Apply position offset
        adjustedPosition += new Vector3(positionOffset.x, 0, 0);

        // Adjust position to the ground
        adjustedPosition = AdjustHeightToGround(adjustedPosition);

        // Set Banana Man's position
        transform.position = adjustedPosition;

        // Align rotation
        AlignRotation();
        Debug.Log($"Position and rotation adjusted. New Position: {transform.position}, New Rotation: {transform.rotation.eulerAngles}");
    }

    public void AlignRotation()
    {
        if (golfBall == null) return;

        // Calculate the direction to the ball
        Vector3 directionToBall = (golfBall.position - transform.position).normalized;
        directionToBall.y = 0; // Ignore vertical alignment

        // Rotate to face the ball and apply rotation offset
        Quaternion targetRotation = Quaternion.LookRotation(directionToBall, Vector3.up);
        targetRotation *= Quaternion.Euler(0, rotationOffset, 0);

        transform.rotation = targetRotation;
    }

    public Vector3 GetCurrentOffset()
    {
        return new Vector3(positionOffset.x, 0, positionOffset.z);
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
}
