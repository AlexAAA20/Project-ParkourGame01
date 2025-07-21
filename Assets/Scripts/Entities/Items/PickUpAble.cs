using UnityEngine;

public class PickUpAble : MonoBehaviour, IUsable
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
    public void Use ( PlayerMain player )
    {
        PopupSystem.CastPopupOutside( PopupController.Colors.Basic, "Not sure what I'm supposed to do.", "" );
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
    }

    // this is a tag.
}


public interface IUsable
{
    public void Use ( PlayerMain player );
}