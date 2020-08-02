using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckPlayer : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            GetComponentInParent<CharacterMovement>().isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            GetComponentInParent<CharacterMovement>().isGrounded = false;
        }
    }

}
