using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : UiMinHealthBar
{
    
    // Start is called before the first frame update
    void Start()
    {
        if (entity == null)
            entity = GameObject.Find("Enemy_Boss").GetComponent<Entity>();
        if(entity.health.healthBar==null)
        {
            entity.health.healthBar = this.GetComponentInChildren<Slider>();
        }
    }
    protected override void OnEnable()
    {
    }
    protected override void OnDisable()
    {
    }
}
