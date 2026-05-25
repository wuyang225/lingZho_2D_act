using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_attack : PlayerState
{
    private float attvelocitytimer;

    private const int FirstcomboIndex = 1;
    private int comboIndex=1;
    private int comboLimit = 3;
    private float combotimer=0.3f;
    private bool comboattackQueued;
    private float attDir;
    private Vector2 attmoveVelocity;
    public Player_attack(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }
    public override void Enter()
    {
        base.Enter();
        SyncAttackSpeed();
        attDir =Input.GetAxisRaw("Horizontal")!=0 ?Input.GetAxisRaw("Horizontal") : player.facingDir;
        attvelocitytimer = player.Attvelocitytimer;
        changecomboIndex();
        comboattackQueued = false;
        anim.SetInteger("attackint", comboIndex);
        Generateattmovevelocity();
        soundMusic.Instance.ChangeAudioClip("Player_Attack",false);

    }
    public override void Update()
    {
        base.Update();
        Handleattvelocitytimer();
        if (Input.GetMouseButtonDown(0)&&comboIndex!=3)
        {
            comboattackQueued = true;
        }
       
        if (triggeratt)
        {
            if (comboattackQueued)
            {
                anim.SetBool("baseattack", false);
                player.Enterattstatewithdelay();
            }
            else player.statemachine.ChangeState(player.idlestate);
        }
    }

    public override void Exit()
    {
        comboIndex++;
        player.comborAttactesettimer = combotimer;
        base.Exit();
        soundMusic.Instance.SetSoundMusicIsOpen(false);
    }

    private void Handleattvelocitytimer()
    {
        attvelocitytimer -= Time.deltaTime;
        if(attvelocitytimer<0)
        {
            player.setvelocity(0, rigi.velocity.y);
        }
    }

    private void Generateattmovevelocity()
    {
        attmoveVelocity = player.attmovevelocity[comboIndex-1];
        player.setvelocity(attmoveVelocity.x * attDir, attmoveVelocity.y);
    }

    private void changecomboIndex()
    {
        if(comboIndex>comboLimit)
        {
            comboIndex = FirstcomboIndex;
        }
        if(player.comborAttactesettimer < 0)
        {
            comboIndex = FirstcomboIndex;
        }
    }
}
