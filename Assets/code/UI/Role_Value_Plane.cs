using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Role_Value_Plane : BasePanel
{
    [Header("引用关联")]
    private Entity_stat playerStat;

    [Header("Dropdown 引用（可选，可删除）")]
    [SerializeField] private Dropdown physicalDmgDropdown;
    [SerializeField] private Dropdown healthDropdown;
    [SerializeField] private Dropdown elementalDropdown;
    [SerializeField] private Dropdown elementalDmgDropdown;

    [Header("按钮引用（按Hierarchy对应）")]
    [SerializeField] private Button physicalDmgBtn;
    [SerializeField] private Button healthBtn;
    [SerializeField] private Button elementalBtn;
    [SerializeField] private Button elementalDmgBtn;

    [SerializeField] private Text PromptText;
    [SerializeField] private Image PromptIma;

    // 提示文本配置
    private const string NO_ELEMENT_PROMPT = "请先添加元素类型！";
    private const string EMPTY_PROMPT = "";

    // 物理伤害选项枚举
    public enum PhysicalDmgOption
    {
        Add10PhysicalDmg,
        Add10CritChance,
        Add50CritPower,
        Add10AttackSpeed
    }

    // 生命选项枚举
    public enum HealthOption
    {
        Add100MaxHealth,
        RestoreFullHealth,
        Add5PercentHealthRegen
    }

    // 元素攻击选项枚举
    public enum ElementalOption
    {
        AddFireEffect,
        AddIceEffect,
        AddLightningEffect
    }

    // 元素伤害选项枚举
    public enum ElementalDmgOption
    {
        Add10FireDmg,
        Add20PercentIceDuration,
        Add10LightningDmg
    }

    // 记录当前选中的选项（默认第一个）
    public PhysicalDmgOption currentPhysicalOption = PhysicalDmgOption.Add10PhysicalDmg;
    public HealthOption currentHealthOption = HealthOption.Add100MaxHealth;
    public ElementalOption currentElementalOption = ElementalOption.AddFireEffect;
    public ElementalDmgOption currentElementalDmgOption = ElementalDmgOption.Add10FireDmg;

    public override void Init()
    {
        // 自动获取玩家和属性组件
        if (player == null)
            player = FindObjectOfType<Player>();
        if (player != null)
            playerStat = player.GetComponent<Entity_stat>();

        // 初始化提示文本（默认隐藏）
        InitPromptText();

        // 绑定按钮点击事件
        physicalDmgBtn.onClick.AddListener(OnPhysicalDmgBtnClicked);
        healthBtn.onClick.AddListener(OnHealthBtnClicked);
        elementalBtn.onClick.AddListener(OnElementalBtnClicked);
        elementalDmgBtn.onClick.AddListener(OnElementalDmgBtnClicked);

        // （可选）如果保留Dropdown，绑定Dropdown选择事件来更新当前选项
        physicalDmgDropdown.onValueChanged.AddListener(index => {
            currentPhysicalOption = (PhysicalDmgOption)index;
        });
        healthDropdown.onValueChanged.AddListener(index => {
            currentHealthOption = (HealthOption)index;
        });
        elementalDropdown.onValueChanged.AddListener(index => {
            currentElementalOption = (ElementalOption)index;
        });
        elementalDmgDropdown.onValueChanged.AddListener(index => {
            currentElementalDmgOption = (ElementalDmgOption)index;
        });
    }

    /// <summary>
    /// 初始化提示文本（默认隐藏）
    /// </summary>
    private void InitPromptText()
    {
        if (PromptText != null)
        {
            PromptText.text = EMPTY_PROMPT;
            PromptIma.gameObject.SetActive(false);
            PromptText.color = Color.red; // 提示文本设为红色
        }
    }

    /// <summary>
    /// 显示提示文本
    /// </summary>
    private void ShowPromptText()
    {
        if (PromptText != null)
        {
            PromptText.text = NO_ELEMENT_PROMPT;
            PromptIma.gameObject.SetActive(true);
            PromptText.DOFade(1, 0.2f); // 淡入动画，更流畅
        }
    }

    /// <summary>
    /// 隐藏提示文本
    /// </summary>
    private void HidePromptText()
    {
        if (PromptText != null)
        {
            PromptText.text = EMPTY_PROMPT;
            PromptText.DOFade(0, 0.2f); // 淡出动画
        }
    }

    #region 按钮点击事件处理
    private void OnPhysicalDmgBtnClicked()
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        if (playerStat == null) return;

        switch (currentPhysicalOption)
        {
            case PhysicalDmgOption.Add10PhysicalDmg:
                playerStat.GetStatByType(StatType.Damage).AddBuffs(10, "PhysicalDmg_10");
                break;
            case PhysicalDmgOption.Add10CritChance:
                playerStat.GetStatByType(StatType.CritChance).AddBuffs(10, "CritChance_10");
                break;
            case PhysicalDmgOption.Add50CritPower:
                playerStat.GetStatByType(StatType.CritPower).AddBuffs(50, "CritPower_50");
                break;
            case PhysicalDmgOption.Add10AttackSpeed:
                playerStat.GetStatByType(StatType.AttackSpeed).AddBuffs(0.1f, "AttackSpeed_10");
                break;
        }
        // 点击后隐藏面板
        UIManager.Instance.HidePanel<Role_Value_Plane>();
    }

    private void OnHealthBtnClicked()
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        if (playerStat == null || player.health == null) return;

        switch (currentHealthOption)
        {
            case HealthOption.Add100MaxHealth:
                playerStat.GetStatByType(StatType.MaxHealth).AddBuffs(100, "MaxHealth_100");
                break;
            case HealthOption.RestoreFullHealth:
                player.health.IncreaseHealth(playerStat.GetMaxHealth());
                break;
            case HealthOption.Add5PercentHealthRegen:
                playerStat.GetStatByType(StatType.HealthRegen).AddBuffs(10, "HealthRegen_5Percent");
                break;
        }
        UIManager.Instance.HidePanel<Role_Value_Plane>();
    }

    private void OnElementalBtnClicked()
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        if (playerStat == null) return;

        // 设置选中的元素类型
        switch (currentElementalOption)
        {
            case ElementalOption.AddFireEffect:
                playerStat.offenseGroup.elementType = ElementType.Fire;
                break;
            case ElementalOption.AddIceEffect:
                playerStat.offenseGroup.elementType = ElementType.Ice;
                break;
            case ElementalOption.AddLightningEffect:
                playerStat.offenseGroup.elementType = ElementType.Lightning;
                break;
        }

        // 选择元素后隐藏提示文本（如果显示的话）
        HidePromptText();

        UIManager.Instance.HidePanel<Role_Value_Plane>();
        
    }

    private void OnElementalDmgBtnClicked()
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        if (playerStat == null) return;

        // 核心校验：元素类型为None时显示提示，不执行修改
        if (playerStat.offenseGroup.elementType == ElementType.None)
        {
            ShowPromptText();
            return; // 直接返回，不执行属性修改
        }

        // 元素类型已选择，隐藏提示文本并执行修改
        HidePromptText();

        switch (currentElementalDmgOption)
        {
            case ElementalDmgOption.Add10FireDmg:
                playerStat.GetStatByType(StatType.FireDamage).AddBuffs(10, "FireDmg_10");
                break;
            case ElementalDmgOption.Add20PercentIceDuration:
                playerStat.GetStatByType(StatType.IceDuration).AddBuffs(0.2f, "IceDuration_20Percent");
                break;
            case ElementalDmgOption.Add10LightningDmg:
                playerStat.GetStatByType(StatType.LightningDamage).AddBuffs(10, "LightningDmg_10");
                break;
        }
        UIManager.Instance.HidePanel<Role_Value_Plane>();
    }
    #endregion

    public override void HideMe(UnityAction callBack)
    {
        EnablePlayerScript();
        if (transform != null)
            transform.DOMove(new Vector2(this.transform.position.x, this.transform.position.y - 50), 0.3f);
        base.HideMe(callBack);
        if (player.isShop)
            UIManager.Instance.ShowPanel<ShopPanel>();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        transform.DOMove(new Vector2(this.transform.position.x, this.transform.position.y - 50), 0.3f).From();
        DisablePlayerScript();
        // 每次打开面板重置提示文本为隐藏状态
        InitPromptText();
    }

}