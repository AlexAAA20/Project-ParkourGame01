using System;
using UnityEngine;

public class LevelPiece : MonoBehaviour
{
    // position where a new LevelPiece will start
    public Transform endPiece;

    // decorational
    public string pieceName;

    // 
    public Checkpoint attached;

    // used to pair equal-sized gaps with equal-sized rooms.
    public string startConnectorName;
    public string endConnectorName;

    public Action CheckpointActivated;

    public void Spawn( )
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            try
            {
                transform.GetChild( i ).GetComponent<LevelLoadSpawner>( ).Spawn( );
            }
            catch ( System.Exception ) { }
        }
        attached.WhenUsed += () => CheckpointActivated.Invoke( );
    }
}
