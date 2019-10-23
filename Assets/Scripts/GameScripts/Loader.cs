using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public GameObject scanner;

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void PointerEnter()
    {
        scanner.SetActive(true);
    }

    public void PointerExit()
    {
        scanner.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
