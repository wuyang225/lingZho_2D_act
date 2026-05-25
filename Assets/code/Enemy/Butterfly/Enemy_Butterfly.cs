using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Butterfly : Enemy
{
    public Vector3 moveRangePos;
    public GameObject butterflyBullet;
    public Enemy_ButterflyBullet Bullet;
    public Enemy_Buttermove enemy_Buttermovestate;
    public Enemy_Butterbattel enemy_Butterbattel;
    public Enemy_ButterDeadAir enemy_ButterDeadAir;


    [SerializeField] private float bulletMoveSpeed = 20;
    [SerializeField] private Transform attpos;
    public float lastMoveYfac=1;
    protected override void Awake()
    {
        base.Awake();
        idlestate = new Enemy_idle(this, statemachine, "idle");
        enemy_Buttermovestate = new Enemy_Buttermove(this, statemachine, "move");
        attack1state = new Enemy_Attack1(this, statemachine, "attack1");
        enemy_Butterbattel = new Enemy_Butterbattel(this, statemachine, "battel");
        enemy_ButterDeadAir = new Enemy_ButterDeadAir(this, statemachine, "dead");
        stunnedstate = new Enemy_stunned(this, statemachine, "stunned");
        movestate = enemy_Buttermovestate;
        deadstate = enemy_ButterDeadAir;
        battelstate = enemy_Butterbattel;
        moveRangePos = Groundcheck.position;
    }
    protected override void Start()
    {
        base.Start();
        statemachine.Initialize(idlestate);
    }
    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(moveRangePos, new Vector3(GroundDistance, GroundDistance));
        Gizmos.DrawLine(Groundcheck.position, Groundcheck.position + new Vector3(0, -GroundDistance*0.09f));
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerCheckpos.position, playerCheckdistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(playerCheckpos.position, new Vector3(attackistance * 2, attackistance * 2));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wallcheck1.position, wallcheckDistance);

    }
  
    protected override void HandleCollisionDetection()
    {
        facingGround = Physics2D.Raycast(Groundcheck.position, Vector2.down, GroundDistance * 0.09f, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Raycast(Groundcheck.position, Vector2.down, GroundDistance * 0.09f, 1 << LayerMask.NameToLayer("wall"));
    }
    public Transform getPlayerReferenceCircle()
    {
        if (player == null)
        {
            player = PlayerDetectionCircle().transform;
        }
        return player;
    }
    public Collider2D PlayerDetectionCircle()
    {
        Collider2D hit = Physics2D.OverlapCircle(playerCheckpos.position,playerCheckdistance, whatisPlayer);
        //if(hit.collider!=null)
        //print(LayerMask.LayerToName(hit.collider.gameObject.layer));
        if (hit == null || hit.gameObject.layer != LayerMask.NameToLayer("Player"))
            return null;
        else
        {
            print("player");
            return hit;
        }

    }
    protected override void Update()
    {
        base.Update();
        CreateBullet();
    }

    private void CreateBullet()
    {
        if (createBullet)
        {
            print("s");
            createBullet = false;
            GameObject obj = Instantiate(butterflyBullet, attpos.position, Quaternion.identity);
            Bullet = obj.GetComponent<Enemy_ButterflyBullet>();
            Bullet.SetupArrow(bulletMoveSpeed * DirectionToPlayery(), player.transform, entity_Combat);
        }
    }
    public Collider2D WallDetectionCircle()
    {
        Collider2D hit = Physics2D.OverlapCircle(wallcheck1.position, wallcheckDistance, 1 << LayerMask.NameToLayer("wall"));
        //if(hit.collider!=null)
        //print(LayerMask.LayerToName(hit.collider.gameObject.layer));
        if (hit == null || hit.gameObject.layer != LayerMask.NameToLayer("wall"))
            return null;
        else
        {
            print("player");
            return hit;
        }

    }
    public Collider2D PlayerDetectionBox()
    {
        Collider2D hit = Physics2D.OverlapBox(playerCheckpos.position,new Vector2(attackistance*2, attackistance*2),0, whatisPlayer);
        //if(hit.collider!=null)
        //print(LayerMask.LayerToName(hit.collider.gameObject.layer));
        if (hit == null || hit.gameObject.layer != LayerMask.NameToLayer("Player"))
            return null;
        else
        {
            return hit;
        }

    }
    protected float DirectionToPlayery()
    {
        if (player == null)
            return 0;
        return player.position.y > transform.position.y ? -1 : 1;
    }
}
