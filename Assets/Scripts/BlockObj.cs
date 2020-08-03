using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockObj : MonoBehaviour
{
    public bool canBeDestroyed = true;
    public float startTime, secondsAlive = 0;
    public int secondsLiving, min, max;
    public SpriteRenderer spr;
    public float colorGrad = .05f;

    private Image image1;
    private Image image2;
    private Image image3;

    // Start is called before the first frame update
    void Start()
    {
        secondsLiving = Random.Range(min, max);
        spr = GetComponent<SpriteRenderer>();

        image1 = GetComponentsInChildren<Image>()[0];
        image2 = GetComponentsInChildren<Image>()[1];
        image3 = GetComponentsInChildren<Image>()[2];

        if(canBeDestroyed)
        {
            image1.enabled = true;
            image2.enabled = true;
            image3.enabled = false;
        }
        else
        {
            image1.enabled = false;
            image2.enabled = false;
            image3.enabled = true;
        }
    }

    public void StartBlock()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(canBeDestroyed)
        {
            secondsAlive += Time.deltaTime * (GameManager.Instance.isRewinding ? 2 : 1);

            if (secondsAlive >= secondsLiving)//Destroy block
            {
                foreach (Collider2D obj in Physics2D.OverlapCircleAll(transform.position, 1, LayerMask.GetMask("Enemy")))
                {
                    obj.GetComponent<Enemy>().isFalling = true;
                }

                Destroy(gameObject);
            }
            image1.sprite = UIManager.Instance.pixleNumbers[Mathf.Max(Mathf.FloorToInt((secondsLiving - secondsAlive) / 10), 0)];
            image2.sprite = UIManager.Instance.pixleNumbers[Mathf.Max(Mathf.FloorToInt((secondsLiving - secondsAlive) % 10), 0)];

            float alpha = 1 - (secondsAlive / secondsLiving);
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, alpha);
        }
    }
}
