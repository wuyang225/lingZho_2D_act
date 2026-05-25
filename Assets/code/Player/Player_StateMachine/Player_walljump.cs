using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_walljump : PlayerState
{
    public Player_walljump(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }
    public override void Enter()
    {
        base.Enter();
        OtherMusic.Instance.ChangeAudioClip("jump", false);
        player.setvelocity(player.walljumpDir.x*-player.facingDir, player.walljumpDir.y);
    }

    public override void Update()
    {
        base.Update();
        player.setvelocity(rigi.velocity.x, rigi.velocity.y);
        if (rigi.velocity.y < 0)
        {
            player.statemachine.ChangeState(player.jumpfallstate);
        }
        if (player.walltag)
        {
            player.statemachine.ChangeState(player.wallSlidestate);

        }
        if (Input.GetMouseButtonDown(0))
        {
            player.statemachine.ChangeState(player.jumpattstate);
        }
    }
}
