using UnityEngine;

public class Enemy_Butterbattel : Enemy_battel
{
    private Enemy_Butterfly enemy_Butterfly;
    // 移动速度倍率（可在Inspector调整，控制靠近速度）
    [SerializeField] private float moveLerpSpeed = 2f;

    public Enemy_Butterbattel(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
        enemy_Butterfly = en as Enemy_Butterfly;
    }

    public override void Enter()
    {
        anim.SetBool(stateName, true);
        triggeratt = false;
        enemy.battlestimeDirction = enemy.battlestime;
        enemy.setvelocity(0, 0);
        lastTimeAtt = Time.time;

        // 空引用校验：防止enemy_Butterfly为空
        if (enemy_Butterfly != null && player == null)
        {
            player = enemy_Butterfly.getPlayerReferenceCircle();
        }

        // 实时更新朝向（始终面对玩家）
        enemy.HandleFilp(DirectionToPlayer());
    }

    public override void Update()
    {
        updataAnimationParameters();
        statetimer -= Time.deltaTime;
        // 计算与玩家的实时距离（XY轴全距离）
        // 检测是否丢失玩家视野，返回闲置状态
        if (!enemy_Butterfly.PlayerDetectionCircle() && enemy.battlestimeDirction < 0)
        {
            enemy_Butterfly.moveRangePos = enemy_Butterfly.transform.position;
            enemy.setvelocity(0, 0);
            enemy.battlestimeDirction = enemy.battlestime;
            enemy.statemachine.ChangeState(enemy.movestate);
        }
       
        if (enemy_Butterfly.transform.position.y - player.position.y < enemy_Butterfly.attackistance-1)
        {
            float yVeloicty = enemy.battlespeedmove;
            enemy.setvelocity(rigi.velocity.x, yVeloicty);
        } 
        else
        // 攻击条件满足：进入攻击状态
        if (enemy_Butterfly.PlayerDetectionBox() && enemy_Butterfly.PlayerDetectionCircle() && CanArrack())
        {
            enemy.setvelocity(0, 0);
            enemy.statemachine.ChangeState(enemy.attack1state);
        }
        else
        {
            float xVeloicty = enemy.canChesePlayer && enemy_Butterfly.PlayerDetectionBox() == false ? enemy.battlespeedmove * DirectionToPlayer() : 0.01f * DirectionToPlayer();
            float yVeloicty = enemy.canChesePlayer && enemy_Butterfly.transform.position.y - player.position.y > enemy_Butterfly.attackistance ?
                enemy.battlespeedmove * DirectionToPlayery() : 0.01f * DirectionToPlayery();
            enemy.setvelocity(xVeloicty, yVeloicty);

        }
    }

    protected override float DistancetoPlayer()
    {
        if (player == null)
            return float.MaxValue;
        // 保留XY轴全距离计算
        return Vector3.Distance(enemy_Butterfly.transform.position, player.position);
    }

    // 确保方向计算正确（仅X轴朝向）
    protected override float DirectionToPlayer()
    {
        if (player == null)
            return 0;
        return player.position.x > enemy_Butterfly.transform.position.x ? 1 : -1;
    }
    protected float DirectionToPlayery()
    {
        if (player == null)
            return 0;
        return player.position.y > enemy_Butterfly.transform.position.y ? 1 : -1;
    }
}