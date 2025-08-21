using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class PlayerEffector : MonoBehaviour
{
    public PlayerMain pm;
    public List<Effect> effects;
    public GameObject effectSample;
    [SerializeField]
    public List<Pair> effectSprites;
    public Transform setter;
    public Sprite defaultEffect;
    public Vector3 offset;
    public Vector3 start;
    public void Start ( )
    {
        effects = new List<Effect>();
        pm = GetComponent<PlayerMain>();
    }

    public void Update ( )
    {
        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].Execute( pm );
            if ( effects[i].framesLeft <= 0 )
            {
                PopupSystem.CastPopupOutside( PopupController.Colors.Alright, $"You lost {effects[i].effectName}.", "" );
                Destroy( effects[i].related.gameObject );
                effects.RemoveAt( i );
            }
        }
        Refresh( );
    }

    public void Apply ( Effect effect, int time, bool silent=false )
    {
        Effect nEffect = effect.Dupe( );
        foreach (var item in effects)
        {
            if ( item.effectName == effect.effectName )
            {
                item.maxFrames += time;
                item.framesLeft += time;
                if ( !silent ) PopupSystem.CastPopupOutside( PopupController.Colors.Pink, $"You stacked {effect.effectName}.", $"{item.framesLeft} F" );
                Refresh( );
                return;
            }
        }
        GameObject eff = Instantiate( effectSample, setter );
        nEffect.related = eff.GetComponent<EffectController>();
        if ( !silent ) PopupSystem.CastPopupOutside( PopupController.Colors.Pink, $"You received {effect.effectName}.", $"{time} F" );
        nEffect.framesLeft = time;
        nEffect.maxFrames = time;
        nEffect.pe = this;
        try
        {
            nEffect.sprite = effectSprites.First( a => a.key == effect.effectName ).value;
        }
        catch ( Exception ex )
        {
            nEffect.sprite = defaultEffect;
        }
        effects.Add( nEffect );
        Refresh( );
    }

    public bool Remove ( string name )
    {
        foreach ( var item in effects )
        {
            if ( item.effectName == name )
            {
                item.framesLeft = 0;
                return true;
            }
        }
        return false;
    }
    public bool Transformate ( string name, Effect into, float prof, bool remove=true )
    {
        foreach ( var item in effects )
        {
            if ( item.effectName == name )
            {
                int time = Mathf.RoundToInt((float)item.framesLeft * prof);
                if ( remove ) item.framesLeft = 0;
                PopupSystem.CastPopupOutside( PopupController.Colors.Orange, $"{name} turned into {into}.", $"{time} F" );
                Apply( into, time );
                return true;
            }
        }
        return false;
    }
    public void Refresh( )
    {
        Vector3 at = start;
        foreach (var item in effects)
        {
            item.related.transform.localPosition = at;
            at += offset;
            item.related.SetIt( item.sprite, $"{item.framesLeft}", item.good, item.effectName );
        }
    }
}
[Serializable]
public class Pair
{
    public string key;
    public Sprite value;
}

public class Effect
{
    public static Effect bleeding = new Effect("Bleeding", (x, y) =>
    {
        x.intFlag++;
        if ( x.intFlag >= 60 )
        {
            y.Damage(1, true);
            x.framesLeft--;
            x.intFlag = 0;
        }
    });
    public static Effect regeneration = new Effect("Regeneration", (x, y) =>
    {
        if ( y.hp < 100 )
        {
            y.Heal(0.2f, true);
            x.framesLeft--;
        }
    }, true);
    public static Effect ramboMode = new Effect("Rambo Mode", (x, y) =>
    {
        y.pewpew.currAmmo = y.pewpew.maxAmmo;
        x.intFlag++;
        if ( x.intFlag >= 120 )
        {
            x.framesLeft--;
            x.intFlag = 0;
        }
    }, true);
    public static Effect immunePoison = new Effect("Poison Immunity", (x, y) =>
    {
        foreach (var item in x.pe.effects)
        {
            if ( item.effectName == "Poisoned" || item.effectName == "Seizure" )
            {
                x.framesLeft -= item.framesLeft;
                item.framesLeft = 0;
                if ( x.framesLeft <= 0 )
                {
                    PopupSystem.CastPopupOutside( PopupController.Colors.Red, $"The antidode lost it's power.", "" );
                }
            }
        }
        x.intFlag++;
        if ( x.intFlag >= 120 )
        {
            x.framesLeft--;
            x.intFlag = 0;
        }
    }, true);
    public static Effect immuneCripple = new Effect("Cast Immunity", (x, y) =>
    {
        foreach (var item in x.pe.effects)
        {
            if ( item.effectName == "Slowed" || item.effectName == "Crippled" )
            {
                x.framesLeft -= item.framesLeft;
                item.framesLeft = 0;
                if ( x.framesLeft <= 0 )
                {
                    PopupSystem.CastPopupOutside( PopupController.Colors.Red, $"The cast split in half.", "" );
                }
            }
        }
        x.intFlag++;
        if ( x.intFlag >= 120 )
        {
            x.framesLeft--;
            x.intFlag = 0;
        }
    }, true);
    public static Effect armoredUp = new Effect("Nanobot", (x, y) =>
    {
        y.Armor(0.2f, true);
        x.intFlag++;
        if ( x.intFlag >= 30 )
        {
            x.framesLeft--;
            x.intFlag = 0;
        }
    }, true);
    public static Effect harshBleeding = new Effect("Hard-Bleed", (x, y) =>
    {
        x.intFlag++;
        if ( x.intFlag >= 30 )
        {
            y.Damage(2, true);
            x.framesLeft--;
            x.intFlag = 0;
        }
        y.drseuss.Transformate("Bleeding", harshBleeding, 0.5f);
    });
    public static Effect poisoned = new Effect("Poisoned", (x, y) =>
    {
        x.intFlag++;
        if ( x.intFlag >= 120 )
        {
            y.Damage(x.framesLeft / 4f);
            x.framesLeft--;
            x.intFlag = 0;
        }
    });
    public static Effect hasty = new Effect("Hasty", (x, y) =>
    {
        y.pm.speedMultiplier = 1.6f;
        x.intFlag++;
        if ( x.intFlag >= 60 )
        {
            x.framesLeft--;
            x.intFlag = 0;
        }
    }, true);
    public static Effect adrenaline = new Effect("Adrenaline-d", (x, y) =>
    {
        y.pm.speedMultiplier = 3f;
        x.intFlag++;
        if ( x.intFlag >= 30 )
        {
            x.framesLeft--;
            x.intFlag = 0;
        }
    });
    public static Effect bloodhurl = new Effect("Bloodhurl", (x, y) =>
    {
        x.intFlag++;
        if ( x.intFlag >= 60 )
        {
            y.Damage(x.framesLeft, true);
            x.framesLeft = x.framesLeft + 1;
            x.intFlag = 0;
        }
    } );

    public static Effect bandaged = new Effect("Bandaged", (x, y) =>
    {
        foreach (var item in x.pe.effects)
        {
            if ( item.effectName == "Bleeding" || item.effectName == "Hard-Bleed" )
            {
                x.framesLeft -= item.framesLeft;
                item.framesLeft = 0;
                if ( x.framesLeft <= 0 )
                {
                    PopupSystem.CastPopupOutside( PopupController.Colors.Red, $"The bandage fell off.", "" );
                }
            }
        }
        x.intFlag++;
        if ( x.intFlag >= 120 )
        {
            if ( y.hp < 100 )
            {
                y.Heal(1f, true);
                x.framesLeft--;
                x.intFlag = 0;
            }
        }
    }, true);

    public static Effect flagRegeneration = new Effect("Flag Regeneration", (x, y) =>
    {
        foreach (var item in x.pe.effects)
        {
            if ( item.effectName == "Bleeding" || item.effectName == "Hard-Bleed" )
            {
                item.framesLeft = 0;
            }
        }
        x.intFlag++;
        y.Heal(0.2f, true);
        if ( x.intFlag >= 60 )
        {
            x.framesLeft--;
            x.intFlag = 0;
        }
        if ( x.framesLeft > 10 )
        {
            x.framesLeft = 10;
        }
    }, true);

    public static Effect disarmed = new Effect("Disarmed", (x, y) =>
    {
        y.pewpew.letFire = false;
        x.intFlag++;
        if ( x.intFlag >= 40 )
        {
            x.framesLeft--;
            x.intFlag = 0;
        }
    });
    public static Effect slowed = new Effect("Slowed", (x, y) =>
    {
        y.pm.speedMultiplier = 0.8f;
        x.intFlag++;
        if ( x.intFlag >= 60 )
        {
            x.framesLeft--;
            x.intFlag = 0;
        }
    });
    public static Effect crippled = new Effect("Crippled", (x, y) =>
    {
        x.intFlag++;
        if ( x.intFlag >= 120 )
        {
            if ( !y.pm.isAirborne ) y.pm.speedMultiplier = 0.3f;
            x.framesLeft--;
            x.intFlag = 0;
        }
    });
    public static Effect seizure = new Effect("Seizure", (x, y) =>
    {
        x.intFlag++;
        y.pm.speedMultiplier *= -1f;
        if ( x.intFlag >= 60 )
        {
            x.framesLeft--;
            x.intFlag = 0;
        }
    });

    public static Effect slippery = new Effect("Slip-up", (x, y) =>
    {
        for (int i = 0; i < y.nikeeBag.owned.Count; i++)
        {
            y.nikeeBag.Put( );
        }
        y.pewpew.currAmmo = 0;
        x.framesLeft = 0;
    });


    public static List<Effect> baseEffects = new List<Effect>()
    {
        bleeding, harshBleeding, poisoned, crippled, disarmed, slowed, slippery, // base
        bandaged, seizure, flagRegeneration, hasty, // update 2
        regeneration, armoredUp, bloodhurl, adrenaline, immuneCripple, immunePoison, ramboMode // update 3
    };

    public int framesLeft;
    public string effectName;
    public int maxFrames;
    int intFlag = 0;
    public Action<Effect, PlayerMain> action;
    public Sprite? sprite;
    public EffectController? related;
    public bool invisible = false;
    public bool good = false;
    public PlayerEffector pe;

    public Effect ( string name, Action<Effect, PlayerMain> action, bool good = false, bool invisible=false)
    {
        effectName = name;
        this.action = action;
        this.invisible = invisible;
        this.good = good;
    }

    public void Execute ( PlayerMain on ) => action.Invoke( this, on );

    public Effect Dupe( )
    {
        Effect newEffect = new( effectName, action, invisible );
        newEffect.framesLeft = framesLeft;
        newEffect.sprite = sprite;
        newEffect.related = related;
        newEffect.maxFrames = maxFrames;
        newEffect.good = good;
        return newEffect;
    }
}
