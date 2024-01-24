using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

//Based on Nathan Farrer's 'PlayerAiming.cs' script: https://gitlab.com/GameDevTV/unity-multiplayer/unity-multiplayer/-/blob/main/Assets/Scripts/Core/Player/PlayerAiming.cs 
public class PlayerAim : NetworkBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private InputReader inputReader;

    private void LateUpdate()
    {
        if (IsOwner)
        {
            Vector2 mouseScreenPos = inputReader.LookPosition;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            playerBody.up = new Vector2(mouseWorldPos.x - playerBody.position.x, mouseWorldPos.y - playerBody.position.y);
        }
    }
}
