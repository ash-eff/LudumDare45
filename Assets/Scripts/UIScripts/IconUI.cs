using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconUI : MonoBehaviour
{
    public WindowUI window;

    public void OpenWindow()
    {
        if (!window.gameObject.activeInHierarchy)
        {
            window.gameObject.SetActive(true);
        }
    }
}
