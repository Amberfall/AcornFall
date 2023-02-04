using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
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
            var ut = NetworkManager.Singleton.GetComponent<UNetTransport>();
            
            var ip = System.Net.Dns.GetHostEntry("ggj.skipsabeatmusic.com");
            //Debug.Log(ip.AddressList[0].ToString());
            ut.ConnectAddress = ip.AddressList[0].ToString();
            NetworkManager.Singleton.StartClient();
        }
#elif UNITY_SERVER
        // this is dumb, but it works, so it's not dumb.
        // except Unity is dumb.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 10;   

        var ut = NetworkManager.Singleton.GetComponent<UNetTransport>();
        ut.ConnectAddress = "localhost";
        NetworkManager.Singleton.StartServer();
#elif UNITY_WEBGL
        // auto connect to the server

        var ut = NetworkManager.Singleton.GetComponent<UNetTransport>();
        ut.ConnectAddress = "ggj.skipsabeatmusic.com";
        NetworkManager.Singleton.StartClient();

        Debug.Log(e);
#endif

    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Bootstrap()
    {
        Debug.Log("Instantiating Network Objects");
        var manager = Object.Instantiate(Resources.Load("NetworkManagerPrefab"));
        Object.DontDestroyOnLoad(manager);

        var serverData = Object.Instantiate(Resources.Load("ServerDataPrefab"));
        Object.DontDestroyOnLoad(serverData);
    }
}