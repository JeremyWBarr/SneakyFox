using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{

    public Animator transition;
    public float transition_time = 1f;

    public void LoadLevel(int level_id) {
        StartCoroutine(LoadLevelWork(level_id));
    }

    IEnumerator LoadLevelWork(int level_id) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(this.transition_time);

        SceneManager.LoadScene(level_id);
    }
}
