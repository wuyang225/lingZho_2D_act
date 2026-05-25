using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GoblinAttack2 : Enemy_State
{
    private float startAttRadius;
    private CapsuleCollider2D col;
    private float startcolRadius;
    public Enemy_GoblinAttack2(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
    }
    public override void Enter()
    {
        base.Enter();
        SyncAttackSpeed();
        startAttRadius = enemy.entity_Combat.targetCheckRadius;
        enemy.entity_Combat.targetCheckRadius = 3f;
        col = enemy.transform.GetComponent<CapsuleCollider2D>();
        startcolRadius = col.offset.x;
        col.offset = new Vector2(1*enemy.facingDir,col.offset.y);
    }
    public override void Update()
    {
        base.Update();
        if(enemy.canstunned)
        {
            enemy.setvelocity(6 * enemy.facingDir, rigi.velocity.y);
        }
        if (triggeratt && enemy.canstunned == false)
        {
            enemy.statemachine.ChangeState(enemy.battelstate);
        }
    }
    public override void Exit()
    {
        base.Exit();
        enemy.entity_Combat.targetCheckRadius = startAttRadius;
        col.offset = new Vector2(startcolRadius, col.offset.y);
    }
}
