using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat_OffenseGroup
{
    public Stat attackSpeed;
    // 物理伤害
    public Stat damage;
    // 暴击倍率
    public Stat critPower;
    // 暴击概率
    public Stat critChance;

    // 火焰伤害
    public Stat fireDamage;
    // 冰霜伤害
    public Stat iceDamage;
    public Stat iceDuration;
    // 雷电伤害
    public Stat lightingDamage;
    public ElementType elementType = ElementType.None;
}