using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    float position = 0;
    public float speed = 0.02f;
    public bool forwards = true;
    Vector3 origin;
    public Vector3 start;
    public Vector3 end;

    public void Start ( )
    {
        origin = transform.position;
    }
    public void Update ( )
    {
        position += ( forwards ? speed : -speed ) / (start.magnitude + end.magnitude);
        if ( position > 1 ) forwards = false;
        if ( position < 0 ) forwards = true;

        transform.position = Vector3.Lerp( start + origin, end + origin, position );
    }

    public void OnDrawGizmos ( )
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(start + transform.position, end + transform.position);
    }


}
