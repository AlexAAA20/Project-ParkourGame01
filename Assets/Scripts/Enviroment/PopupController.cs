using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    public TMP_Text primary;
    public TMP_Text secondary;
    Image sr;
    public void Start ( )
    {
        sr = GetComponent<Image>();
    }
    public enum Colors
    {
        Basic,
        Red,
        Orange,
        Green,
        Meh,
        Alright
    }
    
    public void WriteColor( Colors col )
    {
        if ( sr == null )
        {
            sr = GetComponent<Image>();
        }
        switch ( col )
        {
            case Colors.Basic:
                sr.color = new Color( 0, 0, 0, 0.4f );
                break;

            case Colors.Red:
                sr.color = new Color( 1, 0, 0, 0.4f );
                break;

            case Colors.Green:
                sr.color = new Color( 0, 1, 0, 0.4f );
                break;

            case Colors.Orange:
                sr.color = new Color( 1, 0.6f, 0, 0.4f );
                break;

            case Colors.Meh:
                sr.color = new Color( 1, 0.8f, 0, 0.4f );
                break;

            case Colors.Alright:
                sr.color = new Color( 0.8f, 1, 0, 0.4f );
                break;
        }
    }
    public void WriteText ( string primary, string secondary )
    {
        this.primary.text = primary;
        this.secondary.text = secondary;
    }
}
