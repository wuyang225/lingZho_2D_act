using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Entity_StatusHandler : MonoBehaviour
{
    // 当前生效的元素效果，默认为无
    private ElementType currentEffect = ElementType.None;
    Entity entity;
    Entity_VFX entityVfx;
    Entity_Health entityHealth;
    Entity_stat entitystat;

    [Header("Electrify effect details")]
    // 闪电打击的特效预制体（用于实例化雷电视觉表现）
    [SerializeField] private GameObject lightingStrikeVfx;
    // 当前充能值（0~maximumCharge，用于控制充能进度）
    [SerializeField] private float currentCharge;
    // 最大充能值（默认1，代表充能满值）
    [SerializeField] private float maximumCharge = 1;

    // 充能协程的引用，用于追踪/终止充能过程
    private Coroutine electrifyCo;
    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
        entityHealth = GetComponent<Entity_Health>();
        entitystat = GetComponent<Entity_stat>();
    }
    public void ApplyLightEffect(float duration, float damage,float charge)
    {
        currentCharge += charge;
        if(currentCharge >= maximumCharge)
        {
            DolightningStrike(damage);
            StopElectrifyEffect();
            return;
        }
        if (electrifyCo != null)
        {
            StopCoroutine(electrifyCo);
        }
        electrifyCo= StartCoroutine(lightningEffectCo(duration));
    }

    private void StopElectrifyEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        entityVfx.StopAllVfx();
    }

    private void DolightningStrike(float damage)
    {
        Instantiate(lightingStrikeVfx, transform.position, Quaternion.identity);
        entityHealth.ReduceHealth(damage);
    }
    private IEnumerator lightningEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Lightning);
        yield return new WaitForSeconds(duration);
        StopElectrifyEffect();

    }
    public void ApplyBurnEffect(float duration, float damage)
    {
        StartCoroutine(BurnEffectCo(duration, damage));
    }
    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        // 标记当前元素效果为火焰
        currentEffect = ElementType.Fire;
        // 播放火焰状态的视觉特效（如红色闪烁）
        entityVfx.PlayOnStatusVfx(duration, ElementType.Fire);

        // 每秒伤害次数（这里设置为每秒2次）
        int ticksPerSecond = 2;
        // 计算总伤害次数：每秒次数 × 总时长
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);
        // 每次伤害的数值：总伤害 ÷ 总次数
        float damagePerTick = totalDamage / tickCount;
        // 每次伤害的间隔时间：1秒 ÷ 每秒次数
        float tickInterval = 1f / ticksPerSecond;

        // 循环执行伤害
        for (int i = 0; i < tickCount; i++)
        {
            // 对实体造成一次伤害
            entityHealth.ReduceHealth(damagePerTick);
            // 等待一个间隔时间后再进行下一次伤害
            yield return new WaitForSeconds(tickInterval);
        }

        // 燃烧效果结束，重置当前元素效果为无
        currentEffect = ElementType.None;
    }
    // 协程：执行冰冻效果的完整流程
    public void ApplyChilledEffect(float duration, float slowMultiplier)
    {
        entity.SlowDownEntity(duration, slowMultiplier);
        StartCoroutine(ChilledEffectCo(duration));
    }

    private IEnumerator ChilledEffectCo(float duration)
    {
        // 1. 将当前元素效果标记为冰冻
        currentEffect = ElementType.Ice;

        // 2. 播放冰冻状态的视觉特效（颜色闪烁）
        entityVfx.PlayOnStatusVfx(duration, ElementType.Ice);

        // 3. 等待冰冻效果持续指定的时间
        yield return new WaitForSeconds(duration);

        // 4. 效果结束，将当前元素效果重置为无
        currentEffect = ElementType.None;
    }
    public bool CanBeApplied(ElementType element)
    {
        if (element==ElementType.Lightning && currentEffect==ElementType.Lightning)
        {
            return true;
        }
        // 规则：只有当当前没有任何元素效果时，才能应用新效果
        return currentEffect == ElementType.None;
    }
}
