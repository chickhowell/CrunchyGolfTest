using UnityEngine;

public class HoleManager : MonoBehaviour
{
    public int holeIndex; // Set this in the Unity Inspector (1 for Hole1, 2 for Hole2, etc.)
    private int strokeCount = 0;

    private HoleUIManager uiManager;

    private void Start()
    {
        strokeCount = 0;
        uiManager = FindObjectOfType<HoleUIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateUI(GameManager.Instance.GetTotalStrokes(), strokeCount);
        }
    }

    public void IncrementStrokes()
    {
        strokeCount++;
        Debug.Log($"Hole {holeIndex}: Strokes incremented to {strokeCount}");
        if (uiManager != null)
        {
            uiManager.UpdateUI(GameManager.Instance.GetTotalStrokes(), strokeCount);
        }
    }

    public void CompleteHole()
    {
        Debug.Log($"Hole {holeIndex} completed with {strokeCount} strokes");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveStrokes(holeIndex - 1, strokeCount);
        }
        GameManager.Instance.LoadNextHole();
    }
}
