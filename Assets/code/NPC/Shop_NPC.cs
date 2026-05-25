using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Shop_NPC : MonoBehaviour
{
    [Header("引用关联")]
    [SerializeField] private GameObject BtnText; // 商店交互按钮文本
    [SerializeField] private float floatAmplitude = 0.01f; // 上下浮动距离
    [SerializeField] private float floatSpeed = 3f; // 浮动周期
    [Header("NPC翻转配置")]
    [SerializeField] private Transform npcTransform; // NPC的Transform（可空，默认取自身）

    private Transform btnTextTransform;
    private Vector3 btnTextOriginalPos; // 文本初始位置
    private bool isPlayerInRange = false;
    private bool isFacingRight = true; // NPC初始朝向标记

    private void Awake()
    {
        // 初始化NPC Transform（默认取自身）
        if (npcTransform == null)
            npcTransform = transform;

        // 初始化交互文本
        if (BtnText != null)
        {
            btnTextTransform = BtnText.GetComponent<Transform>();
            btnTextOriginalPos = btnTextTransform.position;
            BtnText.SetActive(false);
        }

        // 记录NPC初始朝向（默认朝右）
        isFacingRight = npcTransform.localScale.x > 0;
    }

    private void Update()
    {
        if (isPlayerInRange)
        {
            BtnText.SetActive(true);
            HandleToolTipFloat();
            if (Input.GetKeyDown(KeyCode.E))
                OnClickOpenShop();
        }
    }

    /// <summary>
    /// 处理文本上下浮动
    /// </summary>
    private void HandleToolTipFloat()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        btnTextTransform.position = btnTextOriginalPos + new Vector3(0, yOffset, 0);
    }

    /// <summary>
    /// 翻转NPC朝向（180°）
    /// </summary>
    private void FlipNPC()
    {
        // 直接翻转X轴缩放实现180°转向
        isFacingRight = !isFacingRight;
        Vector3 newScale = npcTransform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * (isFacingRight ? 1 : -1);
        npcTransform.localScale = newScale;
    }

    /// <summary>
    /// 玩家进入触发器范围
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPlayerInRange)
        {
            print("玩家进入商店NPC范围");
            isPlayerInRange = true;
            other.GetComponent<Player>().isShop = true;


        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 检测玩家在NPC左侧/右侧，决定是否翻转
        bool isPlayerOnLeft = collision.transform.position.x > npcTransform.position.x;
        if ((isPlayerOnLeft && isFacingRight) || (!isPlayerOnLeft && !isFacingRight))
        {
            FlipNPC(); // 180°翻转朝向玩家
        }
        
    }

    /// <summary>
    /// 玩家离开触发器范围
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isPlayerInRange)
        {
            print("玩家离开商店NPC范围");
            isPlayerInRange = false;
            other.GetComponent<Player>().isShop = false;
            HideBtnText();

        }
    }

    /// <summary>
    /// 隐藏按钮文本
    /// </summary>
    private void HideBtnText()
    {
        if (BtnText == null) return;
        BtnText.SetActive(false);
    }

    /// <summary>
    /// 点击交互按钮时打开商店面板
    /// </summary>
    public void OnClickOpenShop()
    {
        UIManager.Instance.ShowPanel<ShopPanel>();
    }
}