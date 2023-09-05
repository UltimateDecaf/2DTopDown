using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class LeaderboardRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private FixedString32Bytes playerName;
    public int Score { get; private set; }

    public void Initialize(FixedString32Bytes name, int score)
    {
        playerName = name;
        UpdateScore(score);
    }

    public void UpdateScore(int score)
    {
        Score = score;
        UpdateRowText();
    }


    public void UpdateRowText()
    {
        int rank = transform.GetSiblingIndex() + 1;
        rankText.text = rank.ToString();

        playerNameText.text = playerName.ToString();

        scoreText.text = Score.ToString();


    }
}


