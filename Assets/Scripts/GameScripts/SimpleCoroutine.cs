using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCoroutine : MonoBehaviour
{
    private static SimpleCoroutine m_Instance = null;
    public static SimpleCoroutine Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (new GameObject("SimpleCoroutine")).AddComponent<SimpleCoroutine>();
                DontDestroyOnLoad(m_Instance.gameObject);
            }
            return m_Instance;
        }
    }
}
