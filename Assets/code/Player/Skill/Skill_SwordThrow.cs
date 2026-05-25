using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill_SwordThrow : Skill_Base
{
    [SerializeField] private float throwPower = 4;

    private Vector3 backPlayerpos;

    public bool isSpin=false;
    public bool spinUpGrade = false;
    public bool isSpinActive = false;
    [SerializeField] private GameObject swordSpinPrefab;
    public DamageScaleData spinDamageData = new DamageScaleData();
    public float maxDistance=5;
    public float attackPerSecond=6;
    public float attackDestorytime=3;

    //用于预测轨迹的点预制体
    [SerializeField] private GameObject predictionDot;
    [SerializeField] private GameObject swordPrefab;
    public SkillObject_Sword currentSword;

    //轨迹点的总数量（默认20个）
    [SerializeField] private int numberOfDots = 20;

    //轨迹点之间的间隔（默认0.05f）
    [SerializeField] private float spaceBetweenDots = .05f;

    // 存储所有轨迹点的Transform数组
    private Transform[] dots;

    // 确认后的投掷方向（由外部输入设置）
    private Vector2 confirmedDirection;

    private float swordGravity;

    protected override void Awake()
    {
        base.Awake();
        swordGravity = swordPrefab.GetComponent<Rigidbody2D>().gravityScale;
        dots = GenerateDots();
    }
    public override void Update()
    {
        base.Update();
    }
    // 确认投掷轨迹方向
    public void ConfirmTrajectory(Vector2 direction) => confirmedDirection = direction;

    public bool  CanUseSpinSkill()
    {
        if (OnCooldown() || isSpinActive == false)
        {
            return false;
        }
        return true;
    }
    public void ThrowSword()
    {
        GameObject newSword;
        damageData.damage = player.stat.offenseGroup.damage.GetValue();
        spinDamageData.damage = player.stat.offenseGroup.damage.GetValue()*0.3f;
        if (isUpGrade)
            damageData.elementType = ElementType.Fire;
        else damageData.elementType = ElementType.None;
        if (spinUpGrade)
            spinDamageData.elementType = ElementType.Lightning;
        else spinDamageData.elementType = ElementType.None;
        if (isSpin)
        {
            // 在轨迹预测点数组的第2个点（索引1）的位置，实例化剑预制体
            newSword = Instantiate(swordSpinPrefab, dots[1].position, Quaternion.identity);
            // 获取新生成剑对象上的 SkillObject_Sword 组件
            currentSword = newSword.GetComponent<SkillObject_Sword_spin>();

            // 初始化剑的行为：传入自身（Skill_SwordThrow）作为引用，并设置投掷力度
            currentSword.SetUpSword(this, GetThrowPower(), spinDamageData);
        }
        else
        {
            newSword = Instantiate(swordPrefab, dots[1].position, Quaternion.identity);
            // 获取新生成剑对象上的 SkillObject_Sword 组件
            currentSword = newSword.GetComponent<SkillObject_Sword>();

            // 初始化剑的行为：传入自身（Skill_SwordThrow）作为引用，并设置投掷力度
            currentSword.SetUpSword(this, GetThrowPower(), damageData);
        }


    }

    public bool IsHasSword()
    {
        if (currentSword != null)
        {
            currentSword.shouldBack = true;
            return true;
        }
        return false;
    }
    // 计算并返回最终的投掷力度向量
    private Vector2 GetThrowPower() => confirmedDirection * (throwPower * 10);
    // 根据给定方向更新所有轨迹点的位置，实现轨迹预测可视化
    public void PredictTrajectory(Vector2 direction)
    {
        SetSkillOnCooldown();
        // 遍历所有轨迹点，逐个更新其位置
        for (int i = 0; i < dots.Length; i++)
        {
            // 计算第 i 个点对应的时间 t = 索引 × 点间距，然后获取该时间点的轨迹位置
            dots[i].position = GetTrajectoryPoint(direction, i * spaceBetweenDots);
        }
    }

    // 计算在给定方向和时间 t 时，剑的预测位置
    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        // 缩放投掷力度，使数值更适合2D物理表现
        float scaledThrowPower = throwPower * 10;

        // 计算初始速度：方向 × 缩放后的力度，决定投掷的初速度和方向
        Vector2 initialVelocity = direction * scaledThrowPower;

        // 计算重力影响：
        // 0.5 * g * t² 是自由落体位移公式，乘以 swordGravity 可单独调整剑的重力权重
        Vector2 gravityEffect = 0.5f * Physics2D.gravity * swordGravity * (t * t);

        // 计算 t 时刻的位移：
        // 初始速度产生的位移 + 重力产生的位移
        Vector2 predictedPoint = (initialVelocity * t) + gravityEffect;

        // 获取玩家（根物体）的位置，作为轨迹的起点
        Vector2 playerPosition = transform.root.position;

        // 返回玩家位置 + 预测位移，得到最终的世界空间位置
        return playerPosition + predictedPoint;
    }
    // 启用/禁用所有轨迹点的显示
    public void EnableDots(bool enable)
    {
        // 遍历所有轨迹点，统一设置显隐状态
        foreach (Transform t in dots)
        {
            t.gameObject.SetActive(enable);
        }
    }

    // 生成轨迹点阵列
    private Transform[] GenerateDots()
    {
        // 初始化轨迹点数组
        Transform[] newDots = new Transform[numberOfDots];

        // 循环实例化所有轨迹点
        for (int i = 0; i < numberOfDots; i++)
        {
            // 实例化点预制体，位置为当前技能对象位置，无旋转，父物体为自身
            newDots[i] = Instantiate(predictionDot, transform.position, Quaternion.identity, transform).transform;
            // 默认隐藏所有轨迹点
            newDots[i].gameObject.SetActive(false);
        }

        // 返回生成的轨迹点数组
        return newDots;
    }
}