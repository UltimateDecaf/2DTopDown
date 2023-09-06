using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    private NetworkClient networkClient;
    private JoinAllocation allocation;
    private const string MenuSceneName = "Menu";
    private SessionLeaderboard sessionLeaderboard;

    public async Task<bool> InitAsync() 
    {
        await UnityServices.InitializeAsync();

        networkClient = new NetworkClient(NetworkManager.Singleton); 

        AuthState authState = await AuthenticationWrapper.DoAuth();

        if(authState == AuthState.Authenticated)
        {
            return true;
        }

        return false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

    public async Task StartClientAsync(string joinCode)
    {
        try
        {
            allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        PlayerData playerData = new PlayerData
        {
            playerName = PlayerPrefs.GetString(NameSelector.PlayerNameKey, "NoName"),
            playerAuthId = AuthenticationService.Instance.PlayerId
        };
    
        string payload = JsonUtility.ToJson(playerData);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;

        NetworkManager.Singleton.StartClient();
        sessionLeaderboard = SessionLeaderboard.Instance;
        CoroutinePerformer.Instance.StartCoroutine(CheckForSessionLeaderboardInitialization());
    }

    private IEnumerator CheckForSessionLeaderboardInitialization()
    {
        while (!CurrentSceneChecker.Instance.GetCurrentSceneName().Equals("Game"))
        {
            yield return new WaitForSeconds(0.1f); // wait for a short time before checking again
        }

        sessionLeaderboard = SessionLeaderboard.Instance;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientDisconnected(ulong clientId)
    {
        sessionLeaderboard.ClientDisconnected(clientId);
    }

    private void OnClientConnected(ulong clientId)
    {
        sessionLeaderboard.ClientConnected(clientId);
    }

    public void Disconnect()
    {
        networkClient.Disconnect();
    }

    public void Dispose()
    {
        networkClient?.Dispose();
    }

}
