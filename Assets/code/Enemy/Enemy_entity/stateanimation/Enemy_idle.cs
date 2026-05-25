using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_idle : Enemy_Groundstate
{
    public Enemy_idle(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
    }
    public override void Enter()
    {
        base.Enter();
        statetimer = enemy.idletime;
    }

    public override void Update()
    {
        base.Update();
        if(statetimer<0||rigi.velocity.x!=0)
        {
            enemy.statemachine.ChangeState(enemy.movestate);
        }
    }
}
