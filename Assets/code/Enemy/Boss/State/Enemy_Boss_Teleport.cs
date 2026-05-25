using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy_Boss_Teleport : Enemy_State
{
    private Enemy_Boss enemy_Boss;
    public Enemy_Boss_Teleport(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
        enemy_Boss = en as Enemy_Boss;
    }
    public override void Enter()
    {
        base.Enter();
        enemy_Boss.gameObject.layer = 4;
    }
    public override void Update()
    {
        base.Update();
        if (triggeratt)
            enemy_Boss.statemachine.ChangeState(enemy_Boss.enemy_Boss_Attack);
    }
    public override void Exit()
    {
        base.Exit();
        if (enemy_Boss.bossMode == 1)
        {
            enemy_Boss.transform.position = enemy_Boss.FindTeleportPoint();
        }
        else
        {
            float dir= enemy_Boss.GetPlayerPos().GetComponent<Player>().facingDir;
            Vector3 playerBack = new Vector3(enemy_Boss.GetPlayerPos().position.x + -dir * 5, enemy_Boss.transform.position.y);

            RaycastHit2D hit = Physics2D.Raycast(playerBack, Vector2.down * 3, Mathf.Infinity, enemy_Boss.whatIsGround);
            if (hit.collider != null)
            enemy_Boss.transform.position = playerBack;
              else enemy_Boss.transform.position= new Vector3(enemy_Boss.GetPlayerPos().position.x + dir * 5, enemy_Boss.GetPlayerPos().position.y);
        }
        enemy_Boss.gameObject.layer = 11;
    }
}
