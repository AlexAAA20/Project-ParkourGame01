using UnityEngine;

public class Attacher : MonoBehaviour
{
    public Transform attatchedTo;
    public bool on = true;
    public RotationSavingMethod rotation;

    public enum RotationSavingMethod
    {
        Ignore = 1,
        Keep = 4,
        Zero = 0,
        Track = 5,
        TrackWOffset = 6
    }
    Vector3 margin = Vector3.zero;
    Vector3 rotationMargin = Vector3.zero;

    public void Start ( )
    {
        margin = transform.position - attatchedTo.position;
        switch ( rotation )
        {
            case RotationSavingMethod.Keep:
                rotationMargin = transform.eulerAngles;
                break;
            case RotationSavingMethod.Ignore:
            case RotationSavingMethod.Zero:
            case RotationSavingMethod.Track:
                break;
            case RotationSavingMethod.TrackWOffset:
                rotationMargin = transform.eulerAngles - attatchedTo.eulerAngles;
                break;
        }
    }

    public void Update ( )
    {
        transform.position = attatchedTo.position + margin;
        switch ( rotation )
        {
            case RotationSavingMethod.Ignore:
                break;
            case RotationSavingMethod.Keep:
                transform.eulerAngles = rotationMargin;
                break;
            case RotationSavingMethod.Zero:
                transform.eulerAngles = Vector3.zero;
                break;
            case RotationSavingMethod.Track:
                transform.eulerAngles = attatchedTo.eulerAngles;
                break;
            case RotationSavingMethod.TrackWOffset:
                transform.eulerAngles = attatchedTo.eulerAngles + rotationMargin;
                break;
            default:
                break;
        }
    }
}
