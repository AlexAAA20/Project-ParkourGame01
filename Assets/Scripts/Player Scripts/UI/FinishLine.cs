using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public List<Transform> listOfThingsToGenocide = new List<Transform>();
    public List<Transform> listOfThingsToUnGenocide = new List<Transform>();

    public void Endgame( )
    {
        foreach (var item in listOfThingsToGenocide)
        {
            item.gameObject.SetActive( false );
        }
        foreach ( var item in listOfThingsToUnGenocide )
        {
            item.gameObject.SetActive( true );
        }
    }
}
