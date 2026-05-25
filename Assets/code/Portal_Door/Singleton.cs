using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton.cs — 通用单例基类
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}
