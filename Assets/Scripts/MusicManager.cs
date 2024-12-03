using UnityEngine;
using System.Collections; // Required for IEnumerator

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;        // Assign the AudioSource in the Inspector
    public float fadeDuration = 1.0f;     // Duration for fade-in/out effects

    private static MusicManager instance; // Singleton instance to ensure only one MusicManager exists

    void Awake()
    {
        // Ensure only one instance of MusicManager persists across scenes
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate MusicManager
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // Start playing music
        }
        else
        {
            Debug.LogError("No AudioSource assigned to MusicManager!");
        }
    }

    public void ToggleMusic()
    {
        if (audioSource.isPlaying)
        {
            StartCoroutine(FadeOut()); // Smoothly fade out and pause
        }
        else
        {
            StartCoroutine(FadeIn()); // Smoothly fade in and play
        }
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOut(true)); // Smoothly fade out and stop
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            StartCoroutine(FadeIn()); // Smoothly fade in and play
        }
    }

    private IEnumerator FadeOut(bool stopCompletely = false)
    {
        if (audioSource != null)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
                yield return null;
            }

            audioSource.Pause(); // Pause the music when volume reaches zero
            if (stopCompletely)
            {
                audioSource.Stop(); // Stop the music if specified
            }
            audioSource.volume = startVolume; // Reset volume for the next play
        }
    }

    private IEnumerator FadeIn()
    {
        if (audioSource != null)
        {
            float targetVolume = audioSource.volume;
            audioSource.volume = 0;
            audioSource.Play(); // Start playing the music

            while (audioSource.volume < targetVolume)
            {
                audioSource.volume += targetVolume * Time.deltaTime / fadeDuration;
                yield return null;
            }

            audioSource.volume = targetVolume; // Ensure volume reaches the target
        }
    }
}
