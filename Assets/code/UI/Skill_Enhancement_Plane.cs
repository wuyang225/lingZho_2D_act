using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class Skill_Enhancement_Plane : BasePanel
{
    [Header("技能配置引用")]
    [SerializeField] private Player_SkillManager skillManager;

    // 技能UI组引用（按Hierarchy结构对应）
    [Header("技能UI组")]
    [SerializeField] private RectTransform dashBk;
    [SerializeField] private RectTransform swordThrowBk;
    [SerializeField] private RectTransform swordThrowSpinBk;
    [SerializeField] private RectTransform swordThrowQiBk;
    [SerializeField] private Image swordThrowQiima;
    [SerializeField] private RectTransform skillShardBk;

    // 按钮文本引用（显示升级类型）
    [Header("升级选项文本")]
    [SerializeField] private Text dashText;
    [SerializeField] private Text swordThrowText;
    [SerializeField] private Text swordThrowSpinText;
    [SerializeField] private Text swordThrowQiText;
    [SerializeField] private Text skillShardText;

    // 动画时长
    private float _animDuration = 0.3f;
    private Tween _showTween;
   
    public override void Init()
    {
        // 自动查找玩家身上的技能管理器
        if (skillManager == null)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
                skillManager = player.GetComponent<Player_SkillManager>();
            else
                Debug.LogError("Skill_Enhancement_Plane: 未找到Player对象！");
        }
        if (skillManager.swordQi.isUpGrade)
            swordThrowQiima.color = Color.blue;
        // 初始化UI状态（根据技能三阶段状态显示对应文本）
        InitUIState();

        // 绑定按钮事件
        BindButtonEvents();
    }

    /// <summary>
    /// 初始化UI状态，根据技能三阶段状态显示对应文本
    /// 阶段1：未激活 → 显示"激活技能"
    /// 阶段2：已激活未升级 → 显示"解锁进阶模式：XXX"
    /// 阶段3：已升级/强化 → 显示"强化技能：XXX"（永久）
    /// </summary>
    private void InitUIState()
    {
        // Dash 技能：解锁进阶=冲刺生成碎片 | 强化=增加10%碎片伤害
        UpdateSkillText(dashText, skillManager.dash, "冲刺生成碎片", "增加50%碎片伤害");
        // SwordThrow (普通)：解锁进阶=火焰元素伤害 | 强化=增加10%火焰伤害
        UpdateSkillText(swordThrowText, skillManager.swordThrow, "火焰元素伤害", "增加30%火焰伤害");
        // SwordThrowSpin (旋转模式)：解锁进阶=旋转剑+雷电元素 | 强化=增加10%雷电伤害
        UpdateSkillText(swordThrowSpinText, skillManager.swordThrow, "旋转剑+雷电元素", "增加50%雷电伤害", true);
        // SwordQi (剑气)：解锁进阶=冰霜元素+减速 | 强化=增加10%冰霜伤害
        UpdateSkillText(swordThrowQiText, skillManager.swordQi, "冰霜元素+减速", "增加50%冰霜持续时间");
        // Shard (碎片)：解锁进阶=瞬移+回血 | 强化=增加10%爆炸伤害
        UpdateSkillText(skillShardText, skillManager.shard, "瞬移+回血", "增加50%爆炸伤害");
    }

    /// <summary>
    /// 根据技能三阶段状态更新文本显示
    /// </summary>
    /// <param name="text">目标文本框</param>
    /// <param name="skill">目标技能</param>
    /// <param name="enhanceTip">解锁进阶模式的提示文字</param>
    /// <param name="enhance">强化技能的提示文字</param>
    /// <param name="isSwordThrowSpin">是否为旋转剑</param>
    private void UpdateSkillText(Text text, Skill_Base skill, string enhanceTip, string enhance, bool isSwordThrowSpin = false)
    {
        if (skill == null || text == null) return;

        if (isSwordThrowSpin)
        {
            Skill_SwordThrow skillSpin = skill as Skill_SwordThrow;
            // 阶段1：未激活 → 激活技能
            if (!skillSpin.isSpinActive)
            {
                text.text = "激活技能";
            }
            // 阶段3：已升级/强化 → 永久显示强化效果（可重复点击）
            else if (skillSpin.spinUpGrade)
            {
                text.text = $"强化技能：{enhance}";
                text.color = Color.green;
            }
            // 阶段2：已激活未升级 → 解锁进阶模式
            else
            {
                text.text = $"解锁进阶模式：{enhanceTip}";
            }
        }
        else
        {
            // 阶段1：未激活 → 激活技能
            if (!skill.isActive)
            {
                text.text = "激活技能";
            }
            // 阶段3：已升级/强化 → 永久显示强化效果（可重复点击）
            else if (skill.isUpGrade)
            {
                text.text = $"强化技能：{enhance}";
                text.color = Color.green;
            }
            // 阶段2：已激活未升级 → 解锁进阶模式
            else
            {
                text.text = $"解锁进阶模式：{enhanceTip}";
            }
        }
    }

    /// <summary>
    /// 绑定所有技能按钮的点击事件（三阶段均保持可点击）
    /// </summary>
    private void BindButtonEvents()
    {
        // Dash：激活→解锁进阶=冲刺生成碎片→强化=增加10%碎片伤害
        BindSingleButton(dashBk, dashText, skillManager.dash, UpgradeType.Auto, "冲刺生成碎片", "增加50%碎片伤害");
        // SwordThrow（普通）：激活→解锁进阶=火焰元素伤害→强化=增加10%火焰伤害
        BindSingleButton(swordThrowBk, swordThrowText, skillManager.swordThrow, UpgradeType.Auto, "火焰元素伤害", "增加30%火焰伤害");
        // SwordThrowSpin（旋转）：激活→解锁进阶=旋转剑+雷电元素→强化=增加10%雷电伤害
        BindSingleButton(swordThrowSpinBk, swordThrowSpinText, skillManager.swordThrow, UpgradeType.UnlockMode, "旋转剑+雷电元素", "增加50%雷电伤害", true);
        // SwordQi（剑气）：激活→解锁进阶=冰霜元素+减速→强化=增加10%冰霜伤害
        BindSingleButton(swordThrowQiBk, swordThrowQiText, skillManager.swordQi, UpgradeType.Auto, "冰霜元素+减速", "增加50%冰霜持续时间");
        // Shard（碎片）：激活→解锁进阶=瞬移+回血→强化=增加10%爆炸伤害
        BindSingleButton(skillShardBk, skillShardText, skillManager.shard, UpgradeType.Auto, "瞬移+回血", "增加50%爆炸伤害");
    }

    /// <summary>
    /// 绑定单个按钮逻辑（三阶段均保持可点击，三阶段后执行强化伤害）
    /// </summary>
    /// <param name="panelRoot">按钮父节点</param>
    /// <param name="textBtn">文本框</param>
    /// <param name="targetSkill">目标技能</param>
    /// <param name="defaultType">默认升级类型</param>
    /// <param name="enhanceTip">解锁进阶模式的提示文字</param>
    /// <param name="enhance">强化技能的提示文字</param>
    /// <param name="isSwordThrowSpin">是否为旋转剑</param>
    private void BindSingleButton(RectTransform panelRoot, Text textBtn, Skill_Base targetSkill, UpgradeType defaultType, string enhanceTip, string enhance, bool isSwordThrowSpin = false)
    {
        Button btn = panelRoot.GetComponentInChildren<Button>();
        if (btn == null || targetSkill == null) return;

        // 移除原有禁用逻辑，始终保持按钮可点击
        btn.interactable = true;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
            if (isSwordThrowSpin)
            {
                Skill_SwordThrow skillSpin = targetSkill as Skill_SwordThrow;
                // 自动判断当前升级阶段
                UpgradeType actualType = defaultType;
                if (!skillSpin.isSpinActive)
                {
                    actualType = UpgradeType.Activate; // 阶段1：激活
                }
                else if (!skillSpin.spinUpGrade)
                {
                    actualType = UpgradeType.UnlockMode; // 阶段2：解锁进阶模式
                }
                else
                {
                    actualType = UpgradeType.AddDamage; // 阶段3：强化伤害（永久执行）
                }

                // 执行升级逻辑
                ExecuteUpgrade(skillSpin, actualType, textBtn, enhanceTip, enhance, true);
            }
            else
            {
                // 自动判断当前升级阶段
                UpgradeType actualType = defaultType;
                if (!targetSkill.isActive)
                {
                    actualType = UpgradeType.Activate; // 阶段1：激活
                }
                else if (!targetSkill.isUpGrade)
                {
                    actualType = UpgradeType.UnlockMode; // 阶段2：解锁进阶模式
                }
                else
                {
                    actualType = UpgradeType.AddDamage; // 阶段3：强化伤害（永久执行）
                }

                // 执行升级逻辑
                ExecuteUpgrade(targetSkill, actualType, textBtn, enhanceTip, enhance);
            }
            // 关闭面板
            UIManager.Instance.HidePanel<Skill_Enhancement_Plane>();
            if (player.isShop)
                UIManager.Instance.ShowPanel<ShopPanel>();
        });
    }

    /// <summary>
    /// 执行具体的升级逻辑 + 更新文本提示（三阶段后重复执行强化伤害）
    /// </summary>
    private void ExecuteUpgrade(Skill_Base skill, UpgradeType type, Text textBtn, string enhanceTip, string enhance, bool isSwordThrowSpin = false)
    {
        if (skill == null || textBtn == null) return;

        if (isSwordThrowSpin)
        {
            Skill_SwordThrow skillSpin = skill as Skill_SwordThrow;
            switch (type)
            {
                case UpgradeType.Activate:
                    // 阶段1：激活旋转剑模式
                    skillSpin.isSpinActive = true;
                    textBtn.text = $"解锁进阶模式：{enhanceTip}"; // 更新为阶段2文本
                    Debug.Log($"激活旋转剑技能");
                    break;

                case UpgradeType.UnlockMode:
                    // 阶段2：解锁旋转剑进阶模式
                    skillSpin.spinUpGrade = true;
                    HandleElementalDamage(skillSpin, true); // 首次强化
                    textBtn.text = $"强化技能：{enhance}"; // 更新为阶段3文本
                    textBtn.color = Color.green;
                    Debug.Log($"解锁旋转剑进阶模式 → {enhanceTip}");
                    break;

                case UpgradeType.AddDamage:
                    // 阶段3：重复强化伤害（每次点击都提升伤害）
                    HandleElementalDamage(skillSpin, true);
                    textBtn.text = $"强化技能：{enhance}"; // 保持阶段3文本
                    textBtn.color = Color.green;
                    Debug.Log($"旋转剑再次强化 → {enhance}");
                    break;

                case UpgradeType.Auto:
                    if (!skillSpin.isSpinActive)
                    {
                        ExecuteUpgrade(skill, UpgradeType.Activate, textBtn, enhanceTip, enhance, true);
                    }
                    else if (!skillSpin.spinUpGrade)
                    {
                        ExecuteUpgrade(skill, UpgradeType.UnlockMode, textBtn, enhanceTip, enhance, true);
                    }
                    else
                    {
                        ExecuteUpgrade(skill, UpgradeType.AddDamage, textBtn, enhanceTip, enhance, true);
                    }
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case UpgradeType.Activate:
                    // 阶段1：激活技能
                    skill.SetActive(true);
                    textBtn.text = $"解锁进阶模式：{enhanceTip}"; // 更新为阶段2文本
                    Debug.Log($"激活技能: {skill.GetType().Name}");
                    break;

                case UpgradeType.UnlockMode:
                    // 阶段2：解锁进阶模式 + 首次强化
                    skill.isUpGrade = true;
                    if (skill is Skill_SwordQi swordQi)
                    {
                        swordQi.ChangeSwordQiColor();
                    }
                    HandleElementalDamage(skill);
                    textBtn.text = $"强化技能：{enhance}"; // 更新为阶段3文本
                    textBtn.color = Color.green;
                    Debug.Log($"解锁进阶模式: {skill.GetType().Name} → {enhanceTip}");
                    break;

                case UpgradeType.AddDamage:
                    // 阶段3：重复强化伤害（每次点击都提升伤害）
                    HandleElementalDamage(skill);
                    textBtn.text = $"强化技能：{enhance}"; // 保持阶段3文本
                    textBtn.color = Color.green;
                    Debug.Log($"技能再次强化: {skill.GetType().Name} → {enhance}");
                    break;

                case UpgradeType.Auto:
                    if (!skill.isActive)
                    {
                        ExecuteUpgrade(skill, UpgradeType.Activate, textBtn, enhanceTip, enhance);
                    }
                    else if (!skill.isUpGrade)
                    {
                        ExecuteUpgrade(skill, UpgradeType.UnlockMode, textBtn, enhanceTip, enhance);
                    }
                    else
                    {
                        ExecuteUpgrade(skill, UpgradeType.AddDamage, textBtn, enhanceTip, enhance);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 处理元素伤害或强化逻辑（三阶段后重复强化）
    /// </summary>
    private void HandleElementalDamage(Skill_Base skill, bool isSwordThrowSpin = false)
    {
        // 冲刺（Dash）：每次强化提升10%基础伤害
        if (skill is Skill_Dash dash)
        {
            dash.damageData.damage *= 1.1f; // 每次点击提升10%
            dash.isUpGrade = true;
            Debug.Log($"冲刺当前伤害：{dash.damageData.damage}");
        }
        // 投掷剑（普通模式）：每次强化提升10%火焰伤害
        else if (skill is Skill_SwordThrow swordThrow)
        {
            if (!isSwordThrowSpin)
            {
                swordThrow.damageData.elementType = ElementType.Fire;
                swordThrow.damageData.elementDamageScale *= 1.30f; // 每次点击提升10%
                swordThrow.isUpGrade = true;
                Debug.Log($"普通剑火焰伤害倍率：{swordThrow.damageData.elementDamageScale}");
            }
            // 旋转剑模式：每次强化提升10%雷电伤害
            else
            {
                swordThrow.spinDamageData.elementType = ElementType.Lightning;
                swordThrow.spinDamageData.elementDamageScale *= 1.5f; // 每次点击提升10%
                swordThrow.spinUpGrade = true;
                Debug.Log($"旋转剑雷电伤害倍率：{swordThrow.spinDamageData.elementDamageScale}");
            }
        }
        // 剑气（SwordQi）：每次强化提升50%冰霜持续时间
        else if (skill is Skill_SwordQi swordQi)
        {
            swordQi.damageData.elementType = ElementType.Ice;
            swordQi.damageData.elementDurationScale *= 1.5f; // 每次点击提升50%
            swordQi.isUpGrade = true;
            Debug.Log($"剑气冰霜持续倍率：{swordQi.damageData.elementDurationScale}");
        }
        // 碎片（Shard）：每次强化提升10%爆炸伤害
        else if (skill is Skill_Shard shard)
        {
            shard.damageData.damage *= 1.5f; // 每次点击提升10%
            shard.isUpGrade = true;
            Debug.Log($"碎片爆炸伤害：{shard.damageData.damage}");
        }
    }

    /// <summary>
    /// 重写显示逻辑，添加入场动画
    /// </summary>
    public override void ShowMe()
    {
        base.ShowMe();
        transform.DOMove(new Vector2(transform.position.x, transform.position.y - 50), 0.3f).From();
        DisablePlayerScript();
    }

    /// <summary>
    /// 重写隐藏逻辑
    /// </summary>
    public override void HideMe(UnityAction callBack)
    {
        EnablePlayerScript();
        if (transform != null)
            transform.DOMove(new Vector2(transform.position.x, transform.position.y - 50), 0.3f);
        base.HideMe(callBack);
    }
}

/// <summary>
/// 升级模式枚举
/// </summary>
public enum UpgradeType
{
    Activate,     // 阶段1：激活
    UnlockMode,   // 阶段2：解锁进阶模式
    AddDamage,    // 阶段3：强化伤害（可重复执行）
    Auto          // 自动识别阶段
}

