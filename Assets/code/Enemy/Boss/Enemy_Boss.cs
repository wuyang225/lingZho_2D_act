using System.Collections;
using UnityEngine;

public class Enemy_Boss : Enemy,IConterable
{
    public Enemy_Boss_Teleport enemy_Boss_Teleport;
    public Enemy_Boss_battel enemy_Boss_battel;
    public Enemy_Boss_Attack enemy_Boss_Attack;
    public Enemy_Boss_SpellCast enemy_Boss_SpellCast;
    public Enemy__Boss_Dead enemy__Boss_Dead ;
    [SerializeField] private Transform backGroundcheck;
    [SerializeField] private Transform wallcheck;
    public bool backFacingGround = true;

    public float maxBattelTime = 5;
    [Header("Reapert Teleport")]
    public LayerMask whatIsGround;
    [SerializeField] private BoxCollider2D arenaBounds;
    public Transform airWall;
    [SerializeField] private float offsetCenterY = 4f;
    public float chanceToTeleport = .70f;
    public float defaultTeleportChance;

    [Header("Reaper Spellcast")]
    [SerializeField] private GameObject spellCastPrefab;
    public float spellCastIntervalTime = 1f;
    [SerializeField] private float spellCastStateCooldown = 10;
    public float SpellcastTime = 4;
    public float isAttacktime = 3;
    private float baseSkillCooling = 6;
    public float skillCooling = 6;

    public float bossMode = 1;
    private bool firstEnterBattel=true;
    private bool firstHealthhuaf=true;
    protected override void Awake()
    {
        base.Awake();
        idlestate = new Enemy_idle(this, statemachine, "idle");
        movestate = new Enemy_move(this, statemachine, "move");
        stunnedstate = new Enemy_stunned(this, statemachine, "stunned");
        enemy_Boss_Teleport = new Enemy_Boss_Teleport(this, statemachine, "teleport");
        enemy_Boss_battel = new Enemy_Boss_battel(this, statemachine, "battel");
        enemy_Boss_Attack = new Enemy_Boss_Attack(this, statemachine, "attack1");
        enemy_Boss_SpellCast = new Enemy_Boss_SpellCast(this, statemachine, "spellCast");
        enemy__Boss_Dead = new Enemy__Boss_Dead(this, statemachine, "dead");
        deadstate = enemy__Boss_Dead;
        battelstate = enemy_Boss_battel;
    }
    protected override void Start()
    {
        base.Start();
        statemachine.Initialize(idlestate);
        defaultTeleportChance = chanceToTeleport;
        arenaBounds.transform.parent = null;
        airWall.parent = null;
        baseSkillCooling = skillCooling;
        bossMode = 1;
        fill();
    }
    public float getBasebaseSkillCooling() => bossMode == 1 ? baseSkillCooling : baseSkillCooling / 2;
    public void setBasebaseSkillCooling()
    {
        firstEnterBattel = false;
    }
    public bool getfirstEnterBattel()
    {
        return firstEnterBattel;
    }

        public bool ShouldTeleport()
    {
        if (bossMode == 1)
            chanceToTeleport = 1 - defaultTeleportChance;
        else chanceToTeleport = defaultTeleportChance;
        if (Random.value < chanceToTeleport)
        {
            return true;
        }

        return false;
    }
    public void CreatespellCastPrefab(Transform playerpos)
    {
        Player player = playerpos.GetComponent<Player>();
        float movePos= player.statemachine.currentState==player.movestate? player.facingDir * 4f:0;
        Vector3 pos = new Vector3(playerpos.position.x + movePos, playerpos.position.y+2);
        GameObject obj =  GameObject.Instantiate(spellCastPrefab, pos, Quaternion.identity);
        obj.transform.GetComponent<Enemy_spellCast_Obj>().SetupArrow(entity_Combat,bossMode);
    }
    public override void TryEnterBattlestate(Transform player)
    {
        if (statemachine.currentState == battelstate)
            return;
        if (statemachine.currentState == attack1state)
            return;
        if (statemachine.currentState == enemy_Boss_SpellCast)
            return;
        if (canstunned)
            return;
        this.player = player;
        statemachine.ChangeState(battelstate);
    }
    protected override void Update()
    {
        base.Update();

        // 二阶段触发条件：血量≤50% 且 未触发过
        if (health.currentHp <= health.entitiystat.GetMaxHealth() / 2 && firstHealthhuaf)
        {
            statemachine.ChangeState(enemy_Boss_Teleport);
            firstHealthhuaf = false;
            health.entitiystat.resourcesGroup.Health_Regen.SetValue(40);
            health.CanRegenHealth = true;
            UIManager.Instance.ShowPanel<FadePanel>();

            StartCoroutine(HideFadePanelAfterDelay(0.5f));

            bossMode = 2;
        }
    }

    // 协程：延迟指定时间后隐藏渐隐面板
    private IEnumerator HideFadePanelAfterDelay(float delay)
    {
        // 等待指定时长（0.5秒）
        yield return new WaitForSeconds(delay);

        // 隐藏渐隐面板
        UIManager.Instance.HidePanel<FadePanel>();

        // 可选：添加面板隐藏后的额外逻辑（如相机震动、音效等）
        // CameraShake.Instance.Shake(0.5f, 0.2f);
    }
    public override RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheckpos.position, Vector2.right * facingDir, playerCheckdistance, whatisPlayer | 1 << LayerMask.NameToLayer("wall"));
        //if(hit.collider!=null)
        //print(LayerMask.LayerToName(hit.collider.gameObject.layer));
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            hit = Physics2D.Raycast(wallcheck1.position, Vector2.right * facingDir, playerCheckdistance, whatisPlayer | 1 << LayerMask.NameToLayer("wall"));
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;
        else return hit;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(backGroundcheck.position, backGroundcheck.position + new Vector3(0, -GroundDistance));
        Gizmos.color = Color.green;
        Gizmos.DrawRay(wallcheck1.position + new Vector3(facingDir * attackistance, 0, 0), new Vector3(facingDir * (playerCheckdistance - attackistance), 0));
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallcheck1.position, new Vector3(facingDir * attackistance, 0));
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(wallcheck1.position, new Vector3(facingDir * minimumattackistance, 0));
        Gizmos.DrawLine(wallcheck.position, wallcheck.position + new Vector3(wallcheckDistance * facingDir, 0));
    }
    protected override void HandleCollisionDetection()
    {
        base.HandleCollisionDetection();
        backFacingGround = Physics2D.Raycast(backGroundcheck.position, Vector2.down, GroundDistance, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Raycast(backGroundcheck.position, Vector2.down, GroundDistance, 1 << LayerMask.NameToLayer("wall"));
        if(walltag==false)
        walltag = Physics2D.Raycast(wallcheck.position, Vector2.right * facingDir, wallcheckDistance, 1 << LayerMask.NameToLayer("wall"));
    }
    public Vector3 FindTeleportPoint()
    {
        int maxAttampts = 10;
        float bossWithColliderHalf = col.bounds.size.x / 2;

        for (int i = 0; i < maxAttampts; i++)
        {
            float randomX = Random.Range(arenaBounds.bounds.min.x + bossWithColliderHalf,
                                        arenaBounds.bounds.max.x - bossWithColliderHalf);

            Vector2 raycastPoint = new Vector2(randomX, arenaBounds.bounds.max.y);

            RaycastHit2D hit = Physics2D.Raycast(raycastPoint, Vector2.down*3, Mathf.Infinity, whatIsGround);

            if (hit.collider != null)
                return hit.point + new Vector2(0, offsetCenterY);
        }

        return transform.position;
    }
}
