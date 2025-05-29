using UnityEditor;
using UnityEngine;


public class RoomGenerator : EditorWindow
{
    Vector2 size;
    GameObject prefab;
    float thickness;


    [MenuItem("Tools/Room Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(RoomGenerator));
    }

    public void OnGUI ( )
    {
        GUILayout.Label("Room Generator", EditorStyles.boldLabel);

        size = EditorGUILayout.Vector2Field( "Size", size );
        prefab = (GameObject)EditorGUILayout.ObjectField( prefab, typeof( GameObject ), false );
        thickness = EditorGUILayout.FloatField( thickness );

        if (GUILayout.Button("Generate"))
        {
            GenerateRoom( );
        }
    }
    public void GenerateRoom( )
    {
        GameObject root = new GameObject( "Generated Room" );

        GameObject leftWall = Generate( );
        GameObject rightWall = Generate( );
        GameObject topWall = Generate( );
        GameObject bottomWall = Generate( );

        leftWall.transform.SetParent( root.transform );
        rightWall.transform.SetParent( root.transform );
        topWall.transform.SetParent( root.transform );
        bottomWall.transform.SetParent( root.transform );

        Shape( leftWall, new( thickness, size.y * 2 + thickness ) );
        Shape( rightWall, new( thickness, size.y * 2 + thickness ) );
        Shape( topWall, new( size.x * 2 + thickness, thickness ) );
        Shape( bottomWall, new( size.x * 2 + thickness, thickness ) );

        leftWall.transform.position = new( -size.x, 0 );
        rightWall.transform.position = new( size.x, 0 );
        topWall.transform.position = new( 0, size.y );
        bottomWall.transform.position = new( 0, -size.y );
    }

    public GameObject Generate ( ) => Instantiate( prefab );

    public void Shape( GameObject thing, Vector2 size ) => thing.transform.localScale = size;
}
