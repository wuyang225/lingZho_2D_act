using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SoundPanel : BasePanel
{
    [Header("背景音乐设置")]
    [Tooltip("背景音乐音量滑块（bkText下的SoundSlider）")]
    [SerializeField] private Slider bkMusicSlider;
    [Tooltip("背景音乐开关（bkText下的SoundToggle）")]
    [SerializeField] private Toggle bkMusicToggle;

    [Header("音效设置")]
    [Tooltip("音效音量滑块（SoundText下的SoundSlider）")]
    [SerializeField] private Slider soundEffectSlider;
    [Tooltip("音效开关（SoundText下的SoundToggle）")]
    [SerializeField] private Toggle soundEffectToggle;
    [SerializeField] private Button Exitbtn;
    private AudioData audioData ;
    
    public override void Init()
    {
        audioData = DataManage.Instance.LoadAudioData();
        bkMusicSlider.value = audioData.bkMusicVolume;
        bkMusicToggle.isOn = audioData.bkEnable;
        soundEffectSlider.value = audioData.SoundVolume;
        soundEffectToggle.isOn = audioData.soundEnable;
        bkMusicSlider.onValueChanged.AddListener(changebkMusicVoluem);
        bkMusicToggle.onValueChanged.AddListener(changebkMusicEnable);
        soundEffectSlider.onValueChanged.AddListener(changesoundMusicVoluem);
        soundEffectToggle.onValueChanged.AddListener(changesoundMusicEnable);
        Exitbtn.onClick.AddListener(() =>
        {
            OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
            UIManager.Instance.HidePanel<SoundPanel>();
            EnablePlayerScript();
        });
    }

    private void changebkMusicVoluem(float value)
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        audioData.bkMusicVolume = value;
        DataManage.Instance.SaveData(audioData, "Audio_Setup");
        bkMusic.Instance.updateMusicState();
    }
    private void changebkMusicEnable(bool value)
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        audioData.bkEnable = value;
        DataManage.Instance.SaveData(audioData, "Audio_Setup");
        bkMusic.Instance.updateMusicState();
    }
    private void changesoundMusicEnable(bool value)
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        audioData.soundEnable = value;
        DataManage.Instance.SaveData(audioData, "Audio_Setup");
        PlayerMusic.Instance.updateMusicState();
        OtherMusic.Instance.updateMusicState();
        soundMusic.Instance.updateMusicState();
    }
    private void changesoundMusicVoluem(float value)
    {
        OtherMusic.Instance.ChangeAudioClip("Onbutton", false);
        audioData.SoundVolume = value;
        DataManage.Instance.SaveData(audioData, "Audio_Setup");
        PlayerMusic.Instance.updateMusicState();
        OtherMusic.Instance.updateMusicState();
        soundMusic.Instance.updateMusicState();
    }
  
    public override void ShowMe()
    {
        base.ShowMe();
        // 显示面板时立即禁用Player脚本
        DisablePlayerScript();
    }

    // 重写隐藏方法：隐藏面板时恢复Player脚本
    public override void HideMe(UnityAction callBack)
    {
        isShow = false;
        canvasGroup.alpha = 0;
        // 恢复Player脚本
        EnablePlayerScript();
        // 记录淡出成功后执行的函数
        hideCallBack = callBack;
    }
}