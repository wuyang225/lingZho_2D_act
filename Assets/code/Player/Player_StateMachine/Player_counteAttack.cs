using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_counteAttack : PlayerState
{
    public Player_Combat combat;
    public bool enterperformed;
    private float counttime ;
    public Player_counteAttack(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }
    public override void Enter()
    {
        base.Enter();
        counttime = player.counttime;
        combat = player.GetComponent<Player_Combat>();
        player.isEntercount = true;
        enterperformed = false;
        statetimer = player.counteAttacktimer;


    }
    public override void Update()
    {
        rigi.velocity = Vector2.zero;
        base.Update();
        if (statetimer < 0)
            player.statemachine.ChangeState(player.idlestate);
        counttime -= Time.deltaTime;
        if (combat.isEnemyAttack()&& counttime>0)
        {
            combat.EnterHandConterable();
            enterperformed = true;
            player.statemachine.ChangeState(player.counteAttackPerformedstate);
            
        }

    }
    public override void Exit()
    {
        base.Exit();
        if(enterperformed==false) player.isEntercount = false;
    }
}
