using TMPro;
using UnityEngine;

public class PlayerStatReader : MonoBehaviour
{
    public PlayerMain readFrom;

    public TMP_Text airborneState;
    public TMP_Text crouchedState;
    public TMP_Text special;

    bool lowFPSmode;

    public void Update ( )
    {
        if ( Input.GetKeyDown( KeyCode.V ) )
        {
            lowFPSmode = !lowFPSmode;
        }

        if (lowFPSmode)
        {
            special.color = new( 255, 160, 0 );
            special.text = "LOW FPS ON";
            Application.targetFrameRate = 12;
        }
        else
        {
            special.text = "";
            Application.targetFrameRate = 60;
        }



        bool airborne = readFrom.pm.isAirborne;
        bool crouched = readFrom.pm.isCrouching;

        if ( airborne )
        {
            airborneState.color = Color.green;
            airborneState.text = "AIR";
        }
        else
        {
            airborneState.color = Color.red;
            airborneState.text = "GRD";
        }

        if ( crouched )
        {
            crouchedState.color = Color.green;
            crouchedState.text = "CRH";
        }
        else
        {
            crouchedState.color = Color.gray;
            crouchedState.text = "";
        }

    }
}
