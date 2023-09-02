using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UIManager :MonoBehaviour
{
    public string joinCode;
    [SerializeField] private TextMeshProUGUI joinCodeField;

    private void Start()
    { 
        joinCodeField.text = HostSingleton.Instance.GameManager.JoinCode;
    }
}
