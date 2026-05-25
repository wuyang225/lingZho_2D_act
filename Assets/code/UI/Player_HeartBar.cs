using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HeartBar : BasePanel
{
    private Entity_Health playerHealth;

    [SerializeField] private Slider slider;
    [SerializeField] private Text hp_text;
    public override void Init()
    {
       
    }

    protected override void Update()
    {
        playerHealth = player.transform.GetComponent<Entity_Health>();
        base.Update();

        float currentHp = playerHealth.currentHp;
        float maxHp = playerHealth.entitiystat.GetMaxHealth();
        float hpRatio = currentHp / maxHp;

        slider.value = hpRatio;

        hp_text.text = $"{FormatNumber(currentHp)}/{maxHp:F0}";
    }

    /// <summary>
    /// 整数不显示小数，有小数则保留一位
    /// </summary>
    private string FormatNumber(float value)
    {
        if (Mathf.Approximately(value, Mathf.RoundToInt(value)))
            return Mathf.RoundToInt(value).ToString();
        else
            return value.ToString("F1");
    }


}
