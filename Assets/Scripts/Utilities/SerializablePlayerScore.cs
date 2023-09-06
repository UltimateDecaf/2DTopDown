using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializablePlayerScore //this class is needed so that clientRpcs can be performed
{
    public int ScoreValue; 

    public SerializablePlayerScore(PlayerScore playerScore)
    {
        this.ScoreValue = playerScore.score.Value; 
    }

    public PlayerScore ToPlayerScore()
    {
        PlayerScore newPlayerScore = new PlayerScore();
        newPlayerScore.score.Value = this.ScoreValue;
        return newPlayerScore;
    }
}

