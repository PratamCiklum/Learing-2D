using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillToScore : MonoBehaviour
{
    public static int score;
    [SerializeField] TextMeshProUGUI scoreText;

    private void Awake()
    {
        score = 0;
    }
    private void Update()
    {
        scoreText.text = "Score : " + score.ToString();
    }
}
