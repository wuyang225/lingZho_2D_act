using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel_Background : MonoBehaviour
{
    public static Parallel_Background Instance { get; private set; }

    private Camera mainCamera;
    private float lastCameramovex;
    private float camerleftEdge;
    private float camerrightEdge;

    private float lastCameramovey;
    private float camerTopEdge;
    private float camerBottomEdge;

    [SerializeField]
    private Parallel_Layer[] backagroundlayers;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        mainCamera = Camera.main;
        lastCameramovex = mainCamera.transform.position.x;
        lastCameramovey = mainCamera.transform.position.y;

        foreach (Parallel_Layer layer in backagroundlayers)
        {
            layer.setLayerImagewith();
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Main")
            DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        // 设置目标帧率为 -1 即为无上限
        Application.targetFrameRate = -1;

        // 同时关闭垂直同步（否则会被显示器刷新率限制）
        QualitySettings.vSyncCount = 0;
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void FixedUpdate()
    {
        // 场景切换后重新获取相机引用
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) return;
            lastCameramovex = mainCamera.transform.position.x;
            lastCameramovey = mainCamera.transform.position.y;
        }

        float distanceToMove = lastCameramovex - mainCamera.transform.position.x;
        lastCameramovex = mainCamera.transform.position.x;
        camerrightEdge = mainCamera.transform.position.x + distanceToMove + 3;
        camerleftEdge = mainCamera.transform.position.x + distanceToMove - 3;

        foreach (Parallel_Layer layer in backagroundlayers)
        {
            layer.move(distanceToMove);
            layer.Loopimage(camerrightEdge, camerleftEdge);
        }

        float distanceToMoveY = lastCameramovey - mainCamera.transform.position.y;
        lastCameramovey = mainCamera.transform.position.y;
        camerTopEdge = mainCamera.transform.position.y + distanceToMoveY + 3;
        camerBottomEdge = mainCamera.transform.position.y + distanceToMoveY - 3;

        foreach (Parallel_Layer layer in backagroundlayers)
        {
            layer.MoveY(distanceToMoveY);
        }
    }
}
