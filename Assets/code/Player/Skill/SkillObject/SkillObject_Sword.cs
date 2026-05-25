using UnityEngine;

/// <summary>
/// 飞剑技能对象类，继承自 SkillObject_Base
/// 负责处理飞剑的运动、碰撞、回程逻辑
/// </summary>
public class SkillObject_Sword : SkillObject_Base
{
    /// <summary>剑的技能管理器，负责控制冷却等逻辑</summary>
    protected Skill_SwordThrow swordManager;

    /// <summary>刚体组件，用于物理运动控制</summary>
    protected Rigidbody2D rigi;

    /// <summary>玩家的 Transform，用于计算回程方向和距离</summary>
    public Transform playerTransform;

    /// <summary>飞剑回程时的移动速度</summary>
    public float backPlayerSpeed = 20;

    /// <summary>飞剑与玩家之间允许的最大距离，超过后强制回程</summary>
    public float maxAllowedDistance = 20;

    /// <summary>是否应该回程的标志位</summary>
    public bool shouldBack = false;

    /// <summary>
    /// 初始化组件，获取刚体引用
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        rigi = this.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 每帧更新：
    /// 1. 使剑的朝向始终与速度方向一致（看起来像在"飞"）
    /// 2. 处理回程逻辑
    /// 3. 如果插入的敌人已死亡，则触发回程
    /// </summary>
    protected virtual void Update()
    {
        // 让剑头始终朝向飞行方向
        this.transform.right = rigi.velocity;

        // 处理回程逻辑（距离过远或已触发回程标志）
        HandleBackPlayer();

        // 如果剑插在某个敌人身上，且该敌人已死亡，则触发回程
        if (this.GetComponentInParent<Enemy_Health>() != null
            && this.GetComponentInParent<Enemy_Health>().isdead)
            shouldBack = true;
    }

    /// <summary>
    /// 设置飞剑的初始状态
    /// </summary>
    /// <param name="swordManager">管理该剑的技能控制器</param>
    /// <param name="diretion">飞剑的初始速度向量（方向 + 速度大小）</param>
    /// <param name="damage">伤害倍率数据</param>
    public virtual void SetUpSword(Skill_SwordThrow swordManager, Vector2 diretion, DamageScaleData damage)
    {
        // 设置刚体速度，使剑朝指定方向飞出
        rigi.velocity = diretion;

        // 保存技能管理器引用
        this.swordManager = swordManager;

        // 获取技能管理器所在根对象（即玩家）的 Transform
        playerTransform = swordManager.transform.root;

        // 保存伤害数据
        this.damageScaleData = damage;
    }

    /// <summary>
    /// 触发器碰撞事件：剑碰到物体时，停止运动并造成伤害
    /// </summary>
    /// <param name="collision">碰到的碰撞体</param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // 停止剑的物理模拟并将剑设为碰撞对象的子物体（插入效果）
        StopSword(collision);

        // 对碰撞体造成技能伤害
        CreateDamageBox(damageScaleData, collision);
    }

    /// <summary>
    /// 创建伤害并应用到指定碰撞体
    /// </summary>
    /// <param name="damageScaleData">伤害倍率数据</param>
    /// <param name="collider">受击的碰撞体</param>
    protected void CreateDamageBox(DamageScaleData damageScaleData, Collider2D collider)
    {
        // 将碰撞体封装为数组（接口要求传入数组格式）
        Collider2D[] col = new Collider2D[] { collider };

        // 调用玩家战斗组件的技能伤害方法
        playerCombat.CreatSkillDamage(damageScaleData, col);
    }

    /// <summary>
    /// 处理飞剑回程逻辑：
    /// - 超出最大距离时强制回程
    /// - 回程中每帧向玩家移动
    /// - 到达玩家附近后销毁自身，并可能缩短冷却
    /// </summary>
    public void HandleBackPlayer()
    {
        // 计算当前与玩家的距离
        float distance = Vector2.Distance(playerTransform.position, transform.position);

        // 超出最大允许距离：强制触发回程，并关闭物理模拟
        if (distance > maxAllowedDistance)
        {
            shouldBack = true;
            rigi.simulated = false; // 关闭物理，防止继续受重力/速度影响
        }

        // 如果不需要回程，直接返回
        if (shouldBack == false)
            return;

        // 每帧向玩家位置移动（线性插值）
        transform.position = Vector2.MoveTowards(
            transform.position,
            playerTransform.position,
            Time.deltaTime * backPlayerSpeed
        );

        // 到达玩家附近（距离小于 0.5 单位）时执行回收逻辑
        if (distance < .5f)
        {
            // 销毁飞剑游戏对象
            Destroy(this.gameObject);

            // 重置回程标志（虽然对象即将销毁，但保持逻辑完整性）
            shouldBack = false;

            // 如果当前处于"旋转模式"，回程后缩短冷却时间（奖励机制）
            if (swordManager.isSpin == true)
                swordManager.cooldown /= 1.5f;
        }
    }

    /// <summary>
    /// 停止飞剑：关闭物理模拟，并将剑附着到碰撞对象上（插入效果）
    /// </summary>
    /// <param name="collision">被插入的碰撞体</param>
    private void StopSword(Collider2D collision)
    {
        // 关闭刚体物理模拟，使剑静止
        rigi.simulated = false;

        // 将剑设为碰撞对象的子物体，随目标一起移动
        transform.SetParent(collision.transform);
    }
}
