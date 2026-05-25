using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ArcherElf : Enemy
{
    [SerializeField] private Enemy_Archer_battel ArcherBattelstate;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Enemy_ArcherElfArrow archerElfArrow;
    [SerializeField] private float arrowMoveSpeed=20;
    [SerializeField] private Transform backGroundcheck;
    public bool backFacingGround=true;
    protected override void Awake()
    {
        base.Awake();
        idlestate = new Enemy_idle(this, statemachine, "idle");
        movestate = new Enemy_move(this, statemachine, "move");
        attack1state = new Enemy_Attack1(this, statemachine, "attack1");
        ArcherBattelstate = new Enemy_Archer_battel(this, statemachine, "battel");
        deadstate = new Enemy_Dead(this, statemachine, "dead");
        stunnedstate = new Enemy_stunned(this, statemachine, "stunned");
        battelstate = ArcherBattelstate;
            }
    protected override void Start()
    {
        base.Start();
        statemachine.Initialize(idlestate);
    }
    protected override void Update()
    {
        base.Update();
        CreateArrow();
    }

    private void CreateArrow()
    {
        if(createBullet)
        {
            print("s");
            createBullet = false;
            Vector3 vec = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
            GameObject obj= Instantiate(arrow, vec, Quaternion.identity);
            archerElfArrow = obj.GetComponent<Enemy_ArcherElfArrow>();
            archerElfArrow.SetupArrow(arrowMoveSpeed * facingDir, entity_Combat);
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(backGroundcheck.position, backGroundcheck.position + new Vector3(0, -GroundDistance));
    }
    protected override void HandleCollisionDetection()
    {
        base.HandleCollisionDetection();
        backFacingGround = Physics2D.Raycast(backGroundcheck.position, Vector2.down, GroundDistance, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Raycast(backGroundcheck.position, Vector2.down, GroundDistance, 1 << LayerMask.NameToLayer("wall"));
    }
}
