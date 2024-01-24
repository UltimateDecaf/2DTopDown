using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

//Based on Nathan Farrer's 'MainMenu.cs' script: https://gitlab.com/GameDevTV/unity-multiplayer/unity-multiplayer/-/blob/main/Assets/Scripts/UI/MainMenu.cs?ref_type=heads

// This script has methods that are used by main menu buttons.
public class MainMenu : MonoBehaviour
{

    [SerializeField] private TMP_InputField joinCodeField;
    public const string MenuName = "Menu";
    public async void StartHost()
    {
        await HostSingleton.Instance.GameManager.StartHostAsync(); 
    }

    public async void StartClient()
    {
        await ClientSingleton.Instance.GameManager.StartClientAsync(joinCodeField.text);
    }


    public void LeaveGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            HostSingleton.Instance.GameManager.Shutdown();
        }

        ClientSingleton.Instance.GameManager.Disconnect();
    }
}
