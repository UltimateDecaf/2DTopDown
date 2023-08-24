using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostGameManager
{
    private Allocation allocation;
    private string joinCode;
    private const int MaxConnections = 4;
    private const string GameSceneName = "Game";
    private NetworkServer networkServer;
  public async Task StartHostAsync()
    {
        try
        {
            allocation = await Relay.Instance.CreateAllocationAsync(MaxConnections);
        }
        catch(Exception exception) 
        {
            Debug.Log(exception);
            return;
        }

        try
        {
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);

        }
        catch (Exception exception)
        {
            Debug.Log(exception);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation, "udp");
        transport.SetRelayServerData(relayServerData);
        networkServer = new NetworkServer(NetworkManager.Singleton);

        PlayerData playerData = new PlayerData
        {
            playerName = PlayerPrefs.GetString(NameSelector.PlayerNameKey, "NoName"),
            playerAuthId = AuthenticationService.Instance.PlayerId
        };

        string payload = JsonUtility.ToJson(playerData);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
        NetworkManager.Singleton.StartHost();

        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
    }

    public string GetJoinCode()
    {
        return joinCode;
    }

}
