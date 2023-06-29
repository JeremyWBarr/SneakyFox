using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispController : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject player;
    public AudioSource woop;
    
    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("Player");
        
        var force = 100f;
        var angle = Random.Range(0f, 360f);
        angle *= Mathf.Deg2Rad;
        float x = Mathf.Cos(angle) * force;
        float y = Mathf.Sin(angle) * force;

        rb.AddForce(new Vector3(x, y, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < 2f) {
            rb.AddForce((player.transform.position - transform.position)*50f);
        } else if(Vector3.Distance(transform.position, player.transform.position) < 5f) {
            rb.AddForce((player.transform.position - transform.position));
        } 
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Player") {
            var game = GameObject.Find("Game");
            game.GetComponent<GameController>().Update_Multiplier(4);
            game.GetComponents<AudioSource>()[0].Play(0);
            Destroy(gameObject);
        }
    }
}
