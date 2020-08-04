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
    public Vector3 backPos;
    public float startx, endx;

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
        background = GameObject.FindGameObjectWithTag("Background").transform;
        backPos = new Vector3(0, 0, 100);
        startx = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position.x;
        endx = GameObject.FindGameObjectWithTag("LevelEnd").transform.position.x;
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            GameObject.FindGameObjectWithTag("Player").TryGetComponent<CharacterMovement>(out player);
        }
        else
        {
            backPos.x = (player.transform.position.x/ (endx-startx)) * 5;
            background.localPosition = backPos;
        }
    }

    public void LoseLife()
    {
        livesLeft--;
        if(livesLeft!=0)
        {
            lifeCounter.GetComponentsInChildren<Image>()[livesLeft].enabled = false;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void changeAmmoCount(int ammo)
    {
        Image[] tempAmmo = ammoCount.GetComponentsInChildren<Image>();
        tempAmmo[0].sprite = pixleNumbers[ammo / 10];
        tempAmmo[1].sprite = pixleNumbers[ammo % 10];
    }

    public void changeScore(int s)
    {
        Image[] tempScore = score.GetComponentsInChildren<Image>();
        tempScore[0].sprite = pixleNumbers[s / 100];
        tempScore[1].sprite = pixleNumbers[s / 10];
        tempScore[2].sprite = pixleNumbers[s % 10];

    }
}
