using UnityEngine;
public class NoShittyNumbers : MonoBehaviour
{
    int currChildCount = 0;
    void Start ( )
    {
        Clear( );
    }
    void Update ( )
    {
        if ( transform.childCount != currChildCount ) Clear( );
    }
    void Clear( )
    {
        currChildCount = transform.childCount;
        for ( int i = 0; i < currChildCount; i++ )
        {
            Transform child = transform.GetChild(i);
            string newName = child.name;
            newName = newName.Split( " (" )[0];
            child.name = newName;
        }
    }
}
