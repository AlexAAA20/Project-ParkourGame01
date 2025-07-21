using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
            }
            catch ( System.Exception ) { }
            text.text = item;
            if (inventory.selected == i && inventory.owned.Count > 0) text.color = new Color( 1f, 0.6f, 0 );
            else text.color = new Color( 1, 1, 1 );

        }
    }
}
