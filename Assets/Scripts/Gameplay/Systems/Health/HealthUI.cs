using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

//Based on Nathan Farrer's 'HealthDisplay.cs' script: https://gitlab.com/GameDevTV/unity-multiplayer/unity-multiplayer/-/blob/main/Assets/Scripts/Core/Combat/HealthDisplay.cs?ref_type=heads

/* Lari Basangov's addition:
 * - Game Over screen is enabled upon losing all health.
 */

// This screen updates health UI, and toggles the game over screen upon losing all health.
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
