using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_idle : Player_Groundstate
{
    public Player_idle(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.setvelocity(0,0);
        anim.SetFloat("xvelocity",0);
    }
    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space) == false)
        {
        player.setvelocity(0, 0);
        }
        anim.SetFloat("xvelocity",0);
        if ((Input.GetAxisRaw("Horizontal") != 0 && player.walltag == false))
            if (Input.GetKeyDown(KeyCode.Space) == false && Input.GetMouseButtonDown(0) == false)
            {
            player.statemachine.ChangeState(player.movestate);
        }
        if ((Input.GetAxisRaw("Horizontal") != player.facingDir && player.walltag)&& Input.GetAxisRaw("Horizontal") !=0)
            if (Input.GetKeyDown(KeyCode.Space) == false && Input.GetMouseButtonDown(0) == false)
            {
            player.statemachine.ChangeState(player.movestate);
        }
        
    }
    public override void Exit()
    {
        base.Exit();
        player.setvelocity(0, rigi.velocity.y);
    }
}
