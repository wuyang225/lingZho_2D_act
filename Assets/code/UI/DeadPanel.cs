using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;  // 添加这行
public class DeadPanel : BasePanel
{

    [Header("UI 引用")]
    [SerializeField] private Button AgainButton;      // 开始游戏按钮
    [SerializeField] private Button BackHomeButton;       // 退出游戏按钮

    protected override void Awake()
    {
        base.Awake();
        Time.timeScale = 1;
        // 绑定按钮事件
        if (AgainButton != null)
            AgainButton.onClick.AddListener(OnStartButtonClicked);
        AgainButton.gameObject.SetActive(false);
        if (BackHomeButton != null)
            BackHomeButton.onClick.AddListener(OnExitButtonClicked);
        BackHomeButton.gameObject.SetActive(false);

    }

    protected void showBtn()
    {
        BackHomeButton.gameObject.SetActive(true);
        AgainButton.gameObject.SetActive(true);
    }
    /// <summary>
    /// 开始游戏按钮点击事件
    /// </summary>
    private void OnStartButtonClicked()
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        // 开始过场协程
        StartCoroutine(StartGameCoroutine("GameScene"));
    }

    /// <summary>
    /// 退出游戏按钮点击事件
    /// </summary>
    private void OnExitButtonClicked()
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        // 应用退出：如果是编辑器环境停止运行，否则退出游戏
        StartCoroutine(StartGameCoroutine("MainScene"));
    }

    /// <summary>
    /// 过场动画 + 加载场景协程（手动控制切换）
    /// </summary>
    private IEnumerator StartGameCoroutine(string sceneName)
    {
        
        UIManager.Instance.HidePanel<Player_HeartBar>();
        UIManager.Instance.HidePanel<Enter_AttributeButton>();
        UIManager.Instance.HidePanel<SkillManagePanel>();
        UIManager.Instance.HidePanel<GoldNumber>();
        UIManager.Instance.HidePanel<BossHealth_BarPanel>();
        UIManager.Instance.HidePanel<SettingButton>();
        UIManager.Instance.HidePanel<OpertionButton>();
        UIManager.Instance.ShowPanel<FadePanel>();
        yield return new WaitForSeconds(0.2f);
        Destroy(player.gameObject);
        // 1. 开始加载场景，但不自动切换
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.allowSceneActivation = false; // 关键：禁止自动切换场景
        // 4. 等待场景加载完成（加载进度到0.9表示资源加载完成）
        while (ao.progress < 0.9f)
        {
            yield return null; // 等待加载
        }
        
        // 5. 手动触发场景切换（这一步才会真正切场景）
        ao.allowSceneActivation = true;
        yield return ao; // 等待场景切换完成
        if (sceneName == "GameScene")
            Camera.main.transform.parent.transform.GetComponentInChildren<CinemachineVirtualCamera>().Follow = GameObject.Find("Player").transform;
        yield return new WaitForSeconds(0.2f);
        // 6. 场景切换后隐藏主面板（可选）
        UIManager.Instance.HidePanel<DeadPanel>(false);
        
        if (sceneName== "GameScene")
        {
            bkMusic.Instance.ChangeAudioClip("game_bk_music");
            UIManager.Instance.ShowPanel<Player_HeartBar>();
            UIManager.Instance.ShowPanel<Enter_AttributeButton>();
            UIManager.Instance.ShowPanel<SkillManagePanel>();
            UIManager.Instance.ShowPanel<GoldNumber>();
            UIManager.Instance.ShowPanel<SettingButton>();
            UIManager.Instance.ShowPanel<OpertionButton>();
        }
        else if(sceneName == "MainScene")
        {
            UIManager.Instance.ShowPanel<MainPanel>();
        }
        UIManager.Instance.HidePanel<FadePanel>();
    }

    // 重写显示方法，面板打开时自动淡入显示
    public override void ShowMe()
    {
        base.ShowMe();

        Invoke(nameof(showBtn), 1);
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