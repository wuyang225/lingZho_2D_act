using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal_Door : MonoBehaviour
{
    [Header("Target Scene")]
    public string targetSceneName;

    [Header("Fade Settings")]
    [SerializeField] protected float fadeDuration = 0.5f;

    [Header("Spawn Position")]
    [SerializeField] protected Vector3 spawnPosition;

    protected bool isTransitioning = false;

    protected Player player;

    public Collider2D range;

    protected virtual void Start()
    {
        Camera.main.transform.parent.GetComponent<Camera_player>().SetBoundingShape(range);
    }

    public void SetInit(string sceneName, Vector3 pos)
    {
        targetSceneName = sceneName;
        spawnPosition = pos;

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        StartCoroutine(TransitionScene());
    }

    /// <summary>
    /// 过场动画 + 异步加载场景
    /// </summary>
    protected virtual IEnumerator TransitionScene()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (isTransitioning) yield break;
        isTransitioning = true;
        player.lastScencePos = new Vector3(player.transform.position.x - 1, player.transform.position.y + 0.2f, 0);
        player.lastScenceName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // 1. 开始异步加载场景，禁止自动切换
        AsyncOperation ao = SceneManager.LoadSceneAsync(targetSceneName);
        ao.allowSceneActivation = false;

        // 2. 显示 FadePanel
        UIManager.Instance.ShowPanel<FadePanel>();

        // 3. 等待场景资源加载完成
        while (ao.progress < 0.9f)
        {
            yield return null;
        }

        // 7. 设置 Player 位置
        ao.completed += ((a) =>
        {
            
            SetPlayerSpawnPosition();

            MapRandomSeedManager.Instance.InitMap();
        });
        // 5. 手动触发场景切换
        ao.allowSceneActivation = true;

        // 6. 等待场景切换完毕
        yield return ao;

    }

    /// <summary>
    /// 场景切换后设置 Player 位置
    /// </summary>
    protected void SetPlayerSpawnPosition()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (player != null)
        {
            player.transform.position = spawnPosition;
            Debug.Log($"[Portal_Door] Player 已传送到 {spawnPosition}");
        }
        else
        {
            Debug.LogWarning("[Portal_Door] 未找到 Player 对象，请确保 Player 有 \"Player\" Tag");
        }
    }
}
