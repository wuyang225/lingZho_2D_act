using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_Shard : SkillObject_Base
{
    // 在 Inspector 中配置的特效预制体，用于爆炸时生成视觉表现
    [SerializeField] private GameObject vfxPrefab;

    public void SetUpShard(float detinationTime,DamageScaleData damage)
    {
        damageScaleData = damage;
        Invoke(nameof(Explode), detinationTime);
    }    
    public void Explode()
    {
        // 对范围内的敌人执行伤害逻辑
        CreateDamage(damageScaleData);

        // 在当前位置实例化特效预制体，播放爆炸视觉效果
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);

        // 销毁自身，结束技能生命周期
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检测碰撞对象是否为敌人：如果没有 Enemy 组件，则直接返回
        if (collision.GetComponent<Enemy>() == null)
            return;

        // 碰撞对象是敌人，触发爆炸
        Explode();
    }

}
