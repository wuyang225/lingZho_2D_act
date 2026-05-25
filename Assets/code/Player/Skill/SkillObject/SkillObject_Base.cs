using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected Player_Combat playerCombat;
    //"判定为敌人的层级掩码"
    [SerializeField] protected LayerMask whatIsEnemy;

    //用于检测范围的目标点（为空则使用自身位置）
    [SerializeField] protected Transform targetCheck;

    //检测半径（默认1）
    [SerializeField] protected float checkRadius = 1f;

    [SerializeField] protected float swoedDirection = 1;
    [SerializeField] public DamageScaleData damageScaleData = new DamageScaleData();

    private Collider2D col;
    protected Animator anim;
    protected virtual  void Awake()
    {
        col = this.transform.GetComponent<Collider2D>();
        anim = this.transform.GetComponentInChildren<Animator>();
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Combat>();
    }
    protected virtual void CreateDamage(DamageScaleData damageScaleData)
    {
        playerCombat.CreatSkillDamage(damageScaleData,EnemiesAround(transform, checkRadius));
    }

    protected virtual Collider2D[] EnemiesAround(Transform t, float radius)
    {
        // 使用2D圆形重叠检测，返回指定层级的碰撞器
        return Physics2D.OverlapCircleAll(new Vector3(t.position.x + col.offset.x* swoedDirection* transform.lossyScale.x, 
            t.position.y + col.offset.y* swoedDirection* transform.lossyScale.y), radius, whatIsEnemy);
    }

    /// <summary>
    /// 在Scene视图中绘制检测范围的辅助线框
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        // 若未指定目标点，则使用自身Transform
        if (targetCheck == null)
            targetCheck = transform;

        // 绘制线框球体，直观显示检测范围
        Gizmos.DrawWireSphere(new Vector3(targetCheck.position.x + col.offset.x* swoedDirection* transform.lossyScale.x,
            targetCheck.position.y + col.offset.y* swoedDirection* transform.lossyScale.y), checkRadius);
    }
}