using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public PowerUp currentPowerUp;
    public int maxAmmo, ammoCount, minAmmoNeed;

    public float horSpeed = 5f;
    public float jumpSpeed;
    public Rigidbody2D rb;
    public float groundPadding = .1f;
    public GameObject projectile, shieldHolder;
    public bool isGrounded, isStunned;
    public float defaultWeaponCooldown, weaponCooldown, currentWeaponCooldown, stunnedCooldown, currentSCooldown;
    public float whiteCooldown, currentWhiteCooldown;
    public Animator anim;
    public int facing = 1;
    public bool canJump = false;

    public Color stunColor;
    public bool hasInfAmmo = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ammoCount = maxAmmo;
        anim = GetComponentInChildren<Animator>();
        ammoCount = maxAmmo;
        UIManager.Instance.changeAmmoCount(ammoCount);
        rb = GetComponent<Rigidbody2D>();
        defaultWeaponCooldown = currentWeaponCooldown = weaponCooldown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!GameManager.Instance.isRewinding)
            move(Input.GetAxisRaw("Horizontal") * horSpeed * Time.deltaTime, (Input.GetAxisRaw("Vertical") > 0 && (isGrounded || canJump)));
        anim.SetBool("move", Input.GetAxisRaw("Horizontal") != 0);
        if (Input.GetAxisRaw("Horizontal") < 0)
            facing = -1;
        else if (Input.GetAxisRaw("Horizontal") > 0)
            facing = 1;
        transform.localScale = new Vector3(facing, 1, 1);
    }

    private void Update()
    {
        if (!GameManager.Instance.isRewinding)
        {
            if (currentWeaponCooldown <= 0)
            {
                if (Input.GetAxisRaw("Fire1") > 0 && ammoCount > minAmmoNeed)
                {
                    Instantiate(projectile, transform.position, Quaternion.identity);
                    //if(!hasInfAmmo)
                        ammoCount--;
                    currentWeaponCooldown = weaponCooldown;
                    UIManager.Instance.changeAmmoCount(ammoCount);
                    GameManager.Instance.ammoCount = ammoCount;
                }
            }
            else
            {
                currentWeaponCooldown -= Time.deltaTime;
            }

            if(isStunned)
            {
                if(currentSCooldown >= stunnedCooldown)
                {
                    currentSCooldown = 0;
                    isStunned = false;

                    horSpeed = 5;
                }
                else
                {
                    currentWhiteCooldown = 0;
                    GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(GetComponentInChildren<SpriteRenderer>().color, stunColor, currentSCooldown);
                    currentSCooldown += Time.deltaTime;
                }
            }
            else if(GetComponentInChildren<SpriteRenderer>().color != Color.white)
            {
                if(currentWhiteCooldown < whiteCooldown)
                {
                    GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(GetComponentInChildren<SpriteRenderer>().color, Color.white, currentWhiteCooldown);
                    currentWhiteCooldown += Time.deltaTime;
                }
                else
                {
                    currentWhiteCooldown = 0;
                }
            }

            if(rb.velocity.y > 1)
            {
                anim.SetBool("jump", true);
                anim.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if(rb.velocity.y < -1)
            {
                anim.SetBool("jump", true);
                anim.transform.rotation = Quaternion.Euler(0, 0, facing * -90);
            }
            else
            {
                anim.SetBool("jump", false);
                anim.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
                
        }
        else
        {
            anim.SetBool("jump", false);
            anim.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void AddNewShield(GameObject obj)
    {
        Instantiate(obj, shieldHolder.transform);
        ShieldObj[] shields = shieldHolder.GetComponentsInChildren<ShieldObj>();
        GameManager.Instance.numShields = shields.Length;
        int index = 0;
        float dist = .75f, shieldSpeed = 2;
        foreach (ShieldObj shield in shields)
        {
            shield.transform.rotation = Quaternion.identity;
            switch (index)
            {
                case 0:
                    shield.transform.RotateAround(transform.position, shield.axis, 180);
                    break;
                case 1:
                    dist *= -1;
                    break;
                case 2:
                    dist *= 1.5f;
                    shieldSpeed *= -1;
                    break;
                case 3:
                    shield.transform.RotateAround(transform.position, shield.axis, 180);
                    dist *= -1;
                    break;
                default:
                    break;
            }
            shield.distance = dist;
            shield.speed = shieldSpeed;
            shield.Setup();
            index++;
        }
    }

    void move(float hor, bool jump)
    {
        if(jump)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpSpeed);
            canJump = false;
        }

        transform.position = new Vector3(transform.position.x + hor, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("FlyProj") || (collision.gameObject.CompareTag("Enemy") && (!collision.gameObject.GetComponent<Enemy>().isStunned || collision.gameObject.GetComponent<Enemy>().canFly)))
        {
            Projectile p;
            bool isProj = collision.gameObject.TryGetComponent(out p);
            if (isProj && p.isWebbing)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                horSpeed = 2.5f;
                isStunned = true;
            }
            else
            {
                gameObject.GetComponent<TimeRewind>().startRewind = true;
            }
        }
        else if(collision.gameObject.CompareTag("BottomWall") || collision.gameObject.CompareTag("FireTrap"))
        {
            GameObject.FindGameObjectWithTag("PlayerSpawn").GetComponent<PlayerSpawn>().SpawnPlayer();
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            Debug.Log(collision.transform.CompareTag("Enemy") && collision.transform.GetComponent<Enemy>().isStunned && rb.velocity.y == 0);
        }

        if (collision.transform.CompareTag("Enemy") && collision.transform.GetComponent<Enemy>().isStunned && rb.velocity.y == 0)
        {
            Debug.Log("Hello");
            canJump = true;
        }
    }
}
