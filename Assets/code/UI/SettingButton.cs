using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : BasePanel
{
    public Button settingButton;
    public override void Init()
    {
        settingButton.onClick.AddListener(() =>
        {
            OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
            UIManager.Instance.ShowPanel<SettingPanel>();
        }
        );
    }

}
