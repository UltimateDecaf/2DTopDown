using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
  
     // Based on Nathan Farrer's Project
     /*
      * Lari Basangov (me) has added following functionality:
      * - Instantiation of CoroutinePerformer - script that allows running coroutines in the regular C# classes
      * - Instantiation of CurrentSceneChecker - script that checks currently loaded scene
      * 
      * This functionality is needed to ensure the proper instantiation of the Session leaderboard in-game
      */
  
    [SerializeField] private ClientSingleton clientPrefab;
    [SerializeField] private HostSingleton hostPrefab;
    [SerializeField] private CoroutinePerformer coroutinePerformerPrefab;
    [SerializeField] private CurrentSceneChecker currentSceneCheckerPrefab;

   private async void Start()
    {
        DontDestroyOnLoad(gameObject); //ensure that the object would not be destroyed in the process

        if(CoroutinePerformer.Instance == null) //coroutine performer allows using coroutines in regular C# classes, might need it or might delete it later
        {
            Instantiate(coroutinePerformerPrefab);
        }

        if (CurrentSceneChecker.Instance == null)
        {
            Instantiate(currentSceneCheckerPrefab);
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
