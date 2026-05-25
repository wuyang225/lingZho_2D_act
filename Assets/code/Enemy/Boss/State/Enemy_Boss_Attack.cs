using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss_Attack : Enemy_Attack1
{
    private Enemy_Boss enemy_Boss;
    private Transform player;
    public Enemy_Boss_Attack(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
        enemy_Boss = en as Enemy_Boss;
    }
    public override void Enter()
    {
        anim.SetBool(stateName, true);
        triggeratt = false;
        SyncAttackSpeed();
        enemy_Boss.skillCooling -= enemy_Boss.getBasebaseSkillCooling() / 3;
        player = enemy_Boss.GetPlayerPos();
        enemy.HandleFilp(DirectionToPlayer());
    }
    public override void Update()
    {
        updataAnimationParameters();
        statetimer -= Time.deltaTime;
        if (triggeratt)
        {
            if (enemy_Boss.ShouldTeleport())
            {
                enemy_Boss.statemachine.ChangeState(enemy_Boss.enemy_Boss_Teleport);
            }
            else
            enemy_Boss.statemachine.ChangeState(enemy_Boss.enemy_Boss_battel);
        }
    }
    protected virtual float DirectionToPlayer()
    {
        if (player == null)
            return 0;
        return enemy.transform.position.x - player.position.x > 0 ? -1 : 1;
    }
}
