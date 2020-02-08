using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackableSource : MonoBehaviour, IInteractable
{
    public string consoleName;
    public string manufacturersInformation;
    public string difficultyRating;
    public bool unlocked;
    public WindowUI window;

    public void Interact()
    {

    }

    public string BeingTouched()
    {
        return "press e to hack";
    }
}
