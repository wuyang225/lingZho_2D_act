using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

[DefaultExecutionOrder(-100)]
public class Entity_stat : MonoBehaviour
{
    public Stat_OffenseGroup offenseGroup;
    public Stat_ResourcesGroup resourcesGroup;
    public string setUpname = "setUpDefault";
    public Stat_SetupSO setUpDefault = new Stat_SetupSO();

    public virtual void Awake()
    {
        InitializationData();
    }

    public void InitializationData()
    {
        setUpDefault = DataManage.Instance.LoadStatSetupData(setUpname);
        print(setUpDefault);
        // 8. 同步配置到属性组
        InitStatsFromDefaultConfig();
    }

    // 从setUpDefault（JSON配置）同步属性值到各个属性组
    protected void InitStatsFromDefaultConfig()
    {
        // 安全校验：确保setUpDefault不为空
        if (setUpDefault == null)
        {
            Debug.LogError("InitStatsFromDefaultConfig: setUpDefault为空，无法初始化属性！", this);
            return;
        }

        // 1. 初始化资源组（生命/生命回复）
        if (resourcesGroup != null)
        {
            resourcesGroup.Max_Health?.SetValue(setUpDefault.maxHealth);
            resourcesGroup.Health_Regen?.SetValue(setUpDefault.healthRegen);
        }
        else
        {
            Debug.LogError("InitStatsFromDefaultConfig: resourcesGroup未赋值！", this);
        }

        // 2. 初始化进攻组（物理/元素伤害、暴击等）
        if (offenseGroup != null)
        {
            offenseGroup.attackSpeed?.SetValue(setUpDefault.attackSpeed);
            offenseGroup.damage?.SetValue(setUpDefault.damage);
            offenseGroup.critChance?.SetValue(setUpDefault.critChance);
            offenseGroup.critPower?.SetValue(setUpDefault.critPower);
            offenseGroup.fireDamage?.SetValue(setUpDefault.fireDamage);
            offenseGroup.iceDamage?.SetValue(setUpDefault.iceDamage);
            offenseGroup.iceDuration?.SetValue(setUpDefault.iceDuration);
            offenseGroup.lightingDamage?.SetValue(setUpDefault.lightningDamage);
            offenseGroup.elementType=setUpDefault.elementType;
        }
        else
        {
            Debug.LogError("InitStatsFromDefaultConfig: offenseGroup未赋值！", this);
        }
    }

    // ========== 原有方法保持不变 ==========
    // 计算最终的元素伤害值
    public float GetElementalDamage(out ElementType elementType, float scale = 1)
    {
        // 获取三种元素伤害的基础值
        float fireDamage = offenseGroup.fireDamage.GetValue();
        float iceDamage = offenseGroup.iceDamage.GetValue();
        float lightningDamage = offenseGroup.lightingDamage.GetValue();

        elementType = offenseGroup.elementType;
        // 初始化最高伤害为火焰伤害
        float highestDamage = fireDamage;
        if (highestDamage > 10)

        // 检查并更新最高伤害为冰霜伤害
        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
        }

        // 检查并更新最高伤害为闪电伤害
        if (lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
        }

        // 如果最高伤害小于等于10，则直接返回0，避免无效伤害
        if (highestDamage <= 10)
            return 0;

        // 计算最终总伤害：最高伤害
        float finalDamage = highestDamage;

        // 返回最终计算出的元素伤害
        return finalDamage * scale;
    }

    public float PerformAttack(out bool isCritHit, float scale = 1)
    {
        float basesDamage = offenseGroup.damage.GetValue();
        float totalDamage = basesDamage;

        // 计算基础暴击率
        float baseCritChance = offenseGroup.critChance.GetValue();
        // 总暴击率
        float critChance = baseCritChance;

        // 计算基础暴击倍率
        float baseCritPower = offenseGroup.critPower.GetValue();
        // 总暴击倍率（转换为伤害乘数，如 150 / 100 = 1.5倍）
        float critPower = (baseCritPower) / 100;

        // 随机判定是否触发暴击
        isCritHit = Random.Range(0, 100) < critChance;
        // 计算最终伤害：暴击则总基础伤害 × 暴击倍率，否则直接使用总基础伤害
        float finalDamage = isCritHit ? totalDamage * critPower : totalDamage;

        return finalDamage * scale;
    }

    public float GetMaxHealth()
    {
        float baseHp = resourcesGroup.Max_Health.GetValue();
        return baseHp;
    }

    public Stat GetStatByType(StatType type)
    {
        // 使用switch语句根据类型分发获取对应的属性引用
        switch (type)
        {
            // 资源类（生命与回复）
            case StatType.MaxHealth: return resourcesGroup.Max_Health;
            case StatType.HealthRegen: return resourcesGroup.Health_Regen;


            // 进攻类（物理与法术战斗属性）
            case StatType.AttackSpeed: return offenseGroup.attackSpeed;
            case StatType.Damage: return offenseGroup.damage;
            case StatType.CritChance: return offenseGroup.critChance;
            case StatType.CritPower: return offenseGroup.critPower;

            // 元素伤害类
            case StatType.FireDamage: return offenseGroup.fireDamage;
            case StatType.IceDamage: return offenseGroup.iceDamage;
            case StatType.IceDuration: return offenseGroup.iceDuration;
            case StatType.LightningDamage: return offenseGroup.lightingDamage;


            // 默认分支：处理未定义的属性类型
            default:
                Debug.LogWarning($"StatType {type} not implemented yet.");
                return null;
        }
    }
}