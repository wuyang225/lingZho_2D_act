using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_jumpattack : PlayerState
{
    private bool triggerjump;
    public Player_jumpattack(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }

    public override void Enter()
    {
        base.Enter();
        triggerjump = true;
        player.setvelocity(player.jumpattmovevelocity.x*player.facingDir, player.jumpattmovevelocity.y);
        soundMusic.Instance.ChangeAudioClip("Player_Attack", false);

    }

    public override void Update()
    {
        base.Update();
        
        if(player.facingGround&&triggerjump)
        {
            triggerjump = false;
            anim.SetTrigger("jumpattckend");
            player.setvelocity(0, player.jumpattmovevelocity.y);
        }
        if(triggeratt)
        {
            player.statemachine.ChangeState(player.idlestate);
        }
       
    }
    public override void Exit()
    {
        base.Exit();
        soundMusic.Instance.SetSoundMusicIsOpen(false);
    }
}
