using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Player") {
            StartCoroutine(LoseRoutine());
        }
    }

    IEnumerator LoseRoutine() {
        Time.timeScale = 0f;
        var game = GameObject.Find("Game").GetComponent<GameController>();
        var pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        yield return PlayerManager.Instance.SubmitScoreRoutine(game.score);
        yield return new WaitForSecondsRealtime(0.5f);

        GameObject.Find("LevelLoader").GetComponent<TransitionController>().LoadLevel(0);
        Time.timeScale = 1f;
    }
}
