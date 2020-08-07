using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenLevels : MonoBehaviour
{
    public int score;

    public static BetweenLevels _instance;
    public static BetweenLevels Instance { get { return _instance; } }

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
        DontDestroyOnLoad(gameObject);
    }

    public void LevelSwitch()
    {
        score = GameManager.Instance.totalScore;
    }
}
