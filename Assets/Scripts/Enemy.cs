using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDeadly = false;
    public float health, speed;
    public GameObject leftGC, rightGC, frontGC, GFX;
    public Vector3 flipScale;
    public bool canFly, canCrawl, canFlipAgain = true;
    public float flyxSpeed = 4, flyySpeed = 2;

    private float startTime = 0, endTime = 0;
    private int movex, movey;

    private float startProj = 0, endProj = 3;
    private float startStun = 0, endStun = 3;
    public bool isStunned = false;

    public Vector3[] movementVects;
    public Vector3 startVect;
    public bool canStartRot = true, canRot = false, hitF = false, isFalling = false, canFinishRot = false;
    public int index, nextIndex;
    public float fallSpeed = 1;

    public Color pColor, fly4, fly3, fly2, fly1;

    public bool isFlyActive = false;

    private void Start()
    {
        //endProj = 1000;
        movementVects = new[] { new Vector3(0, 0), new Vector3(1, 0), new Vector3(0, -1), new Vector3(-1, 0), new Vector3(0, 1) };
        index = 1;
    }

    public int NextIndex(int ind)
    {
        int i = ind;

        i++;
        if (i >= movementVects.Length)
            i = 1;

        return i;
    }

    public int PrevIndex(int ind)
    {
        int i = ind;

        i--;
        if (i < 1)
            i = 4;

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
                    RaycastHit2D hitL = Physics2D.Raycast(leftGC.transform.position, Quaternion.Euler(transform.rotation.eulerAngles) * Vector3.down, .1f, LayerMask.GetMask("Ground"));
                    RaycastHit2D hitR = Physics2D.Raycast(rightGC.transform.position, Quaternion.Euler(transform.rotation.eulerAngles) * Vector3.down, .1f, LayerMask.GetMask("Ground"));

                    if (hitF)
                    {
                        startVect = transform.rotation.eulerAngles;
                        hitF = false;
                        nextIndex = index;
                        index = PrevIndex(index);
                        startVect.z += 90f;
                        transform.eulerAngles = startVect;
                    }

                    if (!hitL && canStartRot && !hitR) //Start rotating
                    {
                        canRot = true;
                        canStartRot = false;
                        startVect = transform.rotation.eulerAngles;
                        startVect.z -= 5;
                        transform.eulerAngles = startVect;
                        nextIndex = index;
                        index = 0;
                    }

                    if (canRot && transform.eulerAngles.z % 90 != 0) //Rotate
                    {
                        transform.eulerAngles = startVect;
                        startVect.z -= 5;
                        canFinishRot = true;
                    }
                    else if(canFinishRot)
                    {
                        index = NextIndex(nextIndex);
                        canRot = false;
                        canFinishRot = false;
                    }

                    if (hitR && !canStartRot && !canRot) //End rotation
                    {
                        canStartRot = true;
                    }

                    
                    if(Physics2D.OverlapCircleAll(GFX.transform.position, .75f, LayerMask.GetMask("Ground")).Length == 0)
                    {
                        isFalling = true;
                        nextIndex = 0;
                    }

                }
                else
                {
                    startVect.z = -90;
                    transform.eulerAngles = startVect;
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
                    Projectile p = Instantiate(GameManager.Instance.enemyProj, transform.position, Quaternion.identity).GetComponent<Projectile>();
                    p.isWebbing = true;
                    p.GetComponent<SpriteRenderer>().color = pColor;
                }
            }
            else if (!canFly) //Ant
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y);
                RaycastHit2D hitL = Physics2D.Raycast(leftGC.transform.position, Vector3.down, 1, LayerMask.GetMask("Ground"));
                RaycastHit2D hitR = Physics2D.Raycast(rightGC.transform.position, Vector3.down, 1, LayerMask.GetMask("Ground"));

                if ((!hitL || !hitR) && canFlipAgain)
                {
                    canFlipAgain = false;
                    speed *= -1;
                    flipScale.x = -transform.localScale.x;
                    flipScale.y = transform.localScale.y;
                    flipScale.z = transform.localScale.z;
                    transform.localScale = flipScale;
                }

                if(!canFlipAgain)
                {
                    canFlipAgain = (hitL && hitR);
                }
            }
            else //Fly
            {
                if (isFlyActive)
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
        }

        if(isStunned)
        {
            if(!canCrawl)
            GetComponent<Animator>().SetBool("pause", true);
            else
            {
                GetComponentInChildren<Animator>().SetBool("pause", true);
            }
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
            if (!canCrawl)
                GetComponent<Animator>().SetBool("pause", false);
            else
            {
                GetComponentInChildren<Animator>().SetBool("pause", false);
            }
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
        
        //Fly:
        if(health == 4)
        {
            GetComponent<SpriteRenderer>().color = fly4;
            GameManager.Instance.AddToScore(10);
            isStunned = true;
        }
        else if(health == 3)
        {
            GetComponent<SpriteRenderer>().color = fly3;
            GameManager.Instance.AddToScore(10);
            isStunned = true;
        }
        else if (health == 2)
        {
            GetComponent<SpriteRenderer>().color = fly2;
            GameManager.Instance.AddToScore(10);
            isStunned = true;
        }
        else if (health == 1)
        {
            GetComponent<SpriteRenderer>().color = fly1;
            GameManager.Instance.AddToScore(10);
            isStunned = true;
        }
        else if (health == 0)
        {
            GameManager.Instance.AddToScore(10);
            Destroy(gameObject);
        }

        //All other bugs
        if (health <= 0)
        {
            isStunned = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("MainCamera"))
        {
            isFlyActive = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!canCrawl && !canFly)//Ant
        {
            if(collision.gameObject.CompareTag("Enemy") || hitF)
            {
                speed *= -1;
                flipScale.x = -transform.localScale.x;
                flipScale.y = transform.localScale.y;
                flipScale.z = transform.localScale.z;
                transform.localScale = flipScale;
                hitF = false;
            }
        }

        if(canFly && (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground")))
        {
            speed *= -1;
            movex *= -1;
            movey *= -1;
        }

        if(collision.transform.CompareTag("BottomWall"))
        {
            Destroy(gameObject);
        }

        if(isFalling && collision.transform.CompareTag("Ground"))
        {
            isFalling = false;
            index = 1;
            startVect.z = 0;
            transform.eulerAngles = startVect;
            transform.position = new Vector3(transform.position.x, transform.position.y - .2f);
            fallSpeed = 1;
            canStartRot = true;
            canRot = false;
        }
    }
}
