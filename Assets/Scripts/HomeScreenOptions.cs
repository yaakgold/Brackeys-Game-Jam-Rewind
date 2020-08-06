using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScreenOptions : MonoBehaviour
{
    public Options homeOption = Options.Start;
    public GameObject tempObj;

    public string[] text = new string[3];

    public int index = 0;
    public int stringIndex = 0;
    public float typeTime = .5f;

    private bool startStory = false, startString = true;
    private TextMeshProUGUI textObj;

    private float startTime = 0, endTime = 2, startType = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (homeOption == Options.Start)
        {
            textObj = tempObj.GetComponentInChildren<TextMeshProUGUI>();
            textObj.enabled = false;
            //Debug.Log("Start");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(startStory)
        {
            if(tempObj.GetComponent<Image>().color.a != 1)
                tempObj.GetComponent<Image>().color = new Color(0, 0, 0, tempObj.GetComponent<Image>().color.a + .05f);
            if(tempObj.GetComponent<Image>().color.a >= 1)
            {
                if(index < text.Length && stringIndex < text[index].Length && startString)
                {
                    if (startType > typeTime)
                    {
                        startType = 0;
                        char s = text[index][stringIndex];
                        textObj.text += s;
                        stringIndex++;
                        if (stringIndex == text[index].Length)
                        {
                            startString = false;
                            stringIndex = 0;
                            index++;
                        }
                    }
                    else
                    {
                        startType += Time.deltaTime;
                    }
                }
                else if(!startString)
                {
                    startTime += Time.deltaTime;
                    if(startTime > endTime)
                    {
                        startTime = 0;
                        startString = true;
                        textObj.text = "";
                    }
                }
            }
        }
    }

   public void run()
    {
        switch (homeOption)
        {
            case Options.Tutorial:
                Tutorial();
                break;
            case Options.Start:
                start();
                break;
            case Options.Exit:
                Exit();
                break;
            default:
                break;
        }
    }

    void Tutorial()
    {
        UIManager.Instance.isTut = true;
        tempObj.GetComponent<BoxCollider2D>().enabled = false;
    }

    void EndTutorial()
    {
        UIManager.Instance.isTut = false;
        tempObj.GetComponent<BoxCollider2D>().enabled = true;
    }

    void start()
    {
        tempObj.GetComponent<Image>().enabled = true;
        textObj.enabled = true;
        //textObj = tempObj.GetComponentInChildren<TextMeshProUGUI>();
        startStory = true;
        //SceneManager.LoadScene("Level1");
    }

    void Exit()
    {
        //
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CompareTag("EndTut") && collision.transform.CompareTag("Player"))
            EndTutorial();
    }
}

public enum Options { Tutorial, Start, Exit};
