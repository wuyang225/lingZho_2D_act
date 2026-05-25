using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopPanel : BasePanel
{
    [Header("玩家引用")]
    [SerializeField] private Player player;
    private float currentGold;

    [Header("UI 引用")]
    [SerializeField] private Text CoinNumberText;      // 金币数量文本
    [SerializeField] private Button RoleUpgradeBtn;   // 角色升级按钮
    [SerializeField] private Button SkillAcquisitionBtn; // 技能获取按钮
    [SerializeField] private Text RoleUpgradeText;   // 角色文本
    [SerializeField] private Text SkillAcquisitionText; // 技能文本
    [SerializeField] private Button ExitBtn;         // 退出按钮
    [SerializeField] private Text PromptText;        // 提示文本
    [SerializeField] private Image PromptIma;        // 提示文本

    [Header("配置")]
    [SerializeField] private int enterRoleCost = 5; // 进入角色界面的花费
    [SerializeField] private int enterSkillCost = 5; // 进入技能界面的花费

    // 提示文本常量
    private const string NO_ENOUGH_GOLD_PROMPT = "金币不足！";
    private const string EMPTY_PROMPT = "";

    public override void Init()
    {
        // 自动获取玩家对象
        if (player == null)
            player = FindObjectOfType<Player>();

        // 初始化提示文本（默认隐藏）
        InitPromptText();
        UpdateGoldUI();
        // 绑定按钮事件
        RoleUpgradeBtn.onClick.AddListener(OnRoleUpgradeBtnClicked);
        SkillAcquisitionBtn.onClick.AddListener(OnSkillAcquisitionBtnClicked);
        ExitBtn.onClick.AddListener(OnExitBtnClicked);
        RoleUpgradeText.text = "花费" + enterRoleCost + "金币进入角色升级界面";
        SkillAcquisitionText.text = "花费" + enterSkillCost + "金币进入技能升级界面";
    }

    /// <summary>
    /// 初始化提示文本（默认隐藏）
    /// </summary>
    private void InitPromptText()
    {
        if (PromptText != null)
        {
            PromptText.color = Color.red;
            PromptText.text = EMPTY_PROMPT;
            PromptIma.gameObject.SetActive(false);
            
        }
    }

    /// <summary>
    /// 显示提示文本（带淡入动画）
    /// </summary>
    private void ShowPromptText(string content)
    {
        if (PromptText != null)
        {
            PromptIma.gameObject.SetActive(true);
            PromptText.text = content;
            PromptText.DOFade(1, 0.2f);
            // 2秒后自动隐藏
            DOVirtual.DelayedCall(2f, HidePromptText);
        }
    }

    /// <summary>
    /// 隐藏提示文本（带淡出动画）
    /// </summary>
    private void HidePromptText()
    {
        if (PromptText != null)
        {
            PromptText.DOFade(0, 0.2f).OnComplete(() =>
            {
                PromptText.text = EMPTY_PROMPT;
            });
        }
    }

    /// <summary>
    /// 更新金币显示
    /// </summary>
    private void UpdateGoldUI()
    {
        if (player != null && CoinNumberText != null)
        {
            currentGold = player.goldCoinNumber;
            CoinNumberText.text = currentGold.ToString();
        }
    }

    /// <summary>
    /// 检查金币是否足够
    /// </summary>
    private bool HasEnoughGold(float enterCost)
    {
        return player != null && player.goldCoinNumber >= enterCost;
    }

    /// <summary>
    /// 扣除金币
    /// </summary>
    private void DeductGold(float enterCost)
    {
        if (player != null)
        {
            player.goldCoinNumber -= enterCost;
            UpdateGoldUI();
            GoldNumber.Instance.ChangeGoldNumberUIText(player.goldCoinNumber);
        }
    }

    #region 按钮点击事件
    /// <summary>
    /// 角色升级按钮点击
    /// </summary>
    private void OnRoleUpgradeBtnClicked()
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        if (!HasEnoughGold(enterRoleCost))
        {
            ShowPromptText(NO_ENOUGH_GOLD_PROMPT);
            return;
        }

        // 扣除金币
        DeductGold(enterRoleCost);
        // 关闭商店面板
        UIManager.Instance.HidePanel<ShopPanel>();
        // 打开角色升级面板
        UIManager.Instance.ShowPanel<Role_Value_Plane>();
    }

    /// <summary>
    /// 技能获取按钮点击
    /// </summary>
    private void OnSkillAcquisitionBtnClicked()
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        if (!HasEnoughGold(enterSkillCost))
        {
            ShowPromptText(NO_ENOUGH_GOLD_PROMPT);
            return;
        }

        // 扣除金币
        DeductGold(enterSkillCost);
        // 关闭商店面板
        UIManager.Instance.HidePanel<ShopPanel>();
        // 打开技能强化面板
        UIManager.Instance.ShowPanel<Skill_Enhancement_Plane>();
    }

    /// <summary>
    /// 退出按钮点击
    /// </summary>
    private void OnExitBtnClicked()
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        UIManager.Instance.HidePanel<ShopPanel>();
    }
    #endregion

    #region 面板生命周期
    public override void ShowMe()
    {
        base.ShowMe();
        // 入场动画
        transform.DOMoveY(transform.position.y - 50, 0.3f).From();
        // 更新金币显示
        UpdateGoldUI();
        // 重置提示文本
        InitPromptText();
        DisablePlayerScript();
    }

    public override void HideMe(UnityAction callBack)
    {
        EnablePlayerScript();
        // 退场动画
        if (transform != null)
            transform.DOMoveY(transform.position.y - 50, 0.3f);
        base.HideMe(callBack);
    }
    #endregion
}