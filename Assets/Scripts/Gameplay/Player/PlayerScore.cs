using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Leaderboards;
using UnityEngine;

public class PlayerScore : NetworkBehaviour
{
    public NetworkVariable<int> score;
    [SerializeField] private GameObject scoreUI;
    [SerializeField] private PlayerNameGetter playerNameGetter; 
   // public NetworkVariable<int> missedShots;
   // public NetworkVariable<int> landedShots;
    public float accuracy;
    public new FixedString32Bytes name;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            score.Value = 0;
            name = playerNameGetter.PlayerName.Value;
        }
        if (!IsOwner)
        {
            scoreUI.SetActive(false);
        }
        
    }
    public override void OnNetworkDespawn()
    {
        
    }

    public void ModifyPlayerScore(int scoreToAdd)
    {
        score.Value += scoreToAdd;
    }


}
