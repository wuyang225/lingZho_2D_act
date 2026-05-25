using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_wallSlide : PlayerState
{
    public Player_wallSlide(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }
 
    public override void Update()
    {
        base.Update();
        Handwalldown();
        if (player.wallSlidetag == false)
        {
            player.statemachine.ChangeState(player.movestate);
        }
        if (player.facingGround)
        {
            if(player.facingDir != Input.GetAxisRaw("Horizontal"))
            player.fill();
            player.statemachine.ChangeState(player.idlestate);
            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.statemachine.ChangeState(player.walljumpstate);
        }
    }
   
    private void Handwalldown()
    {
        if(Input.GetAxisRaw("Vertical")<0)
        {
            player.setvelocity(Input.GetAxisRaw("Horizontal"), rigi.velocity.y);
        }
        else
        {
            player.setvelocity(Input.GetAxisRaw("Horizontal"), rigi.velocity.y*player.inwallmovespeed);
        }
    }
}
