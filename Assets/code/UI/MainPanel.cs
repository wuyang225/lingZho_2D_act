using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    [Header("UI 引用")]
    [SerializeField] private Button StartButton;      // 开始游戏按钮
    [SerializeField] private Button ShoundButton;       // 退出游戏按钮
    [SerializeField] private Button ExitButton;       // 退出游戏按钮

    [Header("过场配置")]
    [SerializeField] private float fadeDuration = 1f; // 淡入淡出时长

    protected override void Awake()
    {
        base.Awake();

             // 绑定按钮事件
        if (StartButton != null)
            StartButton.onClick.AddListener(OnStartButtonClicked);
        
        if (ShoundButton != null)
            ShoundButton.onClick.AddListener(OnSoundButtonClicked);

        if (ExitButton != null)
            ExitButton.onClick.AddListener(OnExitButtonClicked);
    }

    /// <summary>
    /// 开始游戏按钮点击事件
    /// </summary>
    private void OnStartButtonClicked()
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        // 开始过场协程
        StartCoroutine(StartGameCoroutine());
    }
    private void OnSoundButtonClicked()
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        // 开始过场协程
        UIManager.Instance.ShowPanel<SoundPanel>();
    }
    /// <summary>
    /// 退出游戏按钮点击事件
    /// </summary>
    private void OnExitButtonClicked()
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        Application.Quit();
    }

    /// <summary>
    /// 过场动画 + 加载场景协程（手动控制切换）
    /// </summary>
    private IEnumerator StartGameCoroutine()
    {
        // 1. 开始加载场景，但不自动切换
        AsyncOperation ao = SceneManager.LoadSceneAsync("GameScene");
        ao.allowSceneActivation = false; // 关键：禁止自动切换场景

        UIManager.Instance.ShowPanel<FadePanel>();

        // 4. 等待场景加载完成（加载进度到0.9表示资源加载完成）
        while (ao.progress < 0.9f)
        {
            yield return null; // 等待加载
        }

        // 5. 手动触发场景切换（这一步才会真正切场景）
        ao.allowSceneActivation = true;
        yield return ao; // 等待场景切换完成
        Camera.main.transform.parent.transform.GetComponentInChildren<CinemachineVirtualCamera>().Follow = GameObject.Find("Player").transform;
        UIManager.Instance.HidePanel<FadePanel>();
        // 6. 场景切换后隐藏主面板（可选）
        UIManager.Instance.HidePanel<MainPanel>();
    }

    // 重写显示方法，面板打开时自动淡入显示
    public override void ShowMe()
    {
        base.ShowMe();

    }
    public override void HideMe(UnityAction callBack)
    {
        isShow = false;
        canvasGroup.alpha = 0;
        //记录 传入的 当淡出成功后会执行的函数
        hideCallBack = callBack;
    }
    public override void Init()
    {
    }
}