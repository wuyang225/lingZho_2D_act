using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAniamtionEvent : EntityAniamtionEvent
{
    private Enemy enemy;
    private Enemy_VFX vfx;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        vfx = GetComponentInParent<Enemy_VFX>();
    }

    private void EnableConter()
    {
        enemy.enableConter(true);
        vfx.enableAttackAlart(true);
    }
    private void EnableConterVFX()
    {
        vfx.enableAttackAlart(true);
    }
    private void DisableConter()
    {
        enemy.enableConter(false);
    }
    private void DisableConterVFX()
    {
        vfx.enableAttackAlart(false);
    }
    public void CurrentArrowTrigger()
    {
        enemy.enablecreateArrow(true);
    }
}
