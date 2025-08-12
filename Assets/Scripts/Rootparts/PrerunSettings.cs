using UnityEngine;

public class PrerunSettings : MonoBehaviour
{
    public float time = 300f;

    void Start ( )
    {
        Application.targetFrameRate = 60;
    }

}