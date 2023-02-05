using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

/// <summary>
/// Gets and recieves data on the server.
/// </summary>
public class ServerNetworking : NetworkBehaviour
{
    private int _fails = 0;
    private int _wins = 0;
    private List<Vector2Int> _bonuses = new List<Vector2Int>();

    private readonly NetworkVariable<ServerData> _serverDataNetworkVar =
        new NetworkVariable<ServerData>();


    public int Fails { get => _fails; private set => _fails = value; }
    public int Wins { get => _wins; private set => _wins = value; }
    public List<Vector2Int> Bonuses { get => _bonuses; private set => _bonuses = value; }
    public ServerData ServerData { get => _serverDataNetworkVar.Value; }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            ReadState();
            var tc = FindObjectOfType<TitleController>();
            if (tc != null)
                tc.OnServerConnected();

            _serverDataNetworkVar.OnValueChanged += NetworkVarChanged;
        }
        else if (IsHost)
        {
            var tc = FindObjectOfType<TitleController>();
            if (tc != null)
                tc.OnServerConnected();
        }
        else
        {
            
        }

        DontDestroyOnLoad(this);
    }

    private void NetworkVarChanged(ServerData previousValue, ServerData newValue)
    {
        Fails = _serverDataNetworkVar.Value.Fails;
        Wins = _serverDataNetworkVar.Value.Wins;

        Bonuses.Clear();
        Bonuses.AddRange(_serverDataNetworkVar.Value.Bonuses);
    }

    public void RecordWin()
    {
        ClientRecordWinServerRPC();
    }

    public void RecordLoss(Vector2Int locOfDeath)
    {
        ClientRecordFailServerRPC(locOfDeath);
    }

    public void ConsumeBonus(Vector2Int loc)
    {
        ClientConsumeBonusServerRPC(loc);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ClientRecordWinServerRPC()
    {
        Wins++;
        WriteState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ClientRecordFailServerRPC(Vector2Int loc)
    {
        Fails++;

        if (_bonuses.Contains(loc) == false)
        {
            _bonuses.Add(loc);
        }

        WriteState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ClientConsumeBonusServerRPC(Vector2Int loc)
    {
        if (_bonuses.Contains(loc))
        {
            _bonuses.Remove(loc);
            WriteState();
        }
    }

    void WriteState()
    {
        Debug.Log("Writing State");
        _serverDataNetworkVar.Value = new ServerData(Bonuses)
        {
            Fails = this.Fails,
            Wins = this.Wins            
        };            
    }

    [ContextMenu("Force Read State")]
    public void ReadState()
    {
        Debug.Log("Reading State");
        Fails = _serverDataNetworkVar.Value.Fails;
        Wins = _serverDataNetworkVar.Value.Wins;

        Bonuses.Clear();
        Bonuses.AddRange(_serverDataNetworkVar.Value.Bonuses);
    }

}

public struct ServerData : INetworkSerializable
{
    public int Fails;
    public int Wins;
    public Vector2Int[] Bonuses;

    public ServerData(List<Vector2Int> bonusList)
    {
        Fails = 0;
        Wins = 0;
        Bonuses = bonusList.ToArray();
    }


    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Fails);
        serializer.SerializeValue(ref Wins);

        if (Bonuses == null)
            Bonuses = new Vector2Int[0] { };

        serializer.SerializeValue(ref Bonuses);
    }


}