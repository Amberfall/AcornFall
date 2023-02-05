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
    private int _wins1 = 4;
    private int _wins2 = 1;
    private int _wins3 = 1;

    private List<Vector3Int> _bonuses = new List<Vector3Int>();

    private readonly NetworkVariable<ServerData> _serverDataNetworkVar =
        new NetworkVariable<ServerData>();


    public int Fails { get => _fails; private set => _fails = value; }
    public int Wins1 { get => _wins1; private set => _wins1 = value; }
    public int Wins2 { get => _wins2; private set => _wins2 = value; }
    public int Wins3 { get => _wins3; private set => _wins3 = value; }
    public List<Vector3Int> Bonuses { get => _bonuses; private set => _bonuses = value; }
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
            WriteState();
        }

        DontDestroyOnLoad(this);
    }

    private void NetworkVarChanged(ServerData previousValue, ServerData newValue)
    {
        Fails = _serverDataNetworkVar.Value.Fails;
        Wins1 = _serverDataNetworkVar.Value.Wins1;
        Wins2 = _serverDataNetworkVar.Value.Wins2;
        Wins3 = _serverDataNetworkVar.Value.Wins3;

        Bonuses.Clear();
        Bonuses.AddRange(_serverDataNetworkVar.Value.Bonuses);
    }

    public void RecordWin(int difficulty)
    {
        ClientRecordWinServerRPC(difficulty);
    }

    public void RecordLoss(Vector3Int locOfDeath)
    {
        ClientRecordFailServerRPC(locOfDeath);
    }

    public void ConsumeBonus(Vector3Int loc)
    {
        ClientConsumeBonusServerRPC(loc);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ClientRecordWinServerRPC(int difficulty)
    {
        if (difficulty == 1)
        {
            Wins1++;
        }
        else if (difficulty == 2)
        {
            Wins2++;
        }
        else
        {
            Wins3++;
        }
        
        WriteState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ClientRecordFailServerRPC(Vector3Int loc)
    {
        Fails++;

        if (_bonuses.Contains(loc) == false)
        {
            _bonuses.Add(loc);
        }

        WriteState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ClientConsumeBonusServerRPC(Vector3Int loc)
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
            Fails = Fails,
            Wins1 = Wins1,
            Wins2 = Wins2,
            Wins3 = Wins3,
        };            
    }

    [ContextMenu("Force Read State")]
    public void ReadState()
    {
        Debug.Log("Reading State");
        Fails = _serverDataNetworkVar.Value.Fails;
        Wins1 = _serverDataNetworkVar.Value.Wins1;
        Wins2 = _serverDataNetworkVar.Value.Wins2;
        Wins3 = _serverDataNetworkVar.Value.Wins3;

        Bonuses.Clear();
        Bonuses.AddRange(_serverDataNetworkVar.Value.Bonuses);
    }

}

public struct ServerData : INetworkSerializable
{
    public int Fails;
    public int Wins1;
    public int Wins2;
    public int Wins3;
    public Vector3Int[] Bonuses;

    public ServerData(List<Vector3Int> bonusList)
    {
        Fails = 0;
        Wins1 = 0;
        Wins2 = 0;
        Wins3 = 0;
        Bonuses = bonusList.ToArray();
    }


    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Fails);
        serializer.SerializeValue(ref Wins1);
        serializer.SerializeValue(ref Wins2);
        serializer.SerializeValue(ref Wins3);

        if (Bonuses == null)
            Bonuses = new Vector3Int[0] { };

        serializer.SerializeValue(ref Bonuses);
    }


}