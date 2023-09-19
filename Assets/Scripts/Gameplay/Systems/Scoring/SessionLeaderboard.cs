using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

//SessionLeaderboard class is a singleton that updates players' scores and updates leaderboard UI for all players
//Created by Lari Basangov
public class SessionLeaderboard : NetworkBehaviour
{
    public static SessionLeaderboard Instance;
    [SerializeField] private LeaderboardUI leaderboardUI;
    public Dictionary<FixedString32Bytes, PlayerScore> nameToPlayerScores = new Dictionary<FixedString32Bytes, PlayerScore>();
    [SerializeField] private int MaxConnections = 4;
<<<<<<< HEAD

    public event Action<ulong> OnClientConnectedEvent;
    public event Action<ulong> OnClientDisconnectedEvent;
=======
    public Dictionary<NetworkObject, LeaderboardUI> playerToLeaderboardUI = new Dictionary<NetworkObject, LeaderboardUI>();
    public event Action<ulong> OnClientConnect;
    public event Action<ulong> OnClientDisconnect;
>>>>>>> sessionleaderboard-fixes



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
<<<<<<< HEAD
           
            UpdatePlayersDictionary();
        }
      

        
=======
            UpdatePlayersDictionary();
        }
            
       
>>>>>>> sessionleaderboard-fixes
    }
  
    void Update()
    {
        if (IsServer)
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
            Debug.Log("START ITERATION!");

            ulong playerId = players[k].GetComponent<NetworkObject>().OwnerClientId;
            PlayerData playerData =
                HostSingleton.Instance.GameManager.NetworkServer.GetPlayerDataUsingClientId(playerId);
            FixedString32Bytes name = playerData.playerName;
            if(nameToPlayerScores.ContainsKey(name)) { continue; }
            players[k].GetComponent<PlayerScore>().OnScoreChanged += OnScoreUpdate;
            nameToPlayerScores.Add(name, players[k].GetComponent<PlayerScore>());
            
            Debug.Log(name + " added to leaderboard script");
        }
    }

    public void UpdateLeaderboardUI()
    {
        List<KeyValuePair<FixedString32Bytes, PlayerScore>> sortedScores = new List<KeyValuePair<FixedString32Bytes, PlayerScore>>(nameToPlayerScores);

        sortedScores.Sort((pair1, pair2) => pair2.Value.score.Value.CompareTo(pair1.Value.score.Value));
<<<<<<< HEAD
        leaderboardUI.UpdateLeaderboardUI(sortedScores);    
=======

        UpdatePlayersLeaderboards(sortedScores);
     
>>>>>>> sessionleaderboard-fixes
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

<<<<<<< HEAD
=======
    public void ClientConnected(ulong clientId)
    {
        OnClientConnect?.Invoke(clientId);
    }

    public void ClientDisconnected(ulong clientId) 
    { 
        OnClientDisconnect?.Invoke(clientId); 
    }

    public void RegisterPlayer(NetworkObject playerNetworkObject, LeaderboardUI leaderboardUI)
    {
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
   
        if(playerToLeaderboardUI.Count > 0) 
        {
            foreach (var entry in playerToLeaderboardUI)
            {
                LeaderboardUI playerLeaderboardUI = entry.Value;
                if (playerLeaderboardUI != null)
                {
                    playerLeaderboardUI.UpdateLeaderboardUI(sortedScores);
                }
               
            }
        }
      
    }
>>>>>>> sessionleaderboard-fixes
}
