using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHackable
{
    bool AccessGranted();
    void Hack();
    GameObject GetAttachedGameObject();
    string GetSystemName();
}
