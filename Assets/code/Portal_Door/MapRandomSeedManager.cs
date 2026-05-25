using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图随机种子管理器 - 单例
/// </summary>
public class MapRandomSeedManager : MonoBehaviour
{
    // ========== 单例实现 ==========
    public static MapRandomSeedManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 第一次创建时初始化随机种子
        InitializeRandomSeed();
    }

    // ========== 随机种子相关 ==========
    public int randomSeed2;
    public int randomSeed3;

    public GameObject reward;
    public GameObject map_1;
    public GameObject map_2;
    public GameObject map_3;

    // 当前选中的地图场景名
    [Header("当前地图")]
    [SerializeField] public string currentMapSceneName;

    // 可用地图列表
    private readonly string[] availableMaps = { "GameScene2", "GameScene3" };

    /// <summary>
    /// 初始化随机种子，在第一次创建时调用
    /// </summary>
    private void InitializeRandomSeed()
    {
        // 初始化时随机选择一个地图
        RandomSelectMap();
    }

    public void RandomSelectMap()
    {
        randomSeed2 = UnityEngine.Random.Range(1,4);
        randomSeed3 = UnityEngine.Random.Range(1,4);
    }

    public void InitMap()
    {
        currentMapSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        print(currentMapSceneName);

        reward=GameObject.Find("Reward");
        if(reward!=null)
        reward.SetActive(false);
        map_1 = GameObject.Find("1");
        map_2 = GameObject.Find("2");
        map_3 = GameObject.Find("3");
        if (currentMapSceneName == "GameScene2")
        {
            switch (randomSeed2)
            {
                case 1:
                    map_1.SetActive(true);
                    map_2.SetActive(false);
                    map_3.SetActive(false);
                    break;
                case 2:
                    map_1.SetActive(false);
                    map_2.SetActive(true);
                    map_3.SetActive(false);
                    break;
                case 3:
                    map_1.SetActive(false);
                    map_2.SetActive(false);
                    map_3.SetActive(true);
                    break;
            }

        }
        if(currentMapSceneName == "GameScene3")
        {
            switch (randomSeed3)
            {
                case 1:
                    map_1.SetActive(true);
                    map_2.SetActive(false);
                    map_3.SetActive(false);
                    break;
                case 2:
                    map_1.SetActive(false);
                    map_2.SetActive(true);
                    map_3.SetActive(false);
                    break;
                case 3:
                    map_1.SetActive(false);
                    map_2.SetActive(false);
                    map_3.SetActive(true);
                    break;
            }
        }
            Invoke("hideFadePanel", 0.3f);
        
    }
    private void hideFadePanel()
    {
        // 隐藏 FadePanel
        UIManager.Instance.HidePanel<FadePanel>(false);
    }
}
