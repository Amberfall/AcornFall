using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Netcode.Transports.WebSocket;
using UnityEngine;



public class NetworkLoader : MonoBehaviour
{
    public bool EditorCreateOurOwnHost = false;

    private void Start()
    {
#if UNITY_EDITOR

        if (EditorCreateOurOwnHost)
        {
            // testing is in place, no need to connect to the cloud.
            // we can instead be our own server
            Debug.Log("Detected editor start. Creating our own local server and connecting to it");
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            var transport = NetworkManager.Singleton.GetComponent<WebSocketTransport>();
            transport.ConnectAddress = "ggj.skipsabeatmusic.com";
            NetworkManager.Singleton.StartClient();
        }
#elif UNITY_SERVER
        // this is dumb, but it works, so it's not dumb.
        // except Unity is dumb.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 10;   

        var transport = NetworkManager.Singleton.GetComponent<WebSocketTransport>();
        transport.ConnectAddress = "localhost";
        NetworkManager.Singleton.StartServer();
#elif UNITY_WEBGL
        // auto connect to the server
        var transport = NetworkManager.Singleton.GetComponent<WebSocketTransport>();
        transport.ConnectAddress = "34.105.51.253";
        var success = NetworkManager.Singleton.StartClient();
        if (success == false)
        {
            Debug.LogError("Could not connect to server");
        }
#endif

    }

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    //static void Bootstrap()
    //{
    //    Debug.Log("Instantiating Network Objects");
    //    var manager = Object.Instantiate(Resources.Load("NetworkManagerPrefab"));
    //    Object.DontDestroyOnLoad(manager);

    //    var serverData = Object.Instantiate(Resources.Load("ServerDataPrefab"));
    //    Object.DontDestroyOnLoad(serverData);
    //}
}