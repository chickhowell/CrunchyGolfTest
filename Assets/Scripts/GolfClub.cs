using UnityEngine;

[CreateAssetMenu(fileName = "New Golf Club", menuName = "Golf/Club")]
public class GolfClub : ScriptableObject
{
    public string clubName;                // Name of the club (e.g., Driver, Iron)
    public float minForceMultiplier = 50f; // Minimum force multiplier
    public float maxForceMultiplier = 150f; // Maximum force multiplier
    public float loftAngle = 0f;           // Loft angle for vertical shots
    public float accuracyModifier = 1f;    // Adjusts accuracy range
    public float minTargetDistance = 5f;   // Minimum target distance
    public float maxTargetDistance = 50f;  // Maximum target distance
    public bool isPutter = false;          // Indicates if this club is a putter

    [Header("Audio Settings")]
    public AudioClip[] hitSounds;          // Sounds specific to this club
}
