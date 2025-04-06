using UnityEngine;

public class IsometricCameraFollow : MonoBehaviour
{
    public Transform target;  // Assign Player in the Inspector
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 8f, -8f);  // Adjust for isometric view

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
