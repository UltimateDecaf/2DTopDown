using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Based on Nathan Farrer's Unity Multiplayer Project: https://gitlab.com/GameDevTV/unity-multiplayer/unity-multiplayer

// This script ensures that player is following their avatar, and not the other players
// by increasing Cinemachine's Virtual Camera priority.
public class CameraForPlayer : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private int ownerPriority = 15;
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            virtualCamera.Priority = ownerPriority;
        }
    }
}
