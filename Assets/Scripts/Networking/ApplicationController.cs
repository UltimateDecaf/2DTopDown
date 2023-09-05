using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    /*<summary>
     * ApplicationController handles creation of client's and host's gamemanagers during networkboot scene. 
    </summary>*/
    [SerializeField] private ClientSingleton clientPrefab;
    [SerializeField] private HostSingleton hostPrefab;
    [SerializeField] private CoroutinePerformer coroutinePerformerPrefab;

   private async void Start()
    {
        DontDestroyOnLoad(gameObject); //ensure that the object would not be destroyed in the process

        if(CoroutinePerformer.Instance == null) //coroutine performer allows using coroutines in regular C# classes, might need it or might delete it later
        {
            Instantiate(coroutinePerformerPrefab);
        }

        await LaunchInMode();
    }

    private async Task LaunchInMode()
    {

        HostSingleton hostSingleton = Instantiate(hostPrefab); //instantiates host game manager
        hostSingleton.CreateHost();
        ClientSingleton clientSingleton = Instantiate(clientPrefab); //instantiates client game manager

        bool authenticated = await clientSingleton.CreateClient(); 

        if (authenticated)
        {
            //by the time main menu is loaded, we have instantiated Host Game Manager and Client Game Manager, they are alreay active
            clientSingleton.GameManager.GoToMenu();
        }


    }

}
