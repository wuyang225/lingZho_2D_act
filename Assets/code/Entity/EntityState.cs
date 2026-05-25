using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityState
{
    protected StateMachine stateMachine;
  
    protected string stateName;

    protected Animator anim;
    protected Rigidbody2D rigi;

    protected Entity_stat stat;
    protected bool triggeratt;
    protected float statetimer;
    public EntityState(StateMachine sM, string sN)
    {
        stateMachine = sM;
        stateName = sN;
    }

    public virtual void Enter()
    {
        //Debug.Log(stateName + "状态进入");
        anim.SetBool(stateName, true);
        triggeratt = false;
    }
    public virtual void Update()
    {
        updataAnimationParameters();
        statetimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        //Debug.Log(stateName + "状态退出");
        anim.SetBool(stateName, false);
    }


    public void Callattstatetrigger()
    {
        triggeratt = true;
    }

     public virtual void updataAnimationParameters()
    {
        
    }
    public void SyncAttackSpeed()
    {
        float attSpeed = stat.offenseGroup.attackSpeed.GetValue();
        anim.SetFloat("attackSpeedMultip", attSpeed);
    }
}
