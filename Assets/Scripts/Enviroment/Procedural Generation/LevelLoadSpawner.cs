using System.Collections.Generic;
using UnityEngine;

public class LevelLoadSpawner : MonoBehaviour
{
    public List<DummyEnemy.DropItem> possibles;
    
    public void Spawn( )
    {
        float val = Random.value * 100;
        foreach (var item in possibles)
        {
            if ( val > item.min && val < item.max )
            {
                GameObject obj = Instantiate( item.obj, transform );
                obj.transform.position = transform.position;
                obj.name = obj.name.Split( "(" )[0];
            }
        }
    }
}
