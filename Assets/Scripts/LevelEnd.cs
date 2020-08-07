using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    public GameObject tempObj;

    public string[] text = new string[3];
    public int index = 0, stringIndex = 0;
    public float typeTime = .02f;

    private bool startStory = false, startString = true, newText = false, quickType = false;
    private TextMeshProUGUI textObj;
    private Image image1, image2;
    private float startTime = 0, endTime = 1, startType = 0;

    // Start is called before the first frame update
    void Start()
    {
        textObj = tempObj.GetComponentInChildren<TextMeshProUGUI>();
        textObj.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startStory)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
                quickType = true;
            if (tempObj.GetComponent<Image>().color.a != 1)
                tempObj.GetComponent<Image>().color = new Color(0, 0, 0, tempObj.GetComponent<Image>().color.a + .05f);
            if (tempObj.GetComponent<Image>().color.a >= 1)
            {
                if (index < text.Length && stringIndex < text[index].Length && startString)
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
                        //different endings for strings
                        if (stringIndex == text[index].Length - 1)
                        {
                            if (s == '+')
                            {
                                textObj.text += " ";
                            }
                            else if (s == '/')
                            {
                                textObj.text += "\n";
                            }
                            else if (s == '-')
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
                            //changes the images accordingly
                            textObj.text += s;
                            switch (SceneManager.GetActiveScene().buildIndex)
                            {
                                case 1:
                                    switch (index)
                                    {
                                        case 5:
                                            image1.enabled = true;
                                            break;
                                        case 6:
                                            image1.rectTransform.localScale = new Vector3(2, 2, 1);
                                            break;
                                        case 7:
                                            image1.GetComponent<Animator>().SetBool("cheese", true);
                                            break;
                                        case 8:
                                            image1.enabled = false;
                                            break;
                                        case 14:
                                            image1.enabled = true;
                                            image1.GetComponent<Animator>().SetBool("fly", true);
                                            break;
                                        case 17:
                                            image2.enabled = true;
                                            break;
                                        case 19:
                                            image1.enabled = false;
                                            image2.enabled = false;
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                case 2:
                                    switch(index)
                                    {
                                        case 1:
                                            image1.enabled = true;
                                            break;
                                        case 3:
                                            image1.enabled = false;
                                            break;
                                        case 5:
                                            image1.enabled = true;
                                            image1.GetComponent<Animator>().SetBool("nocheese", true);
                                            break;
                                        case 8:
                                            image1.enabled = false;
                                            break;
                                        case 11:
                                            image1.enabled = true;
                                            image1.GetComponent<Animator>().SetBool("spider", true);
                                            break;
                                        case 13:
                                            image2.enabled = true;
                                            break;
                                        case 15:
                                            image1.enabled = false;
                                            image2.enabled = false;
                                            break;
                                        default:
                                            break;
                                    }
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
                            if (index == text.Length && SceneManager.GetActiveScene().buildIndex != 3)
                            {
                                GameManager.Instance.AddToScore(100);
                                //BetweenLevels.Instance.LevelSwitch();
                                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                            }
                            quickType = false;
                        }
                    }
                    else
                    {
                        startType += Time.deltaTime;
                    }
                }
                else if (!startString)
                {
                    startTime += Time.deltaTime;
                    if (startTime > endTime || quickType)
                    {
                        startTime = 0;
                        startString = true;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && SceneManager.GetActiveScene().buildIndex != 3)
        {
            tempObj.GetComponent<Image>().enabled = true;
            textObj.enabled = true;
            startStory = true;
            image1 = tempObj.GetComponentsInChildren<Image>()[1];
            image2 = tempObj.GetComponentsInChildren<Image>()[2];
            startStory = true;
        }
    }
}
