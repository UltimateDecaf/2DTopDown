using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : NetworkBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image healthLineImage;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private PlayerScore playerScore;
    [SerializeField] private TMP_Text scoreGameOverText;

    public Action<Health> OnHealthChanged;

    public override void OnNetworkSpawn()
    {
        if(!IsClient) return;
        health.CurrentHealth.OnValueChanged += HandleHealthChanged;

    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient) return;
        health.CurrentHealth.OnValueChanged -= HandleHealthChanged;
    }
   
    private void HandleHealthChanged(int oldHealth, int newHealth)
    {
        healthLineImage.fillAmount = (float) newHealth / health.MaxHealth;
        if((float) newHealth / health.MaxHealth <= 0)
        {
            gameOverScreen.SetActive(true);
            scoreGameOverText.text = "Your score: " + playerScore.score.Value;

        }
    }
}
