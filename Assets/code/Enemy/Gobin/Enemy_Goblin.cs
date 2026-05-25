using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Goblin : Enemy
{
    public Enemy_GoblinAttack2 attack2state;
    public Enemy_Gobinbattel gobin_battelstate;
    protected override void Awake()
    {
        base.Awake();
        idlestate = new Enemy_idle(this, statemachine, "idle");
        movestate = new Enemy_move(this, statemachine, "move");
        attack1state = new Enemy_Attack1(this, statemachine, "attack1");
        attack2state = new Enemy_GoblinAttack2(this, statemachine, "attack2");
        gobin_battelstate = new Enemy_Gobinbattel(this, statemachine, "battel");
        deadstate = new Enemy_Dead(this, statemachine, "dead");
        stunnedstate = new Enemy_stunned(this, statemachine, "stunned");
        battelstate = gobin_battelstate;
    }
    protected override void Start()
    {
        base.Start();
        statemachine.Initialize(idlestate);
    }


}
