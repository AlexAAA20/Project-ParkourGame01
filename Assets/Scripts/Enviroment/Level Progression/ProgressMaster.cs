using System.Collections.Generic;
using UnityEngine;

public class ProgressMaster : MonoBehaviour
{
    public List<FlagScript> flags = new List<FlagScript>();
    public List<EnemySpawner> toTrigger = new List<EnemySpawner>();
    public FlagScript portal;
    public FinishLine gg;
    public bool active = false;

    public void Update ( )
    {
        foreach (var item in flags)
        {
            if ( !item.recaptured )
            {
                return;
            }   
        }
        if ( !active )
        {
            active = true;
            PopupSystem.CastPopupOutside( PopupController.Colors.Pink, $"The world cracks as well as your sanity.", "" );
            PopupSystem.CastPopupOutside( PopupController.Colors.Pink, $"Find the exit and jump into the gap.", "" );
            portal.canBeCapped = true;
            foreach (var item in toTrigger)
            {
                item.spawning = true;
            }
            Medpack.TryGetEffect( "Bloodhurl", out Effect eff );
            GameObject.FindGameObjectWithTag( "Player" ).GetComponent<PlayerMain>( ).drseuss.Apply( eff, 1 );
        }
        if ( portal.recaptured == true )
        {
            gg.Endgame( );
            Destroy( this );
        }
    }
}
