using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDeadly = false;
    public float health, speed;
    public GameObject leftGC, rightGC, frontGC;
    public Vector3 flipScale;
    public bool canFly, canCrawl;
    public float flyxSpeed = 4, flyySpeed = 2;

    private float startTime = 0, endTime = 0;
    private int movex, movey;

    private float startProj = 0, endProj = 3;
    private float startStun = 0, endStun = 3;
    public bool isStunned = false;

    public Vector3[] movementVects;
    public Vector3 startVect;
    public bool canStartRot = true, canRot = false, hitF = false, isFalling = false;
    public int index, nextIndex;
    public float fallSpeed = 1;

    private void Start()
    {
        endProj = 100;
        movementVects = new[] { new Vector3(0, 0), new Vector3(1, 0), new Vector3(0, -1), new Vector3(-1, 0), new Vector3(0, 1) };
        index = 1;
    }

    public int NextIndex(int ind)
    {
        int i = ind;

        i++;
        if (i >= movementVects.Length)
            i = 1;
        //Debug.Log(i);
        return i;
    }

    public int PrevIndex(int ind)
    {
        int i = ind;

        i--;
        if (i < 1)
            i = 4;
        Debug.Log(i);
        return i;
    }

    private void Update()
    {
        //Movement
        if (!GameManager.Instance.isRewinding && !isStunned)
        {
            if (canCrawl) //Spider
            {
                if (!isFalling)
                {
                    RaycastHit2D hitL = Physics2D.Raycast(leftGC.transform.position, Quaternion.Euler(transform.rotation.eulerAngles) * Vector3.down, .1f);
                    RaycastHit2D hitR = Physics2D.Raycast(rightGC.transform.position, Quaternion.Euler(transform.rotation.eulerAngles) * Vector3.down, .1f);

                    if (hitF)
                    {
                        startVect = transform.rotation.eulerAngles;
                        startVect.z += 90f;
                        transform.eulerAngles = startVect;
                        hitF = false;
                        nextIndex = index;
                        index = PrevIndex(index);
                    }

                    if (!hitL && canStartRot && !hitR) //Start rotating
                    {
                        canRot = true;
                        canStartRot = false;
                        startVect = transform.rotation.eulerAngles;
                        startVect.z -= 90;
                        nextIndex = index;
                        index = 0;
                    }

                    if (canRot) //Rotate
                    {
                        transform.eulerAngles = startVect;
                        canRot = false;
                        index = NextIndex(nextIndex);
                    }

                    if (hitR && !canStartRot && !canRot) //End rotation
                    {
                        canStartRot = true;
                    }

                }
                else
                {
                    index = 2;
                    fallSpeed = 3;
                }

                transform.position += movementVects[index] * speed * Time.deltaTime * fallSpeed;

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
            else if (!canFly) //Ant
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
            else //Fly
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

        if(isFalling && collision.transform.CompareTag("Ground"))
        {
            isFalling = false;
            index = 1;
            startVect.z = 0;
            transform.eulerAngles = startVect;
            fallSpeed = 1;
            canStartRot = true;
            canRot = false;
        }
    }
}
