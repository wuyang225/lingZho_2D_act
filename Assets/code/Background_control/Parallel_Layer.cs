using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Parallel_Layer : MonoBehaviour
{
    [SerializeField]
    private Transform background;
    [SerializeField]
    private float parallaxMultiplier; // 原有X轴视差系数（完全保留）

    // 新增Y轴视差系数（独立控制）
    [SerializeField]
    private float parallaxMultiplierY = 0.3f;

    // ========== 新增1：指定“参考背景”（拖入另一个背景的Transform）==========
    [SerializeField]
    [Tooltip("X轴轮播时，Y轴跟随这个参考背景的Y轴")]
    private Transform referenceBackgroundY;

    // 原有X轴尺寸变量（完全保留）
    private float imageFUllwith;
    private float imageHuafwith;
    // 原有Y轴尺寸变量（已存在，直接复用）
    private float imageFUllhight;
    private float imageHuafhight;

    // 保留初始Y轴（作为未指定参考背景时的兜底）
    private float originalYPos;

    // ========== 原有方法（仅修改setLayerImagewith和Loopimage）==========
    public void setLayerImagewith()
    {
        imageFUllwith = this.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        imageHuafwith = imageFUllwith / 2;
        imageFUllhight = this.GetComponentInChildren<SpriteRenderer>().bounds.size.y;
        imageHuafhight = imageFUllhight / 2;

        // 记录初始Y轴（兜底用）
        if (background != null)
        {
            originalYPos = background.position.y;
        }
    }

    public void move(float distancetoMove)
    {
        background.position -= Vector3.right * (parallaxMultiplier * distancetoMove);
    }

    public void Loopimage(float camerrightEdge, float camerleftEdge)
    {
        float imagerightpos = background.position.x + imageHuafwith + 15;
        float imageleftpos = background.position.x - imageHuafwith - 15;

        // ========== 核心修改2：获取目标Y轴（优先用参考背景，没有则用初始Y轴）==========
        float targetY = referenceBackgroundY != null ? referenceBackgroundY.position.y : originalYPos;

        if (camerrightEdge > imagerightpos)
        {
            // X轴正常轮播，Y轴用目标Y轴
            background.position = new Vector3(
                background.position.x + imageFUllwith,
                targetY,
                background.position.z
            );
        }
        if (camerleftEdge < imageleftpos)
        {
            // X轴正常轮播，Y轴用目标Y轴
            background.position = new Vector3(
                background.position.x - imageFUllwith,
                targetY,
                background.position.z
            );
        }
    }


    public void MoveY(float distanceToMoveY)
    {
        background.position -= Vector3.up * (parallaxMultiplierY * distanceToMoveY);
    }

    /// <summary>
    /// 新增：Y轴循环（和X轴逻辑一致，仅换轴）
    /// </summary>
    public void LoopY(float camerTopEdge, float camerBottomEdge)
    {
        float imageTopPos = background.position.y + imageHuafhight + 15;
        float imageBottomPos = background.position.y - imageHuafhight - 15;

        if (camerTopEdge > imageTopPos)
        {
            background.position += new Vector3(0, imageFUllhight, 0);
        }
        if (camerBottomEdge < imageBottomPos)
        {
            background.position -= new Vector3(0, imageFUllhight, 0);
        }
    }
}