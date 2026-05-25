using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dash : PlayerState
{
    private float dashdir;
    public Player_Dash(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }

    public override void Enter()
    {
        player.gameObject.layer = LayerMask.NameToLayer("Dash");
        soundMusic.Instance.ChangeAudioClip("Dash");
        rigi.gravityScale = 0;
        base.Enter();
        player.playerVfx.DoImageEcho(player.dashDuration);
        dashdir = Input.GetAxisRaw("Horizontal") != 0 ? Input.GetAxisRaw("Horizontal") : player.facingDir;
        statetimer = player.dashDuration;
    }

    public override void Update()
    {

        base.Update();
        if (dashdir < 1 && dashdir > -1)
            dashdir = player.facingDir;
         player.setvelocity(player.dashDistance * dashdir,0);
        //rigi.velocity = new Vector2(player.dashDistance * dashdir, 0);
       // Debug.Log(player.walltag);

        iswallstate();

        if (statetimer<0)
        {
           
            if(player.facingGround)
            player.statemachine.ChangeState(player.idlestate);
            else player.statemachine.ChangeState(player.jumpfallstate);
        }
        

    }
    private void iswallstate()
    {
        if (player.wallSlidetag && player.facingGround == false)
        {
            player.statemachine.ChangeState(player.wallSlidestate);
        }

    }

    public override void Exit()
    {
        rigi.gravityScale = 3.5f;
        base.Exit();
        player.gameObject.layer = LayerMask.NameToLayer("Player");
        player.setvelocity(0, 0);
        rigi.velocity = new Vector2(0, 0);
        soundMusic.Instance.SetSoundMusicIsOpen(false);
    }
}
