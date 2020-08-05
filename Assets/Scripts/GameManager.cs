using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isRewinding = false;
    public bool isPaused;
    public Image rewindVisual, timeVis;
    public float waitTime = 2, timer;
    public GameObject enemyProj;
    public GameObject player;
    public int numShields = 0, ammoCount = -1;

    public static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isRewinding)
        {
            rewindVisual.enabled = true;
            if (!isPaused)
            {
                //rewindVisual.color = rewindVisual.color = new Color(1, 0, 0, .5f);
                //rewindVisual.transform.Rotate(new Vector3(0, 0, -1), 2);
            }
            else
            {
                //rewindVisual.color = new Color(0, 0, 1, .5f);
                timeVis.enabled = true;
                timeVis.sprite = UIManager.Instance.pixleNumbers[Mathf.Max(Mathf.CeilToInt(timer), 0)];
                timer -= Time.deltaTime;
            }
            rewindVisual.transform.Rotate(new Vector3(0, 0, -1), 2);
        }
        else
        {
            rewindVisual.transform.eulerAngles *= 0;
            rewindVisual.enabled = false;
            timeVis.enabled = false;
            timer = waitTime;
        }
    }
}
