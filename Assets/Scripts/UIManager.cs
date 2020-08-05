using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //public GameObject mainCanvas;
    public GameObject lifeCounter;
    public GameObject ammoCount;
    public GameObject score;
    public int livesLeft = 3;
    public List<Sprite> pixleNumbers = new List<Sprite>();
    public CharacterMovement player;
    public Transform background;
    public float backy = 0;
    public float backMove = 5;
    public Vector3 backPos;
    public float startx, endx;

    public bool hasLife, hasAmmo, hasScore, hasBack;

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
        if(livesLeft!=0)
        {
            if (hasLife)
                lifeCounter.GetComponentsInChildren<Image>()[livesLeft].enabled = false;
            else
                Debug.Log("GET LIFECOUNTER COMPONENT");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void changeAmmoCount(int ammo)
    {
        if (hasAmmo)
        {
            Image[] tempAmmo = ammoCount.GetComponentsInChildren<Image>();
            tempAmmo[0].sprite = pixleNumbers[ammo / 10];
            tempAmmo[1].sprite = pixleNumbers[ammo % 10];
        }
        else
            Debug.Log("GET AMMOCOUNTER COMPONENT");
    }

    public void changeScore(int s)
    {
        if (hasScore)
        {
            Image[] tempScore = score.GetComponentsInChildren<Image>();
            tempScore[0].sprite = pixleNumbers[s / 100];
            tempScore[1].sprite = pixleNumbers[s / 10];
            tempScore[2].sprite = pixleNumbers[s % 10];
        }
        else
            Debug.Log("GET SCORECOUNTER COMPONENT");
    }
}
