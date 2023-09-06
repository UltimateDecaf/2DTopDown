using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SessionLeaderboard : NetworkBehaviour
{
    public static SessionLeaderboard Instance;
    public Dictionary<FixedString32Bytes, PlayerScore> nameToPlayerScores = new Dictionary<FixedString32Bytes, PlayerScore>();
    [SerializeField] private int MaxConnections = 4;
    public Dictionary<NetworkObject, LeaderboardUI> playerToLeaderboardUI = new Dictionary<NetworkObject, LeaderboardUI>();
    public event Action<ulong> OnClientConnect;
    public event Action<ulong> OnClientDisconnect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("Instance of SessionLeaderboard is there!");
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

        OnClientConnect += ClientConnected;
        OnClientDisconnect += ClientDisconnected;
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
        if (nameToPlayerScores.Count == MaxConnections) { return; }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int k = 0; k < players.Length; k++)
        {
            ulong playerId = players[k].GetComponent<NetworkObject>().OwnerClientId;
            PlayerData playerData = HostSingleton.Instance.GameManager.NetworkServer.GetPlayerDataUsingClientId(playerId);
            FixedString32Bytes name = playerData.playerName;
            if (nameToPlayerScores.ContainsKey(name)) { continue; }

            NetworkObject playerNetworkObject = players[k].GetComponent<NetworkObject>();
            LeaderboardUI playerLeaderboardUI = players[k].GetComponent<LeaderboardUI>();
            if (playerNetworkObject != null || playerLeaderboardUI != null)
            {
                SessionLeaderboard.Instance.RegisterPlayer(playerNetworkObject, playerLeaderboardUI);
            }
            else
            {
                Debug.Log("Some of the fields are null!");
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
        List<KeyValuePair<FixedString32Bytes, SerializablePlayerScore>> serializableSortedScores = ConvertToSerializable(sortedScores);
        UpdatePlayersLeaderboardsClientRpc(ConvertToSerializableData(serializableSortedScores));
    }

    public void OnScoreUpdate()
    {
        UpdateLeaderboardUI();
    }

    private void OnDestroy()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerScore>().OnScoreChanged -= OnScoreUpdate;
        }
    }

    public void ClientConnected(ulong clientId)
    {
        Debug.Log("CLIENT CONNECTED");
        OnClientConnect?.Invoke(clientId);
        UpdatePlayersDictionary();
        UpdateLeaderboardUI();
    }

    public void ClientDisconnected(ulong clientId)
    {
        OnClientDisconnect?.Invoke(clientId);
        NetworkObject playerNetworkObject = FindNetworkObjectByClientId(clientId);
        if (playerNetworkObject != null)
        {
            UnregisterPlayer(playerNetworkObject);
        }
        UpdateLeaderboardUI();
    }

    private NetworkObject FindNetworkObjectByClientId(ulong clientId)
    {
        var networkObjects = FindObjectsOfType<NetworkObject>();
        foreach (var networkObject in networkObjects)
        {
            if (networkObject.OwnerClientId == clientId)
            {
                return networkObject;
            }
        }
        return null;
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
        if (playerToLeaderboardUI.Count > 0)
        {
            Debug.Log("playerToLeaderboardUI.Count is greater than 0");
            foreach (var entry in playerToLeaderboardUI)
            {
                Debug.Log("Iteration x to update player's leaderboard");

                LeaderboardUI playerLeaderboardUI = entry.Value;
                if (playerLeaderboardUI != null)
                {
                    playerLeaderboardUI.UpdateLeaderboardUI(sortedScores);
                }
                Debug.Log("Your dictionary seems to have null values");
            }
        }
    }

    [Serializable]
    public class SerializablePlayerScoreData
    {
        public string[] Keys;
        public SerializablePlayerScore[] Values;
    }

    public SerializablePlayerScoreData ConvertToSerializableData(List<KeyValuePair<FixedString32Bytes, SerializablePlayerScore>> sortedScores)
    {
        var data = new SerializablePlayerScoreData
        {
            Keys = new string[sortedScores.Count],
            Values = new SerializablePlayerScore[sortedScores.Count]
        };

        for (int i = 0; i < sortedScores.Count; i++)
        {
            data.Keys[i] = sortedScores[i].Key.ToString();
            data.Values[i] = sortedScores[i].Value;
        }

        return data;
    }

    private List<KeyValuePair<FixedString32Bytes, SerializablePlayerScore>> ConvertToSerializable(List<KeyValuePair<FixedString32Bytes, PlayerScore>> sortedScores)
    {
        List<KeyValuePair<FixedString32Bytes, SerializablePlayerScore>> serializableList = new List<KeyValuePair<FixedString32Bytes, SerializablePlayerScore>>();

        foreach (var scoreData in sortedScores)
        {
            SerializablePlayerScore serializablePlayerScore = new SerializablePlayerScore(scoreData.Value);
            KeyValuePair<FixedString32Bytes, SerializablePlayerScore> pair = new KeyValuePair<FixedString32Bytes, SerializablePlayerScore>(scoreData.Key, serializablePlayerScore);
            serializableList.Add(pair);
        }

        return serializableList;
    }

    private List<KeyValuePair<FixedString32Bytes, PlayerScore>> ConvertToDeserializable(List<KeyValuePair<FixedString32Bytes, SerializablePlayerScore>> sortedScores)
    {
        List<KeyValuePair<FixedString32Bytes, PlayerScore>> originalList = new List<KeyValuePair<FixedString32Bytes, PlayerScore>>();

        foreach (var scoreData in sortedScores)
        {
            PlayerScore originalPlayerScore = scoreData.Value.ToPlayerScore();
            KeyValuePair<FixedString32Bytes, PlayerScore> pair = new KeyValuePair<FixedString32Bytes, PlayerScore>(scoreData.Key, originalPlayerScore);
            originalList.Add(pair);
        }

        return originalList;
    }

    [ServerRpc]
    public void UpdatePlayersLeaderboardsServerRpc(SerializablePlayerScoreData serializablePlayerScoreData)
    {
        Debug.Log("UpdatePlayersLeaderboardsServerRpc called");
        List<KeyValuePair<FixedString32Bytes, SerializablePlayerScore>> sortedScores = new List<KeyValuePair<FixedString32Bytes, SerializablePlayerScore>>();

        for (int i = 0; i < serializablePlayerScoreData.Keys.Length; i++)
        {
            FixedString32Bytes key = new FixedString32Bytes(serializablePlayerScoreData.Keys[i]);
            SerializablePlayerScore value = serializablePlayerScoreData.Values[i];
            sortedScores.Add(new KeyValuePair<FixedString32Bytes, SerializablePlayerScore>(key, value));
        }

        UpdatePlayersLeaderboards(ConvertToDeserializable(sortedScores));
    }

    [ClientRpc]
    public void UpdatePlayersLeaderboardsClientRpc(SerializablePlayerScoreData serializablePlayerScoreData)
    {
        Debug.Log("UpdatePlayersLeaderboardsClientRpc called");
        List<KeyValuePair<FixedString32Bytes, SerializablePlayerScore>> sortedScores = new List<KeyValuePair<FixedString32Bytes, SerializablePlayerScore>>();

        for (int i = 0; i < serializablePlayerScoreData.Keys.Length; i++)
        {
            FixedString32Bytes key = new FixedString32Bytes(serializablePlayerScoreData.Keys[i]);
            SerializablePlayerScore value = serializablePlayerScoreData.Values[i];
            sortedScores.Add(new KeyValuePair<FixedString32Bytes, SerializablePlayerScore>(key, value));
        }

        UpdatePlayersLeaderboards(ConvertToDeserializable(sortedScores));
    }
}