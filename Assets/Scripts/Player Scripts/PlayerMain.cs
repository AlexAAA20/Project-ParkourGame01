using UnityEngine;
using UnityEngine.UI;

public class PlayerMain : MonoBehaviour
{
    public Vector3 start;
    public Rigidbody2D rb;
    public PlayerMovement pm;
    public Slider hpBar;

    public float hp = 100;
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
        hp = 100;
        hpBar.value = hp;
    }

    public void Damage( float amount )
    {
        hp -= amount;
        hpBar.value = hp;
        if ( hp <= 0 )
        {
            Respawn( );
        }
    }

}
