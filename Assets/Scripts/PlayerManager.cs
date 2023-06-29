using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;
    public string leaderboardId = "13690";
    public string playerNames = "Names\n";
    public string playerScores = "Scores\n";

    public string playerName = "";

    // Start is called before the first frame update
    private void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;    
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        StartCoroutine(LoginRoutine());
    }

    public void SetPlayerName(string name) {
        this.playerName = name;
        LootLockerSDKManager.SetPlayerName(this.playerName, (response) => {
            if(response.success) {
                Debug.Log("Successfully set player name");
            } else {
                Debug.Log("Failed to set player name: " + response.Error);
            }
        });
    }

    public IEnumerator GetPlayerName() {
        bool done = false;
        LootLockerSDKManager.GetPlayerName((response) => {
            if(response.success) {
                Debug.Log("Successfully got player name");
                this.playerName = response.name;
                done = true;
            } else {
                Debug.Log("Failed to get player name: " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);

        if(this.playerName == "") {
            this.playerName = "Guest" + Random.Range(10000, 99999).ToString();
            SetPlayerName(this.playerName);
        }
        var username_text = GameObject.Find("UsernameInput").GetComponent<TMP_InputField>();
        username_text.text = this.playerName;
        username_text.textComponent.SetText(this.playerName);

        
    }

    IEnumerator LoginRoutine() {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response)=> {
            if(response.success) {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                StartCoroutine(GetPlayerName());
            } else {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
        
    }

    public IEnumerator SubmitScoreRoutine(int score) {
        bool done = false;
        string playerId = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerId, score, 13690, (response) => {
            if(response.success) {
                Debug.Log("Successfully uploaded score");
                done = true;
            } else {
                Debug.Log("Failed: " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchTopHighscoresRoutine() {
        bool done = false;

        LootLockerSDKManager.GetScoreListMain(13690, 10, 0, (response) => {
            if(response.success) {
                Debug.Log("Leaderboard fetch Successfull");

                this.playerNames = "Names\n";
                this.playerScores = "Scores\n";
                LootLockerLeaderboardMember[] members = response.items;

                for(int i = 0; i < members.Length; i++) {
                    this.playerNames += members[i].rank + ". ";
                    if(members[i].player.name != "") {
                        this.playerNames += members[i].player.name;
                    } else {
                        this.playerNames += members[i].player.id;
                    }
                    this.playerScores += members[i].score + "\n";
                    this.playerNames += "\n";
                }
                done = true;
            } else {
                Debug.Log("Leaderboard fetch failed: " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

}
