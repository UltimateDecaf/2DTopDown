using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public int previousScore;
    public int currentScore;
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    private PlayerScore playerScore;
    
    // Start is called before the first frame update
    private void Start()
    {
        previousScore = 0; currentScore = 0;
        playerScore = GetComponent<PlayerScore>();
        scoreDisplay.text = "Score: " + currentScore;
    }

    // Update is called once per frame
    private void Update()
    {
        currentScore = playerScore.score.Value;
        if (previousScore == currentScore) { return; }
        previousScore = currentScore;
        scoreDisplay.text = "Score: " + currentScore;
    }

}
