using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerNameGetter: NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            PlayerData playerData =
                HostSingleton.Instance.GameManager.NetworkServer.GetPlayerDataUsingClientId(OwnerClientId);

            PlayerName.Value = playerData.playerName;
        }
    }
}
