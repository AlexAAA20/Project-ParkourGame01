using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementBetweenScenes : MonoBehaviour
{
    public string into = "";
    public void CallMove( )
    {
        SceneManager.LoadScene( into );
    }

    public void LeaveMove( )
    {
        Application.Quit();
    }
}
