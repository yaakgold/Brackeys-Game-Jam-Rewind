using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDeadly = false;
    public float health, speed;
    public GameObject leftGC, rightGC;
    public Vector3 flipScale;
    public bool canFly;
    public float flyxSpeed = 4, flyySpeed = 2;

    private float startTime = 0, endTime = 0;
    private int movex, movey;

    private float startProj = 0, endProj = 3;
    private float startStun = 0, endStun = 3;
    public bool isStunned = false;

    private void Update()
    {
        //Movement
        if (!GameManager.Instance.isRewinding && !isStunned)
        {
            if (!canFly)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y);
                RaycastHit2D hitL = Physics2D.Raycast(leftGC.transform.position, Vector3.down, 1);
                RaycastHit2D hitR = Physics2D.Raycast(rightGC.transform.position, Vector3.down, 1);
                if (!hitL || !hitR)
                {
                    speed *= -1;
                    flipScale.x = -transform.localScale.x;
                    flipScale.y = transform.localScale.y;
                    flipScale.z = transform.localScale.z;
                    transform.localScale = flipScale;
                }
            }
            else
            {
                if (startTime < endTime)
                {
                    startTime += Time.deltaTime;
                }
                else
                {
                    startTime = 0;
                    endTime = Random.Range(2, 4);
                    movex = Random.Range(-1, 2);
                    movey = Random.Range(-1, 2);
                    while (movey == 0 && movex == 0)
                    {
                        movex = Random.Range(-1, 2);
                    }
                }
                move(movex, movey);
                if (startProj < endProj)
                {
                    startProj += Time.deltaTime;
                }
                else
                {
                    startProj = 0;
                    endProj = Random.Range(1f, 3f);
                    Instantiate(GameManager.Instance.enemyProj, transform.position, Quaternion.identity);
                }
            }
        }

        if(isStunned)
        {
            GetComponent<Animator>().SetBool("pause", true);
            if(!GameManager.Instance.isRewinding)
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (startStun < endStun)
            {
                startStun += Time.deltaTime;
            }
            else
            {
                startStun = 0;
                isStunned = false;
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            GetComponent<Animator>().SetBool("pause", false);
        }
    }

    void move(int x, int y)
    {
        transform.position = new Vector3(transform.position.x + (x * flyxSpeed * Time.deltaTime), transform.position.y + (y * flyySpeed * Time.deltaTime));
        if (x != 0)
            transform.localScale = new Vector3(x, 1, 1);
    }

    public void TakeDamage(float amt)
    {
        health -= amt;
        
        if(health <= 0)
        {
            isStunned = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(canFly && (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground")))
        {
            speed *= -1;
            movex *= -1;
            movey *= -1;
        }
    }
}
