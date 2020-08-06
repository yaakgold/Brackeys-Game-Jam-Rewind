using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderGroundCheck : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Ground"))
        {
            if(!GetComponentInParent<Enemy>().isFalling)
            {
                GetComponentInParent<Enemy>().hitF = true;
            }
        }
    }
}
