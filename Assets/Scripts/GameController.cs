using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool isGameOver;
    private bool isGamePaused;
    private bool isGameRunning;
    private bool isGameWon;

    private int moneyValueStolenByPlayer;
    private int totalMoneyAvailableToSteal;
    private int playerScore;

    private float gameRunTime;
    private float musicVolume = 1f; 
    private float sfxVolume = 1f;

    public bool IsGameOver { get { return isGameOver; } }
    public bool IsGamePaused { get { return isGamePaused; } }
    public bool IsGameRunning { get { return isGameRunning; } }
    public float MusicVolume { get { return musicVolume; } }
    public float SFXVolume { get { return sfxVolume; } }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        CheckForPlayerInput();
    }

    void CheckForPlayerInput()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PauseGameMenu();
        }
    }

    void PauseGameMenu()
    {
        isGamePaused = !isGamePaused;
        pauseMenu.SetActive(isGamePaused);
        if (isGamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void MoneyStolen(int amount)
    {
        moneyValueStolenByPlayer += amount;
    }

    void GameOver()
    {
        if (isGameWon)
        {
            // calculate player score
            // show game won screen with score
        }
        else
        {
            // show game lost screen with stats
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
