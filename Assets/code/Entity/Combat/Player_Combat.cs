using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : Entity_Combat
{
    private Player player;
    public override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }
    public bool isEnemyAttack()
    {
        attTrigger = Getcolldercombat();
        foreach (var item in attTrigger)
        {
            Enemy ene = item.GetComponent<Enemy>();
            if(ene!=null)
            if (ene.canstunned&&ene.statemachine.currentState!=ene.stunnedstate)
                return true;
        }
        
        return false;
    }
    public void EnterHandConterable()
    {
        attTrigger = Getcolldercombat();
        foreach (var item in attTrigger)
        {
            if (player.isEntercount && item.GetComponent<Entity_Health>()?.isdead == false)
                item.GetComponent<Enemy>().HandConterable();
        }
    }
    public override void CreatSkillDamage(DamageScaleData damage, Collider2D[] attTrigger)
    {
        player.skillAttact = true;
        base.CreatSkillDamage(damage, attTrigger);
    }

}
