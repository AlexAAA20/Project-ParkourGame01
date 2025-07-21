using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Services.CloudSave.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class InventorySystem : MonoBehaviour
{
    [Tooltip("Distance at which you can pick up an item.")]
    public float pickDistance;

    public LayerMask ignore;

    public int limit = 3;

    public int selected = 0;

    public Action<bool> OnInventoryUpdate;

    public List<GameObject> owned;
    PlayerMain pm;
    Vector3 cursorPos;

    public void Start ( )
    {
        owned = new List<GameObject>();
        pm = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<PlayerMain>();
    }

    public void Update ( ) 
    {
        Aim( );
        float movement = Input.mouseScrollDelta.y;
        bool changed = false;
        if ( movement > 0 ) { selected++; changed = true; }
        if ( movement < 0 ) { selected--; changed = true; }
        if ( selected >= owned.Count ) { selected = 0; changed = true; }
        if ( selected < 0 ) { selected = owned.Count - 1; changed = true; }
        if ( changed ) OnInventoryUpdate.Invoke( false );

        if ( Input.GetMouseButtonDown( 1 ) && owned.Count < limit )
        {
            GameObject? fetched = Check( );
            if ( fetched != null )
            {
                owned.Add( fetched );
                OnInventoryUpdate.Invoke( false );
            }
        }
        if ( Input.GetKeyDown( KeyCode.F ) && owned.Count > 0 )
        {
            GameObject fetched = owned[selected];
            fetched.SetActive( true );
            fetched.GetComponent<PickUpAble>( ).Unsheathe( ( cursorPos - transform.position ).normalized, transform.position );
            owned.RemoveAt( selected );
            if ( selected >= owned.Count )
            {
                selected--;
            }
            OnInventoryUpdate.Invoke( false );
        }
        if ( Input.GetKeyDown( KeyCode.E ) && owned.Count > 0 )
        {
            IUsable component = owned[selected].GetComponent<IUsable>( );
            component.Use( pm );
            OnInventoryUpdate.Invoke( false );
        }
    }


    public void Aim ( )
    {
        Vector2 pixels = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay( pixels );
        cursorPos = ray.origin;
    }

    public GameObject? Check ( )
    {
        // get the thing hit
        Vector2 dir = cursorPos - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, pickDistance, ~ignore);
        if ( hit.collider == null )
        {
            PopupSystem.CastPopupOutside( PopupController.Colors.Basic, "Can't reach that.", ":(" );
            return null;
        }
        if ( hit.rigidbody != null )
        {
            PickUpAble? picking = hit.rigidbody.GetComponent<PickUpAble>();
            if ( picking != null ) 
            {
                GameObject saving = hit.transform.gameObject;
                saving.SetActive( false );
                return saving;
            }
            else
            {
                PopupSystem.CastPopupOutside( PopupController.Colors.Basic, "For some reason I cant.", ":?" );
            }
        }
        else
        {
            PopupSystem.CastPopupOutside( PopupController.Colors.Basic, "Doesn't even move.", ":(" );
        }
        return null;
    }
}
