using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewind : MonoBehaviour
{
    public bool canRecordTime = true;
    public List<TimeStamp> timeList = new List<TimeStamp>();

    public bool isRewinding = false, startRewind = false;
    public float timeToRewind = 2;

    public float startTime;
    private int index;

    private bool hasStarted = false;
    private Enemy e;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(TryGetComponent<Enemy>(out e) && e.isStunned)
        {
            canRecordTime = false;
        }
        if(canRecordTime && !isRewinding)
        {
            timeList.Add(new TimeStamp(transform.position, Time.time, transform.localScale));
        }
        if(startRewind || (GameManager.Instance.isRewinding && !hasStarted))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            isRewinding = true;
            GameManager.Instance.isRewinding = true;
            canRecordTime = false;
            startTime = 0;
            index = timeList.Count - 1;
            hasStarted = true;
        }
        if(isRewinding || GameManager.Instance.isRewinding)
        {
            index = timeList.Count - 1;
            startRewind = false;
            if (index != 0 && startTime + (Time.deltaTime) < timeToRewind)
            {
                transform.position = timeList[index].position;
                transform.localScale = timeList[index].scale;
                startTime += Time.deltaTime;
                timeList.RemoveAt(index);
                index--;
                Debug.Log(index);
            }
            else if(startTime < timeToRewind + GameManager.Instance.waitTime)
            {
                startTime += Time.deltaTime;
                transform.position = timeList[index].position;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                GameManager.Instance.isPaused = true;
            }
            else
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                isRewinding = false;
                canRecordTime = true;
                GameManager.Instance.isRewinding = false;
                GameManager.Instance.isPaused = false;
                hasStarted = false;
            }
        }

        canRecordTime = true;
    }
}

[System.Serializable]
public class TimeStamp
{
    public float startTime;
    public Vector3 position;
    public Vector3 scale;

    public TimeStamp(Vector3 pos, float time, Vector3 s)
    {
        position = pos;
        startTime = time;
        scale = s;
    }
}
