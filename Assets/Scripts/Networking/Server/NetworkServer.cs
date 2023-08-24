using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer 
{
    private NetworkManager networkManager;
    private Dictionary<ulong, string> clientIdToAuth = new Dictionary<ulong, string>();
    private Dictionary<string, PlayerData> authIdToPlayerData = new Dictionary<string, PlayerData>();
    public NetworkServer(NetworkManager networkManager)
    {
        this.networkManager = networkManager;

        networkManager.ConnectionApprovalCallback += ApprovalCheck;
        networkManager.OnServerStarted += OnNetworkReady;
    }

    private void ApprovalCheck(
        NetworkManager.ConnectionApprovalRequest request, 
        NetworkManager.ConnectionApprovalResponse response)
    {
        string payload = System.Text.Encoding.UTF8.GetString(request.Payload);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(payload);

        clientIdToAuth[request.ClientNetworkId] = playerData.playerAuthId;
        authIdToPlayerData[playerData.playerAuthId] = playerData;

        response.Approved = true;
        response.CreatePlayerObject = true;

    }

    private void OnNetworkReady()
    {
        networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if(clientIdToAuth.TryGetValue(clientId, out string authId))
        {
            clientIdToAuth.Remove(clientId);
            authIdToPlayerData.Remove(authId);
        }
    }
}
