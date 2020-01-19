using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HackingTest : MonoBehaviour
{
    public GameObject thisGameObject;

    public void CloseHack()
    {
        thisGameObject.SetActive(false);
    }
}
