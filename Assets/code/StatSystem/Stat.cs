using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatBuff> buffs=new List<StatBuff>();

    [SerializeField] private float finalValue;
    private bool changeValue = true;

    public void SetValue(float value)
    {
        baseValue = value;
        changeValue = true;
    }
    public float GetValue()
    {
        if(changeValue)
        {
            finalValue = GetFinalValue();
            changeValue = false;
        }
        return finalValue;
    }

    public void AddBuffs(float Value,string source)
    {
        buffs.Add(new StatBuff(Value, source));
        changeValue = true;
    }

    public void RemoveBuffs(string source)
    {
        buffs.RemoveAll(buffs => buffs.source == source);
    }

    public float GetFinalValue()
    {
        float finalValue = baseValue;
        foreach (var item in buffs)
        {
            finalValue += item.value;
        }
        return finalValue;
    }
}

[System.Serializable]
public class StatBuff
{
    public float value;
    public string source;

    public StatBuff(float va,string so)
    {
        value = va;
        source = so;
    }
}

