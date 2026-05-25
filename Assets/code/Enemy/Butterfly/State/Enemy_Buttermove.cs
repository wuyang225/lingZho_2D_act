using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Buttermove : Enemy_move
{
    private Enemy_Butterfly enemy_Butterfly;
    private Vector2 moveEndPos;
    private float firstEnterDurtion;
    private float movex;
    private float movey;
    private float moveSpeedy;
    public Enemy_Buttermove(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
        enemy_Butterfly = en as Enemy_Butterfly;
    }
    public override void Enter()
    {
        anim.SetBool(stateName, true);
        triggeratt = false;
        moveEndPos = GetMoveEndPos();
        moveSpeedy = Random.Range(enemy.moveSpeed - enemy.moveSpeed, enemy.moveSpeed + enemy.moveSpeed);
        firstEnterDurtion = 0.5f;
        if (enemy_Butterfly.transform.position.y > moveEndPos.y-5)
            moveSpeedy = -moveSpeedy;
    }
    public override void Update()
    {
        updataAnimationParameters();
        statetimer -= Time.deltaTime;
        firstEnterDurtion-=Time.deltaTime;
        if (enemy_Butterfly.PlayerDetectionCircle() == true && enemy.canstunned == false)
        {
            enemy.statemachine.ChangeState(enemy.battelstate);
            enemy.battlestimeDirction = enemy.battlestime;
        }
        if(enemy_Butterfly.WallDetectionCircle()==true)
        {
            enemy.setvelocity(0, 0);
            enemy.fill();
            enemy_Butterfly.lastMoveYfac = -enemy_Butterfly.lastMoveYfac;
            enemy.statemachine.ChangeState(enemy.idlestate);
        }
        enemy.setvelocity(enemy.facingDir * enemy.moveSpeed, moveSpeedy);
       
        movex=Mathf.Clamp(enemy_Butterfly.transform.position.x, moveEndPos.x - enemy_Butterfly.GroundDistance, moveEndPos.x);
        movey=Mathf.Clamp(enemy_Butterfly.transform.position.y, moveEndPos.y - enemy_Butterfly.GroundDistance, moveEndPos.y);
        if (enemy_Butterfly.transform.position.x != movex || movey != enemy_Butterfly.transform.position.y)
        {
            if(firstEnterDurtion <= 0)
            {
            enemy.setvelocity(0, 0);
            enemy.fill();
            enemy_Butterfly.lastMoveYfac = -enemy_Butterfly.lastMoveYfac;
            enemy.statemachine.ChangeState(enemy.idlestate);
            }
        }
    }
    private Vector2 GetMoveEndPos()
    {
        Vector2 vec;
        vec.x= enemy_Butterfly.moveRangePos.x+enemy_Butterfly.GroundDistance/2;
        vec.y= enemy_Butterfly.moveRangePos.y+enemy_Butterfly.GroundDistance/2;
        return vec;
    }
}
