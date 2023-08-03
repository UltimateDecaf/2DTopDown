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


    void Awake()
    {
        Instance = this;
        score = 0;
        UpdateScoreUI();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
