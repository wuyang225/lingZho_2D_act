using UnityEngine;

public class Enemy_ArcherElfArrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private Entity_Combat combat;
    private bool haveParent = false;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private LayerMask whatIsWall;

    public bool CanBeCounted => true;

    public void SetupArrow(float xVelocity, Entity_Combat combat)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.velocity = new Vector2(xVelocity, 0);
        this.combat = combat;

        if (xVelocity > 0)
            this.transform.Rotate(0, 180, 0);
        Destroy(this.gameObject, 6);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if collided object is on a layer we want to damage
        if (((1 << collision.gameObject.layer) & whatIsTarget) != 0)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null && player.isEntercount && (player.facingDir > 0 && rb.velocity.x < 0 || player.facingDir < 0 && rb.velocity.x > 0))
            {
                player.statemachine.ChangeState(player.counteAttackPerformedstate);
                HandleCounter();
                combat = player.GetComponent<Player_Combat>();
                return;
            }
            combat.CreatSingleDamage(collision.transform);
            StunckIntoTarget(collision.transform);
        }
        if (((1 << collision.gameObject.layer) & whatIsWall) != 0)
        {
            // 碰到墙体的逻辑：销毁子弹/停止移动
            StunckIntoTarget(collision.transform);
        }
    }
    private void Update()
    {
        if (haveParent)
            if (this.transform.parent.GetComponent<Entity_Health>() != null)
                if (this.transform.parent.GetComponent<Entity_Health>().isdead)
                    Destroy(this.gameObject);
    }
    private void StunckIntoTarget(Transform target)
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        col.enabled = false;
        this.transform.SetParent(target);
        haveParent = true;
        Destroy(this.gameObject, 3);
    }
    public void HandleCounter()
    {
        // 反转箭矢飞行方向（X轴速度取反）
        rb.velocity = new Vector2(rb.velocity.x * -1, 0);
        // 旋转箭矢模型180度，使其朝向新的飞行方向
        transform.Rotate(0, 180, 0);

        // 将目标检测层切换为Enemy层（用于让箭反弹后攻击敌人）
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        whatIsTarget = whatIsTarget | (1 << enemyLayer);
    }

}