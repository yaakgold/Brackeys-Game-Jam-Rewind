﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed, damage, rotateAmount, timeAlive, timeTillDeath;
    public bool isEnemy, isWebbing;

    // Start is called before the first frame update
    void Start()
    {
        if(!isEnemy)
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction = target - myPos;
            direction.Normalize();
            Quaternion rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90);
            transform.rotation = rotation;

            GetComponent<Rigidbody2D>().velocity = direction * 10;
        }
        else
        {
            Vector2 target = new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y);
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction = target - myPos;
            direction.Normalize();
            Quaternion rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90);
            transform.rotation = rotation;

            GetComponent<Rigidbody2D>().velocity = direction * 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timeAlive >= timeTillDeath)
        {
            DestroyProjectile();
        }
        else
        {
            timeAlive += Time.deltaTime;
        }
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        DestroyProjectile();
    }
}
