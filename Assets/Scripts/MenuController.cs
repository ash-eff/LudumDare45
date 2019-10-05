using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool isGamePaused;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private GameController gameController;

    public bool IsGamePaused { get { return isGamePaused; } }
    public float MusicVolume { get { return musicVolume; } }
    public float SFXVolume { get { return sfxVolume; } }

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (gameController.IsGameOver)
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

    public void IncreaseMusicVolume()
    {
        if (musicVolume < 1.0f && musicVolume >= 0.0)
        {
            musicVolume += .1f;
        }
    }

    public void IncreaseSFXVolume()
    {
        {
            if (sfxVolume < 1.0f && sfxVolume > 0.0)
            {
                sfxVolume -= .1f;
            }
        }
    }

    public void DecreaseMusicVolume()
    {
        if (musicVolume < 1.0f && musicVolume > 0.0)
        {
            musicVolume -= .1f;
        }
    }

    public void DecreaseSFXVolume()
    {

    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        //Application.Quit();
    }

    public void ReloadGame()
    {
        Debug.Log("Reload Game");
        //SceneManager.LoadScene(1);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Return To Main Menu");
        //SceneManager.LoadScene(0);
    }
}
