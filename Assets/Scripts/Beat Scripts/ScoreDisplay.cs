using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private Text scoreText;
    private int score = 0;

    void Start()
    {
        scoreText = GetComponent<Text>();
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateScore(int newScore)
    {
        score = newScore;
        UpdateScoreText();
    }
}

