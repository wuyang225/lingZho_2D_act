using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IConterable
{
    public Entity_Combat entity_Combat;

    public Enemy_idle idlestate;
    public Enemy_move movestate;
    public Enemy_Attack1 attack1state;
    public Enemy_battel battelstate;
    public Enemy_Dead deadstate;
    public Enemy_stunned stunnedstate;

    [Header("battle_details")]
    public float battlespeedmove = 3f;
    public float attackistance = 2f;
    public float attacCooldown = 1f;
    public bool canChesePlayer = true;
    public float battlestimeDirction = 3f;
    public float battlestime = 3f;
    public float minimumattackistance =1f;
    public Vector2 RetreatDistance;

    [Header("stunned_details")]
    public float stunnedDuration = .5f;
    public Vector2 stunnedDistance=new Vector2(7,7);
    public bool canstunned = false;

    [Header("movement_details")]
    public float moveSpeed = 1.4f;
    public float idletime = 2f;
    [Range(0,2)]
    public float movespeedAnmation = 1;

    [Header("Player detection")]
    [SerializeField] protected LayerMask whatisPlayer;
    [SerializeField] protected Transform playerCheckpos;
    [SerializeField] protected float playerCheckdistance=10;
    protected Transform player;

    [Header("Attack_details")]
    public bool createBullet=false;

    [Header("dead_details")]
    public float deadDuration = 0.25f;


    protected override void Awake()
    {
        base.Awake();
        entity_Combat = this.GetComponent<Entity_Combat>();
    }
    public void enablecreateArrow(bool enable) => createBullet = enable;
    public void enableConter(bool enable) => canstunned = enable;
    public virtual void TryEnterBattlestate(Transform player)
    {
        if (statemachine.currentState == battelstate)
            return;
        if (statemachine.currentState == attack1state)
            return;
        if (canstunned)
            return;
        this.player = player;
        statemachine.ChangeState(battelstate);
       
    }
    public override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        // 保存减速前的各项速度原始值，用于之后恢复
        float originalMoveSpeed = moveSpeed;
        float originalBattleSpeed = battlespeedmove;
        float originalAnimSpeed = anim.speed;

        // 计算实际的速度倍率（1 - 减速倍率）
        // 例如：slowMultiplier = 0.5 → speedMultiplier = 0.5，即速度变为原来的50%
        float speedMultiplier = 1 - slowMultiplier;

        // 应用减速：将移动速度、战斗移动速度、动画播放速度按倍率降低
        moveSpeed = moveSpeed * speedMultiplier;
        battlespeedmove = battlespeedmove * speedMultiplier;
        anim.speed = anim.speed * speedMultiplier;

        // 等待指定的持续时间，期间保持减速状态
        yield return new WaitForSeconds(duration);

        // 持续时间结束，恢复各项速度到原始值
        moveSpeed = originalMoveSpeed;
        battlespeedmove = originalBattleSpeed;
        anim.speed = originalAnimSpeed;

    }

    public virtual Transform getPlayerReference()
    {
        if(player==null)
        {
            player = PlayerDetection().transform;
        }
        return player;
    }
    public Transform GetPlayerPos()
    {
        return GameObject.Find("Player").transform;
    }
    protected override void Update()
    {
        base.Update();
        battlestimeDirction -= Time.deltaTime;
        if(playerCheckpos==null)
        playerCheckpos = this.transform;
    }
    public virtual RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheckpos.position, Vector2.right * facingDir, playerCheckdistance, whatisPlayer | 1<<LayerMask.NameToLayer("wall"));
        //if(hit.collider!=null)
        //print(LayerMask.LayerToName(hit.collider.gameObject.layer));
        if ( hit.collider == null||hit.collider.gameObject.layer != LayerMask.NameToLayer("Player") )
            return default;
        else return hit;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawRay(playerCheckpos.position+new Vector3(facingDir * attackistance,0,0), new Vector3(facingDir * (playerCheckdistance- attackistance), 0));
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(playerCheckpos.position, new Vector3(facingDir * attackistance, 0));
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(playerCheckpos.position, new Vector3(facingDir * minimumattackistance, 0));
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        statemachine.ChangeState(deadstate);
    }
    protected void HandlePlayerDeath()
    {
        statemachine.ChangeState(idlestate);
    }

    protected void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }
    protected void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }

    protected void DisableSelf()
    {
        Destroy(gameObject);
    }
    public void HandConterable()
    {
        if (canstunned)
        {
            statemachine.ChangeState(stunnedstate);
        }
    }
}
