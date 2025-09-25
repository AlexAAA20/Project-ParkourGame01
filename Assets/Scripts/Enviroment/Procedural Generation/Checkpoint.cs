using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool used;
    public static PlayerMain player;
    public Action WhenUsed;

    private void Start ( )
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();
    }

    public void Update ( )
    {
        if ( player.transform.position.x > transform.position.x && !used )
        {
            PopupSystem.CastPopupOutside( PopupController.Colors.Green, "Reached a checkpoint.", ":D" );
            player.start = transform.position;
            used = true;
            WhenUsed?.Invoke( );

        }
    }
}
