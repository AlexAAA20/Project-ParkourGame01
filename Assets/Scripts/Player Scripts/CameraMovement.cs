using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Tooltip("Control - Camera moved. Current by default.")]
    public Camera control;
    [Tooltip("Interia - Speed impact on the position of the camera.")]
    public float inertia = 0.5f;
    [Tooltip("Smooth - Smoothness. Smaller numbers - more smoothness (0-1).")]
    public float smooth = 0.04f;
    [Tooltip("Boost - Additional direction in where to keep the camera.")]
    public Vector3 boost;

    Rigidbody2D rb = null;

    private void Start ( )
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate ( )
    {
        Vector3 position = rb.position;
        position += (Vector3)rb.linearVelocity * inertia;
        position += boost;
        control.transform.position = Vector3.Lerp( control.transform.position, position, smooth );
    }
}
