using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill_Dash :Skill_Base
{
    public void CreateShard()
    {
        if(isUpGrade)
        skillManager.shard.CreateDashShard(damageData);
    }
}
