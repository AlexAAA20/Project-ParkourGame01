using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    public Vector3 start;
    public Rigidbody2D rb;
    public PlayerMovement pm;

    private void Start ( )
    {
        start = transform.position;
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
    }

    public void Respawn ( )
    {
        transform.position = start;
        rb.linearVelocity = Vector3.zero;
    }

}
