using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmWindowUI : WindowUI
{
    public HackingTest hackingTest;

    public void Confirm()
    {
        hackingTest.ConnectToHost();
        this.gameObject.SetActive(false);
    }
}
