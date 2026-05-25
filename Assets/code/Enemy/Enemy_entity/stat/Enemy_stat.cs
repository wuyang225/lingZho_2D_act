using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_stat : Entity_stat
{
    public override void Awake()
    {
        setUpname = "Enemy_setUp";
        base.Awake();
    }
}
