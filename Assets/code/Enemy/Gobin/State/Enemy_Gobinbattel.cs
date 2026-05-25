using UnityEngine;

public class Enemy_Gobinbattel : Enemy_battel
{
    private Enemy_Goblin enemy_Goblin;
    public Enemy_Gobinbattel(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
        enemy_Goblin = en as Enemy_Goblin;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.attacCooldown = 0.5f;
    }
    public override void Update()
    {
        updataAnimationParameters();
        statetimer -= Time.deltaTime;
        if (enemy.PlayerDetection() == false && enemy.battlestimeDirction < 0 && enemy_Goblin.statemachine.currentState!= enemy_Goblin.attack2state)
        {
            enemy.battlestimeDirction = enemy.battlestime;
            enemy.statemachine.ChangeState(enemy.idlestate);
        }
        if (DistancetoPlayer() < enemy.attackistance && enemy.PlayerDetection() == true)
        {
            if (DistancetoPlayer() > 2.5f && CanArrack())
            {

                enemy_Goblin.statemachine.ChangeState(enemy_Goblin.attack2state);
            }
            else if (DistancetoPlayer() < 2.5f)
            {
                enemy_Goblin.statemachine.ChangeState(enemy_Goblin.attack1state);
            }

        }

        float xVeloicty = enemy.canChesePlayer ? enemy.battlespeedmove * DirectionToPlayer() : 0.01f * DirectionToPlayer();
        if (!ShouldRetreat())
        {
            if (enemy.walltag == false)
                enemy.setvelocity(xVeloicty, rigi.velocity.y);
        }
        enemy.HandleFilp(DirectionToPlayer());

    }
}
