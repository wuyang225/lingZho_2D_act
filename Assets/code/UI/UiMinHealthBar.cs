using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMinHealthBar : MonoBehaviour
{
    public Entity entity;

    protected void Awake()
    {
        if(entity==null)
        entity = this.GetComponentInParent<Entity>();
    }
    protected virtual void OnEnable()
    {
        entity.onFliped += HandleFilp;
    }
    protected virtual void OnDisable()
    {
        entity.onFliped -= HandleFilp;
    }

    protected virtual void HandleFilp()
    {
        transform.rotation = Quaternion.identity;
    }
}
