using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.CloudSave;
using UnityEngine;
using System.Collections.Generic;

public class CloudSaveScript : MonoBehaviour
{
    async void Start ( )
    {
        await UnityServices.InitializeAsync( );

        if ( !AuthenticationService.Instance.IsSignedIn )
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync( );
            Debug.Log( "Signed in as: " + AuthenticationService.Instance.PlayerId );
        }

        CoinCollector.OnGain += SaveData;

    }

    private void Update ( )
    {
        if ( Input.GetKeyDown(KeyCode.B))
        {
            LoadData( );
        }
    }

    async Task SaveData ( CoinCollector _, int coins )
    {
        Dictionary<string, object> toSave = new Dictionary<string, object>() { };
        toSave["coins"] = coins;

        Debug.Log( "Saved" );

        await CloudSaveService.Instance.Data.ForceSaveAsync( toSave );
        await CloudSaveService.Instance.Data.Player.SaveAsync( toSave );
    }

    async Task LoadData ( )
    {
        Dictionary<string, string> toSave = await CloudSaveService.Instance.Data.LoadAllAsync( );

        Debug.Log( "Loaded" );

        CoinCollector.Set?.Invoke( int.Parse( toSave["coins"] ) );

    }
}
