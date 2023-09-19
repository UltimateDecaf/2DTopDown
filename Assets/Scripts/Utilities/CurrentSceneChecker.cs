using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentSceneChecker : MonoBehaviour
{
    public static CurrentSceneChecker Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
