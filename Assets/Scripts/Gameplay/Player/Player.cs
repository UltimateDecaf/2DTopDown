using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
   [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public PlayerScore PlayerScore { get; private set; }
    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();
    public static event Action<Player> OnPlayerSpawned;
    public static event Action<Player> OnPlayerDespawned;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            PlayerData playerData = HostSingleton.Instance.GameManager.NetworkServer.GetPlayerDataUsingClientId(OwnerClientId);
            PlayerName.Value = playerData.playerName;

            OnPlayerSpawned?.Invoke(this);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            OnPlayerDespawned?.Invoke(this);    
        }
    }

}
