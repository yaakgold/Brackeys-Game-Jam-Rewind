using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : PowerUp
{
    public GameObject shield;

    private Collision2D coll;

    public override void UsePower(Collision2D col)
    {
        transform.position = new Vector3(0, 1000);
        coll = col;
        coll.gameObject.GetComponent<CharacterMovement>().AddNewShield(shield);
        CancelPower();
    }

    public override void CancelPower()
    {   
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
        if (powerUpTimeStarted)
        {
            timeTillDeath -= Time.deltaTime;

            if (timeTillDeath <= 0)
            {
                CancelPower();
            }
        }
    }
}
