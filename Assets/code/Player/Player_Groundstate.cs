using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Groundstate : PlayerState
{
    public Player_Groundstate(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }


    public override void Update()
    {
        base.Update();
        
        if (Input.GetKeyDown(KeyCode.Space)&&(player.facingGround||player.juampTime>=0))
        {
            player.statemachine.ChangeState(player.jumpstate);
        }
        if (Input.GetKeyDown(KeyCode.F) && skillsManager.swordQi.CanUseSkill())
        {
            player.statemachine.ChangeState(player.jumpattstate);
            player.skillManager.swordQi.CreateSwordQi();
        }
        if (Input.GetKeyDown(KeyCode.G) && skillsManager.swordThrow.CanUseSkill()&& skillsManager.swordThrow.IsHasSword() == false)
        {
            skillsManager.swordThrow.isSpin = false;
            player.statemachine.ChangeState(player.swordThrowstate);
        }
        if (Input.GetKeyDown(KeyCode.V) && skillsManager.swordThrow.CanUseSpinSkill()
            && skillsManager.swordThrow.IsHasSword() == false&&skillsManager.swordThrow.isSpinActive)
        {
            skillsManager.swordThrow.isSpin = true;
            skillsManager.swordThrow.cooldown *= 1.5f;
            player.statemachine.ChangeState(player.swordThrowstate);
        }

        if (rigi.velocity.y< 0.1 && player.facingGround == false&&player.iskonck==false&&player.statemachine.currentState!=player.dashstate&& player.juampTime <= 0)
        {
            player.statemachine.ChangeState(player.jumpfallstate);
            player.skillAttact = false;
        }

        if(Input.GetMouseButtonDown(0) && player.facingGround)
        {
            player.statemachine.ChangeState(player.baseattstate);
            player.skillAttact = false;
        }
        if(Input.GetMouseButtonDown(1) && player.facingGround)
        {
            player.statemachine.ChangeState(player.counteAttack);
            player.skillAttact = false;
        }
      
    }
}
