using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

//Created by Lari Basangov
public class PlayerUIShow : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject LeaderboardUI;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            inputReader.ShowLeaderboardEvent += ShowLeaderboard;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            inputReader.ShowLeaderboardEvent -= ShowLeaderboard;
        }
    }

    private void ShowLeaderboard(bool showLeaderboard)
    {
        if (IsOwner)
        {
            LeaderboardUI.SetActive(showLeaderboard);
        }
    }
}
