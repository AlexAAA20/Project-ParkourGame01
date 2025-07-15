using System;
using System.Threading.Tasks;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public int coins = 0;
    static public Func<CoinCollector, int, Task> OnGain;
    static public Action Reset;
    static public Action<int> Set;
    public void Start ( )
    {
        Set += SetV;
        Reset += Restart;
    }
    public void Gain(int amount=1)
    {
        coins += amount;
        Debug.Log( $"gained {amount} coins" );
        OnGain?.Invoke(this, coins);
    }

    public void Restart ( ) { coins = 0; }
    public void SetV ( int to ) { coins = to; }

    public void Update ( )
    {
        if ( Input.GetKeyDown( KeyCode.V ) )
        {
            Gain( 1 );
        }


    }
}
