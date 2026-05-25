using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : Player_Groundstate
{
    
    public Player_move(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }
    public override void Enter()
    {
        base.Enter();
        soundMusic.Instance.ChangeAudioClip("run");
    }
    public override void Update()
    {
        base.Update();
        player.setvelocity(Input.GetAxisRaw("Horizontal") * player.speedmove,rigi.velocity.y);

        
        if (Input.GetAxisRaw("Horizontal") == 0||
            (player.walltag&& (Input.GetKeyDown(KeyCode.Space) == false & Input.GetMouseButtonDown(0) == false))) 
        {
            player.statemachine.ChangeState(player.idlestate);
        }
        if (anim.GetFloat("xvelocity") == 0)
        {
            player.statemachine.ChangeState(player.idlestate);
        }
     
    }
    public override void Exit()
    {
        base.Exit();
        player.setvelocity(0, rigi.velocity.y);
        soundMusic.Instance.SetSoundMusicIsOpen(false);
    }
}
