using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamgable 
{
    void TakeDamage(float Damage,float elementalDamage,ElementType elementType, Transform damageDealer);
}
