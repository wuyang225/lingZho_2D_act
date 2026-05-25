using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golden_Chest : MonoBehaviour, IDamgable
{
    protected Animator anim;
    protected Rigidbody2D rigi;
    protected Entity_VFX vfx;
    // 缓存宝箱的碰撞体（避免重复获取，提升性能）
    protected Collider2D chestCollider;

    [SerializeField] protected Vector2 konckback = new Vector2(0, 2);
    [SerializeField] protected GameObject GoldCoinobj;
    [SerializeField] protected float GoldCoinobjNumber;
    // 标记宝箱是否已打开（防止重复触发）
    protected bool isChestOpened = false;

    protected void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigi = GetComponentInChildren<Rigidbody2D>();
        vfx = GetComponent<Entity_VFX>();
        // 提前获取宝箱的碰撞体组件（BoxCollider2D/CircleCollider2D等）
        chestCollider = GetComponent<Collider2D>();
    }

    public virtual void TakeDamage(float Damage, float elementalDamage, ElementType elementType, Transform damageDealer)
    {
        // 防止宝箱被重复攻击触发多次逻辑
        if (isChestOpened) return;
        isChestOpened = true;

        vfx.PlayOnDamageVFX();
        rigi.angularVelocity = Random.Range(-200f, 200f);
        rigi.velocity = konckback;
        anim.SetBool("open", true);
        UIManager.Instance.ShowPanel<Skill_Enhancement_Plane>();

        // 生成金币
        CreateGoldCoin(GoldCoinobjNumber);

        StartCoroutine(DisableChestColliderAfterDelay(1f)); // 1秒后关闭（匹配动画时长）
    }

    protected void CreateGoldCoin(float GoldCoinobjNumber)
    {
        for (int i = 0; i < GoldCoinobjNumber; i++)
        {
            GameObject obj = Instantiate(GoldCoinobj, this.transform.position, Quaternion.identity);
            obj.GetComponent<Gold_Coin>().CreatGold_Coin(1);
        }
    }

    /// <summary>
    /// 关闭宝箱的碰撞检测（核心方法）
    /// </summary>
    protected void DisableChestCollider()
    {
        // 空引用检查：防止宝箱没有挂载Collider2D组件导致报错
        if (chestCollider != null)
        {
            // 禁用碰撞体，完全取消宝箱的所有碰撞检测（物理碰撞、射线检测等）
            chestCollider.enabled = false;
            rigi.simulated = false;
            Debug.Log("宝箱碰撞检测已关闭");
        }
    }


    // 新增协程方法
    protected IEnumerator DisableChestColliderAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        DisableChestCollider();
    }
}