using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageScaleData 
{
    public float elementDamageScale;
    public float elementDurationScale;
    public ElementType elementType;
    public float damage;

    public DamageScaleData()
    {
        elementDamageScale = 1;
        elementDurationScale = 1;
        elementType = ElementType.None;
        damage =     10;
    }
    public DamageScaleData(float scale,ElementType el,float da,float durScale)
    {
        elementDamageScale = scale;
        elementDurationScale = durScale;
        elementType = el;
        damage = da;
    }
}
