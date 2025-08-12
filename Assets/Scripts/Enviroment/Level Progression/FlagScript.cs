using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlagScript : MonoBehaviour
{
    public bool recaptured;
    public bool canBeCapped;
    public float distance;
    bool prevcBC;
    public List<Transform> defenders = new List<Transform>();
    public Transform player;
    public bool isPortal;
    SpriteRenderer spriteRenderer;

    public void Start ( )
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag( "Player" ).transform;
    }

    public void Update ( )
    {
        if ( !isPortal )
        {
            DripCheck( );
        }
        if ( Vector2.Distance((Vector2)player.transform.position, (Vector2)transform.position) < distance && canBeCapped )
        {
            if ( !recaptured )
            {
                recaptured = true;
                PopupSystem.CastPopupOutside( PopupController.Colors.Green, $"A flag has been capped.", "" );
            }
            else if ( !isPortal )
            {
                Medpack.TryGetEffect( "Flag Regeneration", out Effect eff );
                player.GetComponent<PlayerMain>( ).drseuss.Apply( eff, 5, true );
            }
        }
        if ( prevcBC != canBeCapped && !isPortal )
        {
            PopupSystem.CastPopupOutside( PopupController.Colors.Alright, $"A flag has been cleared of defenders.", "" );
        }
        if ( recaptured )
        {
            spriteRenderer.color = isPortal ? new Color( 1, 1, 1 ) : new Color( 0, 0.5f, 1 );
            if ( isPortal )
            {
                Vector3 requiredForce = transform.position - player.transform.position;
                requiredForce *= 20f;

                player.GetComponent<Rigidbody2D>( ).AddForce(requiredForce);
            }
        }
        else if ( canBeCapped )
        {
            spriteRenderer.color = isPortal ? new Color( 0, 1, 0 ) : new Color( 1, 1, 1 );
        }
        else 
        {
            spriteRenderer.color = isPortal ? new Color( 0, 0, 0, 0 ) : new Color( 1, 0, 0 );
        }
        prevcBC = canBeCapped;
    }

    public void DripCheck( )
    {
        canBeCapped = true;
        foreach ( var item in defenders )
        {
            if ( item != null ) { canBeCapped = false; break; }
        }
    }
}
