using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_dead : PlayerState
{
    private bool isdead;
    public Player_dead(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }
    public override void Enter()
    {
        PlayerMusic.Instance.ChangeAudioClip("Player_hurt", false, 0.8f);
        base.Enter();
        isdead = true;
        player.isdead = isdead;
        Input.ResetInputAxes();
        player.gameObject.layer = 4;
    }
    public override void Update()
    {
        base.Update();
        if (player.facingGround)
            rigi.simulated = false;
    }
}
