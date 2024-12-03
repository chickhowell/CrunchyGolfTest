using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentHole = 1; // Starts at hole 1
    public int[] strokes;       // Stores strokes for each hole
    public int totalScore = 0;  // Total strokes across all holes

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (strokes == null || strokes.Length != 18)
        {
            strokes = new int[18];
        }
    }

    public void SaveStrokes(int holeIndex, int strokeCount)
    {
        if (holeIndex >= 0 && holeIndex < strokes.Length)
        {
            strokes[holeIndex] = strokeCount;
            UpdateTotalScore();
        }
    }

    private void UpdateTotalScore()
    {
        totalScore = 0;
        foreach (int stroke in strokes)
        {
            totalScore += stroke;
        }
        Debug.Log($"Total Strokes Updated: {totalScore}");
    }

    public int GetTotalStrokes()
    {
        return totalScore;
    }

    public void LoadNextHole()
    {
        if (currentHole < 18)
        {
            currentHole++;
            string nextSceneName = "Hole" + currentHole;
            Debug.Log($"Loading next scene: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("All holes completed! Loading EndScreen.");
            SceneManager.LoadScene("EndScreen");
        }
    }
}
