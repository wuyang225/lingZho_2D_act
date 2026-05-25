using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wooden_Chest : Golden_Chest
{
    public override void TakeDamage(float Damage, float elementalDamage, ElementType elementType, Transform damageDealer)
    {
        // 防止宝箱被重复攻击触发多次逻辑
        if (isChestOpened) return;
        isChestOpened = true;

        vfx.PlayOnDamageVFX();
        rigi.angularVelocity = Random.Range(-200f, 200f);
        rigi.velocity = konckback;
        anim.SetBool("open", true);
        UIManager.Instance.ShowPanel<Role_Value_Plane>();

        // 生成金币
        CreateGoldCoin(GoldCoinobjNumber);

        StartCoroutine(DisableChestColliderAfterDelay(1f)); // 1秒后关闭（匹配动画时长）
    }
}
