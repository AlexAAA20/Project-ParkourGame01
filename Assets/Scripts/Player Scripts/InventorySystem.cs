using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class InventorySystem : MonoBehaviour
{
    [Tooltip("Distance at which you can pick up an item.")]
    public float pickDistance;

    public LayerMask ignore;

    public int limit = 3;

    public Action<bool> OnInventoryUpdate;

    public List<GameObject> owned;
    Vector3 cursorPos;

    public void Start ( )
    {
        owned = new List<GameObject>();
    }

    public void Update ( ) 
    {
        Aim( );
        if ( Input.GetMouseButtonDown( 1 ) && owned.Count < limit )
        {
            GameObject? fetched = Check( );
            if ( fetched != null )
            {
                owned.Add( fetched );
                Debug.Log( fetched.name );
                OnInventoryUpdate.Invoke( false );
            }
        }
        if ( Input.GetMouseButtonDown( 2 ) && owned.Count > 0 )
        {
            GameObject fetched = owned[0];
            fetched.SetActive( true );
            fetched.GetComponent<PickUpAble>( ).Unsheathe( ( cursorPos - transform.position ).normalized, transform.position );
            owned.RemoveAt( 0 );
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
        if ( hit.rigidbody != null )
        {
            PickUpAble? picking = hit.rigidbody.GetComponent<PickUpAble>();
            if ( picking != null ) 
            {
                GameObject saving = hit.transform.gameObject;
                saving.SetActive( false );
                return saving;
            }
        }
        return null;
    }
}
