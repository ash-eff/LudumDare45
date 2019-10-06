using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private bool isGameOver;
    private bool isGameRunning;
    private bool isGameWon;
    private int playerScore;
    private float gameRunTime;

    public bool IsGameOver { get { return isGameOver; } }
    public bool IsGameRunning { get { return isGameRunning; } }

    public void GameLost()
    {
        isGameWon = false;
        GameOver();
    }

    public void GameWon()
    {
        isGameWon = true;
        GameOver();
    }

    public void CalculatePlayerScore(int currentMoneyStolen)
    {
        Debug.Log("Calculate Score with value: " + currentMoneyStolen);
    }

    void GameOver()
    {
        isGameOver = true;
        if (isGameWon)
        {
            // calculate player score
            // show game won screen with score
        }
        else
        {
            // show game lost screen with stats
            Debug.Log("GAME OVER! YOU LOSE!");
        }
    }
}
