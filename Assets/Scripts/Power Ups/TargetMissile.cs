﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMissile : PowerUp
{
    public GameObject projectile, defaultProj;

    private Collision2D coll;

    public override void UsePower(Collision2D col)
    {
        transform.position = new Vector3(0, 1000);
        coll = col;
        col.gameObject.GetComponent<CharacterMovement>().weaponCooldown = speed;
        col.gameObject.GetComponent<CharacterMovement>().projectile = projectile;
        
    }

    public override void CancelPower()
    {
        coll.gameObject.GetComponent<CharacterMovement>().weaponCooldown = coll.gameObject.GetComponent<CharacterMovement>().defaultWeaponCooldown;
        coll.gameObject.GetComponent<CharacterMovement>().projectile = defaultProj;
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
