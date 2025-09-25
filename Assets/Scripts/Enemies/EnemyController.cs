using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Transform enemy;
    PlayerMovement pm;
    public LayerMask enemyMask;
    public LayerMask propMask;
    public GameObject healthbar;
    public float offset = 0.4f;
    public Action OnDeathEvent;
    LayerMask filter;
    DummyEnemy controls;
    Rigidbody2D rb;
    GameObject thingy;
    public bool inSight = false;
    int memory = 0;

    public void Start ( )
    {
        controls = GetComponent<DummyEnemy>( );

        thingy = Instantiate(healthbar);
        HealthbarController controller = thingy.GetComponent<HealthbarController>();
        controller.alsoStatsLol = controls;
        controller.emulator = this;
        controller.Start( );
        thingy.transform.SetParent( null );
        thingy.transform.position = transform.position + Vector3.up * offset;
        Attacher atch = thingy.AddComponent<Attacher>( );
        atch.attatchedTo = transform;
        atch.rotation = Attacher.RotationSavingMethod.Zero;
        atch.Start( );

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
        bool act = false;

        List<RaycastHit2D> hits = Physics2D.RaycastAll(pm.connected.pewpew.cursorPos, Vector3.forward).ToList();
        foreach ( var item in hits )
        {
            if ( item.collider.gameObject == gameObject ) act = true;
        }
        if ( pm.connected.pewpew.targetted == gameObject || act || EmulateDetectEnemy(20) )
        {
            thingy.SetActive( true );
            thingy.GetComponent<Attacher>( ).Update( );
            thingy.GetComponent<HealthbarController>( ).Update( );
        }
        else
        {
            thingy.SetActive( false );
        }


        if ( !controls.prop )
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
                rb.linearVelocityX = Mathf.Clamp( rb.linearVelocityX, -controls.limitSpeed, controls.limitSpeed );
                if (legit)
                {
                    memory = controls.memoryTime;
                }
                if (enemy.transform.position.y > transform.position.y + 2)
                {
                    rb.AddForceY( controls.jumpPower, ForceMode2D.Impulse );
                }
            }
            else
            {
                rb.AddForceX( Mathf.MoveTowards( rb.linearVelocityX, 0, controls.stoppingForce ) - rb.linearVelocityX, ForceMode2D.Impulse );
            }
            inSight = val != null;
        }
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
    public bool EmulateDetectEnemy ( float distance )
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            (enemy.position - transform.position).normalized,
            distance,
            ~filter);
        if ( hit.transform == enemy )
        {
            return true;
        }
        return false;
    }

    public void DieOnDeath( )
    {
        if ( controls.Dead( ) )
        {
            OnDeathEvent?.Invoke( );
            Destroy( thingy );
            for (int x = 0; x < controls.dropStacks; x++)
            { 
                float slot = UnityEngine.Random.Range(0f, 100f);
                List<DummyEnemy.DropItem> selected = enemy.GetComponent<PlayerMain>().hp < 50 && !controls.ignoreLowDrops ? controls.lowDrops : controls.drops;
                foreach ( var item in selected )
                {
                    if ( item.min <= slot && item.max >= slot )
                    {
                        GameObject i = Instantiate(item.obj, null, true);
                        i.transform.position = transform.position;
                        i.name = i.name.Replace( "(Clone)", "" );
                    }
                }
            }
            Destroy( gameObject );
        }
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
        if ( collision.relativeVelocity.magnitude > controls.minPlayerImpactSpeed && !controls.prop )
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
                    foreach ( var item in controls.applies )
                    {
                        Effect eff = null;
                        bool found = Medpack.TryGetEffect( item.to, out eff );
                        if ( found )
                        {
                            collision.gameObject.GetComponent<PlayerMain>().drseuss.Apply( eff, Mathf.CeilToInt(item.efficency * collision.relativeVelocity.magnitude) );
                        }
                        else
                        {
                            Debug.LogWarning( $"did not find effect '{item.to}'" );
                        }
                    }
                    collision.gameObject.GetComponent<PlayerMain>( ).Damage( collision.relativeVelocity.magnitude * controls.damageMulti );
                }
                catch ( NullReferenceException ) { }
            }
        }
    }
}
