using JetBrains.Annotations;
using UnityEngine;

public class WaterZone : MonoBehaviour
{
    public float yZone;
    public float depthForceMultiplier;
    public Vector2 stream;
    public float drag;

    // the deeper something is, the more force it will use to swim up.
    public void OnTriggerStay2D ( Collider2D collision )
    {
        if ( collision.gameObject.GetComponent<Rigidbody2D>( ) == null ) return;
        Transform tf = collision.transform;
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>( );
        float force = Mathf.Clamp(yZone - tf.position.y, 0, transform.lossyScale.y * 2) * depthForceMultiplier;
        rb.AddForceAtPosition( Vector2.up * force, tf.position + Vector3.down * 0.2f );
        rb.AddForce( stream );
        rb.linearVelocityY = Mathf.MoveTowards( rb.linearVelocityY, 0, drag );
    }

    public void OnDrawGizmos ( )
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube( transform.position, transform.lossyScale );

    }

    public void OnDrawGizmosSelected ( )
    {
        Gizmos.color = Color.red;
        Vector3 baseline = new Vector3( -9999, yZone, 0 );
        Vector3 endLine = new Vector3( 9999, yZone, 0 );
        Gizmos.DrawLine( baseline, endLine );
    }
}
