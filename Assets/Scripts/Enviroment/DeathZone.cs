using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public bool killNPCs = true;
    public Vector3 TPTrashAt = Vector3.zero;
    public bool playerIsTrash = false;
    private void OnDrawGizmos ( )
    {
        Gizmos.color = new( 1, 0, 0, 0.5f );
        Gizmos.DrawCube( transform.position, transform.lossyScale );
    }

    private void OnDrawGizmosSelected ( )
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine( transform.position, TPTrashAt );
    }

    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if (!playerIsTrash)
        {
            try { collision.transform.GetComponent<PlayerMain>( ).Respawn( );
                PopupSystem.CastPopupOutside( PopupController.Colors.Red, "You have died to the killzone.", "" ); 
                return; 
            }
            catch { }
        }
        if (killNPCs)
        {
            try {
                collision.transform.GetComponent<DummyEnemy>( ).health = 0;
                PopupSystem.CastPopupOutside( PopupController.Colors.Green, $"{collision.transform.name} has died to the killzone.", "" );
                return; 
            }
            catch { }
        }
        collision.transform.position = TPTrashAt;
    }
}
