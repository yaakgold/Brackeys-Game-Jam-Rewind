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
    private Image image1, image2;

    private float startTime = 0, endTime = 1, startType = 0;
    private bool newText = false, quickType = false;
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
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
                quickType = true;
            if(tempObj.GetComponent<Image>().color.a != 1)
                tempObj.GetComponent<Image>().color = new Color(0, 0, 0, tempObj.GetComponent<Image>().color.a + .05f);
            if(tempObj.GetComponent<Image>().color.a >= 1)
            {
                if(index < text.Length && stringIndex < text[index].Length && startString)
                {
                    if (startType > typeTime || quickType)
                    {
                        startType = 0;
                        if (newText)
                        {
                            textObj.text = "";
                            newText = false;
                        }
                        char s = text[index][stringIndex];
                        if(stringIndex == text[index].Length -1)
                        {
                            if(s == '+')
                            {
                                textObj.text += " ";
                            }
                            else if(s == '/')
                            {
                                textObj.text += "\n";
                            }
                            else if(s == '-')
                            {
                                newText = true;
                            }
                            else
                            {
                                textObj.text += s;
                            }
                        }
                        else
                        {
                            textObj.text += s;
                            switch (index)
                            {
                                case 5:
                                    image1.enabled = true;
                                    break;
                                case 6:
                                    image1.GetComponent<Animator>().SetBool("x", true);
                                    break;
                                case 7:
                                    image2.enabled = true;
                                    break;
                                case 8:
                                    image1.enabled = false;
                                    image2.enabled = false;
                                    break;
                                case 9:
                                    image1.enabled = true;
                                    image1.rectTransform.position = new Vector2(500, 200);
                                    image1.GetComponent<Animator>().SetBool("ant", true);
                                    break;
                                case 11:
                                    image1.GetComponent<Animator>().SetBool("fade", true);
                                    break;
                                case 12:
                                    image2.enabled = true;
                                    image2.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                                    image2.GetComponent<Animator>().SetBool("block", true);
                                    break;
                                case 14:
                                    image2.GetComponent<Animator>().SetBool("cheese", true);
                                    break;
                                case 15:
                                    image2.rectTransform.localScale = new Vector3(2f, 2f, 1);
                                    break;
                                case 16:
                                    image2.rectTransform.localScale = new Vector3(3f,3f, 1);
                                    break;
                                case 17:
                                    image2.rectTransform.localScale = new Vector3(4f, 4f, 1);
                                    break;
                                case 19:
                                    image2.enabled = false;
                                    break;
                                default:
                                    break;
                            }
                        }
                        stringIndex++;
                        if (stringIndex == text[index].Length)
                        {
                            startString = false;
                            stringIndex = 0;
                            index++;
                            if(index == text.Length)
                                SceneManager.LoadScene("Level1");
                            quickType = false;
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
                    if(startTime > endTime || quickType)
                    {
                        startTime = 0;
                        startString = true;
                        //textObj.text = "";
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
        image1 = tempObj.GetComponentsInChildren<Image>()[1];
        image2 = tempObj.GetComponentsInChildren<Image>()[2];
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
