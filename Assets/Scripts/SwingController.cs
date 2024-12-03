using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwingController : MonoBehaviour
{
    [Header("Gameplay Components")]
    public GolfBallController golfBall; // Reference to the golf ball
    public GameObject target;           // The target marker

    [Header("UI Components")]
    public Slider powerGauge;           // Power gauge slider
    public Slider accuracyGauge;        // Accuracy gauge slider
    public RectTransform targetLine;    // Target line for aiming
    public TextMeshProUGUI perfectShotText;
    public TextMeshProUGUI clubNameText;

    [Header("Target Configuration")]
    public float targetMoveSpeed = 10f;

    [Header("Gameplay Settings")]
    public float gaugeSpeed = 2f;
    public float perfectAccuracyRange = 0.1f;
    public float perfectShotTextDuration = 2f;

    [Header("Club Configuration")]
    public GolfClub[] clubs; // Array of available clubs
    private int selectedClubIndex = 0;
    private GolfClub currentClub;

    [Header("Audio and Animation")]
    public GolfHitSoundManager hitSoundManager; // Reference to the sound manager
    public Animator playerAnimator;            // Reference to the player's Animator
    public AudioClip perfectHitSound;          // Sound for a perfect hit

    private enum Phase { Idle, Power, Accuracy, Swinging }
    private Phase currentPhase = Phase.Idle;

    private float power = 0f;
    private float accuracy = 0f;
    private float currentTargetDistance;

    void Start()
    {
        currentClub = clubs[selectedClubIndex];
        currentTargetDistance = Mathf.Clamp(currentClub.minTargetDistance, currentClub.minTargetDistance, currentClub.maxTargetDistance);

        if (target != null) target.SetActive(false); // Start with target hidden
        if (powerGauge != null) powerGauge.gameObject.SetActive(false);
        if (accuracyGauge != null) accuracyGauge.gameObject.SetActive(false);

        UpdateTargetLine();
        UpdateClubUI();

        if (playerAnimator != null)
        {
            playerAnimator.Play("Idle");
            Debug.Log("Animation Triggered: Idle");
        }

        Debug.Log("SwingController Initialized.");
    }

    void Update()
    {
        HandleTargetMovement();

        if (Input.GetKeyDown(KeyCode.Q)) ChangeClub(-1);
        if (Input.GetKeyDown(KeyCode.E)) ChangeClub(1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentPhase == Phase.Power)
            {
                LockPowerAndStartAccuracy();
            }
            else if (currentPhase == Phase.Accuracy)
            {
                LockAccuracyAndShoot();
            }
        }

        if (currentPhase == Phase.Power)
        {
            power = Mathf.PingPong(Time.time * gaugeSpeed, 1f);
            powerGauge.value = power;
            UpdateTargetLine();
        }

        if (currentPhase == Phase.Accuracy)
        {
            accuracy = Mathf.PingPong(Time.time * gaugeSpeed, 1f);
            accuracyGauge.value = accuracy;
        }

        if (currentPhase == Phase.Swinging && BallHasStopped()) ResetAfterShot();
    }

    private void HandleTargetMovement()
    {
        if (target == null) return;

        bool moved = false;

        if (Input.GetKey(KeyCode.W))
        {
            currentTargetDistance += targetMoveSpeed * Time.deltaTime;
            currentTargetDistance = Mathf.Clamp(currentTargetDistance, currentClub.minTargetDistance, currentClub.maxTargetDistance);
            moved = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            currentTargetDistance -= targetMoveSpeed * Time.deltaTime;
            currentTargetDistance = Mathf.Clamp(currentTargetDistance, currentClub.minTargetDistance, currentClub.maxTargetDistance);
            moved = true;
        }

        if (moved)
        {
            if (currentPhase == Phase.Idle)
            {
                StartPowerPhase();
            }

            if (target != null && !target.activeSelf)
            {
                target.SetActive(true);
                Debug.Log("Target Activated.");
            }

            UpdateTargetPosition();
            UpdateTargetLine();
        }
    }

    private void StartPowerPhase()
    {
        Debug.Log("Starting Power Phase.");

        currentPhase = Phase.Power;

        if (powerGauge != null)
        {
            powerGauge.gameObject.SetActive(true);
            power = 0f;
            powerGauge.value = 0f;
        }

        if (accuracyGauge != null) accuracyGauge.gameObject.SetActive(false);
    }

    private void LockPowerAndStartAccuracy()
    {
        Debug.Log("Locking Power and Starting Accuracy Phase.");

        currentPhase = Phase.Accuracy;

        if (powerGauge != null) powerGauge.gameObject.SetActive(false);
        if (accuracyGauge != null)
        {
            accuracyGauge.gameObject.SetActive(true);
            accuracy = 0f;
            accuracyGauge.value = 0f;
        }
    }

    private void LockAccuracyAndShoot()
    {
        Debug.Log("Locking Accuracy and Starting Swing.");

        currentPhase = Phase.Swinging;

        if (accuracyGauge != null) accuracyGauge.gameObject.SetActive(false);

        if (Mathf.Abs(accuracy - 0.5f) <= perfectAccuracyRange)
        {
            ShowPerfectShotUI();
            PlayPerfectHitSound();
        }

        if (currentClub.isPutter)
        {
            playerAnimator.SetTrigger("Putt");
            Debug.Log("Animation Triggered: Putt");
        }
        else if (currentClub.clubName.Contains("Wedge"))
        {
            playerAnimator.SetTrigger("Chip");
            Debug.Log("Animation Triggered: Chip");
        }
        else
        {
            playerAnimator.SetTrigger("Drive");
            Debug.Log("Animation Triggered: Drive");
        }
    }

    public void OnSwingComplete()
    {
        Debug.Log("Swing Animation Complete.");

        float forceMultiplier = Mathf.Lerp(currentClub.minForceMultiplier, currentClub.maxForceMultiplier, power);
        Vector3 direction = Camera.main?.transform.forward ?? Vector3.forward;

        if (!currentClub.isPutter)
        {
            direction += Vector3.up * Mathf.Tan(currentClub.loftAngle * Mathf.Deg2Rad);
        }
        direction.Normalize();

        if (golfBall != null)
        {
            golfBall.HitBall(direction, forceMultiplier);
        }
        else
        {
            Debug.LogError("GolfBall reference is missing!");
        }

        if (hitSoundManager != null && currentClub.hitSounds != null)
        {
            hitSoundManager.PlayRandomHitSound(currentClub.hitSounds);
        }
        else
        {
            Debug.LogWarning("Hit sound manager or club hit sounds are missing!");
        }

        ResetAfterShot();
    }

    private void ResetAfterShot()
    {
        Debug.Log("Resetting After Shot.");
        currentPhase = Phase.Idle;

        if (target != null) target.SetActive(false);
    }

    private void UpdateTargetPosition()
    {
        if (target == null || golfBall == null || Camera.main == null) return;

        Vector3 ballPosition = golfBall.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 targetPosition = ballPosition + cameraForward * currentTargetDistance;

        if (Physics.Raycast(targetPosition + Vector3.up * 50f, Vector3.down, out RaycastHit hit, 100f))
        {
            target.transform.position = hit.point + Vector3.up * 0.5f;
        }
        else
        {
            target.transform.position = new Vector3(targetPosition.x, 0f, targetPosition.z);
        }
    }

    private void UpdateTargetLine()
    {
        if (targetLine == null) return;

        float normalizedDistance = (currentTargetDistance - currentClub.minTargetDistance) / (currentClub.maxTargetDistance - currentClub.minTargetDistance);
        targetLine.anchorMin = new Vector2(normalizedDistance, targetLine.anchorMin.y);
        targetLine.anchorMax = new Vector2(normalizedDistance, targetLine.anchorMax.y);
    }

    private void ChangeClub(int direction)
    {
        selectedClubIndex = (selectedClubIndex + direction + clubs.Length) % clubs.Length;
        currentClub = clubs[selectedClubIndex];
        UpdateClubUI();
    }

    private void UpdateClubUI()
    {
        if (clubNameText != null) clubNameText.text = $"Club: {currentClub.clubName}";
    }

    private bool BallHasStopped()
    {
        if (golfBall != null && golfBall.GetComponent<Rigidbody>() != null)
        {
            Rigidbody ballRigidbody = golfBall.GetComponent<Rigidbody>();
            return ballRigidbody.velocity.sqrMagnitude < 0.01f && ballRigidbody.angularVelocity.sqrMagnitude < 0.01f;
        }
        return false;
    }

    private void ShowPerfectShotUI()
    {
        if (perfectShotText != null)
        {
            perfectShotText.gameObject.SetActive(true);
            Invoke(nameof(HidePerfectShotUI), perfectShotTextDuration);
        }
    }

    private void HidePerfectShotUI()
    {
        if (perfectShotText != null) perfectShotText.gameObject.SetActive(false);
    }

    private void PlayPerfectHitSound()
    {
        if (hitSoundManager != null && perfectHitSound != null)
        {
            hitSoundManager.GetComponent<AudioSource>().PlayOneShot(perfectHitSound);
        }
    }
}
