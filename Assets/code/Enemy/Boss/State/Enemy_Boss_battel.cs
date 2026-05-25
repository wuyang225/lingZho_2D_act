using UnityEngine;

public class Enemy_Boss_battel : Enemy_battel
{
    private Enemy_Boss enemy_Boss;
    private float isAttacktime;
    public Enemy_Boss_battel(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
        enemy_Boss = en as Enemy_Boss;
    }
    public override void Enter()
    {
        if( enemy_Boss.getfirstEnterBattel())
        {
            enemy_Boss.setBasebaseSkillCooling();
            UIManager.Instance.ShowPanel<BossHealth_BarPanel>();
            bkMusic.Instance.ChangeAudioClip("boss_bk_music");
            enemy_Boss.airWall.GetComponent<AirWall>().ActivateWall();
        }
        anim.SetBool(stateName, true);
        triggeratt = false;
        enemy.battlestimeDirction = enemy.battlestime;
        lastTimeAtt = Time.time;
        if (player == null)
        {
            player = enemy.getPlayerReference();
        }
        if (ShouldRetreat())
            enemy.HandleFilp(DirectionToPlayer());

        if (enemy_Boss.bossMode == 1)
        {
            statetimer = enemy_Boss.maxBattelTime;
            isAttacktime = enemy_Boss.isAttacktime;
        }
        else
        {
            statetimer = enemy_Boss.maxBattelTime/2;
            isAttacktime = enemy_Boss.isAttacktime/2;
        }
    }
    public override void Update()
    {
        if (ShouldRetreat() && enemy_Boss.backFacingGround)
        {
            rigi.velocity = new Vector2(enemy.RetreatDistance.x * -DirectionToPlayer(), enemy.RetreatDistance.y);
        }
        updataAnimationParameters();
        statetimer -= Time.deltaTime;
        isAttacktime -= Time.deltaTime;

        enemy_Boss.skillCooling -= Time.deltaTime;
        if (enemy.PlayerDetection() == false)
        {
            player = enemy_Boss.GetPlayerPos();
        }

        if (DistancetoPlayer() < enemy.attackistance && enemy.PlayerDetection() == true && CanArrack())
        {
            enemy.statemachine.ChangeState(enemy_Boss.enemy_Boss_Attack);

        }
        else
        {
            float xVeloicty = enemy.canChesePlayer ? enemy.battlespeedmove * DirectionToPlayer() : 0.01f * DirectionToPlayer();
            if (!ShouldRetreat())
            {
                if (enemy.walltag == false)
                    enemy.setvelocity(xVeloicty, rigi.velocity.y);
                if (enemy.facingGround == false)
                    enemy.setvelocity(0.01f * DirectionToPlayer(), rigi.velocity.y);
            }

            enemy.HandleFilp(DirectionToPlayer());
        }
        if (statetimer <= 0|| isAttacktime<=0 || enemy_Boss.skillCooling <= 0)
        {
            enemy.setvelocity(0, 0);
            if (enemy_Boss.ShouldTeleport())
                enemy_Boss.statemachine.ChangeState(enemy_Boss.enemy_Boss_Teleport);
            else enemy_Boss.statemachine.ChangeState(enemy_Boss.enemy_Boss_SpellCast);
            enemy_Boss.skillCooling = enemy_Boss.getBasebaseSkillCooling();
        }
    }

}
