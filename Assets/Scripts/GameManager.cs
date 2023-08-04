using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //Singleton!

    [SerializeField] private TextMeshProUGUI scoreUI;
    private int score;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI finalScore;
    private Health playerHealth;

    void Awake()
    {
        Instance = this;
        score = 0;
        UpdateScoreUI();
        playerHealth = player.GetComponent<Health>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerHealth(playerHealth.GetHealth());
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreUI.text = "Score: " + score;
    }

    private void CheckPlayerHealth(int currentHealth)
    {
        if(currentHealth <= 0)
        {
            gameOverScreen.SetActive(true);
            finalScore.text = "Final score: " + score; 
        }
    }
}
