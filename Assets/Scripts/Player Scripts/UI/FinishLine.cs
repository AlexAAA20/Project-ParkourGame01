using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public static bool end = false;
    public float curr = 1f;
    public float speed = 1.01f;
    public List<Transform> listOfThingsToGenocide = new List<Transform>();
    public List<Transform> listOfThingsToUnGenocide = new List<Transform>();

    public void Endgame( )
    {
        end = true;
        foreach (var item in listOfThingsToGenocide)
        {
            item.gameObject.SetActive( false );
        }
        foreach ( var item in listOfThingsToUnGenocide )
        {
            item.gameObject.SetActive( true );
        }
    }

    public void Update ( )
    {
        if ( end )
        {
            curr /= speed;
            if ( curr <= 0.01f )
                Time.timeScale = 0;
            else
                Time.timeScale = curr;
        }
    }
}
