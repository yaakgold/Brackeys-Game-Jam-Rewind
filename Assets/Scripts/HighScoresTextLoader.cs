using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoresTextLoader : MonoBehaviour
{
    public TextMeshPro text;

    private void Start()
    {
        text.text = "High Scores:\n\n";
        foreach (HighScore hs in HighScoreSave.allScores.scores)
        {
            text.text += hs.name + " - " + hs.score + "\n";
            text.text += "-----------\n";
        }
    }
}
