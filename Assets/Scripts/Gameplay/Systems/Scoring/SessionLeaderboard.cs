using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class SessionLeaderboard : NetworkBehaviour
{
    public static SessionLeaderboard Instance;
    public Dictionary<FixedString32Bytes, PlayerScore> nameToPlayerScores = new Dictionary<FixedString32Bytes, PlayerScore>();
    [SerializeField] private int MaxConnections = 4;



    //On start, add host to the dictionary
    //On clients connect, add clients to dictionary
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Instance = this;
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
            Debug.Log("START ITERATION!");
            PlayerData playerData =
                HostSingleton.Instance.GameManager.NetworkServer.GetPlayerDataUsingClientId(OwnerClientId);
            FixedString32Bytes name = playerData.playerName;
            if(nameToPlayerScores.ContainsKey(name)) { continue; }
            nameToPlayerScores.Add(name, players[k].GetComponent<PlayerScore>());
            Debug.Log(name + " added to leaderboard script");
        }
    }
}
