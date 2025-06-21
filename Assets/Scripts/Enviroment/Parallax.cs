using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cameraTransform;     // Reference to the camera
    public Vector3 parallaxMultiplier;    // Separate multipliers for X, Y, and Z axes

    private Vector3 lastCameraPosition;

    void Start ( )
    {
        // Initialize the last camera position
        if ( cameraTransform == null )
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;
    }

    void Update ( )
    {
        // Calculate the camera's movement
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // Apply the vector multiplier to each axis
        transform.position += new Vector3(
            deltaMovement.x * parallaxMultiplier.x,
            deltaMovement.y * parallaxMultiplier.y,
            deltaMovement.z * parallaxMultiplier.z
        );

        // Update the last camera position
        lastCameraPosition = cameraTransform.position;
    }
}