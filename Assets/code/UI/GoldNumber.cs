using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldNumber : BasePanel
{
    private static GoldNumber instance;
    public static GoldNumber Instance=>instance;

    public Text goldNumText;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
    public override void Init()
    {

    }
    public void ChangeGoldNumberUIText(float number)
    {
        goldNumText.text = number.ToString();
    }
}
