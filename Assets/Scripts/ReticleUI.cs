using UnityEngine;

public class ReticleUI : MonoBehaviour
{
    public GameObject arrowImage; // Reference to the arrow UI element

    void Start()
    {
        if (arrowImage == null)
        {
            Debug.LogError("ReticleUI: Arrow Image is not assigned!");
        }

        ShowArrow(); // Ensure the arrow is visible at the start
    }

    public void ShowArrow()
    {
        if (arrowImage != null)
        {
            arrowImage.SetActive(true);
        }
    }

    public void HideArrow()
    {
        if (arrowImage != null)
        {
            arrowImage.SetActive(false);
        }
    }
}
