using System.Collections.Generic;
using Unity.Netcode;

public class CharacterSelect : NetworkBehaviour
{
    public static CharacterSelect Instance { get; private set; }

    Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake()
    {
        Instance = this;

        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRPC(ServerRpcParams serverRpcParams = default)
    {
        //Set the current client ready state to true
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;

        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playerReadyDictionary.ContainsKey(clientId) == false || playerReadyDictionary[clientId] == false)
            {
                //Client is not ready
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            Loader.NetworkLoad(Loader.Scene.Ran_Game_Scene);
        }
    }
}
