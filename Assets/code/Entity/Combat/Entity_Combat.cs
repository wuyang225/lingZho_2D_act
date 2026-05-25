using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public Collider2D[] attTrigger;
    protected Entity_VFX vfx;
    protected Entity_stat stat;
    [Header("Target detection")]
    [SerializeField] protected Transform targetCheck;
    public float targetCheckRadius = 1;
    [SerializeField] protected LayerMask whatLayer;
    [SerializeField] protected float damage;
    [SerializeField] protected float elementalDamage;
    [Header("Status effect details")]
    [SerializeField] protected float defaultDuration = 2;
    [SerializeField] protected float chillSlowMultiplier = .2f;
    [SerializeField] protected float electrifyChargBuilUp = .4f;

    public virtual void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stat = GetComponent<Entity_stat>();
    }
    public virtual void performAttack()
    {
        bool isCritHit = false;
        damage = stat.PerformAttack(out isCritHit);
        elementalDamage = stat.GetElementalDamage(out ElementType elementType);
        attTrigger = Getcolldercombat();
        foreach (var item in attTrigger)
        {
            IDamgable id = item.GetComponent<IDamgable>();
            if (id == null)
                continue;
            id.TakeDamage(damage, 0, elementType, transform);
            vfx.updateOnhitColor(elementType);
            ApplyStatusEffect(item.transform, elementType);
            vfx.CreateOnHitVFX(item.transform, isCritHit);
        }

    }
    public virtual void CreatSkillDamage(DamageScaleData damage, Collider2D[] attTrigger)
    {
        foreach (var item in attTrigger)
        {
            IDamgable id = item.GetComponent<IDamgable>();
            if (id == null)
                continue;
            id.TakeDamage(damage.damage, 0, damage.elementType, transform);
            vfx.updateOnhitColor(damage.elementType);
            ApplyStatusEffect(item.transform, damage.elementType, damage.elementDamageScale, damage.elementDurationScale);
        }
    }
    public virtual void CreatSingleDamage(Transform col,float Scale=1,ElementType elem=ElementType.None)
    {
        bool isCritHit = false;
        damage = stat.PerformAttack(out isCritHit)*Scale;
        elementalDamage = stat.GetElementalDamage(out ElementType elementType);
        if (elementType == ElementType.None)
            elementType = elem;
        attTrigger = Getcolldercombat();
        IDamgable id = col.GetComponent<IDamgable>();
        id.TakeDamage(damage, 0, elementType, transform);
        vfx.updateOnhitColor(elementType);
        ApplyStatusEffect(col.transform, elementType);
        vfx.CreateOnHitVFX(col.transform, isCritHit);

    }
    public void ApplyStatusEffect(Transform target, ElementType element, float scale = 1, float elementDurationScale = 1)
    {
        // 获取目标实体上的状态处理器组件
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

        // 如果目标没有状态处理器，直接返回，不执行后续逻辑
        if (statusHandler == null)
            return;

        // 判断是否为火焰元素，并且满足应用条件
        if (element == ElementType.Fire && statusHandler.CanBeApplied(ElementType.Fire))
        {
            float fireDamage = stat.offenseGroup.fireDamage.GetValue() * scale;
            statusHandler.ApplyBurnEffect(defaultDuration * elementDurationScale, fireDamage);
        }
        // 判断是否为冰冻元素，并且满足应用条件
        if (element == ElementType.Ice && statusHandler.CanBeApplied(ElementType.Ice))
        {
            // 应用冰冻效果，传入预设的持续时间和减速倍率
            statusHandler.ApplyChilledEffect(stat.offenseGroup.iceDuration.GetValue() * elementDurationScale, chillSlowMultiplier * scale);
        }
        // 判断是否为闪电元素，并且满足应用条件
        if (element == ElementType.Lightning && statusHandler.CanBeApplied(ElementType.Lightning))
        {
            // 应用闪电效果，传入预设的持续时间和减速倍率
            float lightDamage = stat.offenseGroup.lightingDamage.GetValue() * scale;
            statusHandler.ApplyLightEffect(defaultDuration * elementDurationScale, lightDamage, electrifyChargBuilUp);
        }
    }

    public Collider2D[] Getcolldercombat()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatLayer);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }

}
