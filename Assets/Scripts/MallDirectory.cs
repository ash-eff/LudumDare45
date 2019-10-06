using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MallDirectory : MonoBehaviour
{
    public GameObject mallDirectory;

    public void OpenMallDirectory()
    {
        Time.timeScale = 0;
        mallDirectory.SetActive(true);
    }

    public void CloseMallDirectory()
    {
        Time.timeScale = 1;
        mallDirectory.SetActive(false);
    }
}
