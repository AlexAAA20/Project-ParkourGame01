using System;
using System.IO;
using UnityEngine;

public class SaverScript : MonoBehaviour
{
    Transform player;
    string filePath;

    public void Start ( )
    {
        filePath = Path.Combine( Application.persistentDataPath, "playerData.json" );
        player = GameObject.FindWithTag( "Player" ).transform;
        SavePlayer( player );
    }

    public void Update ( )
    {
        if ( Input.GetKeyDown( KeyCode.Y ) )
        {
            SavePlayer( player );
        }
        if ( Input.GetKeyDown( KeyCode.U ) ) 
        {
            player.position = ReadPlayer( );
            player.GetComponent<Rigidbody2D>( ).linearVelocity = Vector2.zero;
        }

    }
    public void SavePlayer( Transform player )
    {
        Vector3 pos = player.transform.position;
        SerializableVector3 v = new(pos.x, pos.y, pos.z);

        JSONMethods.Save( v, "math.json" );
    }
    public Vector3 ReadPlayer ( )
    {
        SerializableVector3 va = JSONMethods.Load<SerializableVector3>( "math.json" );
        return new Vector3(va.x, va.y, 0);
    }
}

[Serializable]
public class SerializableVector3
{
    public float x, y, z;
    public SerializableVector3( float x, float y, float z )
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

public static class JSONMethods
{
    public static string baseFilepath = Application.persistentDataPath;

    public static void Save<T>( T obj, string file )
    {
        string json = JsonUtility.ToJson(obj, true);
        File.WriteAllText( Path.Combine(baseFilepath, file), json );
    }
    public static T Load<T> ( string file )
    {
        string toRead = File.ReadAllText( Path.Combine( baseFilepath, file ) );
        using ( var reader = new StringReader( toRead ) ) 
        {
            T value = JsonUtility.FromJson<T>(reader.ReadToEnd());
            return value;
        }
    }

}