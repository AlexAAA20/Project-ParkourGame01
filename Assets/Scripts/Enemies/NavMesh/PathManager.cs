using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathPiece
{
    public Vector2 position = Vector2.zero;
    public List<int> attachedTo = new List<int>();
    public Types pathType = Types.Walk;
    public enum Types
    {
        Walk
    }
}
public class PathManager : MonoBehaviour
{

    public List<PathPiece> path = new List<PathPiece> ();

}
