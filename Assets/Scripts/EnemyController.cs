using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnemyController : MonoBehaviour
{
    // INSPECTOR ATTRIBUTES
    public Rigidbody2D enemy_rb; 
    public Animator animator;
    public GameObject flashlight_pivot;
    public GameObject flashlight_object;
    public GameObject flashlight_sprite;
    public GameObject puff_prefab;
    public GameObject wisp_prefab;
    public Collider2D enemy1_light_col;
    public Collider2D enemy2_light_col;
    public Light2D flashlight;
    public float enemy_speed = 4f;

    // HIDDEN ATTRIBUTES
    [System.NonSerialized] public float movement_type            = 0;
    [System.NonSerialized] public float angle                    = 0;
    [System.NonSerialized] public float angle_increment          = 1;
    [System.NonSerialized] public float flashlight_angle         = 0;

    [System.NonSerialized] public float hp                       = 1;
    [System.NonSerialized] public int enemy_type                 = 0;

    // TYPE INIT
    public void Enemy_Init(int type) {
        this.enemy_type = type;
        switch(type) {
            case 0:
            Enemy0_Init();
            break;

            case 1:
            Enemy1_Init();
            break;
        }
    }


    public void Enemy0_Init() {
        this.movement_type = 0;
        this.hp = 1;
        this.animator.SetInteger("Type", 0);

        this.flashlight_object.transform.localPosition  = new Vector3(0, 0, 0);
        this.flashlight.pointLightInnerAngle            = 360f;
        this.flashlight.pointLightOuterAngle            = 360f;
        this.flashlight.pointLightInnerRadius           = 2.5f;
        this.flashlight.pointLightOuterRadius           = 3f;
        this.flashlight.intensity                       = 0f;

        this.enemy1_light_col.enabled = true;
        this.enemy2_light_col.enabled = false;
        this.flashlight_sprite.SetActive(false);

        var angles = new List<int> {0, 45, 90, 135, 180, 225, 270, 315};
        this.angle = angles[Random.Range(0, angles.Count)];

        this.flashlight_pivot.transform.rotation = Quaternion.Euler(0f, 0f, 360 - this.angle);
    }

    public void Enemy1_Init() {
        this.movement_type = 1;
        this.hp = 1;
        this.animator.SetInteger("Type", 1);

        this.flashlight_object.transform.localPosition  = new Vector3(0, 0.6f, 0);
        this.flashlight.pointLightInnerAngle            = 30f;
        this.flashlight.pointLightOuterAngle            = 35f;
        this.flashlight.pointLightInnerRadius           = 5.5f;
        this.flashlight.pointLightOuterRadius           = 6f;
        this.flashlight.intensity                       = 0f;

        this.enemy1_light_col.enabled = false;
        this.enemy2_light_col.enabled = true;
        this.flashlight_sprite.SetActive(true);

        StartCoroutine(Change_Direction());
    }




    // METHODS
    
    /*
        MOVEMENT TYPES:
        0 - bounce
        1 - random bounce
        2 - ...


    */
    void Set_Movement_Type(int type) {
        this.movement_type = type;
    }


    void Update_Movement()
    {
        switch(this.movement_type) {
            case 0:
            this.Bounce_Movement();
            break;

            case 1:
            this.Random_Bounce_Movement();
            break;
        }
    }

    // MOVEMENT TYPES
    void Bounce_Movement() {
        this.enemy_rb.velocity = new Vector2(
            Mathf.Sin(this.angle * Mathf.Deg2Rad) * this.enemy_speed,
            Mathf.Cos(this.angle * Mathf.Deg2Rad) * this.enemy_speed
        );
        this.transform.rotation = Quaternion.Euler(0f, 0f, -this.angle);
    }
    
    void Random_Bounce_Movement() {
        this.angle += angle_increment;
        this.flashlight_pivot.transform.rotation = Quaternion.Euler(0f, 0f, 360 - this.angle);
        this.enemy_rb.velocity = new Vector2(
            Mathf.Sin(this.angle * Mathf.Deg2Rad) * this.enemy_speed,
            Mathf.Cos(this.angle * Mathf.Deg2Rad) * this.enemy_speed
        );
        this.transform.rotation = Quaternion.Euler(0f, 0f, -this.angle);
    }



    void Start()
    {
        // Enemy1_Init();
    }

    void FixedUpdate()
    {
        this.Update_Movement();
    }
    
    void Update() {
        if(this.flashlight.intensity < 0.8f) {
            this.flashlight.intensity += 0.05f;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(this.movement_type == 0) {
            if(col.gameObject.tag == "HWall") {
                this.angle = 180 - this.angle;
                this.flashlight_pivot.transform.rotation = Quaternion.Euler(0f, 0f, 360 - this.angle);
            }
            if(col.gameObject.tag == "VWall") {
                this.angle = 360 - this.angle;
                this.flashlight_pivot.transform.rotation = Quaternion.Euler(0f, 0f, 360 - this.angle);
            }
        }

        if(col.gameObject.tag == "Bullet") {
            hp--;
            if(hp <= 0) {
                var game = GameObject.Find("Game");
                game.GetComponent<GameController>().Update_Score((this.enemy_type+1)*2);

                var player = GameObject.Find("Player");

                AudioSource poof = game.GetComponents<AudioSource>()[2];
                float dist = Vector3.Distance(transform.position, player.transform.position);
                poof.volume = Mathf.Lerp(0.6f, 0.1f, Mathf.InverseLerp(2f, 20f, dist));
                poof.panStereo = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-20f, 20f, transform.position.x - player.transform.position.x));
                poof.Play(0);
                
                Instantiate(puff_prefab, transform.position, Quaternion.Euler(0,0,0));
                Instantiate(wisp_prefab, transform.position, Quaternion.Euler(0,0,0));
                Instantiate(wisp_prefab, transform.position, Quaternion.Euler(0,0,0));
                Destroy(gameObject);
            }
        }
    }

    IEnumerator Change_Direction() {
        yield return new WaitForSeconds(1f);

        var dir = transform.position - new Vector3(0, 0, 0);
        dir = transform.InverseTransformDirection(dir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;

        if((this.angle_increment > 0 && angle > 45f) || (this.angle_increment < 0 && angle < 45f) || Random.Range(0, 8) == 0) {
            this.angle_increment *= -1;
        }

        StartCoroutine(Change_Direction());
    }
}
