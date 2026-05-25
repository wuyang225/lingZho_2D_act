using UnityEngine;
using Cinemachine;
using System.Collections;

public class AirWall : MonoBehaviour
{
    // 数组存储所有Collider2D（自身+子物体）
    private Collider2D[] allColliders;

    [Header("相机设置")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // 拖入你的CM vcam1
    [SerializeField] private Transform bossTransform; // 拖入Boss的Transform
    [SerializeField] private float switchBackDelay = 1f; // 切回玩家的延迟时间（1秒）
    [SerializeField] private float cameraSpeedScale = 0.6f; // 相机移动速度缩放（60%）
    [SerializeField] private Vector2 bossOffset = new Vector2(0, -1f); // Boss位置的偏移（向下1单位，可调整）

    private Transform originalFollowTarget; // 存储原来的Follow目标（玩家）
    private float originalDamping; // 存储相机原始阻尼值（控制移动速度）
    private Vector3 originalFollowOffset; // 存储相机原始偏移值
    private bool isfrist; // 存储相机原始偏移值

    private void Awake()
    {
        // 获取自身+所有子物体的Collider2D（包括非激活的）
        allColliders = GetComponentsInChildren<Collider2D>(true);
        // 自动查找虚拟相机（兼容不同层级结构）
        if (virtualCamera == null)
        {
            virtualCamera = Camera.main?.transform.parent?.GetComponentInChildren<CinemachineVirtualCamera>();
            if (virtualCamera == null)
            {
                virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            }
        }
        isfrist = true;

        // 空引用校验
        if (allColliders == null || allColliders.Length == 0)
        {
            Debug.LogError("AirWall 及其子物体未找到任何 Collider2D 组件！", this);
        }

        // 缓存相机初始配置
        if (virtualCamera != null)
        {
            originalFollowTarget = virtualCamera.Follow;
            // 获取相机的Body组件（控制跟随速度和偏移）
            var vcamBody = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (vcamBody != null)
            {
                originalDamping = vcamBody.m_XDamping; // 缓存原始X轴阻尼
                originalFollowOffset = vcamBody.m_TrackedObjectOffset; // 缓存原始偏移
            }
        }
    }

    public void ActivateWall()
    {
        gameObject.layer = LayerMask.NameToLayer("AirWall");

        // 遍历所有Collider2D，关闭触发器
        foreach (var collider in allColliders)
        {
            if (collider != null)
            {
                collider.isTrigger = false;
                collider.gameObject.layer = LayerMask.NameToLayer("AirWall");
            }
        }
    }

    public void DeactivateWall()
    {
        gameObject.layer = LayerMask.NameToLayer("None");

        // 遍历所有Collider2D，开启触发器
        foreach (var collider in allColliders)
        {
            if (collider != null)
            {
                collider.isTrigger = true;
                collider.gameObject.layer = LayerMask.NameToLayer("None");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int playerLayer2 = LayerMask.NameToLayer("Dash");
        if (collision.gameObject.layer == playerLayer|| collision.gameObject.layer == playerLayer2)
        {
            DeactivateWall();
            if(isfrist)
            {
                isfrist = false;
            StartCoroutine(SwitchCameraFollowCoroutine()); // 启动相机切换协程
            }
        }
    }

    // 相机切换协程：先减速→切到Boss（带偏移）→等待→切回玩家→恢复速度和偏移
    private IEnumerator SwitchCameraFollowCoroutine()
    {
        if (virtualCamera == null || bossTransform == null || originalFollowTarget == null)
        {
            Debug.LogError("VirtualCamera/BossTransform/原始跟随目标未赋值！", this);
            yield break;
        }

        // 1. 获取相机Body组件（控制跟随速度和偏移）
        var vcamBody = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (vcamBody == null)
        {
            Debug.LogError("虚拟相机缺少 FramingTransposer 组件！", this);
            yield break;
        }

        // 2. 减慢相机移动速度（缩放阻尼值）
        float slowedDamping = originalDamping / cameraSpeedScale;
        vcamBody.m_XDamping = slowedDamping;
        vcamBody.m_YDamping = slowedDamping;

        // 3. 设置Boss位置的偏移（向下移动）
        vcamBody.m_TrackedObjectOffset = new Vector3(bossOffset.x, bossOffset.y, originalFollowOffset.z);

        // 4. 切换Follow到Boss
        virtualCamera.Follow = bossTransform;

        // 5. 等待指定延迟时间（1秒）
        yield return new WaitForSeconds(switchBackDelay);

        // 6. 切回原来的Follow目标（玩家）
        virtualCamera.Follow = originalFollowTarget;

        // 7. 恢复相机原始速度和偏移
        yield return new WaitForEndOfFrame();
        vcamBody.m_XDamping = originalDamping;
        vcamBody.m_YDamping = originalDamping;
        vcamBody.m_TrackedObjectOffset = originalFollowOffset;
    }

    //// 可选：离开触发器时关闭空气墙（补充逻辑）
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    int playerLayer = LayerMask.NameToLayer("Player");
    //    if (collision.gameObject.layer == playerLayer)
    //    {
    //        ActivateWall();
    //    }
    //}
}