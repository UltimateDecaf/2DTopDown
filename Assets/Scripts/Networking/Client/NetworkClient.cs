using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkClient
{
    //this class handles the disconnet of the client

    private NetworkManager networkManager;

    public NetworkClient(NetworkManager networkManager)
    {
        this.networkManager = networkManager;

        networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }


    private void OnClientDisconnect(ulong clientId)
    {
        if (clientId == 0 || clientId == networkManager.LocalClientId)
        {

            if (SceneManager.GetActiveScene().name != "Menu")
            {
                SceneManager.LoadScene("Menu");
            }

            if (networkManager.IsConnectedClient)
            {
                networkManager.Shutdown();

            }
        }
    }
}
