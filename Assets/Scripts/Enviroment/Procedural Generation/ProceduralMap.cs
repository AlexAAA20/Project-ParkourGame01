using System;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMap : MonoBehaviour
{
    [Serializable]
    public class LevelPieceItem
    {
        public GameObject obj;
        public LevelPiece piece;
        public bool special = false;
        public int spawnOnlyAt = 0;
    }
    [SerializeField]
    public List<LevelPieceItem> pieces;
    public LevelPieceItem currentlyInScope;
    List<LevelPieceItem> currentPieces;
    int count = 0;

    public void Start ( )
    {
        currentPieces = new List<LevelPieceItem>();
        foreach ( var item in pieces )
        {
            item.piece = item.obj.GetComponent<LevelPiece>( );
        }

        FastGenerate();
    }

    public void Update ( )
    {
        
    }

    public LevelPieceItem GetRandomPiece( )
    {
        List<LevelPieceItem> allow = new List<LevelPieceItem> ();
        bool specialRound = false;
        foreach ( var item in pieces )
        {
            if ( item.special && count % item.spawnOnlyAt == 0 ) { specialRound = true; break; }
        }
        foreach (var item in pieces)
        {
            if ( item.special )
            {
                if ( count % item.spawnOnlyAt == 0 )
                {
                    allow.Add( item );
                }
            }   
            else if ( !specialRound )
            {
                allow.Add( item );
            }
        }

        return allow[UnityEngine.Random.Range( 0, allow.Count )];
    }

    public void FastGenerate( ) => Generate(GetRandomPiece( ) );    

    public void Generate( LevelPieceItem toGenerate )
    {
        Vector3 at = currentlyInScope.obj == null ? Vector3.zero : currentlyInScope.piece.endPiece.position;

        GameObject pic = Instantiate( toGenerate.obj );
        pic.transform.SetParent( transform );
        pic.transform.position = at;
        LevelPiece pxx = pic.GetComponent<LevelPiece>( );
        pxx.Spawn( );
        pxx.CheckpointActivated += FastGenerate;
        if ( currentPieces.Count >= 1 ) currentPieces[currentPieces.Count - 1].piece.CheckpointActivated -= FastGenerate;

        LevelPieceItem piece = new();
        piece.obj = pic;
        piece.piece = pic.GetComponent<LevelPiece>( );
        currentPieces.Add( piece );

        currentlyInScope = piece;

        if ( currentPieces.Count > 7 )
        {
            Destroy( currentPieces[0].obj );
            currentPieces.RemoveAt( 0 );
        }
        count++;
    }
}
