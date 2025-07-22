using UnityEngine;
using UnityEngine.UI;

public class PlayerMain : MonoBehaviour
{
    public Vector3 start;
    public Rigidbody2D rb;
    public PlayerMovement pm;
    public GunScript pewpew;
    public InventorySystem nikeeBag;
    public PlayerEffector drseuss;
    public Slider hpBar;
    public Slider armorBar;
    public int deaths;

    public float hp = 50;
    public float armor = 0;

    bool respawning = false;
    private void Start ( )
    {
        start = transform.position;
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
        pewpew = GetComponent<GunScript>();
        nikeeBag = GetComponent<InventorySystem>();
        drseuss = GetComponent<PlayerEffector>();
    }

    public void Update ( )
    {
        CheckArmor( );
        if ( respawning )
        {
            foreach (var item in drseuss.effects)
            {
                item.framesLeft = 0;
            }
            transform.position = start;
            rb.linearVelocity = Vector3.zero;
            pm.staggerFrames = 0;
            hp = 50;
            armor = 0;
            CheckArmor( );
            hpBar.value = hp;
            armorBar.value = armor;
            if ( Input.anyKeyDown )
            {
                respawning = false;
                deaths++;
                PopupSystem.CastPopupOutside( PopupController.Colors.Basic, "You respawned.", $"{deaths} times" );
            }
        }
    }
    public void CheckArmor( ) => armorBar.gameObject.SetActive( armor > 0 );
    public void Respawn ( )
    {
        respawning = true;
    }

    public void Damage( float amount, bool silent=false )
    {
        if (armor > 0)
        {
            armor -= amount;
            if ( armor < 0 )
            {
                if ( !silent ) PopupSystem.CastPopupOutside( PopupController.Colors.Orange, "Your armor broke.", $"" );
                hp -= -armor;
                hpBar.value = hp;
                armor = 0;
            }
            CheckArmor( );
            armorBar.value = armor;
        }
        else
        {
            hp -= amount;
            hpBar.value = hp;
        }
        if ( amount > 50 )
        {
            drseuss.Apply( Effect.harshBleeding, ( int ) amount - 48 );
            drseuss.Apply( Effect.slowed, ( int ) amount - 48 );
        }
        else if ( amount > 30 )
        {
            drseuss.Apply( Effect.bleeding, (int)amount - 28 );
        }
        if ( hp <= 0 )
        {
            PopupSystem.CastPopupOutside( PopupController.Colors.Red, "You took lethal damage.", $"-{Mathf.Round( amount * 10 ) / 10}" );
            Respawn( );
        }
        else
        {
            if ( !silent ) PopupSystem.CastPopupOutside( PopupController.Colors.Meh, "You took damage.", $"-{Mathf.Round( amount * 10 ) / 10}" );
        }
    }

    public void Heal( float amount, bool silent = false )
    {
        hp += amount;
        hpBar.value = hp;
        if ( hp > 100 )
        {
            hp = 100;
        }
        if ( amount > 0 && !silent )
        {
            PopupSystem.CastPopupOutside( PopupController.Colors.Green, "You healed up.", $"+{Mathf.Round( amount * 10 ) / 10}" );
        }
    }
    public void Armor ( float amount, bool silent = false )
    {
        armor += amount;
        armorBar.value = armor;
        CheckArmor( );
        if ( armor > 100 )
        {
            armor = 100;
        }
        if ( amount > 0 && !silent )
        {
            PopupSystem.CastPopupOutside( PopupController.Colors.Green, "You armored up.", $"+{Mathf.Round( amount * 10 ) / 10}" );
        }
    }

}
