using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class AttributePanel : BasePanel
{
    [Header("物理伤害相关文本")]
    [SerializeField] private Text damageText;          // 攻击力数值
    [SerializeField] private Text criticalChanceText; // 暴击率数值
    [SerializeField] private Text criticalPowerText;   // 暴击倍率数值
    [SerializeField] private Text attackSpeedText;     // 攻击速度数值

    [Header("身体属性相关文本")]
    [SerializeField] private Text maxHealthText;      // 最大生命值数值
    [SerializeField] private Text currentHealthText;   // 当前生命值数值
    [SerializeField] private Text healthRegenText;     // 生命回复数值

    [Header("元素伤害相关文本")]
    [SerializeField] private Text iceDamageText;      // 冰霜伤害数值
    [SerializeField] private Text fireDamageText;      // 火焰伤害数值
    [SerializeField] private Text lightningDamageText; // 雷电伤害数值
    [SerializeField] private Text currentElementTypeText; // 当前元素类型文本

    [Header("额外信息")]
    [SerializeField] private Text coinCountText;       // 当前金币数量文本

    [Header("技能提示UI")]
    [SerializeField] private Text SkillPromptText;       // 技能提示文本
    [SerializeField] private Image SkillPromptImg;       // 技能提示背景

    [SerializeField] private Button exitbtn;       // 退出按钮
    [SerializeField] private Button[] hasSkillbtn;       // 已拥有技能按钮数组
    [SerializeField] private Button[] notSkillbtn;       // 未拥有技能按钮数组

    // 缓存玩家属性组件
    private Entity_stat playerStat;
    private Entity_Health playerHealth;
    // 缓存技能管理器（核心：用于获取技能isActive状态）
    private Player_SkillManager skillManager;


    public override void Init()
    {
        // 初始隐藏技能提示面板
        SkillPromptImg.gameObject.SetActive(false);

        // 查找玩家组件
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerStat = player.GetComponent<Entity_stat>();
            playerHealth = player.GetComponent<Entity_Health>();
            // 获取技能管理器（关键：用于读取技能状态）
            skillManager = player.GetComponent<Player_SkillManager>();
        }
        else
        {
            Debug.LogError("AttributePanel: 未找到Player对象！");
        }

        // ========== 核心修改1：给退出按钮添加音效 ==========
        if (exitbtn != null)
        {
            exitbtn.onClick.RemoveAllListeners();
            exitbtn.onClick.AddListener(() =>
            {
                // 先播放点击音效
                OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
                // 再执行原有逻辑
                UIManager.Instance.HidePanel<AttributePanel>();
            });
        }

        // 绑定已拥有技能按钮的提示事件（已包含音效）
        BindSkillButtonEvents(hasSkillbtn);

        // 绑定未拥有技能按钮的提示事件（已包含音效）
        BindSkillButtonEvents(notSkillbtn, isUnlocked: false);

        // 初始更新属性面板（包含技能按钮显隐）
        UpdateAttributePanel();
    }

    /// <summary>
    /// 面板显示时调用（暂停游戏）
    /// </summary>
    public override void ShowMe()
    {
        base.ShowMe();
        transform.DOMove(new Vector2(this.transform.position.x, this.transform.position.y - 50), 0.3f).From();
        DisablePlayerScript();
        UpdateAttributePanel();
    }

    /// <summary>
    /// 面板隐藏时调用（恢复游戏）
    /// </summary>
    public override void HideMe(UnityAction callBack)
    {
        EnablePlayerScript();
        if (transform != null)
            transform.DOMove(new Vector2(this.transform.position.x, this.transform.position.y - 50), 0.3f);
        base.HideMe(callBack);
    }

    protected override void Update()
    {
        base.Update();
        // 点击空白处关闭技能提示（优化：排除UI点击）
        if (Input.GetMouseButtonDown(0))
        {
            SkillPromptImg.gameObject.SetActive(false);
        }
    }

    // ========== 核心修改2：封装按钮点击音效方法 ==========
    /// <summary>
    /// 播放按钮点击音效
    /// </summary>
    private void PlayButtonClickSound()
    {
        // 空值防护：防止soundMusic单例为空报错
        if (soundMusic.Instance == null)
        {
            Debug.LogWarning("AttributePanel: soundMusic单例为空，无法播放点击音效");
            return;
        }

        soundMusic.Instance.ChangeAudioClip("Onbutton", false);
    }

    /// <summary>
    /// 通用绑定技能按钮事件的方法（已添加音效）
    /// </summary>
    /// <param name="buttons">技能按钮数组</param>
    /// <param name="isUnlocked">是否已解锁（影响提示文本）</param>
    private void BindSkillButtonEvents(Button[] buttons, bool isUnlocked = true)
    {
        if (buttons == null || buttons.Length == 0)
        {
            Debug.LogWarning("AttributePanel: 技能按钮数组为空！");
            return;
        }

        foreach (var item in buttons)
        {
            if (item == null) continue;

            // 先清空旧监听，避免重复绑定
            item.onClick.RemoveAllListeners();

            // 根据按钮名称绑定对应技能提示
            item.onClick.AddListener(() =>
            {
                // ========== 核心修改3：所有技能按钮点击时先播放音效 ==========
                PlayButtonClickSound();

                SkillPromptImg.gameObject.SetActive(true);
                string skillName = item.gameObject.name;

                if (!isUnlocked)
                {
                    switch (skillName)
                    {
                        case "Dash":
                            SkillPromptText.text = "【冲刺】\n状态：未解锁\n\n请先获取该技能后查看详情";
                            break;
                        case "SwordThwor":
                            SkillPromptText.text = "【普通剑投掷】\n状态：未解锁\n\n请先获取该技能后查看详情";
                            break;
                        case "SwordThrowSpin":
                            SkillPromptText.text = "【旋转剑投掷】\n状态：未解锁\n\n请先获取该技能后查看详情";
                            break;
                        case "Shard":
                            SkillPromptText.text = "【碎片】\n状态：未解锁\n\n请先获取该技能后查看详情";
                            break;
                        case "SwordQi":
                            SkillPromptText.text = "【剑气流】\n状态：未解锁\n\n请先获取该技能后查看详情";
                            break;
                        default:
                            SkillPromptText.text = $"未知技能：{skillName}";
                            break;
                    }
                    return;
                }

                // 已解锁技能的提示文本
                switch (skillName)
                {
                    case "Dash":
                        SkillPromptText.text = "【冲刺】\n技能效果：向当前方向快速冲刺，可用于闪避或快速位移；\n\n升级效果：冲刺时自动生成一枚碎片，可对敌人造成伤害";
                        break;
                    case "SwordThwor":
                        SkillPromptText.text = "【普通剑投掷】\n技能效果：向指定方向投掷出剑，剑会钉在碰撞物体上，可再次按键召回；命中敌人造成物理伤害；\n\n升级效果：剑附带火焰元素伤害，提升爆发输出";
                        break;
                    case "SwordThrowSpin":
                        SkillPromptText.text = "【旋转剑投掷】\n技能效果：投掷出高速旋转的剑，持续在最大距离内环绕攻击敌人，一段时间后自动召回；命中敌人造成持续伤害；\n\n升级效果：旋转剑附带雷电元素伤害，攻击频率/范围提升";
                        break;
                    case "Shard":
                        SkillPromptText.text = "【碎片】\n技能效果：首次使用生成一枚停留碎片，一段时间后爆炸；\n\n升级效果：解锁位移+回血功能，实现灵活位移与续航";
                        break;
                    case "SwordQi":
                        SkillPromptText.text = "【剑气流】\n技能效果：向前方发射一道剑气，直线飞行一段距离后消失；命中敌人造成穿透伤害；\n\n升级效果：剑气附带冰霜元素伤害，可减速敌人";
                        break;
                    default:
                        SkillPromptText.text = $"未知技能：{skillName}";
                        break;
                }
            });
        }
    }

    /// <summary>
    /// 更新整个属性面板的数据（包含技能按钮显隐逻辑）
    /// </summary>
    public void UpdateAttributePanel()
    {
        if (playerStat == null || skillManager == null) return;

        // 更新物理伤害
        damageText.text = playerStat.offenseGroup.damage.GetValue().ToString();
        criticalChanceText.text = $"{playerStat.offenseGroup.critChance.GetValue():F1}%";
        criticalPowerText.text = $"{playerStat.offenseGroup.critPower.GetValue():F1}%";
        attackSpeedText.text = $"{playerStat.offenseGroup.attackSpeed.GetValue():F1}倍";

        // 更新身体属性
        maxHealthText.text = playerStat.resourcesGroup.Max_Health.GetValue().ToString() + "Hp";
        currentHealthText.text = playerHealth.currentHp.ToString() + "Hp";
        healthRegenText.text = playerStat.resourcesGroup.Health_Regen.GetValue().ToString() + "%";

        // 更新元素伤害
        iceDamageText.text = (playerStat.offenseGroup.iceDuration.GetValue()).ToString() + "s";
        fireDamageText.text = (playerStat.offenseGroup.fireDamage.GetValue()).ToString();
        lightningDamageText.text = (playerStat.offenseGroup.lightingDamage.GetValue()).ToString();
        // 获取当前元素类型
        playerStat.GetElementalDamage(out ElementType elementType);
        currentElementTypeText.text = elementType.ToString();

        // 更新金币数量
        Player playerComp = playerStat.GetComponent<Player>();
        if (playerComp != null)
        {
            coinCountText.text = playerComp.goldCoinNumber.ToString();
        }

        //根据技能isActive更新按钮显隐 ==========
        UpdateSkillButtonVisibility();
    }

    /// <summary>
    /// 根据技能isActive状态，控制hasSkillbtn和notSkillbtn的显隐
    /// </summary>
    private void UpdateSkillButtonVisibility()
    {
        // 遍历所有已拥有技能按钮，匹配技能名称并更新显隐
        foreach (var hasBtn in hasSkillbtn)
        {
            if (hasBtn == null) continue;
            bool isSkillActive = GetSkillActiveState(hasBtn.gameObject.name);
            // 技能激活则显示已拥有按钮，否则隐藏
            hasBtn.gameObject.SetActive(isSkillActive);
        }

        // 遍历所有未拥有技能按钮，匹配技能名称并更新显隐
        foreach (var notBtn in notSkillbtn)
        {
            if (notBtn == null) continue;
            bool isSkillActive = GetSkillActiveState(notBtn.gameObject.name);
            // 技能未激活则显示未拥有按钮，否则隐藏
            notBtn.gameObject.SetActive(!isSkillActive);
        }
    }

    /// <summary>
    /// 根据技能名称获取对应技能的isActive状态
    /// </summary>
    /// <param name="skillName">技能按钮名称（需与SkillManager中的技能字段名匹配）</param>
    private bool GetSkillActiveState(string skillName)
    {
        // 容错：技能管理器为空时返回false
        if (skillManager == null) return false;

        // 根据技能名称匹配对应技能的isActive状态
        switch (skillName)
        {
            case "Dash":
                return skillManager.dash != null && skillManager.dash.isActive;
            case "SwordThwor":
                return skillManager.swordThrow != null && skillManager.swordThrow.isActive;
            case "SwordThrowSpin":
                return skillManager.swordThrow != null && skillManager.swordThrow.isSpinActive;
            case "Shard":
                return skillManager.shard != null && skillManager.shard.isActive;
            case "SwordQi":
                return skillManager.swordQi != null && skillManager.swordQi.isActive;
            default:
                Debug.LogWarning($"AttributePanel: 未找到技能{skillName}的状态");
                return false;
        }
    }

    // 可选：面板被销毁时强制恢复游戏（兜底逻辑，防止异常）
    private void OnDestroy()
    {
        EnablePlayerScript();
    }
}