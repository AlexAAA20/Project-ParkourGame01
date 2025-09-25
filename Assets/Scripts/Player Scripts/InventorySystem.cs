using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public int limit = 9;

    public int selected = 0;

    int itemCount = 0;

    public Action<BackpackDisplayer.UpdateType> OnInventoryUpdate;
    public float selectedShake = 0;
    public float allShake = 0;

    public List<GameObject> owned;
    PlayerMain pm;
    Vector3 cursorPos;

    public void Start ( )
    {
        owned = new List<GameObject>();
        pm = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<PlayerMain>();
        for (int i = 0; i < limit; i++)
        {
            owned.Add( null );
        }
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
        if ( changed ) OnInventoryUpdate.Invoke( BackpackDisplayer.UpdateType.Light );

        Checkity( KeyCode.Alpha1, 1 );
        Checkity( KeyCode.Alpha2, 2 );
        Checkity( KeyCode.Alpha3, 3 );
        Checkity( KeyCode.Alpha4, 4 );
        Checkity( KeyCode.Alpha5, 5 );
        Checkity( KeyCode.Alpha6, 6 );
        Checkity( KeyCode.Alpha7, 7 );
        Checkity( KeyCode.Alpha8, 8 );
        Checkity( KeyCode.Alpha9, 9 );

        if ( Input.GetMouseButtonDown( 1 ) && itemCount < limit )
        {
            GameObject? fetched = Check( );
            if ( fetched != null )
            {
                for ( int i = 0; i < limit; i++ )
                {
                    if ( owned[i] == null ) { owned[i] = fetched; break; }
                }
                itemCount++;
                OnInventoryUpdate.Invoke( BackpackDisplayer.UpdateType.Light );
            }
        }
        if ( Input.GetKeyDown( KeyCode.F ) && itemCount > 0 )
        {
            selectedShake = 17;
            Put( );
        }
        bool happy = false;
        IUsable component = null;
        try
        {
            component = owned[selected].GetComponent<IUsable>( );
            happy = true;
        }
        catch (Exception)
        {

        }
        if ( happy )
        {
            component.WhenHeldAction( pm );
            if ( Input.GetKeyDown( KeyCode.E ) && itemCount > 0 )
            {
                component.Use( pm );
                selectedShake = 17;
                OnInventoryUpdate.Invoke( BackpackDisplayer.UpdateType.Light );
            }
        }
        OnInventoryUpdate.Invoke( BackpackDisplayer.UpdateType.Update );
    }
    public void Checkity( KeyCode k, int slot )
    {
        if ( Input.GetKeyDown(k) )
        {
            selected = slot - 1;
            OnInventoryUpdate?.Invoke( BackpackDisplayer.UpdateType.Light );
        }
    }

    public void Put( )
    {
        if ( owned[selected] == null ) return;
        GameObject fetched = owned[selected];
        fetched.SetActive( true );
        fetched.GetComponent<PickUpAble>( ).Unsheathe( ( cursorPos - transform.position ).normalized, transform.position );
        owned[selected] = null;
        itemCount--;
        if ( selected >= owned.Count )
        {
            selected--;
        }
        OnInventoryUpdate.Invoke( BackpackDisplayer.UpdateType.Light );
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
                saving.transform.SetParent( null );
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
