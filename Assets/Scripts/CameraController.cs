using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject player           = null;
    private Camera cam_object           = null;
    private Vector3 cam_position        = new Vector3(0f, 0f, -20f);

    public float dampening_multiplier   = 0.1f;
    public float max_offset             = 5f;
    public float shake_magnitude        = 0f;

    public bool lock_x_movement         = false;
    public bool lock_y_movement         = false;

    public Transform BR_Bounds;
    public Transform TL_Bounds;

    private float min_x;
    private float max_x;
    private float min_y;
    private float max_y;

    void Start() {
        cam_object = GetComponent<Camera>();

        cam_position.x = (lock_x_movement) ? transform.position.x : player.transform.position.x;
        cam_position.y = (lock_y_movement) ? transform.position.y : player.transform.position.y;

        var cam_h = cam_object.orthographicSize;    
        var cam_w = cam_h * Screen.width / Screen.height;

        min_x = TL_Bounds.position.x + cam_w;
        max_x = BR_Bounds.position.x - cam_w;
        min_y = BR_Bounds.position.y + cam_h;
        max_y = TL_Bounds.position.y - cam_h;
    }

    void FixedUpdate()
    {
        var player_pos = player.transform.position;

        // Cam offset
        if(!lock_x_movement) cam_position.x = dampen(cam_position.x, player_pos.x);
        if(!lock_y_movement) cam_position.y = dampen(cam_position.y, player_pos.y);

        // Cam bounds
        if(!lock_x_movement) cam_position.x = Mathf.Clamp(cam_position.x, min_x, max_x);
        if(!lock_y_movement) cam_position.y = Mathf.Clamp(cam_position.y, min_y, max_y);

        // Follow player
        transform.position = cam_position + (UnityEngine.Random.insideUnitSphere * shake_magnitude);
    }

    private float dampen(float from, float to) {
        var d = (Mathf.Abs(from - to)*dampening_multiplier);

        if(from - to >  max_offset) return to + max_offset;
        if(from - to < -max_offset) return to - max_offset;
        return from += (to < from) ? -d : d; 
    }

    public void setCamShakeMag(float m) {
        this.shake_magnitude = m;
    }
}
