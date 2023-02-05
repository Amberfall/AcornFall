using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

/// <summary>
/// Gets and recieves data on the server.
/// </summary>
public class ServerNetworking : NetworkBehaviour
{
    private int _fails = 0;
    private int _wins = 0;

    private readonly NetworkVariable<ServerData> _serverDataNetworkVar =
        new NetworkVariable<ServerData>();


    public int Fails { get => _fails; private set => _fails = value; }
    public int Wins { get => _wins; private set => _wins = value; }
    public ServerData ServerData { get => _serverDataNetworkVar.Value; }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            ReadState();
            var tc = FindObjectOfType<TitleController>();
            if (tc != null)
                tc.OnServerConnected();

            //_serverDataNetworkVar.cha
        }
        else
        {

        }

        DontDestroyOnLoad(this);
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

    }

    [ServerRpc(RequireOwnership = false)]
    private void ClientRecordWinServerRPC()
    {
        Wins++;
        WriteState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ClientRecordFailServerRPC(Vector2Int locOfDeath)
    {
        Fails++;
        WriteState();
    }

    void WriteState()
    {
        Debug.Log("Writing State");
        _serverDataNetworkVar.Value = new ServerData()
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


    }

}

public struct ServerData : INetworkSerializable
{
    public int Fails;
    public int Wins;


    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Fails);
        serializer.SerializeValue(ref Wins);
    }


}