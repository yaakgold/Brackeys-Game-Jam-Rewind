using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : PowerUp
{
    public override void UsePower(Collision2D col)
    {
        transform.position = new Vector3(0, 1000);
        powerUpTimeStarted = true;
        Time.timeScale = speed;
    }

    public override void CancelPower()
    {
        powerUpTimeStarted = false;
        Time.timeScale = 1;
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
