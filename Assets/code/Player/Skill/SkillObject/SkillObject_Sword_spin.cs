using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_Sword_spin : SkillObject_Sword
{
    private float maxDistance;
    private float attackPerSecond;
    private float attackTimer;
    private float attackDestorytime;

    public override void SetUpSword(Skill_SwordThrow swordManager, Vector2 diretion, DamageScaleData damage)
    {
        base.SetUpSword(swordManager, diretion, damage);
        maxDistance = swordManager.maxDistance;
        attackPerSecond = swordManager.attackPerSecond;
        attackDestorytime = swordManager.attackDestorytime;
        if (anim != null)
            anim.SetTrigger("spin");
        
    }
    protected override void Update()
    {
        HandleBackPlayer();
        HandleAttact();
        HandleStopping();
    }
    private void HandleStopping()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        attackDestorytime -= Time.deltaTime;
        if (distance > maxDistance)
        {
            rigi.simulated = false;
        }
        if (attackDestorytime <= 0)
            shouldBack = true;
    }
    
    private void HandleAttact()
    {
        attackTimer -= Time.deltaTime;
        if(attackTimer<=0)
        {
            CreateDamage(damageScaleData);
            attackTimer =  1f /attackPerSecond;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        rigi.simulated = false;
    }
}
