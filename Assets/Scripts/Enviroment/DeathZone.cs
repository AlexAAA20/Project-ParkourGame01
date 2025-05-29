using UnityEngine;

public class DeathZone : MonoBehaviour
{

    private void OnDrawGizmos ( )
    {
        Gizmos.color = new( 1, 0, 0, 0.5f );
        Gizmos.DrawCube( transform.position, transform.lossyScale );
    }

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        try
        {
            collision.transform.GetComponent<PlayerMain>( ).Respawn( );
        }
        catch
        {
            Debug.Log( "oh noes" );
        }
    }
}
