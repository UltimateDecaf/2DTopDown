using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Leaderboards;
using UnityEngine;

public class PlayerScore : NetworkBehaviour
{
    public NetworkVariable<int> score;
    

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            score.Value = 0;
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
