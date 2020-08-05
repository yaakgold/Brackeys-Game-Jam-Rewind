﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public bool isHome = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = Instantiate(GameManager.Instance.player, transform.position, Quaternion.identity);
        player.GetComponent<CharacterMovement>().hasInfAmmo = isHome;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>().player = player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer()
    {
        GameObject player = Instantiate(GameManager.Instance.player, transform.position, Quaternion.identity);
        player.GetComponent<CharacterMovement>().hasInfAmmo = isHome;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>().player = player;
        UIManager.Instance.LoseLife();
    }
}
