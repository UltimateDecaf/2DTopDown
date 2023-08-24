using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class JoinCodeHolder :MonoBehaviour
{
    public string joinCode;
    [SerializeField] private TextMeshProUGUI joinCodeField;

    private void Start()
    {
        joinCode = HostSingleton.Instance.GameManager.GetJoinCode();
        joinCodeField.text = joinCode;
    }
}
