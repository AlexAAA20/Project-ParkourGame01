using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    Vector3 start;
    Rigidbody2D rb;
    PlayerMovement pm;
    PlayerJumpMovement pjm;

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
    }
}
