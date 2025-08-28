using System.Collections.Generic;
using UnityEngine;

public class ProgressMaster : MonoBehaviour
{
    // It appears that I am not the only spy.
    //     - Blu Spy (Meet The Spy)

    public List<FlagScript> flags = new List<FlagScript>();
    public List<EnemySpawner> toTrigger = new List<EnemySpawner>();
    public FlagScript portal;
    public FinishLine gg;
    EndpanelGrader what;
    public bool active = false;

    public void Start ( )
    {
        AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA( );
    }

    public void AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA ( )
    {
        what = FindFirstObjectByType<EndpanelGrader>( );
    }

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
            if ( toTrigger.Count > 0 )
            {
                foreach ( var item in toTrigger )
                {
                    item.spawning = true;
                }
            }
            Medpack.TryGetEffect( "Bloodhurl", out Effect eff );
            GameObject.FindGameObjectWithTag( "Player" ).GetComponent<PlayerMain>( ).drseuss.Apply( eff, 1 );
        }
        if ( portal.recaptured == true )
        {
            gg.Endgame( );
            what.Endgame( );
            Destroy( this );
        }
    }
    // line 13 is a joke btw
}
