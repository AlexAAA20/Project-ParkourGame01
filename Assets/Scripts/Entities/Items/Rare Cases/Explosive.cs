using System.Linq;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float range;
    public float force;

    EnemyController enemyController;
    Rigidbody2D rb;

    private void Start ( )
    {
        rb = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();
        enemyController.OnDeathEvent += Boom;
        Debug.Log( $"<color=yellow>BOMB HAS BEEN PLANTED." );
    }

    private void Boom( )
    {
        var allRbs = Resources.FindObjectsOfTypeAll(typeof(Rigidbody2D)).Cast<Rigidbody2D>().ToList();
        foreach (var item in allRbs)
        {
            float dist = Vector2.Distance( item.transform.position, transform.position );
            if ( dist < range )
            {
                var x = item.transform.position.x - transform.position.x;
                var y = item.transform.position.y - transform.position.y;

                item.GetComponent<Rigidbody2D>( ).AddForce( new Vector2( x, y ) * force * (1 - (dist / range)));
            }
        }
    }
}
