using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public int maxAmmo, ammoCount, minAmmoNeed = 1;

    public float horSpeed = 5f;
    public float jumpSpeed;
    public Rigidbody2D rb;
    public float groundPadding = .1f;
    public GameObject projectile, shieldHolder;
    public bool isGrounded;
    public float defaultWeaponCooldown, weaponCooldown, currentWeaponCooldown;

    // Start is called before the first frame update
    void Start()
    {
        ammoCount = maxAmmo;
        rb = GetComponent<Rigidbody2D>();
        defaultWeaponCooldown = currentWeaponCooldown = weaponCooldown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!GameManager.Instance.isRewinding)
            move(Input.GetAxisRaw("Horizontal") * horSpeed * Time.deltaTime, (Input.GetAxisRaw("Vertical") > 0 && isGrounded));
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
                    currentWeaponCooldown = weaponCooldown;
                    UIManager.Instance.changeAmmoCount(ammoCount);
                }
            }
            else
            {
                currentWeaponCooldown -= Time.deltaTime;
            }
        }
    }

    public void AddNewShield(GameObject obj)
    {
        Instantiate(obj, shieldHolder.transform);
        ShieldObj[] shields = shieldHolder.GetComponentsInChildren<ShieldObj>();

        int index = 0;
        float dist = .5f, shieldSpeed = 4;
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
        }

        transform.position = new Vector3(transform.position.x + hor, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("FlyProj") || (collision.gameObject.CompareTag("Enemy") && (!collision.gameObject.GetComponent<Enemy>().isStunned || collision.gameObject.GetComponent<Enemy>().canFly)))
        {
            gameObject.GetComponent<TimeRewind>().startRewind = true;
        }
        else if(collision.gameObject.CompareTag("BottomWall"))
        {
            GameObject.FindGameObjectWithTag("PlayerSpawn").GetComponent<PlayerSpawn>().SpawnPlayer();
            Destroy(gameObject);
        }
    }
}
