using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CoroutinePerformer : MonoBehaviour
{
    public static CoroutinePerformer Instance { get; private set; }
    // Start is called before the first frame update
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
