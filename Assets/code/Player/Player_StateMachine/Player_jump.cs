using UnityEngine;

public class Player_jump : PlayerState
{
    public Player_jump(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.setvelocity(rigi.velocity.x, player.jumpmove);
        soundMusic.Instance.ChangeAudioClip("jump");
    }
    public override void Update()
    {
        player.setvelocity(player.speedmove * 0.7f * Input.GetAxisRaw("Horizontal"), rigi.velocity.y);
        base.Update();
        if (rigi.velocity.y <= 0 && player.statemachine.currentState != player.dashstate)
        {
            player.statemachine.ChangeState(player.jumpfallstate);
        }

        if (Input.GetMouseButtonDown(0))
        {
            player.statemachine.ChangeState(player.jumpattstate);
        }

    }
    public override void Exit()
    {
        base.Exit();
        soundMusic.Instance.SetSoundMusicIsOpen(false);
    }
}
