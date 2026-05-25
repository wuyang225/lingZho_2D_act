using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_battel : Enemy_State
{
    protected Transform player;

    protected float lastTimeAtt;
    public Enemy_battel(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
    }
    public override void Enter()
    {
        base.Enter();
        enemy.battlestimeDirction = enemy.battlestime;
        lastTimeAtt = Time.time;
        if (player == null)
        {
            player = enemy.getPlayerReference();
        }
        if (ShouldRetreat())
        {
            rigi.velocity = new Vector2(enemy.RetreatDistance.x*-DirectionToPlayer(), enemy.RetreatDistance.y);
            enemy.HandleFilp(DirectionToPlayer());
        }
    }
    public override void Update()
    {
        base.Update();
        if (enemy.PlayerDetection()==false&&enemy.battlestimeDirction<0)
        {
            enemy.battlestimeDirction = enemy.battlestime;
            enemy.statemachine.ChangeState(enemy.idlestate);
        }
        if (DistancetoPlayer()<enemy.attackistance&& enemy.PlayerDetection() == true&& CanArrack())
        {
            enemy.statemachine.ChangeState(enemy.attack1state);   
            
        }
        else
        {
                float xVeloicty=enemy.canChesePlayer? enemy.battlespeedmove * DirectionToPlayer():0.01f * DirectionToPlayer();
            if (!ShouldRetreat())
            {
            if (enemy.walltag==false)
            enemy.setvelocity(xVeloicty, rigi.velocity.y);
            }
           enemy.HandleFilp(DirectionToPlayer());
        }
    }

    protected bool CanArrack() => lastTimeAtt + enemy.attacCooldown < Time.time ? true : false;
    protected virtual bool ShouldRetreat() => DistancetoPlayer() < enemy.minimumattackistance;
    protected virtual float DistancetoPlayer()
    {
        if (player == null)
            return float.MaxValue;
        return Mathf.Abs( enemy.transform.position.x - player.position.x);
    }

    protected virtual float DirectionToPlayer()
    {
        if (player == null)
            return 0;
        return enemy.transform.position.x - player.position.x > 0 ? -1 : 1;
    }

}
