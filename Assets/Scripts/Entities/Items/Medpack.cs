using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Medpack : MonoBehaviour, IUsable
{
    [Serializable]
    public class FullRecoveries
    {
        public string name; // Removes this effect if detected. Triggers usage when at least one is removed.
    }

    [Serializable]
    public class SideEffects
    {
        public string name; // Applies this effect if the medpack been used.
        public int frames; // Length of effects. Effects of the same name stack together by addition.
    }
    [Serializable]
    public class Transfusions
    {
        public string from; // From which effect
        public string to;   // To what effect
        public float efficency; // How potent (if 2, then makes effect's 'to' ticks equal twice of 'from'.)
        public bool keep = false; // Do you remove 'from' when this happens?
    }

    public float addHp;
    public float addArmor;
    public bool used = false;
    public int uses = 1;
    public List<FullRecoveries> removes;
    public List<SideEffects> applies;
    public List<SideEffects> mainApplies;
    public List<Transfusions> changes;
    public List<Transfusions> sideChanges;
    string originalName = string.Empty;
    int maxUses = 0;
    Color originalColor;
    SpriteRenderer sr;

    public void Start ( )
    {
        maxUses = uses;
        originalName = gameObject.name;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public static bool TryGetEffect( string name, out Effect effect )
    {
        foreach (var item in Effect.baseEffects)
        {
            if ( item == null )
            {
                Debug.LogWarning( $"a static effect slot is empty!" );
            }
            else if ( item.effectName == name ) // NullReferenceException
            {
                effect = item;
                return true;
            }
        }
        effect = null;
        return false;
    }
    public void Use ( PlayerMain player )
    {
        PlayerMain pm = player;
        if ( !used )
        {
            bool healedAtAll = false;
            if ( pm.hp < 100 && addHp > 0 ) { pm.Heal( addHp ); healedAtAll = true; }
            if ( pm.armor < 100 && addArmor > 0 ) { pm.Armor( addArmor ); healedAtAll = true; }
            Debug.Log( healedAtAll );
            if ( removes.Count > 0 )
            {
                foreach (var item in removes)
                {
                    bool status = pm.drseuss.Remove( item.name );
                    if ( status ) healedAtAll = true;
                }
            }
            Debug.Log( healedAtAll );
            if ( changes.Count > 0 )
            {
                foreach ( var item in changes )
                {
                    Effect eff = null;
                    bool found = TryGetEffect( item.to, out eff );
                    if ( found )
                    {
                        bool status = pm.drseuss.Transformate( item.from, eff, item.efficency, !item.keep );
                        if ( status ) healedAtAll = true;
                    }
                }
            }
            if ( mainApplies.Count > 0 )
            {
                foreach ( var item in mainApplies )
                {
                    Effect eff = null;
                    bool found = TryGetEffect( item.name, out eff );
                    if ( found )
                    {
                        pm.drseuss.Apply( eff, item.frames );
                        healedAtAll = true;
                    }
                }
            }
            Debug.Log( healedAtAll );
            if ( healedAtAll )
            {
                if ( pm.hp + pm.armor > -addHp && addHp < 0 ) { pm.Damage( -addHp ); healedAtAll = true; }
                if ( applies.Count > 0 )
                {
                    foreach ( var item in applies )
                    {
                        Effect eff = null;
                        bool found = TryGetEffect( item.name, out eff );
                        if ( found )
                        {
                            pm.drseuss.Apply( eff, item.frames );
                        }
                    }
                }
                if ( sideChanges.Count > 0 )
                {
                    foreach ( var item in sideChanges )
                    {
                        Effect eff = null;
                        bool found = TryGetEffect( item.to, out eff );
                        if ( found )
                        {
                            bool status = pm.drseuss.Transformate( item.from, eff, item.efficency, !item.keep );
                        }
                    }
                }
                uses--;
                if (uses <= 0 )
                {
                    used = true;
                    name = originalName + " E";
                    sr.color = new Color( 0.5f, 0.5f, 0.5f );
                }
                else
                {
                    name = originalName + $" {uses} L";
                }
                PopupSystem.CastPopupOutside( PopupController.Colors.Green, $"Yummers!", ":)" );
            }
            else
            {
                PopupSystem.CastPopupOutside( PopupController.Colors.Basic, $"I don't need it right now.", ":/" );
            }
        }
        else
        {
            PopupSystem.CastPopupOutside( PopupController.Colors.Orange, $"It's empty now.", ":(" );
        }
    }
}
// This is a bit of a code that I made for a medpack item.
// Whenever it is used, it gets a NullReferenceException at line 52.
// Please help.