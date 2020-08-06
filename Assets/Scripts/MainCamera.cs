using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player;
    public float starty;
    // Start is called before the first frame update
    void Start()
    {
        starty = transform.position.y;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, UIManager.Instance.isTut ? Mathf.Clamp(Mathf.Lerp(transform.position.y, player.transform.position.y, Time.deltaTime), -16, 16) : starty, transform.position.z);
    }
}
