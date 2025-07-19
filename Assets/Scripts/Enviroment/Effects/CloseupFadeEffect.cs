using UnityEngine;

public class CloseupFadeEffect : MonoBehaviour
{
    public float minDistance;
    Transform player;
    SpriteRenderer sr;

    public void Start ( )
    {
        player = GameObject.FindWithTag( "Player" ).transform;
        sr = GetComponent<SpriteRenderer>( );
    }
    void Update()
    {
        float currDistance = Vector2.Distance( player.position, transform.position );
        float transparency = currDistance / minDistance;
        sr.color = new Color( sr.color.r, sr.color.g, sr.color.b, transparency );
    }

    private void OnDrawGizmosSelected ( )
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere( transform.position, minDistance );
    }
}
