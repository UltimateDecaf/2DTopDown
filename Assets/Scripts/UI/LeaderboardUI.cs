using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

//Created by Lari Basangov

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform leaderboardPosition;

    private const int RankTextPosition = 0;
    private const int NameTextPosition = 1;
    private const int ScoreTextPosition = 2;

    private void OnEnable()
    {
        SessionLeaderboard.Instance.OnClientConnectedEvent += UpdateLeaderboardUI;
        SessionLeaderboard.Instance.OnClientDisconnectedEvent += UpdateLeaderboardUI;

<<<<<<< HEAD
    }
    public void UpdateLeaderboardUI(List<KeyValuePair<FixedString32Bytes, PlayerScore>> sortedScores)
   {
        foreach(Transform child in leaderboardPosition)
=======
    private IEnumerator WaitForSessionLeaderboardInitialization()
    {
        while(SessionLeaderboard.Instance == null)
        {
            yield return null;  
        }

        SessionLeaderboard.Instance.OnClientConnect += HandleLeaderboardUI;
        SessionLeaderboard.Instance.OnClientDisconnect -= HandleLeaderboardUI;
    }
    public void UpdateLeaderboardUI(List<KeyValuePair<FixedString32Bytes, PlayerScore>> sortedScores)
   {
        foreach (Transform child in leaderboardPosition)
>>>>>>> sessionleaderboard-fixes
        {
            Destroy(child.gameObject);
            
        }

        int rank = 1;
        foreach(var scoreData in sortedScores)
        {
            GameObject row = Instantiate(rowPrefab, leaderboardPosition);
            TMP_Text[] rowTextElements = row.GetComponentsInChildren<TMP_Text>();
            for(int i = 0;  i < rowTextElements.Length; i++)
            {
                if(i == RankTextPosition)
                {
                    rowTextElements[i].text = rank.ToString();
                }

                if(i == NameTextPosition)
                {
                    rowTextElements[i].text = scoreData.Key.ToString();
                }

                if(i == ScoreTextPosition)
                {
                    scoreData.Value.score.Value.ToString();
                }

                rank++;
            }
        }
   }

    public void UpdateLeaderboardUI(ulong clientid)
    {
        SessionLeaderboard.Instance.UpdateLeaderboardUI();
    }
}
