using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Score : MonoBehaviour
{
    private int score;
    [SerializeField] private TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreText();
    }

    public void AddToScore(int scoreToAdd)
    {
        score += scoreToAdd;
        Debug.Log("Method Called - current score is " + score);
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
