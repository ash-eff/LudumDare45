using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowUI : MonoBehaviour
{

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }
}
