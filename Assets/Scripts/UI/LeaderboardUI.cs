using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform leaderboardPosition;

    private const int RankTextPosition = 0;
    private const int NameTextPosition = 1;
    private const int ScoreTextPosition = 2;

    private void OnEnable()
    {
        StartCoroutine(WaitForSessionLeaderboardInitialization());
    }

    private IEnumerator WaitForSessionLeaderboardInitialization()
    {
        while(SessionLeaderboard.Instance == null)
        {
            yield return null;  
        }

        SessionLeaderboard.Instance.OnClientConnectedEvent += HandleLeaderboardUI;
        SessionLeaderboard.Instance.OnClientDisconnectedEvent += HandleLeaderboardUI;
    }
    public void UpdateLeaderboardUI(List<KeyValuePair<FixedString32Bytes, PlayerScore>> sortedScores)
   {
        Debug.Log(" Player's Leaderboard UI: UpdateLeaderboardUI called");
        foreach (Transform child in leaderboardPosition)
        {
            Destroy(child.gameObject);

        }

        int rank = 1;
        foreach(var scoreData in sortedScores)
        {
            GameObject row = Instantiate(rowPrefab, leaderboardPosition);
            Debug.Log(row + "INSTANTIATED");
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
                   rowTextElements[i].text = scoreData.Value.score.Value.ToString();
                }
            }
            rank++;
        }
   }

    public void HandleLeaderboardUI(ulong clientid)
    {
        SessionLeaderboard.Instance.UpdateLeaderboardUI();
    }

   
}
