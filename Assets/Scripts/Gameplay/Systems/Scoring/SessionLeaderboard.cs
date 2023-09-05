using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;
using UnityEngine.Events;

public class SessionLeaderboard : NetworkBehaviour
{
    public static SessionLeaderboard Instance;
    public Dictionary<FixedString32Bytes, PlayerScore> nameToPlayerScores = new Dictionary<FixedString32Bytes, PlayerScore>();
    [SerializeField] private int MaxConnections = 4;
    public Dictionary<NetworkObject, LeaderboardUI> playerToLeaderboardUI = new Dictionary<NetworkObject, LeaderboardUI>();
    public event Action<ulong> OnClientConnectedEvent;
    public event Action<ulong> OnClientDisconnectedEvent;



    //On start, add host to the dictionary
    //On clients connect, add clients to dictionary

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {  
            UpdatePlayersDictionary();
        }
        
    }
  
    void Update()
    {
        if(IsServer)
        {
            UpdatePlayersDictionary();
        }
        
    }

   void UpdatePlayersDictionary()
    {
        if(nameToPlayerScores.Count == MaxConnections) { return; }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int k = 0; k < players.Length; k++)
        { 

            ulong playerId = players[k].GetComponent<NetworkObject>().OwnerClientId;
            PlayerData playerData =
                HostSingleton.Instance.GameManager.NetworkServer.GetPlayerDataUsingClientId(playerId);
            FixedString32Bytes name = playerData.playerName;
            if(nameToPlayerScores.ContainsKey(name)) { continue; }

            NetworkObject playerNetworkObject = players[k].GetComponent<NetworkObject>();
            LeaderboardUI playerLeaderboardUI = players[k].GetComponent<LeaderboardUI>();
            if(playerNetworkObject != null || playerLeaderboardUI != null) 
            { SessionLeaderboard.Instance.RegisterPlayer(playerNetworkObject, playerLeaderboardUI);
            } 
            else
            {
                Debug.Log("some of the fields are null!");
            }
     
            players[k].GetComponent<PlayerScore>().OnScoreChanged += OnScoreUpdate;
            nameToPlayerScores.Add(name, players[k].GetComponent<PlayerScore>());
            
            Debug.Log(name + " added to leaderboard script");
        }
    }

    public void UpdateLeaderboardUI()
    {
        List<KeyValuePair<FixedString32Bytes, PlayerScore>> sortedScores = new List<KeyValuePair<FixedString32Bytes, PlayerScore>>(nameToPlayerScores);

        sortedScores.Sort((pair1, pair2) => pair2.Value.score.Value.CompareTo(pair1.Value.score.Value));
        Debug.Log("Sorted Scores Count: " + sortedScores.Count);

        UpdatePlayersLeaderboards(sortedScores);
     
    }
   
    public void OnScoreUpdate()
    {
        UpdateLeaderboardUI();
    }

    private void OnDestroy()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerScore>().OnScoreChanged -= OnScoreUpdate;
        }
    }

    public void ClientConnected(ulong clientId)
    {
        OnClientConnectedEvent?.Invoke(clientId);
    }

    public void ClientDisconnected(ulong clientId) 
    { 
        OnClientDisconnectedEvent?.Invoke(clientId); 
    }

    public void RegisterPlayer(NetworkObject playerNetworkObject, LeaderboardUI leaderboardUI)
    {
        Debug.Log("RegisterPlayer called with NetworkObject: " + playerNetworkObject + " and LeaderboardUI: " + leaderboardUI);
        if (!playerToLeaderboardUI.ContainsKey(playerNetworkObject))
        {
            playerToLeaderboardUI.Add(playerNetworkObject, leaderboardUI);
        }
    }

    public void UnregisterPlayer(NetworkObject playerNetworkObject)
    {
        playerToLeaderboardUI.Remove(playerNetworkObject);
    }

    public void UpdatePlayersLeaderboards(List<KeyValuePair<FixedString32Bytes, PlayerScore>> sortedScores)
    {
        Debug.Log("Session Leaderboard: UpdatePlayerLeaderboards called");
        if(playerToLeaderboardUI.Count > 0) 
        {
            Debug.Log("playerToLeaderboardUI.Count is greater than 0");
            foreach (var entry in playerToLeaderboardUI)
            {
                Debug.Log("Iteration x to update player's leaderboard");

                LeaderboardUI playerLeaderboardUI = entry.Value;
                if(playerLeaderboardUI != null)
                {
                     playerLeaderboardUI.UpdateLeaderboardUI(sortedScores);
                }
                Debug.Log("Your dictionary seem to have null values");
               
            }
        }
      
    }
}
