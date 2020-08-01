﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObj : MonoBehaviour
{
    public float startTime, secondsAlive = 0;
    public int secondsLiving, min, max;
    public SpriteRenderer spr;

    // Start is called before the first frame update
    void Start()
    {
        secondsLiving = Random.Range(min, max);
        spr = GetComponent<SpriteRenderer>();
    }

    public void StartBlock()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        secondsAlive += Time.deltaTime;

        if(secondsAlive >= secondsLiving)//Destroy block
        {
            Destroy(gameObject);
        }

        float alpha = 1 - (secondsAlive / secondsLiving);
        spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, alpha);
    }
}