using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmWindowUI : WindowUI
{
    public WindowUI secondaryWindow;

    public void OpenSecondWindow()
    {
        secondaryWindow.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
