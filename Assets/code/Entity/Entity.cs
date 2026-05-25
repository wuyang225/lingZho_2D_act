using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public event Action onFliped;
    public  Animator anim { get; private set; }
    public Rigidbody2D rbody { get; private set; }
    public Collider2D col { get; private set; }
    public StateMachine statemachine { get; private set; }

    public Entity_stat stat { get; private set; }
    public Entity_Health health { get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }

    public Transform wallcheck1;
    public Transform wallcheck2;
    public Transform Groundcheck;
    public bool walltag = false;
    public bool wallSlidetag = false;

    public float facingDir = 1f;
    public float wallcheckDistance = 0.09f;
    public float GroundDistance = 0.18f;
    private bool facingright = true;
    public bool facingGround = true;
    public bool iskonck=false;
    private Vector2 konckback;
    public float konckbackDuration;
    public Coroutine konckbackcor;
    public Coroutine slowDownCor;

    // Start is called before the first frame update

    protected virtual void Awake()
    {
        anim = this.GetComponentInChildren<Animator>();
        rbody = this.GetComponent<Rigidbody2D>();
        col = this.GetComponent<Collider2D>();
        stat = this.GetComponent<Entity_stat>();
        health = this.GetComponent<Entity_Health>();
        statusHandler = this.GetComponent<Entity_StatusHandler>();
        statemachine = new StateMachine();
       
    }
    protected virtual void Start()
    {

    }

    public virtual void EntityDeath()
    {

    }
    public virtual void SlowDownEntity(float duration, float slowMultiplier)
    {
        // 关键逻辑：如果已有减速协程在运行，先终止它（防止协程叠加导致速度多次修改）
        if (slowDownCor != null)
            StopCoroutine(slowDownCor);

        // 启动新的减速协程，并将引用赋值给slowDownCo，便于后续终止
        slowDownCor = StartCoroutine(SlowDownEntityCo(duration, slowMultiplier));
    }
    public virtual IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        yield return null;
    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        statemachine.currentState.Update();
    }
    public void ReciveKonckbackcor(Vector2 konckback, float konckbackDuration)
    {
        if (konckbackcor != null)
            StopCoroutine(konckbackcor);
        iskonck = true;
        konckbackcor = StartCoroutine(onDamageChangeMaterial(konckback, konckbackDuration));
    }
    public virtual IEnumerator onDamageChangeMaterial(Vector2 konckback, float konckbackDuration)
    {
        rbody.velocity = konckback;
        yield return new WaitForSeconds(konckbackDuration);
        iskonck = false;
        if (!health.isdead)
            rbody.velocity = Vector2.zero;
    }
    protected  virtual void HandleCollisionDetection()
    {
        facingGround = Physics2D.Raycast(Groundcheck.position, Vector2.down, GroundDistance, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Raycast(Groundcheck.position, Vector2.down, GroundDistance, 1 << LayerMask.NameToLayer("wall"));
        walltag = Physics2D.Raycast(wallcheck1.position, Vector2.right * facingDir, wallcheckDistance, 1 << LayerMask.NameToLayer("wall"))
            || Physics2D.Raycast(wallcheck2.position, Vector2.right * facingDir, wallcheckDistance, 1 << LayerMask.NameToLayer("wall"));
        wallSlidetag = Physics2D.Raycast(wallcheck1.position, Vector2.right * facingDir, wallcheckDistance, 1 << LayerMask.NameToLayer("wall"))
            && Physics2D.Raycast(wallcheck2.position, Vector2.right * facingDir, wallcheckDistance, 1 << LayerMask.NameToLayer("wall"));
        //print(facingGround);
    }

    public virtual void setvelocity(float x, float y)
    {
        if (iskonck)
            return;
        HandleFilp(x);
        rbody.velocity = new Vector2(x, y);
    }

    public void HandleFilp(float x)
    {
        if (x > 0 && facingright == false)
        {
            fill();
        }
        else if (x < 0 && facingright)
        {
            fill();
        }
    }
    public virtual void fill()
    {
        facingDir = -1 * facingDir;
        facingright = !facingright;
        this.transform.rotation *= Quaternion.AngleAxis(180, Vector2.up);
        onFliped?.Invoke();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallcheck1.position, wallcheck1.position + new Vector3(wallcheckDistance * facingDir, 0));
        Gizmos.DrawLine(wallcheck2.position, wallcheck2.position + new Vector3(wallcheckDistance * facingDir, 0));
        Gizmos.DrawLine(Groundcheck.position, Groundcheck.position + new Vector3(0, -GroundDistance));
    }

    public void CallattstateTrigger()
    {
        statemachine.currentState.Callattstatetrigger();
    }

   
}
