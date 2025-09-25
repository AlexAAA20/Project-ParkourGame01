using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BackpackDisplayer : MonoBehaviour
{
    public GameObject prefab;
    public Vector2 start;
    public Vector2 offset;
    public InventorySystem inventory;
    List<BackpackItemcontroller> texts = new List<BackpackItemcontroller>();

    public enum UpdateType
    {
        Update,
        Light,
        Hard
    }

    public void Start ( )
    {
        Restart( UpdateType.Hard );
        inventory.OnInventoryUpdate += Restart;
    }

    public void Restart(UpdateType hard = UpdateType.Light)
    {
        if (hard == UpdateType.Hard)
        {
            Vector2 pos = start;
            for (int i = 0; i < inventory.limit; i++)
            {
                GameObject o = Instantiate(prefab);
                o.transform.SetParent( transform );
                o.transform.localPosition = pos;
                pos += offset;
                texts.Add(o.GetComponent<BackpackItemcontroller>());
            }
        }

        for (int i = 0; i < texts.Count; i++)
        {
            BackpackItemcontroller BIC = texts[i];
            string currentTitle = "";
            GameObject obj = null;
            bool highlighted = i == inventory.selected;
            bool good = false;
            try
            {
                obj = inventory.owned[i].gameObject;
                good = true;
            }
            catch ( System.Exception )
            {
                BIC.ReRender( "", highlighted ? new Color( 0.3f, 0.3f, 0.3f, 1 ) : new Color( 0, 0, 0, 0.8f ) );
            }
            if ( good )
            {
                PickUpAble colorSetter = obj.GetComponent<PickUpAble>();
                BIC.ReRender( obj.name, highlighted ? ( colorSetter.bgColor ) : ( colorSetter.bgColor * 0.7f ) );
            }
            if ( hard == UpdateType.Update )
            {
                if ( highlighted )
                {
                    BIC.IncreaseToCapVelocity( 4 * Random.Range( 0.5f, 1.5f ) );
                    if ( inventory.selectedShake != 0 )
                    {
                        BIC.AddVelocity( inventory.selectedShake );
                    }
                }
                if ( inventory.allShake != 0 )
                {
                    BIC.AddVelocity( inventory.allShake );
                }
                inventory.selectedShake = 0;
                inventory.allShake = 0;
            }

            /*
            TMP_Text text = texts[i];
            string item = "...";
            bool good = false;
            try
            {
                item = inventory.owned[i].gameObject.name;

                PickUpAble things = inventory.owned[i].GetComponent<PickUpAble>( );
                Image img = text.transform.GetChild( 0 ).GetComponent<Image>();
                text.color = things.textColor;
                img.color = new Color( things.bgColor.r, things.bgColor.g, things.bgColor.b, 0.6f );
                good = true;
            }
            catch ( System.Exception ) { }
            text.text = item;
            Image ximagex = text.transform.GetChild( 0 ).GetComponent<Image>();
            if ( !good )
            {
                text.color = new Color( 1f, 0.6f, 0 );
                ximagex.color = new Color( 0.1f, 0.1f, 0.1f, 0.8f );
            }
            if ( inventory.selected == i && item != "..." )
            {
                ximagex.color = ximagex.color.WithAlpha( 0.2f );
            }
            */
        }
    }
}
