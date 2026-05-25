using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    public static event Action OnPlayerDeath;
    public static Player Instance { get; private set; }  // 新增：单例访问

    public Player_VFX playerVfx;
    public Player_SkillManager skillManager { get; private set; }


    #region 状态参数
    public Player_idle idlestate { get; private set; }
    public Player_move movestate { get; private set; }
    public Player_jump jumpstate { get; private set; }
    public Player_jumpfall jumpfallstate { get; private set; }
    public Player_wallSlide wallSlidestate { get; private set; }
    public Player_walljump walljumpstate { get; private set; }
    public Player_Dash dashstate { get; private set; }
    public Player_attack baseattstate { get; private set; }
    public Player_jumpattack jumpattstate { get; private set; }
    public Player_dead deadstate { get; private set; }
    public Player_counteAttack counteAttack { get; private set; }
    public Player_counteAttackPerformed counteAttackPerformedstate { get; private set; }
    public Player_SwordThrow swordThrowstate { get; private set; }
    #endregion


    public float goldCoinNumber;


    public Vector2 mousePosition;
    [Header("movement_details")]
    public float speedmove = 5f;
    public float jumpmove = 5f;
    public Vector2 walljumpDir;
    public float inAirmovespeed = .7f;
    public float inwallmovespeed = .3f;
    public int dashDistance = 20;
    public float dashDuration = .25f;

    [Header("Attack_details")]
    public float Attvelocitytimer = .2f;
    public float comborAttactesettimer = 0.5f;
    public Coroutine queuedAttco;
    public Vector2 jumpattmovevelocity;
    public Vector3 fallPos;
    public Vector2[] attmovevelocity;

    [Header("counteAttack_details")]
    public float counteAttacktimer = 0.5f;
    public float counttime = 0.1f;
    public bool isEntercount = false;

    public bool isdead = false;
    public bool skillAttact = false;
    public bool isShop = false;

    public Vector3 lastScencePos;
    public string lastScenceName;

    public float juampTime = 0.2f;
    protected override void Awake()
    {
        // ========== 新增：单例核心逻辑 ==========
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // ======================================

        base.Awake();
        UIManager.Instance.ShowPanel<Player_HeartBar>();
        UIManager.Instance.ShowPanel<Enter_AttributeButton>();
        UIManager.Instance.ShowPanel<SkillManagePanel>();
        UIManager.Instance.ShowPanel<GoldNumber>();
        UIManager.Instance.ShowPanel<SettingButton>();
        UIManager.Instance.ShowPanel<OpertionButton>();
        UpdateFallPos();

        skillManager = this.GetComponent<Player_SkillManager>();
        playerVfx = this.GetComponent<Player_VFX>();

        idlestate = new Player_idle(this, statemachine, "idle");
        movestate = new Player_move(this, statemachine, "idle");
        jumpstate = new Player_jump(this, statemachine, "jump");
        jumpfallstate = new Player_jumpfall(this, statemachine, "jump");
        wallSlidestate = new Player_wallSlide(this, statemachine, "wallslide");
        walljumpstate = new Player_walljump(this, statemachine, "jump");
        dashstate = new Player_Dash(this, statemachine, "dash");
        baseattstate = new Player_attack(this, statemachine, "baseattack");
        jumpattstate = new Player_jumpattack(this, statemachine, "jumpattck");
        deadstate = new Player_dead(this, statemachine, "dead");
        counteAttack = new Player_counteAttack(this, statemachine, "counteAttack");
        counteAttackPerformedstate = new Player_counteAttackPerformed(this, statemachine, "counteAttackPerformed");
        swordThrowstate = new Player_SwordThrow(this, statemachine, "playerThrowSword");
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    // ========== 新增：销毁时清理单例 ==========
    protected void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    // ==========================================

    protected override void Start()
    {
        base.Start();
        statemachine.Initialize(idlestate);
    }
    protected override void Update()
    {
        base.Update();
        mousePosition = Input.mousePosition;
        FallAbyss();
        if (facingGround == false)
            juampTime -= Time.deltaTime;
        else juampTime = 0.2f;


    }
    public void UpdateFallPos()
    {
        fallPos = this.transform.position;
        fallPos.x += -facingDir * 2;
        fallPos.y += 1;
    }
    public override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = speedmove;               // 基础移动速度
        float originalJumpForce = jumpmove;               // 跳跃力度
        float originalAnimSpeed = anim.speed;              // 动画播放速度
        Vector2 originalWallJump = walljumpDir;          // 墙跳力度
        Vector2 originalJumpAttack = jumpattmovevelocity;   // 跳跃攻击速度
        Vector2[] originalAttackVelocity = new Vector2[attmovevelocity.Length]; // 地面攻击速度数组
        Array.Copy(attmovevelocity, originalAttackVelocity, attmovevelocity.Length);

        float speedMultiplier = 1 - slowMultiplier;

        speedmove *= speedMultiplier;
        jumpmove *= speedMultiplier;
        anim.speed *= speedMultiplier;
        walljumpDir *= speedMultiplier;
        jumpattmovevelocity *= speedMultiplier;
        // 遍历减少攻击速度数组的每一项
        for (int i = 0; i < attmovevelocity.Length; i++)
        {
            attmovevelocity[i] = attmovevelocity[i] * speedMultiplier;
        }

        yield return new WaitForSeconds(duration);

        speedmove = originalMoveSpeed;
        jumpmove = originalJumpForce;
        anim.speed = originalAnimSpeed;
        walljumpDir = originalWallJump;
        jumpattmovevelocity = originalJumpAttack;

        // 遍历恢复攻击速度数组的每一项
        for (int i = 0; i < attmovevelocity.Length; i++)
        {
            attmovevelocity[i] = originalAttackVelocity[i];
        }
    }
    public void Enterattstatewithdelay()
    {
        if (queuedAttco != null)
            StopCoroutine(queuedAttco);
        queuedAttco = StartCoroutine(EnterattstateWithDelay());
    }
    public override void setvelocity(float x, float y)
    {
        base.setvelocity(x, y);
        anim.SetFloat("xvelocity", rbody.velocity.x);
    }
    private IEnumerator EnterattstateWithDelay()
    {
        yield return new WaitForEndOfFrame();
        statemachine.ChangeState(baseattstate);
    }
    public override void EntityDeath()
    {
        base.EntityDeath();
        statemachine.ChangeState(deadstate);
        Time.timeScale = 0.5f;
        Camera.main.transform.DOShakePosition(0.5f, 0.1f);
        Invoke(nameof(showDeadPanel), 0.5f);
        OnPlayerDeath?.Invoke();
    }
    private void FallAbyss()
    {
        if (fallPos.y-this.transform.position.y > 50)
            StartCoroutine(FallAbyssFade());

        if (fallPos.y - this.transform.position.y > 50)
        {
            health.ReduceHealth(health.entitiystat.GetMaxHealth() * 0.2f);
            this.transform.position = fallPos;
        }
    }
    private IEnumerator FallAbyssFade()
    {
        UIManager.Instance.ShowPanel<FadePanel>();
        yield return new WaitForSeconds(0.2f);
        if (health.currentHp > 0)
            UIManager.Instance.HidePanel<FadePanel>();
    }
    private void showDeadPanel()
    {
        UIManager.Instance.ShowPanel<DeadPanel>();

    }
    public void SetLastScenceMessage(string name, Vector3 pos)
    {
        lastScencePos = pos;
        lastScenceName = name;
    }
    protected override void HandleCollisionDetection()
    {
        Vector3 gPos = new Vector3(Groundcheck.position.x, Groundcheck.position.y - 1);
        facingGround = Physics2D.Raycast(gPos, Vector2.down, GroundDistance, 1 << LayerMask.NameToLayer("Ground")) ||
             Physics2D.Raycast(gPos, Vector2.down, GroundDistance, 1 << LayerMask.NameToLayer("wall"));
        walltag = Physics2D.Raycast(wallcheck1.position, Vector2.right * facingDir, wallcheckDistance, 1 << LayerMask.NameToLayer("wall"))
            || Physics2D.Raycast(wallcheck2.position, Vector2.right * facingDir, wallcheckDistance, 1 << LayerMask.NameToLayer("wall"));
        wallSlidetag = Physics2D.Raycast(wallcheck1.position, Vector2.right * facingDir, wallcheckDistance, 1 << LayerMask.NameToLayer("wall"))
            && Physics2D.Raycast(wallcheck2.position, Vector2.right * facingDir, wallcheckDistance, 1 << LayerMask.NameToLayer("wall"));
        //print(facingGround);
    }
    protected override void OnDrawGizmos()
    {
        Vector3 gPos = new Vector3(Groundcheck.position.x, Groundcheck.position.y - 1);
        Gizmos.DrawLine(wallcheck1.position, wallcheck1.position + new Vector3(wallcheckDistance * facingDir, 0));
        Gizmos.DrawLine(wallcheck2.position, wallcheck2.position + new Vector3(wallcheckDistance * facingDir, 0));
        Gizmos.DrawLine(gPos, gPos + new Vector3(0, -GroundDistance));
    }

}
