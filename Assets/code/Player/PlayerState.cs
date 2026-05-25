using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState: EntityState
{
    protected Player player;
    protected Player_SkillManager skillsManager;
    public PlayerState(Player py,StateMachine sM,string sN):base(sM,sN)
    {
        player = py;
      
        anim = player.anim;
        rigi = player.rbody;
        stat = player.stat;
        skillsManager = player.skillManager;
    }


    public override void Update()
    {
        base.Update();
        player.comborAttactesettimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q)&& skillsManager.shard.CanUseSkill())
        {
            player.skillManager.shard.HandleShardTeleport();
        }
        if ((Input.GetKeyDown(KeyCode.G)|| Input.GetKeyDown(KeyCode.V)) && skillsManager.swordThrow.IsHasSword())
        {
            skillsManager.swordThrow.currentSword.shouldBack = true;
        }
        //Debug.Log(stateName + "状态更新");
        if (Input.GetKeyDown(KeyCode.LeftShift) && candash())
        {
            if (player.konckbackcor != null)
            {
                player.StopCoroutine(player.konckbackcor);
                player.iskonck = false;
            }
            stateMachine.ChangeState(player.dashstate);
            skillsManager.dash.SetSkillOnCooldown();
            skillsManager.dash.CreateShard();
        }
    }

    private bool candash()
    {
        if(skillsManager.dash.CanUseSkill()==false)
        {
            return false;
        }
        if(player.walltag&&player.facingGround==false)
        {
            return false;
        }
        if (player.isdead)
        {
            return false;
        }
        if (player.statemachine.currentState == player.dashstate)
            return false;
        return true;
    }
    public override void updataAnimationParameters()
    {
        base.updataAnimationParameters();
        anim.SetFloat("yvelocity", rigi.velocity.y);
    }
}
