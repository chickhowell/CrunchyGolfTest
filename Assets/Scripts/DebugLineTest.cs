using UnityEngine;

public class DebugLineTest : MonoBehaviour
{
    public Transform startPoint;

    void Update()
    {
        if (startPoint == null) return;

        // Draw a simple red line in the scene
        Vector3 start = startPoint.position;
        Vector3 end = start + Vector3.forward * 10f; // 10 units forward

        Debug.DrawLine(start, end, Color.red, 0.5f);
        Debug.Log($"Drawing a line from {start} to {end}");
    }
}
