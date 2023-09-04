using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInput;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{

    public event Action<bool> FireEvent;
    public event Action<Vector2> MoveEvent;
    public event Action<bool> ShowLeaderboardEvent;
    public Vector2 LookPosition {  get; private set; } 

    private PlayerInput playerInput;

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();
            playerInput.Player.SetCallbacks(this);
        }

        playerInput.Player.Enable();
    }
    public void OnFire(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireEvent?.Invoke(true);
        }
        else if(context.canceled)
        { 
            FireEvent?.Invoke(false); 
        }

        
        
    }

    public void OnLook(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        LookPosition = context.ReadValue<Vector2>();    
    }

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnUI(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            ShowLeaderboardEvent?.Invoke(true);
        } else if (context.canceled)
        {
            ShowLeaderboardEvent?.Invoke(false);
        }
    }
}
