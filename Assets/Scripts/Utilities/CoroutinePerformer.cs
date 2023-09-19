using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Created by Lari Basangov
/// <summary>
/// This class allows running Coroutines in C# classes that do not inherit from MonoBehaviour
/// </summary>
public class CoroutinePerformer : MonoBehaviour
{
    public static CoroutinePerformer Instance { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
