using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Archer_battel : Enemy_battel
{
    private Enemy_ArcherElf enemy_ArcherElf; // 弓箭手专属引用


    // 修复1：构造函数中强制转换Enemy为Enemy_ArcherElf
    public Enemy_Archer_battel(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
        // 安全转换：将基类Enemy转为子类Enemy_ArcherElf
        enemy_ArcherElf = en as Enemy_ArcherElf;
 
    }

    public override void Enter()
    {
        Debug.Log("jinr");
        anim.SetBool(stateName, true);
        triggeratt = false;
        enemy.battlestimeDirction = enemy.battlestime;
        lastTimeAtt = Time.time;
        if (player == null)
        {
            player = enemy.getPlayerReference();
        }

        // 修复2：添加空引用校验，执行撤退逻辑
        if (ShouldRetreat() && enemy_ArcherElf != null && enemy_ArcherElf.backFacingGround)
        {
            // 执行撤退移动（Y轴用0，避免垂直方向异常）
            rigi.velocity = new Vector2(enemy.RetreatDistance.x * -DirectionToPlayer(), 0);
            enemy.HandleFilp(DirectionToPlayer());
        }
    }

}