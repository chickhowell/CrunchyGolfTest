using UnityEngine;

public class GolfHitSoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on this GameObject!");
        }
    }

    public void PlayRandomHitSound(AudioClip[] hitSounds)
    {
        if (hitSounds != null && hitSounds.Length > 0)
        {
            // Select a random sound
            int randomIndex = Random.Range(0, hitSounds.Length);
            AudioClip randomSound = hitSounds[randomIndex];

            // Play the sound
            audioSource.PlayOneShot(randomSound);
        }
        else
        {
            Debug.LogWarning("No hit sounds provided to PlayRandomHitSound!");
        }
    }
}
