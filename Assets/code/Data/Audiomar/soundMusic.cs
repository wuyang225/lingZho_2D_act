using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundMusic : MonoBehaviour
{
    private static soundMusic instance;
    public static soundMusic Instance => instance;
    public AudioSource audios;

    private void Awake()
    {
        // ========== 核心修改：单例防重复逻辑 ==========
        // 如果已存在实例，且不是当前对象，销毁当前对象
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return; // 直接返回，不执行后续初始化
        }

        // 如果是第一个实例，正常初始化
        instance = this;
        audios = this.GetComponent<AudioSource>();
        audios.playOnAwake = true;
        audios.loop = true;
        GameObject.DontDestroyOnLoad(this.gameObject);
        updateMusicState();
    }

    public void SetVolume(float value)
    {
        // 空值防护（防止销毁后调用报错）
        if (audios == null) return;
        audios.volume = value;
    }

    internal void SetSoundMusicIsOpen(bool isOpen)
    {
        if (audios == null) return;
        audios.mute = !isOpen;
    }
    public void updateMusicState()
    {
        audios.volume = DataManage.Instance.LoadAudioData("Audio_Setup").SoundVolume;
        SetSoundMusicIsOpen(DataManage.Instance.LoadAudioData("Audio_Setup").soundEnable);
    }
    public void ChangeAudioClip(string name, bool isloop = true, float pich = 1)
    {
        if (audios == null) return;

        AudioClip newClip = Resources.Load<AudioClip>("Audio/sound music/" + name);
        // 空值防护：加载失败不替换
        if (newClip == null)
        {
            Debug.LogError($"音频加载失败：Audio/sound music/{name}");
            return;
        }
        audios.clip = newClip;
        audios.loop = isloop;
        audios.pitch = pich;
        audios.Play();
        SetSoundMusicIsOpen(true);
        updateMusicState();
    }
}