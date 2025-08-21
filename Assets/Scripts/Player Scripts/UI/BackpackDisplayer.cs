using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class BackpackDisplayer : MonoBehaviour
{
    public GameObject prefab;
    public Vector2 start;
    public Vector2 offset;
    public InventorySystem inventory;
    List<TMP_Text> texts = new List<TMP_Text>();

    public void Start ( )
    {
        Restart( true );
        inventory.OnInventoryUpdate += Restart;
    }

    public void Restart(bool hard = false)
    {
        if (hard)
        {
            Vector2 pos = start;
            for (int i = 0; i < inventory.limit; i++)
            {
                GameObject o = Instantiate(prefab);
                o.transform.SetParent( transform );
                o.transform.localPosition = pos;
                pos += offset;
                texts.Add(o.GetComponent<TMP_Text>());
            }
        }

        for (int i = 0; i < texts.Count; i++)
        {
            TMP_Text text = texts[i];
            string item = "...";
            try
            {
                item = inventory.owned[i].gameObject.name;

                PickUpAble things = inventory.owned[i].GetComponent<PickUpAble>( );
                Image img = text.transform.GetChild( 0 ).GetComponent<Image>();
                text.color = things.textColor;
                img.color = new Color( things.bgColor.r, things.bgColor.g, things.bgColor.b, 0.6f );
            }
            catch ( System.Exception ) { }
            text.text = item;
            Image ximagex = text.transform.GetChild( 0 ).GetComponent<Image>();
            if ( item == "..." )
            {
                text.color = new Color( 1f, 0.6f, 0 );
                ximagex.color = new Color( 0.1f, 0.1f, 0.1f, 0.8f );
            }
            if ( inventory.selected == i && item != "..." )
            {
                ximagex.color = ximagex.color.WithAlpha( 0.2f );
            }
        }
    }
}
