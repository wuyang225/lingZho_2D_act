using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    public SpriteRenderer sprite;
    private Entity entity;
    [SerializeField] private Material OnDamage_VFX_Material;
    [SerializeField] private float onDamage_Materialtime=0.1f;
    private Material startMaterial;
    private Coroutine onDamagecor;
    [SerializeField] private GameObject onHitVFX;
    [SerializeField] private GameObject onCritHitVFX;
    [SerializeField] public Color onHitVFXColor=Color.white;
    [Header("element Colors")]
    [SerializeField] private Color ChillColorVFX = Color.cyan;
    [SerializeField] private Color burnVFX = Color.red;
    [SerializeField] private Color LightVFX = Color.yellow;
    private Color defautColorVFX = Color.white;
    protected virtual void Awake()
    {
        sprite = this.GetComponentInChildren<SpriteRenderer>();
        entity = this.GetComponent<Entity>();
        startMaterial = sprite.material;
        defautColorVFX = onHitVFXColor;
    }
    // 对目标实体播放元素状态的视觉特效
    public void PlayOnStatusVfx(float duration, ElementType element)
    {
        // 判断元素类型是否为冰冻
        if (element == ElementType.Ice)
        {
            // 启动冰冻颜色闪烁的协程
            StartCoroutine(PlayStatusVfxCo(duration, ChillColorVFX));
        }
        if(element==ElementType.Fire)
        {
            // 启动燃烧颜色闪烁的协程
            StartCoroutine(PlayStatusVfxCo(duration, burnVFX));
        }
        if(element==ElementType.Lightning)
        {
            // 启动闪电效果的协程
            StartCoroutine(PlayStatusVfxCo(duration, LightVFX));
        }
    }
    // 停止该MonoBehaviour上运行的所有协程（包括颜色闪烁等Vfx协程）
    public void StopAllVfx()
    {
        StopAllCoroutines();
        sprite.color = Color.white;
        sprite.material = startMaterial;
    }
    private IEnumerator PlayStatusVfxCo(float duration, Color effectColor)
    {
        // 颜色切换的时间间隔（0.25秒切换一次）
        float tickInterval = .25f;
        // 记录已经过去的时间，用于控制循环
        float timeHasPassed = 0;

        // 生成两种亮度不同的颜色，用于闪烁效果
        // 亮色：基础颜色亮度提升20%
        Color lightColor = effectColor * 1.2f;
        // 暗色：基础颜色亮度降低20%
        Color darkColor = effectColor * .8f;

        // 切换标记，用于在两种颜色之间切换
        bool toggle = false;

        // 在特效持续时间内循环执行
        while (timeHasPassed < duration)
        {
            // 根据toggle标记选择颜色：false时用暗色，true时用亮色
            sprite.color = toggle ? lightColor : darkColor;
            // 反转toggle，下一次切换到另一种颜色
            toggle = !toggle;

            // 等待一个时间间隔
            yield return new WaitForSeconds(tickInterval);
            // 累加已过去的时间
            timeHasPassed = timeHasPassed + tickInterval;
        }

        // 特效结束后，将颜色恢复为白色（默认状态）
        sprite.color = Color.white;
    }
    public void CreateOnHitVFX(Transform pos,bool isCritHit)
    {
        GameObject Hitvfx =isCritHit==false? Instantiate<GameObject>(onHitVFX, pos.position, Quaternion.identity)
            : Instantiate<GameObject>(onCritHitVFX, pos.position, Quaternion.identity);
        if (entity.facingDir < 0&& isCritHit)
            Hitvfx.transform.Rotate(0,180,0);
        Hitvfx.GetComponentInChildren<SpriteRenderer>().color =onHitVFXColor;
    }

    public void updateOnhitColor(ElementType elementType)
    {
        if (elementType == ElementType.Ice)
            onHitVFXColor = ChillColorVFX;
        if (elementType == ElementType.Fire)
            onHitVFXColor = burnVFX;
        if (elementType == ElementType.Lightning)
            onHitVFXColor = LightVFX;
        if (elementType == ElementType.None)
            onHitVFXColor = defautColorVFX;
    }
    public void PlayOnDamageVFX()
    {
        if (onDamagecor != null)
            StopCoroutine(onDamagecor);
        onDamagecor = StartCoroutine("onDamageChangeMaterial");
    }
    public IEnumerator onDamageChangeMaterial()
    {
        sprite.material = OnDamage_VFX_Material;
        yield return new WaitForSeconds(onDamage_Materialtime);
        sprite.material = startMaterial;
    }
}
