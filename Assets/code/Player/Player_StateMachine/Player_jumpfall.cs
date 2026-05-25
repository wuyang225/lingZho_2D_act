using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_jumpfall : PlayerState
{
    public Player_jumpfall(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.setvelocity(rigi.velocity.x, rigi.velocity.y);
        player.UpdateFallPos();
    }
    public override void Update()
    {
        player.setvelocity(player.speedmove * 0.7f * Input.GetAxisRaw("Horizontal"), rigi.velocity.y);
        base.Update();
        if (player.facingGround)
        {
            player.statemachine.ChangeState(player.idlestate);
            
        }

        if(player.wallSlidetag)
        {
            player.statemachine.ChangeState(player.wallSlidestate);
          
        }
        if (Input.GetMouseButtonDown(0))
        {
            player.statemachine.ChangeState(player.jumpattstate);
        }

    }

    public override void Exit()
    {
        base.Exit();
        player.setvelocity(rigi.velocity.x, rigi.velocity.y);
    }
}
