using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [Header("地图块引用")]
    [SerializeField] private Transform grid;
    [SerializeField] private Transform gridRight;
    [SerializeField] private Transform gridLeft;

    [Header("玩家引用")]
    [SerializeField] private Transform player;

    [Header("配置")]
    [SerializeField] private float gridWidth = 71.96f; // 单个地图块宽度

    private List<Transform> gridList = new List<Transform>();
    public float lastPlayerX = 0;

    private void Start()
    {
        // 把三个地图块加入列表
        gridList.Add(gridLeft);
        gridList.Add(grid);
        gridList.Add(gridRight);


    }

    private void Update()
    {
        if (player == null) return;
        
        float playerX = player.position.x;
        CheckRightMove(playerX);

    }

    /// <summary>
    /// 检查是否需要向右滚动地图
    /// </summary>
    private void CheckRightMove(float playerX)
    {
        // 当玩家超过最右侧地图块的右边界阈值时，把最左边的地图块移到最右边
        if (playerX > lastPlayerX + gridWidth)
        {
            print("ijn");
            // 取出最左边的 Grid
            Transform leftmost = gridList[0];
            gridList.RemoveAt(0);

            // 计算新位置：当前最右边 Grid 的 X + 宽度
            float newX = playerX+ gridWidth;
            leftmost.position = new Vector3(newX, leftmost.position.y, leftmost.position.z);

            // 加入列表末尾（现在它是最右边的了）
            gridList.Add(leftmost);
            lastPlayerX = playerX;
        }
    }

}