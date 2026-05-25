using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Prompt : MonoBehaviour
{
    [Header("引用关联")]
    [SerializeField] private GameObject BtnText; // 商店交互按钮文本
    [SerializeField] private float floatAmplitude = 0.01f; // 上下浮动距离
    [SerializeField] private float floatSpeed = 3f; // 浮动周期

    private Transform btnTextTransform;
    private Vector3 btnTextOriginalPos; // 文本初始位置
    private bool isPlayerInRange;

    private void Awake()
    {

        // 初始化交互文本
        if (BtnText != null)
        {
            btnTextTransform = BtnText.GetComponent<Transform>();
            btnTextOriginalPos = btnTextTransform.position;
            BtnText.SetActive(false);
        }
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

    /// <summary>
    /// 玩家进入触发器范围
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().isShop = true;
            isPlayerInRange = true;

        }
    }


    /// <summary>
    /// 玩家离开触发器范围
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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
        UIManager.Instance.ShowPanel<OperrationPanel>();
    }
}