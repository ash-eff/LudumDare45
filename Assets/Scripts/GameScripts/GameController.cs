using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject winPanel;
    public TextMeshProUGUI endScoreText;
    public TextMeshProUGUI endTimeText;
    public TextMeshProUGUI endRankText;
    public ValuableItem[] valuableItems;

    private bool isGameOver;
    //private bool isGameRunning;
    private bool isGameWon;
    private int playerScore;
    private float gameRunTime;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip countSound;
    private MenuController menuController;
    private PlayerManager playerManager;

    public bool IsGameOver { get { return isGameOver; } }
    //public bool IsGameRunning { get { return isGameRunning; } }

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

    public void Awake()
    {
        valuableItems = FindObjectsOfType<ValuableItem>();
        playerManager = FindObjectOfType<PlayerManager>();
        menuController = FindObjectOfType<MenuController>();
    }

    //private void Update()
    //{
    //    musicSource.volume = menuController.MusicVolume;
    //    sfxSource.volume = menuController.SFXVolume;
    //}

    void GameOver()
    {
        musicSource.Stop();
        isGameOver = true;
        if (isGameWon)
        {
            StartCoroutine(CalculateScore());
        }
        else
        {
            // show game lost screen with stats
            Debug.Log("GAME OVER! YOU LOSE!");
        }
    }

    IEnumerator CalculateScore()
    {
        endTimeText.text = Time.timeSinceLevelLoad.ToString("00:00:00");
        endRankText.text = "Rank: ";
        endScoreText.text = "Money Stolen: $" + playerScore.ToString("0000");
        winPanel.SetActive(true);
        int moneyStolen = playerManager.TotalMoneyStolen;
        while(moneyStolen > 0)
        {
            sfxSource.PlayOneShot(countSound);
            playerScore++;
            moneyStolen--;
            //playerManager.UpdateMoneyStolenValue(moneyStolen);
            endScoreText.text = "Money Stolen: $" + playerScore.ToString("0000");
            yield return new WaitForSecondsRealtime(.01f);
        }

        float finalScoreTotal = (playerScore / 15) / valuableItems.Length;
        float percentage = Mathf.RoundToInt(finalScoreTotal * 100f);

        if (percentage == 100 && playerManager.lives == 3)
        {
            endRankText.text = "Rank: S+";
        }
        else if(percentage > 99)
        {
            endRankText.text = "Rank: S-";
        }
        else if (percentage > 90)
        {
            endRankText.text = "Rank: A";
        }
        else if (percentage > 80)
        {
            endRankText.text = "Rank: B";
        }
        else if (percentage > 70)
        {
            endRankText.text = "Rank: C";
        }
        else
        {
            endRankText.text = "Rank: D";
        }
    }
}
