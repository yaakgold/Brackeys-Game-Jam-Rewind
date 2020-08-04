using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public ePowerUp pSpawn;
    public List<GameObject> prefabs;
    public List<PowerProbs> powerProbs;
    public bool isSelectingItem = false;

    // Start is called before the first frame update
    void Start()
    {
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
                Instantiate(prefabs.Find(x => x.name.Contains("RapidFire")), transform.position, Quaternion.identity, transform.parent);
                break;
            case ePowerUp.SHOTGUN:
                Instantiate(prefabs.Find(x => x.name.Contains("Burstfire")), transform.position, Quaternion.identity, transform.parent);
                spawned = true;
                break;
            case ePowerUp.SHIELD:
                Instantiate(prefabs.Find(x => x.name.Contains("Shield")), transform.position, Quaternion.identity, transform.parent);
                spawned = true;
                break;
            case ePowerUp.AMMO:
                Instantiate(prefabs.Find(x => x.name.Contains("Ammo")), transform.position, Quaternion.identity, transform.parent);
                spawned = true;
                break;
            default:
                break;
        }

        if (spawned)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(isSelectingItem) //This is the smart pickup system
        {
            //Lower the chance of spawning a shield if the player has 4 already

        }
    }
    public float Choose(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
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
}