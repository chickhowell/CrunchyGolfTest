using UnityEngine;

public class CameraAlignmentController : MonoBehaviour
{
    [Header("Camera Configuration")]
    public Transform cameraTransform;  // The main camera's transform
    public Transform golfBall;         // The golf ball in the scene
    public Transform holeObject;       // The hole the camera should align with

    [Header("Camera Settings")]
    public float cameraDistance = 5f;  // Distance from the ball to position the camera
    public float cameraHeight = 2f;    // Height offset of the camera

    void Start()
    {
        AlignCameraCenterToHole();
    }

    private void AlignCameraCenterToHole()
    {
        if (cameraTransform == null || golfBall == null || holeObject == null)
        {
            Debug.LogError("Required references are missing!");
            return;
        }

        // Calculate the position behind the ball relative to the hole
        Vector3 directionToHole = (holeObject.position - golfBall.position).normalized;
        Vector3 cameraPosition = golfBall.position - directionToHole * cameraDistance;
        cameraPosition.y += cameraHeight; // Add height offset

        // Set camera position
        cameraTransform.position = cameraPosition;

        // Rotate the camera to look directly at the hole
        cameraTransform.LookAt(holeObject.position);

        Debug.Log($"Camera aligned. Position: {cameraTransform.position}, Facing: {holeObject.position}");
    }
}
