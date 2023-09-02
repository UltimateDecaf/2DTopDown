using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class PlayerNameDisplay : MonoBehaviour
{
    [SerializeField] private PlayerNameGetter nameGetter;
    [SerializeField] private TMP_Text nameText;


    private void Start()
    {
        HandlePlayerNameChanged(string.Empty, nameGetter.PlayerName.Value);
        nameGetter.PlayerName.OnValueChanged += HandlePlayerNameChanged;
    }

    private void HandlePlayerNameChanged(FixedString32Bytes previousName, FixedString32Bytes newName)
    {
        nameText.text = newName.ToString();
    }

    private void OnDestroy()
    {
        nameGetter.PlayerName.OnValueChanged -= HandlePlayerNameChanged;
    }
}
