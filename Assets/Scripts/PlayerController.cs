using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    
    // INSPECTOR ATTRIBUTES
    public Rigidbody2D player_rb;
    public GameObject gun_pivot;
    public ParticleSystem muzzle_flash;
    public Light2D muzzle_light;
    public GameObject bullet_prefab;
    public AudioSource shoot;
    public float fire_cd_time = .2f;

    // HIDDEN ATTRIBUTES
    [System.NonSerialized] public float input_x_axis             = 0;
    [System.NonSerialized] public float input_y_axis             = 0;
    [System.NonSerialized] public bool input_fire                = false;

    [System.NonSerialized] public float player_speed             = 10f;

    [System.NonSerialized] public bool can_fire                  = true;
    
    


    // METHODS
    void Update_Inputs() 
    {
        // X & Y axis inputs
        this.input_x_axis = Input.GetAxis("Horizontal");
        this.input_y_axis = Input.GetAxis("Vertical");
        this.input_fire   = Input.GetButton("Fire1");
    }

    void Update_Movement()
    {
        // X & Y movement
        float player_angle = Mathf.Atan2(this.input_x_axis, this.input_y_axis);
        

        if(this.input_x_axis == 0 && this.input_y_axis == 0) {
            this.player_rb.velocity = new Vector2(0, 0);
        } else {
            this.player_rb.velocity = new Vector2(
                Mathf.Sin(player_angle) * this.player_speed,
                Mathf.Cos(player_angle) * this.player_speed
            );
            this.transform.rotation = Quaternion.Euler(0f, 0f, -player_angle * Mathf.Rad2Deg);
        }

        // Gun rotation
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg);
        gun_pivot.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        // Gun shooting
        if(this.input_fire && this.can_fire) {
            shoot.Play(0);
            muzzle_flash.Play();
            muzzle_light.intensity = 2f;
            this.can_fire = false;
            StartCoroutine(Fire_CD());
            var bullet = Instantiate(bullet_prefab, this.transform.position, this.transform.rotation);
            bullet.GetComponent<BulletController>().Shoot(angle);
        }
    }



    void Start()
    {
        
    }

    void Update()
    {
        this.Update_Inputs();

        if(muzzle_light.intensity > 0f) {
            muzzle_light.intensity -= 0.1f;
        }
    }

    void FixedUpdate()
    {
        this.Update_Movement();
    }

    IEnumerator Fire_CD() {
        yield return new WaitForSeconds(this.fire_cd_time);
        this.can_fire = true;
    }


}
