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

public class HostGameManager  //this is the game manager on host's side (host is both, client and a server)
{
    private Allocation allocation;
    public string JoinCode { get; private set; }
    private const int MaxConnections = 4;
    private const string GameSceneName = "Game";
    public NetworkServer NetworkServer { get; private set; }
  public async Task StartHostAsync()
    {
        try  //creating an allocation with the set amount of maxconnections
        {
            allocation = await Relay.Instance.CreateAllocationAsync(MaxConnections);
        }
        catch(Exception exception) 
        {
            Debug.LogError("Relay could not create allocation request " + exception.Message);
            throw;
        }

        try //creating a "game room and generating a join code for it"
        {
            JoinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(JoinCode);

        }
        catch (Exception exception)
        {
            Debug.LogError("Relay could not create a joincode " + exception.Message);
            throw;
        }

        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkServer = new NetworkServer(NetworkManager.Singleton);

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

}
