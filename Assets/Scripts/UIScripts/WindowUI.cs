using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowUI : MonoBehaviour
{
    public GameObject[] iconsSlots;
    public GameObject[] availableIcons;

    private void Start()
    {
        int i = 0;
        foreach(GameObject icon in availableIcons)
        {
            icon.transform.parent = iconsSlots[i].transform;
            icon.transform.localPosition = Vector2.zero;
            i++;
        }
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }
}
