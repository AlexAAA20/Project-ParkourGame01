using System.IO;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float sight;
    public float crouchedSight;
    public float percent { get { return health / maxHealth; } }
    public float speed;
    public float minImpactSpeed;
    public float maxImpactSpeed;
    public float impactDamageModifier;
    public float bounce;
    public float bounceCoff;
    public float tossUp;
    public float knockMulti;

    public bool TakeDamage( float damage )
    {
        if ( Dead( ) ) return false;
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        return damage > 0;
    }
    
    public bool TakeImpactDamage( float impactForce )
    {
        if ( Dead( ) ) return false;
        if ( impactForce < minImpactSpeed ) return false;
        float damage = Mathf.Min(impactForce, maxImpactSpeed);
        damage -= minImpactSpeed;
        damage *= impactDamageModifier;
        return TakeDamage( damage );
    }

    public bool Dead ( ) => health <= 0;
}
