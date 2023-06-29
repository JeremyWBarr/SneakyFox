using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardController : MonoBehaviour
{
    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;

    void Awake()
    {
        UpdateLeaderboard();
    }

    public void UpdateLeaderboard() {
        StartCoroutine(UpdateLeaderboardWork());
    }

    public IEnumerator UpdateLeaderboardWork() {
        yield return PlayerManager.Instance.FetchTopHighscoresRoutine();
        playerNames.text = PlayerManager.Instance.playerNames;
        playerScores.text = PlayerManager.Instance.playerScores;
    }

}
