using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_move : Enemy_Groundstate
{
    public Enemy_move(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
    }
    public override void Enter()
    {
        base.Enter();
        if (enemy.facingGround == false || enemy.walltag)
        {
            enemy.fill();
        }
    }
    public override void Update()
    {
        base.Update();
        enemy.setvelocity(enemy.facingDir* enemy.moveSpeed, rigi.velocity.y);
        if(enemy.facingGround==false||enemy.walltag)
        {
            enemy.statemachine.ChangeState(enemy.idlestate);
        }
        if(rigi.velocity.x!=0&& enemy.PlayerDetection() == false)
        {
            enemy.statemachine.ChangeState(enemy.movestate);
        }
    }

}
