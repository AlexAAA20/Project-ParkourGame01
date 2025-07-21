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
    public float stoppingForce;
    public float minImpactSpeed;
    public float minPlayerImpactSpeed;
    public float durability;
    public float maxImpactSpeed;
    public float impactDamageModifier;
    public float bounce;
    public float bounceCoff;
    public float tossUp;
    public float stunMulti;
    public float damageMulti;
    public int memoryTime;

    public bool ignoreProps;
    public bool friendlyFire;

    public bool TakeDamage( float damage )
    {
        if ( Dead( ) ) return false;
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        if ( Dead( ) )
        {
            PopupSystem.CastPopupOutside( PopupController.Colors.Green, $"An enemy ({name}) has been defeated", "" );
        }
        return damage > 0;
    }
    
    public bool TakeImpactDamage( float impactForce )
    {
        if ( Dead( ) ) return false;
        if ( impactForce < minImpactSpeed ) return false;
        float damage = Mathf.Min(impactForce, maxImpactSpeed);
        damage -= minImpactSpeed;
        damage *= impactDamageModifier;
        PopupSystem.CastPopupOutside( PopupController.Colors.Orange, $"An enemy ({name}) was damaged", $"-{Mathf.Round(damage * 10) / 10}" );
        return TakeDamage( damage );
    }

    public bool Dead ( ) => health <= 0;
}
