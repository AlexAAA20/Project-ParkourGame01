using UnityEngine;

public class PlayerJumpMovement : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    [Tooltip("Jump Strength - Power added upwards on jumping while grounded.")]
    public float jumpStrength = 7f;
    [Tooltip("Override Jump - Override instead of adding velocity when jumping.")]
    public bool overrideJump = false;
    [Tooltip("Is Airborne - A value. Yeah.")]
    public bool isAirborne;
    [Tooltip("I do not think I have to explain this.")]
    public KeyCode bindJump = KeyCode.Space;

    public void Update ( )
    {
        if ( Input.GetKeyDown(bindJump) )
        {
            Jump( );
        }
    }

    public void Jump ( )
    {
        if ( isAirborne ) return;
        if ( overrideJump )
            rb.linearVelocityY = jumpStrength;
        else
            rb.linearVelocityY += jumpStrength;
    }
}
