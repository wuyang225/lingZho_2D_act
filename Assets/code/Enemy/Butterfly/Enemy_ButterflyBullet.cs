using UnityEngine;

public class Enemy_ButterflyBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private Entity_Combat combat;
    private float movespeed;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private LayerMask whatIsWall;
    private Vector2 moveDirection; // 存储飞行方向

    public bool CanBeCounted => true;

    private void Awake()
    {
        // 提前缓存组件，避免空引用
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void SetupArrow(float speed, Transform target, Entity_Combat combat)
    {
        if (rb == null || col == null)
        {
            Awake(); // 兜底重新获取
        }

        this.combat = combat;
        movespeed = speed; // 移除*0.1f，避免速度过慢

        // 修复：计算2D朝向目标的方向（替代LookAt）
        moveDirection = (target.position - transform.position).normalized;

        // 修复：2D旋转（让子弹朝向飞行方向）
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Destroy(gameObject, 6f);
    }

    private void Update()
    {
        // 修复：沿计算好的方向移动（而非硬编码）
        transform.Translate(moveDirection * movespeed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if collided object is on a layer we want to damage
        if (((1 << collision.gameObject.layer) & whatIsTarget) != 0)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null && player.isEntercount && (player.facingDir > 0 && transform.position.x > player.transform.position.x
                || player.facingDir < 0 && transform.position.x < player.transform.position.x))
            {
                player.statemachine.ChangeState(player.counteAttackPerformedstate);
                Destroy(this.gameObject);
                return;
            }
            combat.CreatSingleDamage(collision.transform);
            Destroy(this.gameObject);
        }
        if (((1 << collision.gameObject.layer) & whatIsWall) != 0)
        {
            // 碰到墙体的逻辑：销毁子弹/停止移动
            Destroy(gameObject);
        }
    }
  

}