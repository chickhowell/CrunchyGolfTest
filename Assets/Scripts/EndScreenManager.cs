using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI totalStrokesText; // Displays total strokes on the end screen
    public Button restartButton;            // Button for restarting the game
    public Button exitButton;               // Button for exiting to the main menu

    private void Start()
    {
        // Display the total strokes
        if (GameManager.Instance != null)
        {
            totalStrokesText.text = "Total Strokes: " + GameManager.Instance.GetTotalStrokes();
        }
        else
        {
            Debug.LogError("GameManager instance not found! Unable to display total strokes.");
        }

        // Link button functionality
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        else
        {
            Debug.LogWarning("Restart button is not assigned in the Inspector.");
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitToMainMenu);
        }
        else
        {
            Debug.LogWarning("Exit button is not assigned in the Inspector.");
        }
    }

    /// <summary>
    /// Restarts the game from the first hole.
    /// </summary>
    public void RestartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentHole = 1;
            GameManager.Instance.totalScore = 0;
            GameManager.Instance.strokes = new int[18]; // Reset strokes
        }

        SceneManager.LoadScene("Hole1"); // Replace "Hole1" with the name of your first hole scene
    }

    /// <summary>
    /// Exits to the main menu.
    /// </summary>
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the name of your main menu scene
    }
}
