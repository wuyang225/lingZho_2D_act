using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(700)]
public class Entity_Health : MonoBehaviour, IDamgable
{
    public Entity_stat entitiystat;
    public Slider healthBar;
    protected Entity_VFX vfx;
    protected Entity entity;
    [SerializeField] public bool isdead = false;
    [SerializeField] public float currentHp;

    [Header("血量变化提示")]
    private const string HpChangePrefabPath = "Hp/HpChange";

    [Header("Health regen")]
    [SerializeField] protected float regenInterval = 1;
    [SerializeField] public bool CanRegenHealth = true;
    private float timecode;

    [Header("on Damage Konckback")]
    [SerializeField] private Vector2 konckback = new Vector2(1.5f, 2.5f);
    [SerializeField] private float konckbackDuration = 0.2f;
    [SerializeField] private float HeavyDamageThreshold = 0.3f;
    [SerializeField] private Vector2 Heavy_konckback = new Vector2(7, 7);
    [SerializeField] private float Heavy_konckbackDuration = 0.5f;

    [Header("相机震动")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // 拖入你的 CM vcam1
    [SerializeField] private float shakeDuration = 0.5f; // 摇晃时长
    [SerializeField] private float shakeAmplitude = 2f; // 摇晃强度


    // 缓存当前的震动Tween（防止叠加）
    private Tween currentShakeTween;

    protected virtual void Awake()
    {
        entitiystat = GetComponent<Entity_stat>();
        vfx = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
        currentHp = entitiystat.GetMaxHealth();
        if (healthBar == null)
            healthBar = this.GetComponentInChildren<Slider>();
        timecode = regenInterval;
        updateHealthHpBar();
        virtualCamera = Camera.main.transform.parent.transform.GetComponentInChildren<CinemachineVirtualCamera>();

   
    }

    protected virtual void Update()
    {
        timecode -= Time.deltaTime;
        if(timecode<=0&&CanRegenHealth)
        {
            if (this.GetComponent<Player>() == null)
            IncreaseHealth(entitiystat.resourcesGroup.Health_Regen.GetValue());
            timecode = regenInterval;
        }
        updateHealthHpBar();
    }

    public void updateHealthHpBar()
    {
        if (healthBar == null || entitiystat == null) return;

        float newHealthValue = currentHp / entitiystat.GetMaxHealth();
            healthBar.value = newHealthValue;

        if (healthBar.value <= 0)
            healthBar.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float Damage, float elementalDamage, ElementType elementType, Transform damageDealer)
    {
        vfx?.PlayOnDamageVFX();
        Vector2 konckvec = CalculateKnockback(Damage, damageDealer);
        float backDuration = CalculateDuration(Damage);
        entity?.ReciveKonckbackcor(konckvec, backDuration);
        if (isdead)
        {
            return;
        }
        ReduceHealth(Damage + elementalDamage);
    }


    public virtual void IncreaseHealth(float HealAmont)
    {
        float newcurrentHp = currentHp + HealAmont;
        float maxcurrentHp = entitiystat.GetMaxHealth();

        currentHp = Mathf.Min(newcurrentHp, maxcurrentHp);

        // 实际加血量（防止超出上限时显示错误数字）
        float actualHeal = currentHp - (newcurrentHp - HealAmont);
        SpawnHpChange(actualHeal);

        updateHealthHpBar();
    }


    public virtual void ReduceHealth(float Damage)
    {
        if (isdead) return;

        currentHp -= Damage;
        SpawnHpChange(-Damage); // 负数 = 红色减血
        
        updateHealthHpBar();
        StartCoroutine(samllShakeCameraCoroutine());
        if (currentHp <= 0)
            Die();
    }
    /// <summary>
    /// 在实体头顶生成血量变化浮动文字
    /// </summary>
    private void SpawnHpChange(float amount)
    {
        if (amount == 0)
            return;
        // 从 Resources 加载预制体
        GameObject prefab = Resources.Load<GameObject>(HpChangePrefabPath);
        if (prefab == null)
        {
            Debug.LogWarning("[Entity_Health] 未找到 HpChange 预制体，路径: Resources/" + HpChangePrefabPath);
            return;
        }


        // 实例化到 Canvas 下
        GameObject go = Instantiate(prefab);

        go.transform.position = entity.wallcheck1.position;

        // 初始化数值与颜色
        go.GetComponent<HpChange>().Init(amount);
    }
    protected virtual void Die()
    {
        isdead = true;
        if (CanRegenHealth)
            CanRegenHealth = false;
        entity.EntityDeath();
    }

    protected Vector2 CalculateKnockback(float Damage, Transform damageDealer)
    {
        Vector2 konck = isHeavyDamage(Damage) ? Heavy_konckback : konckback;
        if (isHeavyDamage(Damage))
        {
            StartCoroutine(ShakeCameraCoroutine());
        }
        float isfill = transform.position.x > damageDealer.position.x ? 1 : -1;
        if (transform.position.x == damageDealer.position.x)
            isfill = -damageDealer.GetComponent<Entity>().facingDir;
        return new Vector2(konck.x * isfill, konck.y);
    }

    // 相机震动协程（保留原有Cinemachine震动，可通过useDOTweenShake开关选择）
    private IEnumerator ShakeCameraCoroutine()
    {
        CinemachineBasicMultiChannelPerlin positionNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        // 开启震动
        positionNoise.m_AmplitudeGain = shakeAmplitude; // 0.1
        positionNoise.m_FrequencyGain = shakeAmplitude;

        yield return new WaitForSeconds(shakeDuration); // 0.5秒

        // 关闭震动
        positionNoise.m_AmplitudeGain = 0f;
        positionNoise.m_FrequencyGain = 0f;
    }
    private IEnumerator samllShakeCameraCoroutine()
    {
        CinemachineBasicMultiChannelPerlin positionNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        // 开启震动
        positionNoise.m_AmplitudeGain = shakeAmplitude* 0.5f; // 0.1
        positionNoise.m_FrequencyGain = shakeAmplitude * 0.5f;

        yield return new WaitForSeconds(shakeDuration*0.15f); // 0.5秒

        // 关闭震动
        positionNoise.m_AmplitudeGain = 0f;
        positionNoise.m_FrequencyGain = 0f;
    }

    protected float CalculateDuration(float Damage) => isHeavyDamage(Damage) ? Heavy_konckbackDuration : HeavyDamageThreshold;
    protected bool isHeavyDamage(float Damage) => Damage / entitiystat.GetMaxHealth() >= HeavyDamageThreshold;
}