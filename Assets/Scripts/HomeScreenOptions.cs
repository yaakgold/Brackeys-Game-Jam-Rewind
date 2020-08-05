using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreenOptions : MonoBehaviour
{
    public Options homeOption = Options.Start;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (homeOption)
        {
            case Options.Tutorial:
                break;
            case Options.Start:
                break;
            case Options.Exit:
                break;
            default:
                break;
        }
    }
}

public enum Options { Tutorial, Start, Exit};
