using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniamtionEvent : EntityAniamtionEvent
{
    private Player player;
    protected override void Awake()
    {
        base.Awake();
        player = this.GetComponentInParent<Player>();
    }

    private void ThrowSword() => player.skillManager.swordThrow.ThrowSword();
}
