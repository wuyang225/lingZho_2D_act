using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Base:MonoBehaviour
{
    public Player_SkillManager skillManager;
    public Player player;
    public Entity_Health playerHealth;
    [Header("Skill_details")]
    [SerializeField] public bool isActive=false;
    [SerializeField] public bool isUpGrade=false;
    [SerializeField] public float cooldown;
    [SerializeField]  public float lastTimeUsed;
    [SerializeField] public DamageScaleData damageData=new DamageScaleData();

    protected virtual void Awake()
    {
        lastTimeUsed += cooldown;
        player = this.GetComponentInParent<Player>();
        playerHealth = this.GetComponentInParent<Entity_Health>();
        skillManager = this.GetComponentInParent<Player_SkillManager>();
    }
    public virtual void SetActive(bool value)
    {
        isActive = value;
    }
    public virtual void Update()
    {
        if(lastTimeUsed<=cooldown)
        lastTimeUsed += Time.deltaTime;
    }
    public virtual bool CanUseSkill()
    {
        if(OnCooldown()|| isActive==false)
        {
            return false;
        }
        return true;
    }

    protected bool OnCooldown() => cooldown >= lastTimeUsed;
    public void SetSkillOnCooldown() => lastTimeUsed = 0;
    public void ResetCooldownBy(float value) => lastTimeUsed = lastTimeUsed + value;
    public void ResetCooldown()=> lastTimeUsed = 0;
}
