using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //整体控制淡入淡出的画布组 组件
    protected CanvasGroup canvasGroup;
    //淡入淡出的速度
    protected float alphaSpeed = 10;

    //是否开始显示
    protected bool isShow;

    //当自己淡出成功时 要执行的委托函数
    protected UnityAction hideCallBack;

    // 关键：缓存暂停前的时间缩放值，恢复时还原（兼容慢动作/加速场景）
    protected float _preTimeScale;
    // 缓存暂停前的固定时间步长，避免物理逻辑异常
    protected float _preFixedDeltaTime;
    protected Player player;
    protected bool isPlayerDisabled = false; // 标记Player是否被禁用
    protected virtual void Awake()
    {
        //一开始获取面板上 挂载的 组件 如果没有 我们通过代码 为它添加一个
        canvasGroup = this.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        // 查找Player（增加空引用校验，避免报错）
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<Player>();
        }
        else
        {
            Debug.LogWarning("WinPanel: 未找到Player对象！");
        }
    }
    protected void DisablePlayerScript()
    {
        if (player == null || isPlayerDisabled) return;

        player.rbody.velocity = Vector2.zero;
        player.statemachine.ChangeState(player.idlestate);
        // 禁用Player组件，使其所有Update/FixedUpdate等逻辑停止运行
        player.enabled = false;
        isPlayerDisabled = true;
        Debug.Log("Player脚本已禁用");
    }

    /// <summary>
    /// 恢复Player脚本
    /// </summary>
    protected void EnablePlayerScript()
    {
        if (player == null || !isPlayerDisabled) return;

        // 恢复Player组件
        player.enabled = true;
        isPlayerDisabled = false;
        Debug.Log("Player脚本已恢复");
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// 主要用于 初始化 按钮事件监听等等内容
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 显示自己时  做的事情
    /// </summary>
    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// 隐藏自己时 做的事情
    /// </summary>
    public virtual void HideMe( UnityAction callBack )
    {
        isShow = false;
        canvasGroup.alpha = 1;
        //记录 传入的 当淡出成功后会执行的函数
        hideCallBack = callBack;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //淡入
        if( isShow && canvasGroup.alpha != 1 )
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if( canvasGroup.alpha >= 1 )
                canvasGroup.alpha = 1;
        }
        //淡出
        else if( !isShow)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //应该让管理器 删除自己
                hideCallBack?.Invoke();
            }
        }
    }
   
}
