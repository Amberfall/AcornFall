using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;



public class NetworkLoader : MonoBehaviour
{
    private void Start()
    {


#if UNITY_SERVER
        // this is dumb, but it works, so it's not dumb.
        // except Unity is dumb.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 10;   

        var ut = NetworkManager.Singleton.GetComponent<UNetTransport>();
        ut.ConnectAddress.Address = "localhost";
        NetworkManager.Singleton.StartServer();
#elif UNITY_EDITOR
        // testing is in place, no need to connect to the cloud.
        // we can instead be our own server
        Debug.Log("Detected editor start. Creating our own local server and connecting to it");
        NetworkManager.Singleton.StartHost();

#elif UNITY_WEBGL
        // auto connect to the GCP server
        try
        {
            var ut = NetworkManager.Singleton.GetComponent<UNetTransport>();
            ut.ConnectAddress = "ggj.skipsabeatmusic.com";
            NetworkManager.Singleton.StartClient();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

        DestroyImmediate(this);
#endif

    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Bootstrap()
    {
        Debug.Log("Instantiating Network Objects");
        var newObj = Object.Instantiate(Resources.Load("NetworkManagerPrefab"));
        Object.DontDestroyOnLoad(newObj);
        //DontDestroyOnLoad(newObj);
    }
}