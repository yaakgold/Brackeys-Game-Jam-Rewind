using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class HighScoreSave : MonoBehaviour
{
    public string path;
    public static AllScores allScores = new AllScores();
    public TextMeshProUGUI highScoreText;

    private void Awake()
    {
        path = Application.persistentDataPath + "/highscores.json";
        ReadHighScores();
    }

    public void WriteOutHighScores()
    {
        allScores.scores.Sort((hs1, hs2) => (int)(hs2.score - hs1.score));
        string scores = JsonUtility.ToJson(allScores);
        File.WriteAllText(path, scores);
    }

    public void ReadHighScores()
    {
        allScores = JsonUtility.FromJson<AllScores>(File.ReadAllText(path));
    }
}

[System.Serializable]
public class AllScores 
{
    public List<HighScore> scores = new List<HighScore>();
}

[System.Serializable]
public class HighScore
{
    public int score;
    public string name;

    public HighScore(int s, string n)
    {
        score = s;
        name = n;
    }
}
