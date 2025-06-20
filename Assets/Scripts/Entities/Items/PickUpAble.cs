using UnityEngine;

public class PickUpAble : MonoBehaviour
{
    [Tooltip("Do you toss or just calmly put down?")]
    public bool toss = true;

    [Tooltip("How hard you toss, if you do.")]
    public float tossPower = 15f;

    [Tooltip("Adds angular velocity when tossing.")]
    public float angularToss = 60f;

    [Tooltip("How far from the center of the player does the item appear.")]
    public float appearDistance = 0.5f;

    Rigidbody2D rb;

    public void Start ( )
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Unsheathe ( Vector2 direction, Vector2 initial )
    {
        transform.position = initial + (direction * appearDistance);
        if ( toss )
        {
            Vector2 boost = direction * tossPower;
            rb.AddForce( boost, ForceMode2D.Impulse );
            rb.angularVelocity = angularToss;
        }

        Debug.Log( ( toss ? "Threw out " : "Calmly put down " ) + transform.name );
    }

    // this is a tag.
}
