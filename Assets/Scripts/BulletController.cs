using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float force = 3000f;

    public void Shoot(float angle) {
        angle += Random.Range(-4f, 4f);
        transform.rotation = Quaternion.Euler(0, 0, angle-90f);
        angle *= Mathf.Deg2Rad;
        float x = Mathf.Cos(angle) * force;
        float y = Mathf.Sin(angle) * force;

        rb.AddForce(new Vector3(x, y, 0));
    }


    void OnCollisionEnter2D(Collision2D col) {
        Destroy(gameObject);
    }
}
