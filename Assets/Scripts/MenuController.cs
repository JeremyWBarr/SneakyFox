using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    public TMP_InputField playerNameInput;

    void Start() {
        playerNameInput.text = PlayerManager.Instance.playerName;
        playerNameInput.textComponent.SetText(PlayerManager.Instance.playerName);
    }

    public void PlayGame() {
        GameObject.Find("LevelLoader").GetComponent<TransitionController>().LoadLevel(1);
    }

    public void QuitGame() {
        Application.Quit();
    }
    
    public void UpdatePlayerName() {
        PlayerManager.Instance.SetPlayerName(playerNameInput.text);
    }
}
