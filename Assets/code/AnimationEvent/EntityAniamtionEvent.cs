using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAniamtionEvent : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat combat;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        combat = GetComponentInParent<Entity_Combat>();

    }

    public void baseattover()
    {
        entity.CallattstateTrigger();
    }
    public void CurrentStateTrigger()
    {
        combat.performAttack();
    }

}
