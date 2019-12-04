using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource knockSource;

    public void PlayKnock()
    {
        knockSource.Play();
    }
}
