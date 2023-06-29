using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy_prefab;
    public AudioSource woo_sound;
    public Text score_text;
    public Text multiplier_text;

    [System.NonSerialized] public int score = 0;
    [System.NonSerialized] public int multiplier = 1;
    [System.NonSerialized] public float spawn_interval = 4f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn_Wave());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Update_Score(int amount) {
        this.score += amount * this.multiplier;
        score_text.text =  this.score.ToString();
    }

    public void Update_Multiplier(int amount) {
        this.multiplier += amount;
        multiplier_text.text = "x" + this.multiplier.ToString();
    }

    public void Spawn_Enemy() {
        //TODO:
        int type = 0;
        float type_probability = Random.Range(0f, 100f);
        if(this.spawn_interval < 3f) {
            if(type_probability > 80f) {
                type = 1;
            }
        } else if(this.spawn_interval < 2f) {
            if(type_probability > 50f) {
                type = 1;
            }
        } else if(this.spawn_interval < 1f) {
            if(type_probability > 50f) {
                type = 1;
            }
        }

        float x = Random.Range(-14f, 14f);
        float y = Random.Range(-14f, 14f);
        while(Mathf.Abs(this.player.transform.position.x - x) < 4) {
            x = Random.Range(-14f, 14f);
        }
        while(Mathf.Abs(this.player.transform.position.y - y) < 4) {
            y = Random.Range(-14f, 14f);
        }
        var enemy = Instantiate(enemy_prefab, new Vector3(x, y, -0.9f), Quaternion.Euler(0, 0, 0));
        enemy.GetComponent<EnemyController>().Enemy_Init(type);
        float dist = Vector3.Distance(this.player.transform.position, new Vector3(x, y, -0.9f));
        woo_sound.volume = Mathf.Lerp(0.8f, 0.1f, Mathf.InverseLerp(2f, 20f, dist));
        woo_sound.panStereo = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-20f, 20f, x - this.player.transform.position.x));
        woo_sound.Play(0);
    }

    IEnumerator Spawn_Wave() {
        yield return new WaitForSeconds(this.spawn_interval);
        if(this.spawn_interval > 0.6f) {
            this.spawn_interval -= 0.1f;
        }

        Spawn_Enemy();

        StartCoroutine(Spawn_Wave());
    }
}
