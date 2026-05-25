using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat_DefenseGroup
{
    // 物理防御
    public Stat armor;
    // 闪避率
    public Stat evasion;

    // 火焰抗性
    public Stat fireRes;
    // 冰霜抗性
    public Stat iceRes;
    // 雷电抗性
    public Stat lightningRes;
}