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

    [Tooltip("How is the object turned when it's put.")]
    public float initialRotation;

    Rigidbody2D rb;

    public void Start ( )
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Unsheathe ( Vector2 direction, Vector2 initial )
    {
        Debug.Log( direction.normalized );
        transform.position = initial + (direction.normalized * appearDistance);
        if ( toss )
        {
            Vector2 boost = direction * tossPower;
            rb.AddForce( boost, ForceMode2D.Impulse );
            rb.angularVelocity = angularToss;
        }
        transform.rotation = Quaternion.Euler(0, 0, initialRotation);

        Debug.Log( ( toss ? "Threw out " : "Calmly put down " ) + transform.name );
    }

    // this is a tag.
}
