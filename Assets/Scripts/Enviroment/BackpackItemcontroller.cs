using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackpackItemcontroller : MonoBehaviour
{
    float velocity = 0;
    float angle = 0;
    public TMP_Text itemName;
    public Image itemImage;
    public void Update ( )
    {
        itemImage.transform.rotation = Quaternion.Euler( 0, 0, angle );
        itemName.transform.rotation = Quaternion.identity;
        angle += velocity;
        
        if ( velocity < 1 ) velocity = 0;
        else velocity *= 0.95f;
    }

    public void ReRender( string text, Color background )
    {
        itemName.text = text;
        itemImage.color = background;
    }

    public void AddVelocity( float amount )
    {
        velocity += amount;
    }
    public void IncreaseToCapVelocity ( float amount )
    {
        if ( velocity < amount ) velocity += amount - velocity;
    }
}
