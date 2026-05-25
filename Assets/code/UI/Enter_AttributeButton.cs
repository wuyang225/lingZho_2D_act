using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enter_AttributeButton : BasePanel
{
    [SerializeField] private Button enter;
    public override void Init()
    {
        enter.onClick.AddListener(() =>
        {
            OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
            UIManager.Instance.ShowPanel<AttributePanel>();
        });
    }

}
