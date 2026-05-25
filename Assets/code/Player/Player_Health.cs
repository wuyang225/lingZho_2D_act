using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : Entity_Health
{
    public override void ReduceHealth(float Damage)
    {
        PlayerMusic.Instance.ChangeAudioClip("Player_hurt", false, 1.2f);
        base.ReduceHealth(Damage);
        
    }
    public void BloodSuction(float Damage)
    {
        if (CanRegenHealth && entitiystat.resourcesGroup.Health_Regen.GetValue() != 0)
        {
            float value = Damage * entitiystat.resourcesGroup.Health_Regen.GetValue() / 100f;
            IncreaseHealth(value);
        };
    }
}

