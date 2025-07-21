using System;
using UnityEngine;

public class Medpack : MonoBehaviour, IUsable
{

    public float addHp;
    public float addArmor;
    public bool used = false;
    public int uses = 1;
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
    public void Use ( PlayerMain player )
    {
        PlayerMain pm = player;
        if ( !used )
        {
            bool healedAtAll = false;
            if ( pm.hp < 100 && addHp > 0 ) { pm.Heal( addHp ); healedAtAll = true; }
            if ( pm.armor < 100 && addArmor > 0 ) { pm.Armor( addArmor ); healedAtAll = true; }
            if ( healedAtAll )
            {
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
