using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

/// <summary>
/// Gets and recieves data on the server.
/// </summary>
public class ServerNetworkData : NetworkBehaviour
{
    private int _fails = 0;
    private int _wins = 0;

    private readonly NetworkVariable<ServerDataState> _serverDataState =
        new NetworkVariable<ServerDataState>();

    public bool HasRead = false;
    public UnityEvent StateReadEvent;

    public int Fails { get => _fails; private set => _fails = value; }
    public int Wins { get => _wins; private set => _wins = value; }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            ReadState();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClientRecordWinServerRPC()
    {
        Wins++;
        WriteState();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClientRecordFailServerRPC()
    {
        Fails++;
        WriteState();
    }

    void WriteState()
    {
        Debug.Log("Writing State");
        _serverDataState.Value = new ServerDataState()
        {
            Fails = this.Fails,
            Wins = this.Wins
        };            
    }

    [ContextMenu("Force Read State")]
    public void ReadState()
    {
        Debug.Log("Reading State");
        Fails = _serverDataState.Value.Fails;
        Wins = _serverDataState.Value.Wins;

        StateReadEvent.Invoke();
        HasRead = true;
    }

    struct ServerDataState : INetworkSerializable
    {
        public int Fails;
        public int Wins;


        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Fails);
            serializer.SerializeValue(ref Wins);
        }

        
    }
}
