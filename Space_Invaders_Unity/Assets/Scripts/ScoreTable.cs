using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<ScoreEntry> scoreEntryList;
    private List<Transform> scoreEntryTransformList;

    public TextMeshProUGUI Games_count;

    // Init of the high score table, reading data
    private void Awake()
    {
        entryContainer = transform.Find("ScoreEntryContainer");
        entryTemplate = entryContainer.Find("ScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        // Read saves
        string jsonString = PlayerPrefs.GetString("scoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        scoreEntryTransformList = new List<Transform>();
        foreach (ScoreEntry scoreEntry in highscores.scoreEntryList)
        {
            CreateScoreEntryTransform(scoreEntry, entryContainer, scoreEntryTransformList);
        }

        // Add 1 to counter
        highscores.gamesCounter++;
        // Print no of games
        Games_count.text = highscores.gamesCounter.ToString();

        // Save data
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("scoreTable", json);
        PlayerPrefs.Save();
    }

    // Method drawing one row of high score table
    private void CreateScoreEntryTransform(ScoreEntry scoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 80f;
        
        // Prepare transformation of the template
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        // Print rank
        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }
        entryTransform.Find("Text_Pos").GetComponent<TextMeshProUGUI>().text = rankString;   

        // Print score
        int score = scoreEntry.score;
        entryTransform.Find("Text_Score").GetComponent<TextMeshProUGUI>().text = score.ToString();

        // Print name
        string name = scoreEntry.name;
        entryTransform.Find("Text_Name").GetComponent<TextMeshProUGUI>().text = name;

        transformList.Add(entryTransform);
    }
  
    // Class to store all statistics data
    private class Highscores
    {
        public List<ScoreEntry> scoreEntryList;
        public int gamesCounter;
    }

    // Class to store single score entry
    [System.Serializable]
    private class ScoreEntry
    {
        public int score;
        public string name;
    }
}
