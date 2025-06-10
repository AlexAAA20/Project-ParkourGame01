using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    public Vector3 start;
    public Rigidbody2D rb;
    public PlayerMovement pm;
    public PlayerJumpMovement pjm;

    private void Start ( )
    {
        start = transform.position;
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
        pjm = GetComponent<PlayerJumpMovement>();
        pjm.rb = rb;
    }

    public void Respawn ( )
    {
        transform.position = start;
        rb.linearVelocity = Vector3.zero;
    }

    public void Update ( )
    {
        pjm.isAirborne = pm.isAirborne;
        pjm.isCrouching = pm.isCrouching;
    }
}
