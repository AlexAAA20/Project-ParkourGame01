using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool spawning;
    public GameObject what;
    public float howOften;
    public Vector3 offset;

    public void Start ( )
    {
        StartCoroutine( SpawnFoe( ) );
    }

    public IEnumerator SpawnFoe( )
    {
        while ( true )
        {
            if ( spawning )
            {
                GameObject newThingy = Instantiate(what, transform);
                newThingy.transform.localPosition = offset;
            }
            yield return new WaitForSeconds(howOften);
        }
    }
}
