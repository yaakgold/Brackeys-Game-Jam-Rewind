using System.Collections;
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
        player.GetComponent<CharacterMovement>().ammoCount = player.GetComponent<CharacterMovement>().maxAmmo;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>().player = player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer(int ammoCount)
    {
        GameObject player = Instantiate(GameManager.Instance.player, transform.position, Quaternion.identity);
        player.GetComponent<CharacterMovement>().hasInfAmmo = isHome;
        player.GetComponent<CharacterMovement>().ammoCount = ammoCount;
        Debug.Log(ammoCount);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>().player = player;
        UIManager.Instance.LoseLife();
    }
}
