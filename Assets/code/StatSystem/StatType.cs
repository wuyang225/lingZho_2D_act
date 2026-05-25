using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatType
{
    // 生命与生存类
    MaxHealth,        // 最大生命值
    HealthRegen,      // 生命回复速度

    // 战斗输出类
    AttackSpeed,      // 攻击速度
    Damage,           // 基础伤害
    CritChance,       // 暴击率
    CritPower,        // 暴击伤害倍率

    // 元素伤害类
    FireDamage,       // 火焰伤害加成
    IceDamage,       // 火焰伤害加成
    IceDuration,        // 冰霜伤害加成
    LightningDamage,  // 闪电伤害加成

}
