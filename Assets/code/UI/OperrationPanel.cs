using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OperrationPanel : BasePanel
{
    public Button Exitbtn;
    public override void Init()
    {
        Exitbtn.onClick.AddListener(() =>
        {
            OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
            UIManager.Instance.HidePanel<OperrationPanel>();
        });
    }
    public override void ShowMe()
    {
        base.ShowMe();
        DisablePlayerScript();
    }
    public override void HideMe(UnityAction callBack)
    {
        EnablePlayerScript();
        base.HideMe(callBack);
    }
}
