using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public Sprite spr;
    public string pName;
    public float speed, timeTillDeath;
    public bool powerUpTimeStarted = false;
    public int ammoInc;

    abstract public void UsePower(Collision2D col);
    abstract public void CancelPower();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            CharacterMovement cm = collision.gameObject.GetComponent<CharacterMovement>();
            cm.ammoCount = (cm.ammoCount + ammoInc > cm.maxAmmo) ? cm.maxAmmo : cm.ammoCount + ammoInc;
            UsePower(collision);
            UIManager.Instance.changeAmmoCount(cm.ammoCount);
        }
    }
}