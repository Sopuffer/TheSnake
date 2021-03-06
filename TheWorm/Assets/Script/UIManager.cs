﻿using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    public TMP_Text timeText;

    public float minutes = 0;
    public float seconds = 0;
    public float maxSeconds = 60;
    public float maxMinutes = 10;

    float numberLimit = 10;
    int highScore;

    private void Update()
    {
        TrackingScore();
        CurrentTime();
    }

    public void SaveHighScore()
    {
        int score = playerMovement.points;
        highScore = PlayerPrefs.GetInt("HighScore");
        if (highScore < playerMovement.points)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }    
 
    }

    public void TrackingScore()
    {
        if (playerMovement.points < numberLimit)
        {
            scoreText.text = "Current Score: 0" + playerMovement.points.ToString();
        }
        else
        {
            scoreText.text = "Current Score: " + playerMovement.points.ToString();
        }

        if (highScore < numberLimit)
        {
            highscoreText.text = "High Score: 0" + highScore.ToString();
        }
        else
        {
            highscoreText.text = "High Score: " + highScore.ToString();
        }
    }

    public void CurrentTime()
    {
        seconds += Time.deltaTime;
        if (seconds >= maxSeconds)
        {
            seconds -= maxSeconds;
            minutes++;
        }

        if (minutes < maxMinutes)
        {
            if (seconds < numberLimit)
            {
                timeText.text = "Elapsed Time: 0" + (int)minutes + ":0" + (int)seconds;
            }
            else
            {
                timeText.text = "Elapsed Time: 0" + (int)minutes + ":" + (int)seconds;

            }
        }

        else
        {
            if (seconds < numberLimit)
            {
                timeText.text = "Elapsed Time: " + (int)minutes + ":0" + (int)seconds;
            }
            else
            {
                timeText.text = "Elapsed Time: " + (int)minutes + ":" + (int)seconds;

            }
        }
    }
}
