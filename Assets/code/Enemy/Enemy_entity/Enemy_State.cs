using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_State : EntityState
{
    protected Enemy enemy;
    public Enemy_State(Enemy en, StateMachine sM, string sN) : base(sM, sN)
    {

        enemy = en;

        anim = enemy.anim;
        rigi = enemy.rbody;
        stat = enemy.stat;
    }

    public override void updataAnimationParameters()
    {
        base.updataAnimationParameters();
        anim.SetFloat("xVelocity", rigi.velocity.x);

        anim.SetFloat("battlespeedAnamtion", enemy.battlespeedmove / enemy.moveSpeed);
        anim.SetFloat("movespeedAnmation", enemy.movespeedAnmation);
    }
    
}
