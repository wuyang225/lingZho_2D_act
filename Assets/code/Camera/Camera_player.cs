using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;  // 添加这行

public class Camera_player : MonoBehaviour
{
    public static Camera_player Instance { get; private set; }

    private CinemachineConfiner confiner;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        confiner = GetComponent<CinemachineConfiner>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    /// <summary>
    /// 设置相机边界
    /// </summary>
    public void SetBoundingShape(Collider2D newBounds)
    {
        if (confiner != null)
        {
            confiner.m_BoundingShape2D = newBounds;
        }
    }

    void Update()
    {
    }
}
