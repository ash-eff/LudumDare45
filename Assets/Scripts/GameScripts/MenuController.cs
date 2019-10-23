using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public Image volumeFillBar;
    public Image sfxFillBar;

    private bool isGamePaused;
    private int maxMusicVolumePercent = 100;
    private float musicVolume = 1f;
    private int maxSFXVolumePercent = 100;
    private float sfxVolume = 1f;
    private int volumeChangePercent = 10;
    private GameController gameController;

    public bool IsGamePaused { get { return isGamePaused; } }
    public float MusicVolume { get { return musicVolume; } }
    public float SFXVolume { get { return sfxVolume; } }

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (gameController.IsGameOver)
        {
            return;
        }

        volumeFillBar.fillAmount = musicVolume;
        sfxFillBar.fillAmount = sfxVolume;
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
        float tempMusicVolume = musicVolume * maxMusicVolumePercent;
        if (tempMusicVolume < maxMusicVolumePercent)
        {
            tempMusicVolume += volumeChangePercent;
            musicVolume = Mathf.Round(tempMusicVolume / volumeChangePercent) / volumeChangePercent;
        }
    }

    public void IncreaseSFXVolume()
    {
        float tempSFXVolume = sfxVolume * maxSFXVolumePercent;
        if (tempSFXVolume < maxSFXVolumePercent)
        {
            tempSFXVolume += volumeChangePercent;
            sfxVolume = Mathf.Round(tempSFXVolume / volumeChangePercent) / volumeChangePercent;
        }
    }

    public void DecreaseMusicVolume()
    {
        float tempMusicVolume = musicVolume * maxSFXVolumePercent;
        if (tempMusicVolume > 0f)
        {
            tempMusicVolume -= volumeChangePercent;
            musicVolume = Mathf.Round(tempMusicVolume / volumeChangePercent) / volumeChangePercent;
        }
    }

    public void DecreaseSFXVolume()
    {
        float tempSFXVolume = sfxVolume * maxSFXVolumePercent;
        if (tempSFXVolume> 0f)
        {
            tempSFXVolume -= volumeChangePercent;
            sfxVolume = Mathf.Round(tempSFXVolume / volumeChangePercent) / volumeChangePercent;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void ReloadGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
