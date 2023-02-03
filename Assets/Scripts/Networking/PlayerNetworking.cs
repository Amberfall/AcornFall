using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Gets information from the server
/// </summary>
public class PlayerNetworking : NetworkBehaviour
{
    ServerNetworkData _snd;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        _snd = FindObjectOfType<ServerNetworkData>();
    }

    [ContextMenu("Trigger Fake Win")]
    public void RecordWin()
    {
        _snd.ClientRecordWinServerRPC();
    }

    [ContextMenu("Trigger Fake Loss")]
    public void RecordLoss()
    {
        _snd.ClientRecordFailServerRPC();
    }

    public void GetNewGameInfo()
    {

    }
}
