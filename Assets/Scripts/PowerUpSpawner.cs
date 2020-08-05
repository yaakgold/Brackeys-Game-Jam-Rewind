using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public ePowerUp pSpawn;
    public List<GameObject> prefabs;
    public List<PowerProbs> powerProbs = new List<PowerProbs>();
    public bool isSelectingItem = false, spawnItem = false;
    public CharacterMovement player;
    public bool isHome = false;
    public GameObject power;
    public GameObject powerType;
    public float endTime = 5, startTime = 0;

    public float minAmmoCount;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject prefab in prefabs)
        {
            powerProbs.Add(new PowerProbs(prefab.name, prefab, .25f));
        }

        ChoosePowerUp(pSpawn);
    }

    public void ChoosePowerUp(ePowerUp p)
    {
        bool spawned = false;
        switch (p)
        {
            case ePowerUp.SMART:
                isSelectingItem = true;
                break;
            case ePowerUp.RAPID_FIRE:
                spawned = true;
                powerType = prefabs.Find(x => x.name.Contains("RapidFire"));
                power = Instantiate(powerType, transform.position, Quaternion.identity, transform.parent);
                break;
            case ePowerUp.SHOTGUN:
                powerType = prefabs.Find(x => x.name.Contains("Burstfire"));
                power = Instantiate(powerType, transform.position, Quaternion.identity, transform.parent);
                spawned = true;
                break;
            case ePowerUp.SHIELD:
                powerType = prefabs.Find(x => x.name.Contains("Shield"));
                power = Instantiate(powerType, transform.position, Quaternion.identity, transform.parent);
                spawned = true;
                break;
            case ePowerUp.AMMO:
                powerType = prefabs.Find(x => x.name.Contains("Ammo"));
                power = Instantiate(powerType, transform.position, Quaternion.identity, transform.parent);
                spawned = true;
                break;
            default:
                break;
        }

        if (spawned && !isHome)
            Destroy(gameObject);
        else if (isHome)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isSelectingItem) //This is the smart pickup system
        {
            //Lower the chance of spawning a shield if the player has 4 already
            if(GameManager.Instance.numShields == 4)
            {
                powerProbs.Find(x => x.probName == "Shield").probability = .1f;
            }
            else if(GameManager.Instance.numShields == 0)
            {
                powerProbs.Find(x => x.probName == "Shield").probability = .3f;
            }
            else
            {
                powerProbs.Find(x => x.probName == "Shield").probability = powerProbs.Find(x => x.probName == "Shield").defaultProbability;
            }

            //Raise chance of spawning more ammo if the player is low on ammo
            if(GameManager.Instance.ammoCount <= minAmmoCount && GameManager.Instance.ammoCount > -1)
            {
                powerProbs.Find(x => x.probName == "Ammo").probability = .5f;
            }
            else
            {
                powerProbs.Find(x => x.probName == "Ammo").probability = powerProbs.Find(x => x.probName == "Ammo").defaultProbability;
            }


            //Check if the player is close, if so, spawn in a powerUp
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player)
            {
                if(Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) <= 15)
                {
                    spawnItem = true;
                }
            }
        }

        if(spawnItem)
        {
            Instantiate(prefabs[(int)(Choose(powerProbs))], transform.position, Quaternion.identity, transform.parent);
            Destroy(gameObject);
        }

        if(isHome && !power)
        {
            if (startTime < endTime)
                startTime += Time.deltaTime;
            else
            {
                startTime = 0;
                power = Instantiate(powerType, transform.position, Quaternion.identity, transform.parent);
            }
        }
    }
    public float Choose(List<PowerProbs> probs)
    {
        float total = 0;

        foreach (PowerProbs elem in probs)
        {
            total += elem.probability;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Count; i++)
        {
            if (randomPoint < probs[i].probability)
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i].probability;
            }
        }
        return probs.Count - 1;
    }

}

public enum ePowerUp
{
    SMART,
    RAPID_FIRE,
    SHOTGUN,
    SHIELD,
    AMMO
}

[System.Serializable]
public class PowerProbs
{
    public string probName;
    public GameObject powerUp;
    public float probability, defaultProbability;

    public PowerProbs(string n, GameObject pUp, float prob)
    {
        probName = n;
        powerUp = pUp;
        probability = defaultProbability = prob;
    }
}