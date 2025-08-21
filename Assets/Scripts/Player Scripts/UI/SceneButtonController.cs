using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonController : MonoBehaviour
{
    public string next;
    public string menu;

    public void Restart( )
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene( ).name, LoadSceneMode.Single );
    }
    public void Next ( )
    {
        SceneManager.LoadScene( next );
    }
    public void Menu ( )
    {
        SceneManager.LoadScene( menu );
    }
}
