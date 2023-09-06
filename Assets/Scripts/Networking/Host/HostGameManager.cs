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
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostGameManager  //this is the game manager on host's side (host is both, client and a server)
{
    private Allocation allocation;
    public string JoinCode { get; private set; }
    public readonly int MaxConnections = 4; 
    private const string GameSceneName = "Game";
    private SessionLeaderboard sessionLeaderboard;
    public NetworkServer NetworkServer { get; private set; }

   


    public async Task StartHostAsync() //this method runs when "Host a Game button is pressed"
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

        try //creating a "game room" and generating a join code for it
        {
            JoinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(JoinCode);

        }
        catch (Exception exception)
        {
            Debug.LogError("Relay could not create a joincode " + exception.Message);
            throw;
        }

        RelayServerData relayServerData = new RelayServerData(allocation, "dtls"); //allocate a relay for this room
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkServer = new NetworkServer(NetworkManager.Singleton); //network server is created with reference to the networkmanager instance already active

        PlayerData playerData = new PlayerData //creating playerData variable to store player's name and authentication id, data is stored
        {
            playerName = PlayerPrefs.GetString(NameSelector.PlayerNameKey, "NoName"),
            playerAuthId = AuthenticationService.Instance.PlayerId
        };

        string payload = JsonUtility.ToJson(playerData); //serialize the data to create a payload
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes; //send the payload
        NetworkManager.Singleton.StartHost(); //starting host

        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single); //load the game scene

        CoroutinePerformer.Instance.StartCoroutine(CheckForSessionLeaderboardInitialization()); //this is where coroutine to get the sessionleaderboard starts
   
         NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected; //start listening to action, execute OnClientConnected method when action is performed
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected; //start listening to action, execute OnClientDisconnected method when action is performed

    }

    private IEnumerator CheckForSessionLeaderboardInitialization() //try getting sessionLeaderboard instance until it receives one
    {
        while (sessionLeaderboard == null)
        {
            sessionLeaderboard = SessionLeaderboard.Instance;
            yield return new WaitForSeconds(0.1f); // wait for a short time before checking again
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        sessionLeaderboard.ClientDisconnected(clientId);
    }

    private void OnClientConnected(ulong clientId)
    {
        sessionLeaderboard.ClientConnected(clientId);
    }

    public void Dispose()
    {
        Shutdown();
    }

    public async void Shutdown()
    { 
        NetworkServer?.Dispose();
    }
}
