using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DaytimeCycle : MonoBehaviour
{
    [Serializable]
    public class Cycle
    {

        public List<string> leads;

        // color duhh
        public Gradient gradient;

        // tag for things
        public string tag;

        // tag for things
        public string uniqueTag;

        // frames that it takes for another cycle
        public int length;

        public Color Gradient ( float time ) => gradient.Evaluate( time );

        public Cycle NewSelected ( List<Cycle> cycles ) 
        {
            string selected = leads[UnityEngine.Random.Range( 0, leads.Count - 1 )];
            return cycles.Where( x => x.uniqueTag == selected ).Single( );
        }
    }

    // current frames
    public float frames;
    // % of frames in daysize
    public float fufilness;

    // all cycles
    public List<Cycle> cycles;
    // current cycle
    public Cycle currCycle;

    Camera cam;

    public void Start ( )
    {
        cam = GetComponent<Camera>();
        currCycle = cycles.FirstOrDefault(x => x.tag == "daytime") ?? cycles[0];
        if ( cycles.Where( x => x.tag == "daytime").ToList().Count == 0 )
        {
            Debug.LogWarning( "No cycle has 'daytime' tag. Using the first one in the list..." );
        }
    }

    public void Update ( )
    {
        fufilness = frames / currCycle.length;
        cam.backgroundColor = currCycle.Gradient( fufilness );
    }

    public void FixedUpdate ( )
    {
        frames++;
        if ( frames > currCycle.length ) 
        { 
            frames = 0;
            currCycle = currCycle.NewSelected( cycles );
        }
    }

}
