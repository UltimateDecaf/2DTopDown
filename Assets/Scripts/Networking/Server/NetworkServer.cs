using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

//Based on Nathan Farrer's project
public class NetworkServer 
{
    private NetworkManager networkManager;
    private Dictionary<ulong, string> clientIdToAuth = new Dictionary<ulong, string>();
    private Dictionary<string, PlayerData> authIdToPlayerData = new Dictionary<string, PlayerData>();
    public NetworkServer(NetworkManager networkManager) //this is a constructor for this class that sets the reference to NetworkManger and adds two methods to listen to 
    {
        this.networkManager = networkManager;

        networkManager.ConnectionApprovalCallback += ApprovalCheck;
        networkManager.OnServerStarted += OnNetworkReady;
    }

    private void ApprovalCheck(                                   //we do the connection approval here, it approves it, receives player's authentication id, and data from player data class
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

    private void OnNetworkReady()                       // When the network connection is set up, we add the method to listen to (when the client disconnects)
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
    public PlayerData GetPlayerDataUsingClientId(ulong clientId)
    {
        string authId = clientIdToAuth[clientId];
        if (authId == null) return null;
        if(authIdToPlayerData.TryGetValue(authId, out PlayerData playerData))
        {
            return playerData;
        }
        else
        {
            return null;
        }
    }

    public void Dispose()
    {
        if (networkManager == null) { return; }

        networkManager.ConnectionApprovalCallback -= ApprovalCheck;
        networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
        networkManager.OnServerStarted -= OnNetworkReady;

        if (networkManager.IsListening)
        {
            networkManager.Shutdown();
        }
    }
}
