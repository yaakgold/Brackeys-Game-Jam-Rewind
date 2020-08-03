using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldObj : MonoBehaviour
{
    public float distance, speed;
    public bool startGoing = false;
    public Vector3 axis = new Vector3(0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y - distance);
        startGoing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startGoing)
            transform.RotateAround(transform.parent.position, axis, speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("FlyProj"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
