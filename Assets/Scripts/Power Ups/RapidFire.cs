using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : PowerUp
{
    private Collision2D coll;
    public override void UsePower(Collision2D col)
    {
        Debug.Log("This ran");
        transform.position = new Vector3(0, 1000);
        coll = col;
        coll.gameObject.GetComponent<CharacterMovement>().weaponCooldown = speed;
        coll.gameObject.GetComponent<CharacterMovement>().currentPowerUp = this;
        powerUpTimeStarted = true;
    }

    public override void CancelPower()
    {
        Debug.Log(coll == null);
        coll.gameObject.GetComponent<CharacterMovement>().weaponCooldown = coll.gameObject.GetComponent<CharacterMovement>().defaultWeaponCooldown;
        powerUpTimeStarted = false;
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = spr;
    }

    // Update is called once per frame
    void Update()
    {
        if(powerUpTimeStarted)
        {
            timeTillDeath -= Time.deltaTime;

            if(timeTillDeath <= 0)
            {
                CancelPower();
            }
        }
    }
}
