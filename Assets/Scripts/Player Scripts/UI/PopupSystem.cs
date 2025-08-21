using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupSystem : MonoBehaviour
{
    public GameObject popup;
    public Vector2 spacing;
    public Vector2 startOffset;
    public float destroyAfter;
    List<PopupController> popups;
    public static Action<PopupController.Colors, string, string> createPopup;

    public void Awake ( )
    {
        popups = new List<PopupController>( ) { };
        createPopup = null;
        createPopup += CastPopup;
    }

    [ContextMenu("Cast Test Popup")]
    public void CastTestPopup( )
    {
        CastPopup( PopupController.Colors.Basic, "Hey, i'm a test!", ":)" );
    }
    public void CastPopup( PopupController.Colors col, string primary, string secondary )
    {
        GameObject newPopup = Instantiate( popup, transform );
        newPopup.transform.localPosition = ( Vector3 ) startOffset;
        newPopup.name = $"{popups.Count} ind";
        foreach (var item in popups)
        {
            Shift( item, spacing );
        }
        for (int i = 0; i < popups.Count; i++)
        {
            popups[i].gameObject.name = $"index is {i}";
        }
        PopupController pc = newPopup.GetComponent<PopupController>( );
        popups.Add( pc );
        pc.WriteText( primary, secondary );
        pc.WriteColor( col );
        Debug.Log( "printed out a popup :)" );
        if ( popups.Count > destroyAfter )
        {
            Destroy( popups[0].gameObject );
            popups.RemoveAt( 0 );
        }
    }

    public static void CastPopupOutside( PopupController.Colors col, string primary, string secondary )
    {
        createPopup.Invoke( col, primary, secondary );
    }


    public void Shift( PopupController pc, Vector2 where )
    {
        pc.transform.localPosition += (Vector3)where;
    }
}
