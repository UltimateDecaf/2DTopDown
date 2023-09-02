using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SessionLeaderboard : NetworkBehaviour
{
    public SessionLeaderboard Instance;
    public Dictionary<string, PlayerScore> PlayerNamesToPlayerScores = new Dictionary<string, PlayerScore>();

    public override void OnNetworkSpawn()
    {
        Instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
