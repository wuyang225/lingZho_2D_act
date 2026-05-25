using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Groundstate : Enemy_State
{
    public Enemy_Groundstate(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
    }
    public override void Update()
    {
        base.Update();
        if(enemy.PlayerDetection()==true&&enemy.canstunned==false)
        {
            enemy.statemachine.ChangeState(enemy.battelstate);
            enemy.battlestimeDirction = enemy.battlestime;
        }
    }
    
}
