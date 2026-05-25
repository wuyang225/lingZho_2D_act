using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack1 : Enemy_State
{
    public Enemy_Attack1(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
    }
    public override void Enter()
    {
        base.Enter();
        SyncAttackSpeed();
    }
    public override void Update()
    {
        base.Update();
        if (triggeratt&&enemy.canstunned==false)
        {
            enemy.statemachine.ChangeState(enemy.battelstate);
        }
    }
}
