using System;
using System.Globalization;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemyController : MonoBehaviour
{
    Transform enemy;
    public LayerMask enemyMask;
    DummyEnemy controls;
    Rigidbody2D rb;
    bool inSight = false;

    public void Start ( )
    {
        controls = GetComponent<DummyEnemy>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GameObject.FindWithTag( "Player" ).transform;
    }
    public void Update ( )
    {
        Vector2? val = DetectEnemy( );
        if ( val != null )
        {
            rb.AddForceX( val.Value.x * controls.speed, ForceMode2D.Force );
        }
        inSight = val != null;
        DieOnDeath( );
    }

    public void OnDrawGizmos ( )
    {
        if ( Application.isPlaying )
        {
            Gizmos.color = inSight ? Color.red : Color.green;
            Gizmos.DrawLine( transform.position, enemy.position );
            Gizmos.color = new( 1, 0.6f, 0 );
            Gizmos.DrawWireCube( transform.position + Vector3.up, new Vector3( 1, 0.25f ) );
            Gizmos.DrawCube( transform.position + Vector3.up, new Vector3( controls.percent, 0.25f ) );
        }

    }
    public Vector2? DetectEnemy( )
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (enemy.position - transform.position).normalized, 99999, ~enemyMask);
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
        if ( controls.TakeImpactDamage( collision.relativeVelocity.magnitude ) )
        {
            rb.AddForce( -collision.relativeVelocity * controls.bounce );
        }
        if ( collision.otherRigidbody != null )
        {
            collision.otherRigidbody.AddForce( collision.relativeVelocity * controls.bounceCoff * controls.bounce );
        }
    }
}
