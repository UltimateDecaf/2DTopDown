using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUIShow : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject LeaderboardUI;

    public NetworkObject PlayerNetworkObject => GetComponent<NetworkObject>();  
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            inputReader.ShowLeaderboardEvent += ShowLeaderboard;
           
           SessionLeaderboard.Instance?.RegisterPlayer(PlayerNetworkObject, LeaderboardUI.GetComponent<LeaderboardUI>());
           
            
        }
    }



    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            inputReader.ShowLeaderboardEvent -= ShowLeaderboard;
            StartCoroutine(RegisterPlayerWhenSessionLeaderboardReady());
        }
    }

    private void ShowLeaderboard(bool showLeaderboard)
    {
        if (IsOwner)
        {
            LeaderboardUI.SetActive(showLeaderboard);
        }
    }

    private IEnumerator RegisterPlayerWhenSessionLeaderboardReady()
    {
        while(SessionLeaderboard.Instance == null)
        {
            yield return null;
        }
        SessionLeaderboard.Instance.UnregisterPlayer(PlayerNetworkObject);
    }
}
