using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isRewinding = false;
    public bool isPaused;
    public SpriteRenderer rewindVisual;
    public float waitTime = 2;
    public GameObject enemyProj;

    public static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
                rewindVisual.color = rewindVisual.color = new Color(1, 0, 0, .5f);
            }
            else
            {
                rewindVisual.color = new Color(0, 0, 1, .5f);
            }
        }
        else
        {
            rewindVisual.enabled = false;
        }
    }
}
