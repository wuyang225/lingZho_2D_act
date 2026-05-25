using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ButterDeadAir : Enemy_Dead
{
    public Enemy_ButterDeadAir(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
    }
    public override void Enter()
    {
        anim.SetBool(stateName, true);
        vfx = enemy.GetComponent<Enemy_VFX>();
        //切换到死亡层级
        enemy.gameObject.layer = 4;
        rigi.gravityScale = 3.5f;
        
    }

    public override void Update()
    {

        vfx.enableAttackAlart(false);
        if (enemy.facingGround)
        {
            enemy.setvelocity(0, rigi.velocity.y);
            anim.SetBool("deadGround",true);
            enemy.Invoke("DisableSelf", 3);
            
        }

    }
}
