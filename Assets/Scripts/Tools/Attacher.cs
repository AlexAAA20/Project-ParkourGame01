using UnityEngine;

public class Attacher : MonoBehaviour
{
    public Transform attatchedTo;
    public bool on = true;
    Vector3 margin = Vector3.zero;

    private void Awake ( )
    {
        margin = transform.position - attatchedTo.position;
    }

    private void Update ( )
    {
        transform.position = attatchedTo.position + margin;
    }
}
