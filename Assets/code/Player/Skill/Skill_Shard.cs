using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float destoryTime = 2;
    [SerializeField] private GameObject obj ;
    [SerializeField] private GameObject dashObj ;
    [SerializeField] private bool isSwap=false ;

    public void CreateShard()
    {
        SetSkillOnCooldown();
        obj= Instantiate(shardPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<SkillObject_Shard>().SetUpShard(destoryTime, damageData);
    }
    public void CreateDashShard(DamageScaleData damage)
    {
        dashObj = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        dashObj.GetComponent<SkillObject_Shard>().SetUpShard(destoryTime, damage);
    }
    public override bool CanUseSkill()
    {
        if ((OnCooldown()&&obj==null)||isActive==false)
        {
            return false;
        }
        return true;
    }
    public void HandleShardTeleport()
    {
        if(obj==null)
        {
            CreateShard();
            isSwap = false;
        }
        else if(isSwap==false&&isUpGrade)
        {
            SwapPlayerAndShard();
            ReplyHP(playerHealth.entitiystat.GetMaxHealth()*0.2f);
            obj.GetComponent<SkillObject_Shard>().Explode();
            SetSkillOnCooldown();
            isSwap = true;
        }
    }

    private void SwapPlayerAndShard()
    {
        Vector3 shardPos = obj.transform.position;
        Vector3 playerpos = player.transform.position;

        player.transform.position = shardPos;
        obj.transform.position = playerpos;
    }
    private void ReplyHP(float value)
    {
        player.health.IncreaseHealth(value);
    }
}
