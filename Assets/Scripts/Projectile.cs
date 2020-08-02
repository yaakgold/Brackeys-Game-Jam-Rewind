using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed, damage, rotateAmount = 90f;
    public bool isEnemy;

    // Start is called before the first frame update
    void Start()
    {
        if(!isEnemy)
        {
            GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, rotateAmount) * ((transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized * -speed));
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().ammoCount--;
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, rotateAmount) * ((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized * -speed));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
