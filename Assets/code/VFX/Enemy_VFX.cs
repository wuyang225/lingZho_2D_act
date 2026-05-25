using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_VFX : Entity_VFX
{
    [SerializeField] private GameObject point;
    protected override void Awake()
    {
        base.Awake();
    }
    public void enableAttackAlart(bool enable) => point.SetActive(enable);
}
