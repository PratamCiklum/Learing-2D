using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreInEndScreen : MonoBehaviour
{
    private void Awake()
    {
        TextMeshProUGUI scoreText = GetComponent<TextMeshProUGUI>();
        KillToScore score = GameObject.Find("Player").GetComponent<KillToScore>();
        scoreText.text = "Score : " + KillToScore.score.ToString();
        Debug.Log(scoreText.text);
        Debug.Log(KillToScore.score);
    }
}
