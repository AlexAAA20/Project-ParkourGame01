using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public int maxAmmo;
    public int currAmmo;
    public float reloadTime;
    public float recoil;
    public float knockback;
    public float range;
    public Transform arm;
    public AmmoDisplayScript ammoCounter;
    public KeyCode reload = KeyCode.R;
    public int shootMouseButton = 0;
    public LayerMask ignore;
    public bool letFire;

    bool loading;
    Rigidbody2D rb;
    Vector2 cursorPos;
    Coroutine crt;

    public void Start ( )
    {
        currAmmo = maxAmmo;
        rb = GetComponent<Rigidbody2D>();
        loading = false;
        ammoCounter.Restart( this, true );
    }
    public void OnDrawGizmos ( )
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube( cursorPos, new( 0.1f, 0.1f, 0.01f ) );
    }

    public void Update ( )
    {
        Aim( );

        Vector2 direction = cursorPos - (Vector2)arm.position;
        arm.localRotation = Quaternion.Euler( 0, 0, Mathf.Atan2( direction.y, direction.x ) * Mathf.Rad2Deg );

        if ( Input.GetKeyDown( reload ) && !loading )
        {
            crt = StartCoroutine( Reload( ) );
        }

        if ( Input.GetMouseButtonDown( shootMouseButton ) && letFire )
        {
            if ( currAmmo > 0 )
            {
                Shoot( );
                currAmmo--;
                ammoCounter.Restart( this );
                if ( crt != null )
                {
                    StopCoroutine( crt );
                    crt = null;
                    loading = false;
                }
                if (currAmmo == 0)
                {
                    crt = StartCoroutine( Reload( ) );
                }
            }
            else
            {
                PopupSystem.CastPopupOutside( PopupController.Colors.Basic, "Out of ammo", "" );
            }
        }

        if ( !letFire )
        {
            letFire = true;
        }

    }

    public IEnumerator Reload( )
    {
        loading = true;
        while ( currAmmo < maxAmmo )
        {
            yield return new WaitForSecondsRealtime( reloadTime );
            currAmmo++;
            ammoCounter.Restart( this );
        }
        loading = false;
        StopCoroutine( crt );
        crt = null;
    }

    public void Aim ( )
    {
        Vector2 pixels = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay( pixels );
        cursorPos = ray.origin;
    }

    public void Shoot( )
    {
        // get the thing hit
        Vector2 dir = cursorPos - (Vector2)arm.position;
        RaycastHit2D hit = Physics2D.Raycast(arm.position, dir, range, ~ignore);
        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce( dir.normalized * knockback );
        }
        rb.AddForce( dir.normalized * -recoil );


    }
}
