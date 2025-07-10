using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplayScript : MonoBehaviour
{
    public GameObject prefab;
    public Color loaded;
    public Color unloaded;
    public GunScript stock;
    public Vector2 start;
    public Vector2 offset;
    public SpriteRenderer armRenderer;
    public Gradient grd;
    List<GameObject> ammo = new List<GameObject>( );

    public void Start ( )
    {
        grd.SetKeys(
            new GradientColorKey[] {
            new GradientColorKey(loaded, 1f),
            new GradientColorKey(unloaded, 0f)
            },
            new GradientAlphaKey[] {
            new GradientAlphaKey(1f, 1f), // Fully opaque at both points
            new GradientAlphaKey(1f, 0f)
            }
        );
    }

    public void Restart( GunScript gun, bool hard = false )
    {
        if ( hard )
        {
            foreach (var item in ammo)
            {
                Destroy( item );
            }
            ammo.Clear( );
            Vector2 at = (Vector3)start;
            for (int i = 0; i < gun.maxAmmo ; i++)
            {
                GameObject o = Instantiate( prefab );
                o.transform.SetParent( transform );
                o.transform.position = at;
                o.transform.localScale = Vector3.one;
                at += offset;
                ammo.Add( o );
            }
        }
        armRenderer.color = grd.Evaluate( (float)gun.currAmmo / (float)gun.maxAmmo );
        for (int i = 0; i < ammo.Count; i++ )
        {
            GameObject o = ammo[i];
            Image p = o.GetComponent<Image>();
            if ( i + 1 <= gun.currAmmo )
                p.color = loaded;
            else
                p.color = unloaded;
        }
    }
}
