using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss_SpellCast : Enemy_State
{
    private Enemy_Boss enemy_Boss;
    private float spellCastIntervalTime ;
    private float lastSpellCastIntervalTime ;
    public Enemy_Boss_SpellCast(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
        enemy_Boss = en as Enemy_Boss;
    }
    public override void Enter()
    {
        base.Enter();
        anim.SetBool("spellCastPreformed", false);
        enemy_Boss.setvelocity(0, 0);
        statetimer = enemy_Boss.SpellcastTime;
        spellCastIntervalTime = enemy_Boss.spellCastIntervalTime;
        lastSpellCastIntervalTime = Time.time;
        if (enemy_Boss.bossMode == 2)
            spellCastIntervalTime = spellCastIntervalTime*0.7f;
    }

    public override void Update()
    {
        base.Update();
        Debug.Log(statetimer);
        if (statetimer<=0)
        {
             anim.SetBool("spellCastPreformed", true);
        }
        if (triggeratt)
            stateMachine.ChangeState(enemy_Boss.enemy_Boss_battel);
        if(enemy_Boss.health.isdead)
        {
            stateMachine.ChangeState(enemy_Boss.deadstate);
        }
        if (enemy_Boss.createBullet&& CanCreateSpell())
        {
            lastSpellCastIntervalTime = Time.time;
            enemy_Boss.CreatespellCastPrefab(enemy_Boss.GetPlayerPos());
        }
    }

    public override void Exit()
    {
        anim.SetBool("spellCastPreformed", true);
        base.Exit();
    }
    private bool CanCreateSpell() => Time.time > spellCastIntervalTime + lastSpellCastIntervalTime ? true : false;

}
