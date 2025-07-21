using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Transform enemy;
    PlayerMovement pm;
    public LayerMask enemyMask;
    public LayerMask propMask;
    LayerMask filter;
    DummyEnemy controls;
    Rigidbody2D rb;
    bool inSight = false;
    int memory = 0;

    public void Start ( )
    {
        controls = GetComponent<DummyEnemy>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GameObject.FindWithTag( "Player" ).transform;
        pm = enemy.GetComponent<PlayerMovement>();
        if ( controls.ignoreProps )
        {
            filter = enemyMask | propMask;
        }
        else
        {
            filter = enemyMask;
        }
    }
    public void Update ( )
    {
        Vector2? val = DetectEnemy( );
        bool legit = true;
        if ( memory > 0 )
        {
            val = ( enemy.position - transform.position ).normalized;
            legit = false;
            memory--;
        }
        if ( val != null )
        {
            rb.AddForceX( val.Value.x * controls.speed, ForceMode2D.Force );
            if (legit)
            {
                memory = controls.memoryTime;
            }
        }
        else
        {
            rb.AddForceX( Mathf.MoveTowards( rb.linearVelocityX, 0, controls.stoppingForce ) - rb.linearVelocityX, ForceMode2D.Impulse );
        }
        inSight = val != null;
        DieOnDeath( );
    }

    public void OnDrawGizmos ( )
    {
        if ( Application.isPlaying )
        {
            Gizmos.color = inSight ? Color.red : Color.green;
            Gizmos.DrawLine( transform.position, transform.position + ( enemy.position - transform.position ).normalized * (pm.isCrouching ? controls.crouchedSight : controls.sight) );
            Gizmos.color = new( 1, 0.6f, 0 );
            Gizmos.DrawWireCube( transform.position + Vector3.up, new Vector3( 1, 0.25f ) );
            Gizmos.DrawCube( transform.position + Vector3.up, new Vector3( controls.percent, 0.25f ) );
        }

    }
    public Vector2? DetectEnemy( )
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, 
            (enemy.position - transform.position).normalized, 
            pm.isCrouching ? controls.crouchedSight : controls.sight, 
            ~filter);
        if ( hit.transform == enemy )
        {
            return ( enemy.position - transform.position ).normalized;
        }
        return null;
    }

    public void DieOnDeath( )
    {
        if ( controls.Dead( ) ) Destroy( gameObject );
    }

    public void OnCollisionEnter2D ( Collision2D collision )
    {
        Debug.Log( "Had an impact with magnitude of " + collision.relativeVelocity.magnitude );
        if ( controls.TakeImpactDamage( collision.relativeVelocity.magnitude - controls.durability ) )
        {
            bool xDominant = collision.relativeVelocity.x > Mathf.Abs(collision.relativeVelocity.y);
            Vector2 vel = new();
            if (xDominant)
            {
                vel.x = collision.relativeVelocity.x;
                vel.y = -collision.relativeVelocity.y;
            }
            else
            {
                vel.x = -collision.relativeVelocity.x;
                vel.y = collision.relativeVelocity.y;
            }
            rb.AddForce( vel * controls.bounce / 2 );
            memory += controls.memoryTime / 4;
        }
        if ( collision.relativeVelocity.magnitude > controls.minPlayerImpactSpeed )
        {
            if ( collision.rigidbody != null )
            {
                collision.rigidbody.AddForce( -collision.relativeVelocity * controls.bounceCoff );
                collision.rigidbody.AddForceY( collision.relativeVelocity.x * controls.tossUp + collision.relativeVelocity.y );
                if ( controls.friendlyFire )
                {
                    try
                    {
                        collision.gameObject.GetComponent<DummyEnemy>( ).TakeImpactDamage( collision.relativeVelocity.magnitude );
                    }
                    catch ( NullReferenceException ) { }
                }
                try
                {
                    collision.gameObject.GetComponent<PlayerMovement>( ).ReactImpact(Mathf.FloorToInt( collision.relativeVelocity.magnitude * controls.stunMulti ));
                    PopupSystem.CastPopupOutside( PopupController.Colors.Meh, $"({name}) has staggered you", $"{Mathf.FloorToInt( collision.relativeVelocity.magnitude * controls.stunMulti )} F" );
                    
                    collision.gameObject.GetComponent<PlayerMain>( ).Damage( collision.relativeVelocity.magnitude * controls.damageMulti );
                }
                catch ( NullReferenceException ) { }
            }
        }
    }
}
