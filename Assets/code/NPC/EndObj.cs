using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndObj : MonoBehaviour
{
    // 可选：通过Inspector配置Player层，避免硬编码
    [SerializeField] private LayerMask playerLayer;

    private void Awake()
    {
        // 兜底：如果未配置playerLayer，自动获取Player层
        if (playerLayer == 0)
        {
            playerLayer = 1 << LayerMask.NameToLayer("Player");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 正确的图层判断方式（两种任选其一）
        // 方式1：通过LayerMask判断（推荐，支持多图层）
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            ShowWinPanel();
        }
    }

    // 抽离显示面板逻辑，便于维护
    private void ShowWinPanel()
    {
        // 安全校验：避免UIManager为空报错
        if (UIManager.Instance == null)
        {
            Debug.LogError("UIManager.Instance 为空，无法显示WinPanel！");
            return;
        }

        UIManager.Instance.ShowPanel<WinPanel>();
        Debug.Log("玩家触发终点，已显示胜利面板！");
    }
}