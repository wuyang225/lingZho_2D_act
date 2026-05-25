using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManage
{
    private static DataManage instance = new DataManage();
    public static DataManage Instance => instance;
    private DataManage() { }

    /// <summary>
    /// 保存数据到 persistentDataPath 的JSON文件
    /// </summary>
    public void SaveData<T>(T data, string fileName) where T : data, new()
    {
        string path = Path.Combine(Application.persistentDataPath, $"{fileName}.json");

        try
        {
            string jsonContent = JsonMapper.ToJson(data);
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, jsonContent, System.Text.Encoding.UTF8);
            Debug.Log($"✅ 数据已保存到：{path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ 保存 {fileName}.json 失败：{e.Message}");
        }
    }

    /// <summary>
    /// 优先读 persistentDataPath，没有再读 streamingAssetsPath
    /// </summary>
    public T LoadData<T>(string setUpname) where T : data, new()
    {
        T resultData = new T();
        string jsonContent = string.Empty;
        Debug.Log(Application.persistentDataPath);
        string persistentPath = Path.Combine(Application.persistentDataPath, $"{setUpname}.json");
        string streamingPath = Path.Combine(Application.streamingAssetsPath, $"{setUpname}.json");

        try
        {
            // 1. 优先读取 persistentDataPath（用户修改后的配置）
            if (File.Exists(persistentPath))
            {
                jsonContent = File.ReadAllText(persistentPath, System.Text.Encoding.UTF8);
                Debug.Log($"📂 从 persistentDataPath 加载：{setUpname}.json");
            }

            // 2. persistent 没有，读取 streamingAssetsPath（默认打包配置）
            else if (File.Exists(streamingPath))
            {
                jsonContent = File.ReadAllText(streamingPath, System.Text.Encoding.UTF8);
                Debug.Log($"📂 从 StreamingAssets 加载：{setUpname}.json");

                // 可选优化：从 streaming 加载后，自动保存一份到 persistent（方便后续修改）
                T tempData = JsonMapper.ToObject<T>(jsonContent);
                SaveData(tempData, setUpname);
            }

            // 3. 两个路径都没有，创建默认配置并保存到 persistentDataPath
            else
            {
                Debug.LogWarning($"⚠️ 两个路径都未找到 {setUpname}.json，将创建默认配置");
                resultData = new T();
                SaveData(resultData, setUpname);
                return resultData;
            }

            // 4. 反序列化 JSON 到对象
            if (!string.IsNullOrEmpty(jsonContent))
            {
                resultData = JsonMapper.ToObject<T>(jsonContent);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ 加载 {setUpname}.json 失败：{e.Message}，已使用默认配置");
            resultData = new T();
        }

        return resultData;
    }

    // 快捷加载方法
    public Stat_SetupSO LoadStatSetupData(string fileName = "setUpDefault")
    {
        return LoadData<Stat_SetupSO>(fileName);
    }

    public AudioData LoadAudioData(string fileName = "Audio_Setup")
    {
        return LoadData<AudioData>(fileName);
    }
}