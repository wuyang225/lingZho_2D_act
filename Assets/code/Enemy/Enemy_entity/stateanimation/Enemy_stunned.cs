using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_stunned : Enemy_State
{
    private Enemy_VFX vfx;
    public Enemy_stunned(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
    }
    public override void Enter()
    {
        base.Enter();
        vfx = enemy.GetComponent<Enemy_VFX>();
        vfx.enableAttackAlart(false);
        statetimer = enemy.stunnedDuration;
        rigi.velocity = new Vector2(enemy.stunnedDistance.x * -enemy.facingDir, enemy.stunnedDistance.y);
    }

    public override void Update()
    {
        base.Update();
        if (statetimer < 0)
            enemy.statemachine.ChangeState(enemy.battelstate);
    }
    public override void Exit()
    {
        base.Exit();
        enemy.enableConter(false);
    }

}
