using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreBullets : PowerUp
{
    private Collision2D coll;
    public override void UsePower(Collision2D col)
    {
        coll = col;
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
        
    }
}
