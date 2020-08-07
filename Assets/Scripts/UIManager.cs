using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //public GameObject mainCanvas;
    public GameObject lifeCounter;
    public TextMeshProUGUI ammoCount;
    public TextMeshProUGUI score;
    public int livesLeft = 3;
    public List<Sprite> pixleNumbers = new List<Sprite>();
    public CharacterMovement player;
    public Transform background;
    public float backy = 0;
    public float backMove = 5;
    public Vector3 backPos;
    public float startx, endx;

    public bool hasLife, hasAmmo, hasScore, hasBack, isTut = false;

    public static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

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
        hasBack = !!GameObject.FindGameObjectWithTag("Background");
        if (hasBack)
        {
            background = GameObject.FindGameObjectWithTag("Background").transform;
            backPos = new Vector3(0, backy, 100);
            startx = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position.x;
            endx = GameObject.FindGameObjectWithTag("LevelEnd").transform.position.x;
        }
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        hasLife = !!lifeCounter;
        hasScore = !!score;
        hasAmmo = !!ammoCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            GameObject.FindGameObjectWithTag("Player").TryGetComponent<CharacterMovement>(out player);
        }
        else if(hasBack)
        {
            backPos.x = (player.transform.position.x/ (endx-startx)) * backMove;
            background.localPosition = backPos;
        }
    }

    public void LoseLife()
    {
        livesLeft--;
        GameManager.Instance.AddToScore(-10);
        if(livesLeft!=0)
        {
            if (hasLife)
                lifeCounter.GetComponentsInChildren<Image>()[livesLeft].enabled = false;
            else
                Debug.Log("GET LIFECOUNTER COMPONENT");
        }
        else
        {
            GameManager.Instance.AddToScore(-100);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void changeAmmoCount(int ammo)
    {
        if (hasAmmo)
        {
            ammoCount.text = "" + ammo;
        }
        else
            Debug.Log("GET AMMOCOUNTER COMPONENT");
    }

    public void changeScore(int s)
    {
        if (hasScore)
        {
            score.text = "" + s;
        }
        else
            Debug.Log("GET SCORECOUNTER COMPONENT");
    }
}
