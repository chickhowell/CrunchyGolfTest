using TMPro;
using UnityEngine;

public class HoleUIManager : MonoBehaviour
{
    public TextMeshProUGUI holeText;         // Displays the current hole number
    public TextMeshProUGUI holeStrokesText;  // Displays strokes for the current hole
    public TextMeshProUGUI totalStrokesText; // Displays total strokes for all holes

    private void Start()
    {
        UpdateUI(GameManager.Instance.GetTotalStrokes(), 0);
    }

    public void UpdateUI(int totalStrokes, int holeStrokes)
    {
        if (holeText != null)
        {
            holeText.text = "Hole: " + GameManager.Instance.currentHole;
        }

        if (totalStrokesText != null)
        {
            totalStrokesText.text = "Total Strokes: " + totalStrokes;
        }

        if (holeStrokesText != null)
        {
            holeStrokesText.text = "Hole Strokes: " + holeStrokes;
        }
    }
}
