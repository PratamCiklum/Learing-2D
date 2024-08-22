using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class HighscoreTable : MonoBehaviour
{
    private class Highscore
    {
        public List<HighscoreEntry> entries;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highScoreEntryList;
    private List<Transform> highScoreEntryTransformList;
    private void Awake()
    {
        //PlayerPrefs.DeleteKey("highscoreTable");
        entryContainer = transform.Find("HighscoreEntryContainer");
        entryTemplate = entryContainer.Find("HighscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        UpdateHighscoreList();

    }

    private void UpdateHighscoreList()
    {
        int index = 0;
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        if (jsonString != "")
        {
            Highscore highscore = JsonUtility.FromJson<Highscore>(jsonString);

            QuickSort(highscore.entries, 0, highscore.entries.Count - 1);
            highScoreEntryTransformList = new List<Transform>();
            foreach (HighscoreEntry highScoreEntry in highscore.entries)
            {
                if (index >= 10)
                {
                    if (highScoreEntry.score <= highscore.entries[index-1].score)
                        highscore.entries.Remove(highScoreEntry);
                    string json = JsonUtility.ToJson(highscore);
                    PlayerPrefs.SetString("highscoreTable", json);
                    PlayerPrefs.Save();
                }

                CreateHighScoreEntry(highScoreEntry, entryContainer, highScoreEntryTransformList);
                index++;
            }
        }
        else
        {
            entryContainer.Find("NoHighscore").gameObject.SetActive(true);
        }
    }

    public static void AddHighscoreEntry(int score, string name)
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        name = name.ToLower();
        if (jsonString != "")
        {
            Highscore highscore = JsonUtility.FromJson<Highscore>(jsonString);

            HighscoreEntry highscoreEntry = highscore.entries.Find(e => e.name == name);

            if (highscoreEntry == null)
            {
                HighscoreEntry highScoreEntry = new HighscoreEntry { score = score, name = name };
                highscore.entries.Add(highScoreEntry);
            }
            else
            {
                if (highscoreEntry.score < score)
                {
                    highscoreEntry.score = score;
                }
            }

            string json = JsonUtility.ToJson(highscore);
            PlayerPrefs.SetString("highscoreTable", json);
            PlayerPrefs.Save();
        }
        else
        {
            Highscore highscore = new Highscore();
            highscore.entries = new List<HighscoreEntry> {
                new HighscoreEntry { score = score, name = name }
            };

            string json = JsonUtility.ToJson(highscore);
            PlayerPrefs.SetString("highscoreTable", json);
            PlayerPrefs.Save();
        }
    }

    private void CreateHighScoreEntry(HighscoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 64;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = transformList.Count + "th";
                break;
            case 1:
                rankString = "1st";
                break;
            case 2:
                rankString = "2nd";
                break;
            case 3:
                rankString = "3rd";
                break;
        }

        entryTransform.Find("PosText").GetComponent<TextMeshProUGUI>().text = rankString;

        int score = highScoreEntry.score;

        entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();

        entryTransform.Find("NameText").GetComponent<TextMeshProUGUI>().text = highScoreEntry.name;

        transformList.Add(entryTransform);
    }

    private void QuickSort(List<HighscoreEntry> list, int l, int h)
    {
        if (l < h)
        {
            int j = Partition(list, l, h);
            QuickSort(list, l, j - 1);
            QuickSort(list, j + 1, h);
        }

    }
    private int Partition(List<HighscoreEntry> list, int l, int h)
    {
        int i = l, j = h;
        int pivot = list[l].score;

        while (i < j)
        {
            do
            {
                i++;
            } while (i < j && pivot <= list[i].score);
            while (pivot > list[j].score)
            {
                j--;
            }
            if (i < j)
            {
                Swap(list, i, j);
            }
        }

        Swap(list, j, l);

        return j;
    }
    private void Swap(List<HighscoreEntry> A, int a, int b)
    {
        HighscoreEntry temp = A[a];
        A[a] = A[b];
        A[b] = temp;
    }
}
