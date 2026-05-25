using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Event : MonoBehaviour
{
    private Enemy_spellCast_Obj enemy_SpellCast_Obj;

    protected void Awake()
    {
        enemy_SpellCast_Obj = GetComponentInParent<Enemy_spellCast_Obj>();
    }

    public void EnableDamageTrigger()
    {
        enemy_SpellCast_Obj.nowCreatDamage(true);
    }
    public void DisableDamageTrigger()
    {
        enemy_SpellCast_Obj.nowCreatDamage(false);
    }
}
