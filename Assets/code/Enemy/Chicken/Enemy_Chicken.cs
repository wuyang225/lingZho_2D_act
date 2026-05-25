using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chicken : Enemy
{

    protected override void Awake()
{
    base.Awake();
    idlestate = new Enemy_idle(this, statemachine, "idle");
    movestate = new Enemy_move(this, statemachine, "move");
    attack1state = new Enemy_Attack1(this, statemachine, "attack1");
    battelstate = new Enemy_battel(this, statemachine, "battel");
    deadstate = new Enemy_Dead(this, statemachine, "dead");
    stunnedstate = new Enemy_stunned(this, statemachine, "stunned");
}
protected override void Start()
{
    base.Start();
    statemachine.Initialize(idlestate);
}
}
