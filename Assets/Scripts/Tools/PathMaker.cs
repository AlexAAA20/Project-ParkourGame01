using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/*
namespace Assets.Scripts.Tools
{
    internal class PathMaker : EditorWindow
    {
        // shared
        int currentView = 0;
        PathPiece current = null;
        PathManager selected = null;
        bool addedSCGUI = false;

        // fetch
        Vector2 position = Vector2.zero;
        float safetyZone = 1;

        // connecter
        int conSelected = 0;

        [MenuItem( "Tools/Pathmaker" )]
        public static void ShowWindow ( )
        {
            GetWindow( typeof( PathMaker ) );
        }

        public void OnDestroy ( )
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            addedSCGUI = false;
        }

        public void OnSceneGUI( SceneView sceneView )
        {
            if ( selected == null )
            {
                SceneView.duringSceneGui -= OnSceneGUI;
                addedSCGUI = false;
                return;
            }
            switch ( currentView )
            {
                case 0:
                    {
                        Handles.color = new Color( 1, 0.6f, 0 );
                        Handles.DrawWireCube( position, new( 0.025f, 0.025f ) );
                        foreach ( var item in selected.path )
                        {
                            Handles.color = Color.green;
                            if ( item == current )
                            {
                                Handles.color = new Color( 1, 0.6f, 0 );
                                Handles.DrawLine( position, item.position );
                            }
                            Handles.DrawWireCube( item.position, new( 0.05f, 0.05f ) );
                            Handles.Label( item.position + Vector2.right * 0.2f, $"{item.position.x} {item.position.y}" );
                            Handles.color = Color.cyan;
                            Handles.DrawWireDisc( item.position, Vector3.forward, safetyZone, 2 );
                        }
                        break;
                    }
                case 1:
                    {
                        if ( current == null )
                        {
                            return;
                        }
                        Handles.color = Color.white;
                        Handles.DrawWireCube( current.position, new( 0.05f, 0.05f ) );
                        for (int i = 0; i < current.attachedTo.Count; i++)
                        {
                            PathPiece p = selected.path[i];
                            Handles.color = Color.white;
                            if ( i == conSelected ) 
                                Handles.color = new Color( 1, 0.6f, 0 );
                            Handles.DrawLine( current.position, p.position );
                        }
                        break;
                    }
            }
        }
        public void OnGUI ( )
        {
            GUILayout.Label( "Nodemaker", EditorStyles.largeLabel );
            selected = ( PathManager ) EditorGUILayout.ObjectField( selected, typeof( PathManager ), true );
            if ( selected == null )
            {
                return;
            }
            if ( !addedSCGUI )
            {
                SceneView.duringSceneGui += OnSceneGUI;
                addedSCGUI = true;
            }

            if ( current != null )
            {
                GUILayout.Label( $"X: {current.position.x} Y: {current.position.y}" );
                GUILayout.Label( $"connected to {current.attachedTo.Count} other nodes" );
            }


            switch (currentView)
            {
                case 0:
                    {
                        GUILayout.Label( "Fetch Node", EditorStyles.boldLabel );
                        position = EditorGUILayout.Vector2Field( "Position", position );
                        safetyZone = EditorGUILayout.FloatField( "Distance Allowed", safetyZone );
                        if ( current != null ) GUI.enabled = false;
                        if ( GUILayout.Button( "Fetch" ) )
                        {
                            PathPiece found = null;
                            bool able = FindNearest(selected, position, safetyZone, out found);
                            if ( able )
                            {
                                current = found;
                                Debug.Log( "Found!" );
                            }
                        }
                        if ( current != null ) GUI.enabled = true;

                        break;
                    }
                case 1:
                    {
                        GUILayout.Label( "Connection Node", EditorStyles.boldLabel );
                        if ( current == null ) break;
                        if ( current.attachedTo.Count <= 1 )
                        {
                            EditorGUILayout.HelpBox( "Due to having one connection or none, the slider is turned off. Thank you.", MessageType.Info );
                            conSelected = 0;
                        }
                        else
                        {
                            conSelected = EditorGUILayout.IntSlider( conSelected, 0, current.attachedTo.Count - 1 );
                        }

                        break;
                    }
                case 2:
                    {
                        GUILayout.Label( "Creation Node", EditorStyles.boldLabel );

                        break;
                    }
            }
            if ( current == null ) GUI.enabled = false;
            if ( GUILayout.Button( "Clear Selection" ) ) current = null;
            if ( current == null ) GUI.enabled = true;

            currentView = EditorGUILayout.IntSlider( currentView, 0, 3 );
        }

        private void OnSceneGui ( SceneView obj )
        {
            throw new NotImplementedException( );
        }

        public bool FindNearest( PathManager inside, Vector2 position, float allowedDistance, out PathPiece result )
        {
            List<PathPiece> queued = inside.path;
            foreach (var item in queued)
            {
                float distance = (position - item.position).magnitude;
                bool allowed = allowedDistance > distance;
                if ( allowed )
                {
                    result = item;
                    return true;
                }

            }
            result = null;
            return false;
        }
    }
}
*/