using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Summary : MonoBehaviour
{
    public TextMeshProUGUI Last_pos;
    public TextMeshProUGUI Last_score;

    private string newName;

    public void BackButton()
    {
        AddNewEntry(Interface.currentScore, this.newName);
        SceneManager.LoadScene(0); // Main menu
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Read saves
        string jsonString = PlayerPrefs.GetString("scoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        for (int i = 0; i < highscores.scoreEntryList.Count; i++)
        {
            if (Interface.currentScore > highscores.scoreEntryList[i].score)
            {
                Last_pos.text = (i + 1).ToString();
                break;
            }
            else
            {
                Last_pos.text = (highscores.scoreEntryList.Count + 1).ToString();
            }
        }

        Last_score.text = Interface.currentScore.ToString();
    }

    public void ReadStringInput(string newString)
    {
        // Take 3 first letters from string - .Substring(start, length);
        this.newName = newString.Substring(0, 3);
    }

    // Add new entry to score table
    public void AddNewEntry(int score, string name)
    {
        // Create new score entry
        ScoreEntry newscoreEntry = new ScoreEntry { score = score, name = name };

        // Read saves
        string jsonString = PlayerPrefs.GetString("scoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // Add new entry do highscores
        highscores.scoreEntryList.Add(newscoreEntry);

        // Sort entry list by Score
        for (int i = highscores.scoreEntryList.Count - 1; i > 0; i--)
        {
            if (highscores.scoreEntryList[i].score > highscores.scoreEntryList[i - 1].score)
            {
                //Swap
                ScoreEntry tmp = highscores.scoreEntryList[i];
                highscores.scoreEntryList[i] = highscores.scoreEntryList[i - 1];
                highscores.scoreEntryList[i - 1] = tmp;
            }
        }

        // Remove 11th entry
        if (highscores.scoreEntryList.Count > 10)
        {
            highscores.scoreEntryList.RemoveAt(10);
        }

        // Add 1 to counter
        highscores.gamesCounter++;

        // Save updated highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("scoreTable", json);
        PlayerPrefs.Save();
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
