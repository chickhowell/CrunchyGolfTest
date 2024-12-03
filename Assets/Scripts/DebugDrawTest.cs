using UnityEngine;

public class DebugDrawTest : MonoBehaviour
{
    public Transform startTransform;

    void Update()
    {
        if (startTransform == null) return;

        Vector3 start = startTransform.position;
        Vector3 direction = Vector3.forward * 10f; // Example direction
        Debug.DrawLine(start, start + direction, Color.red, 0.5f);

        Debug.Log($"Drawing Line from {start} to {start + direction}");
    }
}
