using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SessionLeaderboard : NetworkBehaviour
{
    public static SessionLeaderboard Instance;
    public GameObject[] players;
    public Dictionary<string, PlayerScore> nameToPlayerScores = new Dictionary<string, PlayerScore>();
    public int MaxConnections = 4;


    //On start, add host to the dictionary
    //On clients connect, add clients to dictionary
    public override void OnNetworkSpawn()
    {
        Instance = this;
        players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++)
        {
            string playerName = players[i].name;
            nameToPlayerScores.Add(playerName, players[i].GetComponent<PlayerScore>());
            Debug.Log("ADDED TO LEADERBOARD: " + nameToPlayerScores[playerName]);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
