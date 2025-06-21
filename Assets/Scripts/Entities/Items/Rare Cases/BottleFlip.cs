using UnityEngine;

public class BottleFlip : MonoBehaviour
{
    Rigidbody2D rb;

    public float uprightThreshold = 2;
    public float velocityThreshold = 0.1f;
    public float velocityAngularThreshold = 3f;
    public bool flipped = false;

    public bool MatchesConditions( )
    {
        float rot = Mathf.Abs(transform.eulerAngles.z);
        if ( rot > 179 ) rot = 360 - rot;

        if ( rot < uprightThreshold && rb.linearVelocity.magnitude < velocityThreshold && Mathf.Abs(rb.angularVelocity) < velocityAngularThreshold )
        {
            return true;
        }

        return false;
    }

    public void Start ( )
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update ( )
    {
        bool hitIt = MatchesConditions( );
        if ( hitIt && flipped == false )
        {
            flipped = true;
            Debug.Log( "LETS GO BABYYY" );
        }
        if ( !hitIt && flipped == true )
        {
            flipped = false;
            Debug.Log( "aw man" );
        }

    }

}
