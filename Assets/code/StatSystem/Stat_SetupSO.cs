    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class Stat_SetupSO:data
    {
        public float maxHealth = 100;
        public float healthRegen;

        public float attackSpeed = 1;
        public float damage = 10;
        public float critChance;
        public float critPower = 150;

        public DamageScaleData swordDamage=new DamageScaleData();
        public DamageScaleData shardDamage = new DamageScaleData();

        public float fireDamage=10;
        public float iceDamage=10;
        public float iceDuration=2;
        public float lightningDamage=10;

        public ElementType elementType = ElementType.None;
    }
