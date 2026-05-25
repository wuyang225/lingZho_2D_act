using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManagePanel : BasePanel
{
    [Header("技能面板根节点")]
    [SerializeField] private Transform bkImage; // 所有技能图标的父容器

    [Header("冲刺技能UI")]
    [SerializeField] private Image dashFillImage;
    [SerializeField] private Image dashUpgradeImage;
    [SerializeField] private Text dashText; // 复用为锁定文本+按键文本
    [SerializeField] private Text dashCoolingText;

    [Header("普通剑投掷UI")]
    [SerializeField] private Image swordThrowFillImage;
    [SerializeField] private Image swordThrowUpgradeImage;
    [SerializeField] private Text swordThrowText; // 复用为锁定文本+按键文本
    [SerializeField] private Text swordThrowCoolingText;

    [Header("旋转剑投掷UI")]
    [SerializeField] private Image swordThrowSpinFillImage;
    [SerializeField] private Image swordThrowSpinUpgradeImage;
    [SerializeField] private Text swordThrowSpinText; // 复用为锁定文本+按键文本
    [SerializeField] private Text swordThrowSpinCoolingText;

    [Header("剑气流UI")]
    [SerializeField] private Image swordQiFillImage;
    [SerializeField] private Image SwordQiUpgradeImage;
    [SerializeField] private Image swordQiImage; // 剑气流图标（原有）
    [SerializeField] private Text swordQiText; // 复用为锁定文本+按键文本
    [SerializeField] private Text swordQiCoolingText;

    [Header("碎片技能UI")]
    [SerializeField] private Image shardFillImage;
    [SerializeField] private Image shardUpgradeImage;
    [SerializeField] private Text shardText; // 复用为锁定文本+按键文本
    [SerializeField] private Text shardCoolingText;

    // 锁定文本内容常量
    private const string LockedTextContent = "LOCKED";

    // 缓存剑气流原始颜色（仅保留原有图标）
    private Color originalSwordQiIconColor;

    // 缓存技能管理器
    private Player_SkillManager skillManager;

    public override void Init()
    {
        // 1. 查找玩家和技能管理器
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        if (player != null)
        {
            skillManager = player.skillManager;
        }
        else
        {
            Debug.LogError("SkillManagePanel: 未找到Player对象！");
            return;
        }

        // 2. 缓存剑气流图标的原始颜色（仅原有图标）
        if (swordQiImage != null)
            originalSwordQiIconColor = swordQiImage.color;

        // 3. 初始更新一次面板状态
        UpdateSkillPanel();
    }

    /// <summary>
    /// 显示面板时刷新数据
    /// </summary>
    public override void ShowMe()
    {
        base.ShowMe();
        UpdateSkillPanel();
    }

    /// <summary>
    /// 每帧更新技能冷却UI（也可以改为事件驱动，优化性能）
    /// </summary>
    protected override void Update()
    {
        // 动态找回玩家（场景切换后）
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
            if (player != null)
                skillManager = player.skillManager;
        }

        base.Update();
        if (skillManager == null) return;

        UpdateSkillPanel();

        // 仅在剑气流激活时更新颜色，未激活时重置
        if (skillManager.swordQi.isActive)
        {
            swordQiImage.color = skillManager.swordQi.swordColor;
        }
        else
        {
            swordQiImage.color = originalSwordQiIconColor; // 重置为原始颜色
        }
    }

    /// <summary>
    /// 更新整个技能面板的冷却、锁定、文本状态
    /// </summary>
    public void UpdateSkillPanel()
    {
        if (skillManager == null) return;

        // 1. 更新冲刺技能UI（激活/未激活逻辑）
        if (skillManager.dash.isActive)
        {
            UpdateSkillUI(
                skillManager.dash,
                dashFillImage,
                dashUpgradeImage,
                dashText,
                dashCoolingText,
                "L_Shift"
            );
            HideLockedState(dashText);
        }
        else
        {
            ResetSkillUI(dashFillImage, dashUpgradeImage, dashCoolingText);
            ShowLockedState(dashText);
        }

        // 2. 更新普通剑投掷UI
        if (skillManager.swordThrow.isActive)
        {
            UpdateSkillUI(
                skillManager.swordThrow,
                swordThrowFillImage,
                swordThrowUpgradeImage,
                swordThrowText,
                swordThrowCoolingText,
                "G"
            );
            HideLockedState(swordThrowText);
        }
        else
        {
            ResetSkillUI(swordThrowFillImage, swordThrowUpgradeImage, swordThrowCoolingText);
            ShowLockedState(swordThrowText);
        }

        // 3. 更新旋转剑投掷UI
        if (skillManager.swordThrow.isSpinActive)
        {
            if (skillManager.swordThrow == null)
            {
                Debug.LogWarning("SkillManagePanel: 技能实例为空，跳过更新");
                return;
            }

            // 计算冷却进度（0~1）
            float cooldownProgress = Mathf.Clamp01(skillManager.swordThrow.lastTimeUsed / skillManager.swordThrow.cooldown);
            swordThrowSpinFillImage.fillAmount = 1 - cooldownProgress;

            swordThrowSpinUpgradeImage.enabled = skillManager.swordThrow.spinUpGrade;

            // 技能按键文本（激活时显示按键）
            swordThrowSpinText.text = "V";
            HideLockedState(swordThrowSpinText);

            // 更新冷却文本（冷却中显示剩余时间，可用时显示空）
            if (skillManager.swordThrow.CanUseSpinSkill())
            {
                swordThrowSpinCoolingText.text = "";
            }
            else
            {
                float remainingTime = skillManager.swordThrow.cooldown - skillManager.swordThrow.lastTimeUsed;
                swordThrowSpinCoolingText.text = $"{remainingTime:F1}";
            }
        }
        else
        {
            ResetSkillUI(swordThrowSpinFillImage, swordThrowSpinUpgradeImage, swordThrowSpinCoolingText);
            ShowLockedState(swordThrowSpinText);
        }

        // 4. 更新剑气流UI
        if (skillManager.swordQi.isActive)
        {
            UpdateSkillUI(
                skillManager.swordQi,
                swordQiFillImage,
                SwordQiUpgradeImage,
                swordQiText,
                swordQiCoolingText,
                "F"
            );
            HideLockedState(swordQiText);
        }
        else
        {
            ResetSkillUI(swordQiFillImage, SwordQiUpgradeImage, swordQiCoolingText);
            ShowLockedState(swordQiText);
        }

        // 5. 更新碎片技能UI
        if (skillManager.shard.isActive)
        {
            UpdateSkillUI(
                skillManager.shard,
                shardFillImage,
                shardUpgradeImage,
                shardText,
                shardCoolingText,
                "Q"
            );
            HideLockedState(shardText);
        }
        else
        {
            ResetSkillUI(shardFillImage, shardUpgradeImage, shardCoolingText);
            ShowLockedState(shardText);
        }
    }

    /// <summary>
    /// 通用技能UI更新方法（激活状态）
    /// </summary>
    /// <param name="skill">技能实例</param>
    /// <param name="fillImage">冷却填充遮罩</param>
    /// <param name="upgradeImage">升级标识</param>
    /// <param name="skillText">按键/锁定文本</param>
    /// <param name="coolingText">冷却文本</param>
    /// <param name="key">按键名称</param>
    private void UpdateSkillUI(Skill_Base skill, Image fillImage, Image upgradeImage, Text skillText, Text coolingText, string key)
    {
        if (skill == null)
        {
            Debug.LogWarning("SkillManagePanel: 技能实例为空，跳过更新");
            return;
        }

        // 空引用校验
        if (fillImage == null || upgradeImage == null || skillText == null || coolingText == null)
        {
            Debug.LogWarning("SkillManagePanel: 技能UI元素未赋值，跳过更新");
            return;
        }

        // 计算冷却进度（0~1）
        float cooldownProgress = Mathf.Clamp01(skill.lastTimeUsed / skill.cooldown);
        fillImage.fillAmount = 1 - cooldownProgress;

        upgradeImage.enabled = skill.isUpGrade;

        // 技能按键文本（激活时显示按键）
        skillText.text = key;

        // 更新冷却文本（冷却中显示剩余时间，可用时显示空）
        if (skill.CanUseSkill())
        {
            coolingText.text = "";
        }
        else
        {
            // 修复：使用当前技能的冷却时间，而非固定swordThrow
            float remainingTime = skill.cooldown - skill.lastTimeUsed;
            coolingText.text = $"{remainingTime:F1}";
        }
    }

    /// <summary>
    /// 重置未激活技能的UI为原始状态
    /// </summary>
    /// <param name="fillImage">冷却填充图</param>
    /// <param name="upgradeImage">升级标识</param>
    /// <param name="coolingText">冷却文本</param>
    private void ResetSkillUI(Image fillImage, Image upgradeImage, Text coolingText)
    {
        // 空引用校验：避免空指针
        if (fillImage != null) fillImage.fillAmount = 1;
        if (upgradeImage != null) upgradeImage.enabled = false;
        if (coolingText != null) coolingText.text = "";
    }

    /// <summary>
    /// 显示锁定状态（复用技能Text显示LOCKED）
    /// </summary>
    /// <param name="skillText">按键/锁定文本</param>
    private void ShowLockedState(Text skillText)
    {
        if (skillText != null)
        {
            skillText.text = LockedTextContent;
            skillText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("SkillManagePanel: 技能文本未赋值，无法显示锁定状态");
        }
    }

    /// <summary>
    /// 隐藏锁定状态（恢复显示按键文本）
    /// </summary>
    /// <param name="skillText">按键/锁定文本</param>
    private void HideLockedState(Text skillText)
    {
        if (skillText != null)
        {
            skillText.gameObject.SetActive(true); // 确保文本可见（用于显示按键）
        }
    }
}