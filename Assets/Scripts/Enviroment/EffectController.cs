using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{
    public Image img;
    public TMP_Text text;
    public TMP_Text title;
    public List<Image> color;
    public Color good = Color.green;
    public Color bad = Color.red;

    public void SetIt( Sprite img, string text, bool positive=false, string title="" )
    {
        this.img.sprite = img;
        this.text.text = text;
        this.title.text = title;
        foreach ( var item in color )
        {
            item.color = positive ? good : bad;
        }
    }
}
