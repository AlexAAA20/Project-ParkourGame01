using System.Collections.Generic;
using UnityEngine;

public class LootBaggy : MonoBehaviour, IUsable
{
    public List<DummyEnemy.DropItem> drops;
    public float reloadTime = 120;
    public float timeBoost = 6;
    bool used = false;
    float currentTime = 0;
    float progressbarTime = 0;
    string baseName = "";
    PickUpAble pua = null;

    public void Start ( )
    {
        baseName = name;
        pua = GetComponent<PickUpAble>();
    }

    public void Update ( )
    {
        if ( used )
        {
            currentTime += Time.deltaTime;
            progressbarTime -= Time.deltaTime;
            if ( progressbarTime < 0 ) progressbarTime = 0;
            if ( currentTime > reloadTime )
            {
                currentTime = 0;
                used = false;
            }
            float percent = (currentTime / reloadTime);
            if ( progressbarTime > 0 )
            {
                float barsTotal = 12;
                // how much is each bar?
                float timePerBar = reloadTime / barsTotal;
                // how many bars total are written?
                int barsToWrite = Mathf.RoundToInt(currentTime / timePerBar);
                int emptyspace = (int)barsTotal - barsToWrite;
                name = $"[{new string('|', barsToWrite)}{new string( ' ', emptyspace )}]";
            }
            else
            {
                name = baseName + $" L";
            }
            pua.bgColor = new Color(1 - percent, 0.4f, percent);
        }
        else
        {
            name = baseName + " +";
            pua.bgColor = new Color( 0.4f, 1f, 0.4f );
        }
    }

    public void WhenHeldAction ( PlayerMain main )
    {
        Update( ); 
    }
    public void Use(PlayerMain main)
    {
        Update( );
        if ( !used )
        {
            used = true;
            currentTime = 0;
            Debug.Log( $"<color=green>WAHHH!!" );
            float rizz = Random.Range(0f, 100f);
            foreach (var item in drops)
            {
                Debug.Log( $"<color=yellow>thing!" );
                if ( rizz >= item.min && rizz <= item.max )
                {
                    Debug.Log( $"<color=green>yess!" );
                    GameObject thing = Instantiate( item.obj, null );
                    thing.transform.position = main.pm.transform.position;
                    thing.name = thing.name.Replace( "(Clone)", "" );
                }
            }
        }
        else
        {
            progressbarTime = 3;
            currentTime += timeBoost * Random.Range( 0.7f, 1.5f );
        }
    }
}
