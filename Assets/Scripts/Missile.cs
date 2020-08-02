using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed, damage, rotateAmount = 0f;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get Angle in Radians
        float rads = Mathf.Atan2(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y, Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x);
        // Get Angle in Degrees
        float degs = (180 / Mathf.PI) * rads;
        // Rotate Object
        transform.rotation = Quaternion.Euler(0, 0, degs - 90);

        transform.LookAt(transform.rotation.eulerAngles.normalized);

        rb.AddForce(transform.forward * speed, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        //Destroy(gameObject);
    }
}
