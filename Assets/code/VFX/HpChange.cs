using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class HpChange : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    private void Awake()
    {
        // 自动获取子物体上的 TMP 组件
        if (text == null)
            text = GetComponentInChildren<TextMeshPro>();
    }

    /// <summary>
    /// 初始化显示内容：正数=加血(绿)，负数=减血(红)
    /// </summary>
    public void Init(float amount)
    {
        if (amount == 0)
            Destroy(this.gameObject);
        string displayText = FormatNumber(amount);

        if (amount >= 0)
        {
            text.text = "+" + displayText;
            text.color = new Color(0.2f, 0.9f, 0.2f, 1f);
        }
        else
        {
            text.text = displayText;
            text.color = new Color(1f, 0.25f, 0.25f, 1f);
        }

        PlayAnimation();
    }

    /// <summary>
    /// 整数不显示小数，有小数则保留一位
    /// </summary>
    private string FormatNumber(float value)
    {
        // 判断是否有小数部分（四舍五入后再比较）
        float rounded = Mathf.Round(value * 10f) / 10f;
        if (Mathf.Approximately(value, Mathf.RoundToInt(value)))
            return Mathf.RoundToInt(value).ToString(); // 整数部分：显示为整数
        else
            return value.ToString("F1"); // 有小数：保留一位
    }

    private void PlayAnimation()
    {
        // 初始状态：小 + 完全不透明
        transform.localScale = Vector3.one * 0.3f;
        text.alpha = 1f;

        Sequence seq = DOTween.Sequence();

        // 第一阶段：快速放大到 1.3 倍
        seq.Append(transform.DOScale(1.3f, 0.2f).SetEase(Ease.OutBack));

        // 第二阶段：回弹到正常大小
        seq.Append(transform.DOScale(1f, 0.1f).SetEase(Ease.InOutSine));

        // 第三阶段：向上飘动 + 渐变消失
        seq.Append(transform.DOLocalMoveY(transform.localPosition.y + 1f, 0.3f)
                             .SetEase(Ease.OutCubic));
        seq.Join(text.DOFade(0f, 0.6f).SetEase(Ease.InQuad));

        // 动画结束后销毁
        seq.OnComplete(() => Destroy(gameObject));
    }
}
