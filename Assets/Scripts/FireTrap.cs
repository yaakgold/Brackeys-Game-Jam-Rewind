using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public GameObject fireObj;
    public float timeBetweenFires, timeSinceLastFire, timeWithFire, timeFireLast;
    public bool startBetweenFireTime = true, startFireTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircleAll(transform.position, .5f, LayerMask.GetMask("Ground")).Length == 0)
        {
            Destroy(gameObject);
        }

        if (startBetweenFireTime)
        {
            if(timeSinceLastFire >= timeBetweenFires)
            {
                timeSinceLastFire = 0;
                fireObj.SetActive(true);

                startBetweenFireTime = false;
                startFireTime = true;
            }
            else
            {
                timeSinceLastFire += Time.deltaTime;
            }
        }

        if(startFireTime)
        {
            if(timeWithFire >= timeFireLast)
            {
                timeWithFire = 0;
                fireObj.SetActive(false);

                startBetweenFireTime = true;
                startFireTime = false;
            }
            else
            {
                timeWithFire += Time.deltaTime;
            }
        }
    }
}
