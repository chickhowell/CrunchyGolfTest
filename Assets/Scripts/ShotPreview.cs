using UnityEngine;

public class ShotPreview : MonoBehaviour
{
    [SerializeField] private Transform ball; // Reference to the ball's position
    [SerializeField] private Transform cameraTransform; // Reference to the camera for direction
    [SerializeField] private GameObject targetMarkerPrefab; // Prefab for the target marker
    public float fixedDistance = 10f; // Initial distance for the target marker
    public float heightOffset = 1f; // Height offset above the ground
    public float stopThreshold = 0.01f; // Minimum velocity to consider the ball as stopped

    private GameObject targetMarker; // Single target marker instance
    private Rigidbody ballRigidbody;
    private bool isPreviewActive = true; // Tracks if the preview is active
    private bool shotInProgress = false; // Tracks if a shot is in progress
    private bool ballStoppedAfterShot = false; // Tracks if the ball has stopped after a shot
    private bool markerCreated = false; // Tracks if a marker has been created

    void Start()
    {
        Debug.Log("ShotPreview Start called.");
        ValidatePrefab();
        DestroyExistingMarkers();
        CreateMarker();

        if (ball != null)
        {
            ballRigidbody = ball.GetComponent<Rigidbody>();
            if (ballRigidbody == null)
            {
                Debug.LogError("Ball does not have a Rigidbody component! Ensure the ball has a Rigidbody in the Inspector.");
            }
            else
            {
                Debug.Log("ballRigidbody successfully assigned.");
            }
        }
        else
        {
            Debug.LogError("Ball Transform reference is null! Assign the ball in the Inspector.");
        }
    }

    void Update()
    {
        if (shotInProgress)
        {
            if (!ballStoppedAfterShot && BallHasStopped())
            {
                Debug.Log("Ball has stopped after shot.");
                ballStoppedAfterShot = true;
                ResetPreview();
            }
            return; // Skip marker updates during the shot
        }

        if (isPreviewActive && targetMarker != null)
        {
            UpdateTargetMarker();
        }
    }

    public void OnShotTaken()
    {
        if (shotInProgress)
        {
            Debug.LogWarning("OnShotTaken called while shot already in progress.");
            return;
        }

        Debug.Log("OnShotTaken called: Attempting to hide/destroy target marker.");
        shotInProgress = true; // Mark shot as in progress
        ballStoppedAfterShot = false; // Reset ball stopped state
        isPreviewActive = false; // Disable preview mode

        if (targetMarker != null)
        {
            Destroy(targetMarker);
            Debug.Log("Target marker destroyed on shot.");
            targetMarker = null; // Clear reference
            markerCreated = false; // Allow new marker creation
        }
        else
        {
            Debug.LogWarning("No target marker to destroy during OnShotTaken.");
        }

        DestroyExistingMarkers(); // Extra cleanup to avoid rogue markers
    }

    public void ResetPreview()
    {
        if (!shotInProgress || !ballStoppedAfterShot)
        {
            Debug.LogWarning("ResetPreview called prematurely.");
            return; // Ensure this is only called after the shot is completed
        }

        Debug.Log("ResetPreview called: Preparing to recreate marker.");
        shotInProgress = false; // Mark shot as completed
        isPreviewActive = true; // Enable preview mode

        if (!markerCreated)
        {
            Debug.Log("Creating marker in ResetPreview.");
            CreateMarker();
            markerCreated = true; // Mark creation as done
        }
        else
        {
            Debug.LogWarning("Marker already exists, skipping creation.");
        }
    }

    private void UpdateTargetMarker()
    {
        if (targetMarker == null)
        {
            Debug.LogWarning("UpdateTargetMarker called but targetMarker is null.");
            return;
        }

        Vector3 shotDirection = cameraTransform.forward.normalized;
        Vector3 targetPosition = ball.position + shotDirection * fixedDistance;

        if (Physics.Raycast(targetPosition + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f))
        {
            targetMarker.transform.position = hit.point + Vector3.up * heightOffset;
        }
        else
        {
            targetMarker.transform.position = new Vector3(targetPosition.x, heightOffset, targetPosition.z);
        }
    }

    private bool BallHasStopped()
    {
        if (ballRigidbody == null)
        {
            Debug.LogError("ballRigidbody is null! Ensure the ball reference is set.");
            return false; // Assume the ball is still moving if Rigidbody is missing
        }

        bool hasStopped = ballRigidbody.velocity.sqrMagnitude < stopThreshold * stopThreshold &&
                          ballRigidbody.angularVelocity.sqrMagnitude < stopThreshold * stopThreshold;

        Debug.Log($"Ball velocity: {ballRigidbody.velocity}, angular velocity: {ballRigidbody.angularVelocity}, hasStopped: {hasStopped}");
        return hasStopped;
    }

    private void CreateMarker()
    {
        ValidatePrefab();

        if (targetMarkerPrefab == null)
        {
            Debug.LogError("targetMarkerPrefab is null! Cannot create marker.");
            return;
        }

        DestroyExistingMarkers(); // Clean up duplicates before creating a new marker

        targetMarker = Instantiate(targetMarkerPrefab);
        targetMarker.tag = "LandingTarget"; // Ensure the correct tag is assigned
        markerCreated = true;
        Debug.Log($"Target marker created successfully and tagged as 'LandingTarget': {targetMarker.name}");
    }

    private void DestroyExistingMarkers()
    {
        GameObject[] existingMarkers = GameObject.FindGameObjectsWithTag("LandingTarget");
        if (existingMarkers.Length > 0)
        {
            foreach (GameObject obj in existingMarkers)
            {
                Destroy(obj);
                Debug.Log($"Destroyed rogue marker: {obj.name}");
            }
        }
        else
        {
            Debug.LogWarning("No rogue markers with tag 'LandingTarget' found.");
        }
    }

    private void ValidatePrefab()
    {
        if (targetMarkerPrefab == null)
        {
            Debug.LogError("targetMarkerPrefab is not assigned in the Inspector.");
        }
    }
}
