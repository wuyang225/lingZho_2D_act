using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPanel : BasePanel
{
    [Header("UI 引用")]
    [SerializeField] private Button AgainButton;      // 开始游戏按钮
    [SerializeField] private Button BackHomeButton;   // 退出游戏按钮


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
        // 点击按钮时先恢复Player脚本
        EnablePlayerScript();
        StartCoroutine(StartGameCoroutine("GameScene"));
    }

    /// <summary>
    /// 退出游戏按钮点击事件
    /// </summary>
    private void OnExitButtonClicked()
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        // 点击按钮时先恢复Player脚本
        EnablePlayerScript();
        StartCoroutine(StartGameCoroutine("MainScene"));
    }

    /// <summary>
    /// 过场动画 + 加载场景协程（手动控制切换）
    /// </summary>
    private IEnumerator StartGameCoroutine(string sceneName)
    {
        // 1. 隐藏游戏UI面板
        UIManager.Instance.HidePanel<Player_HeartBar>();
        UIManager.Instance.HidePanel<Enter_AttributeButton>();
        UIManager.Instance.HidePanel<SkillManagePanel>();
        UIManager.Instance.HidePanel<GoldNumber>();
        UIManager.Instance.HidePanel<BossHealth_BarPanel>();
        UIManager.Instance.HidePanel<SettingButton>();
        UIManager.Instance.HidePanel<OpertionButton>();
        // 2. 显示淡出面板
        UIManager.Instance.ShowPanel<FadePanel>();
        yield return new WaitForSeconds(0.2f);

        // 3. 开始加载场景，但不自动切换
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.allowSceneActivation = false; // 关键：禁止自动切换场景

        // 4. 等待场景加载完成（加载进度到0.9表示资源加载完成）
        while (ao.progress < 0.9f)
        {
            yield return null; // 等待加载
        }
        Destroy(player.gameObject);
        // 5. 手动触发场景切换（这一步才会真正切场景）
        ao.allowSceneActivation = true;
        yield return ao; // 等待场景切换完成
        if(sceneName== "GameScene")
        Camera.main.transform.parent.transform.GetComponentInChildren<CinemachineVirtualCamera>().Follow = GameObject.Find("Player").transform;
        yield return new WaitForSeconds(0.2f);

        // 6. 场景切换后隐藏胜利面板（强制销毁，无动画）
        UIManager.Instance.HidePanel<WinPanel>(false);

        // 7. 根据场景显示对应UI
        if (sceneName == "GameScene")
        {
            // 新场景加载后重新查找Player并恢复（防止场景切换后引用失效）
            GameObject newPlayerObj = GameObject.Find("Player");
            if (newPlayerObj != null)
            {
                player = newPlayerObj.GetComponent<Player>();
                EnablePlayerScript(); // 确保新场景的Player是启用状态
            }

            UIManager.Instance.ShowPanel<Player_HeartBar>();
            UIManager.Instance.ShowPanel<Enter_AttributeButton>();
            UIManager.Instance.ShowPanel<SkillManagePanel>();
            UIManager.Instance.ShowPanel<GoldNumber>();
            UIManager.Instance.ShowPanel<SettingButton>();
            UIManager.Instance.ShowPanel<OpertionButton>();
        }
        else if (sceneName == "MainScene")
        {
            UIManager.Instance.ShowPanel<MainPanel>();
        }

        // 8. 隐藏淡出面板（确保场景切换完成后再隐藏）
        UIManager.Instance.HidePanel<FadePanel>();
    }

    // 重写显示方法：面板打开时禁用Player脚本 + 自动淡入显示
    public override void ShowMe()
    {
        base.ShowMe();
        // 显示面板时立即禁用Player脚本
        DisablePlayerScript();
        Invoke(nameof(showBtn), 1);
    }

    // 重写隐藏方法：隐藏面板时恢复Player脚本
    public override void HideMe(UnityAction callBack)
    {
        isShow = false;
        canvasGroup.alpha = 0;
        // 恢复Player脚本
        EnablePlayerScript();
        // 记录淡出成功后执行的函数
        hideCallBack = callBack;
    }

    public override void Init() { }

    // 面板销毁时确保恢复Player脚本（防止异常情况）
    private void OnDestroy()
    {
        EnablePlayerScript();
    }
}