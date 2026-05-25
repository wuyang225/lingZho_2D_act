using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy;
    private Player_Health playerHealth;
    [SerializeField] private GameObject GoldCoinobj;
    [SerializeField] private float GoldCoinobjNumber;
    protected override void Awake()
    {
        base.Awake();
        enemy = this.GetComponent<Enemy>();
        playerHealth = GameObject.Find("Player").transform.GetComponent<Player_Health>();
        CanRegenHealth = false;
    }
    public override void TakeDamage(float Damage,float elementalDamage, ElementType elementType, Transform damageDealer)
    {
        vfx?.PlayOnDamageVFX();
        if (damageDealer.GetComponent<Player>()!= null&&damageDealer.GetComponent<Player>().skillAttact==false&&enemy.health.isdead==false)
        {
            enemy.TryEnterBattlestate(damageDealer);
        }
        vfx?.PlayOnDamageVFX();
        Vector2 konckvec = CalculateKnockback(Damage, damageDealer);
        float backDuration = CalculateDuration(Damage);
        if(enemy.statemachine.currentState!=enemy.stunnedstate)
        entity?.ReciveKonckbackcor(konckvec, backDuration);
        if (isdead)
        {
            return;
        }
        ReduceHealth(Damage + elementalDamage);

    }
    protected override void Update()
    {
        base.Update();
        if (this.transform.position.y < -100)
            Destroy(this.gameObject);
    }
    protected override void Die()
    {
        CreateGoldCoin(GoldCoinobjNumber);
        base.Die();
    }
    public override void ReduceHealth(float Damage)
    {
        base.ReduceHealth(Damage);
        playerHealth.BloodSuction(Damage);
        OtherMusic.Instance.ChangeAudioClip("hurt", false,2);
    }
    private void CreateGoldCoin(float GoldCoinobjNumber)
    {
        for (int i = 0; i < GoldCoinobjNumber; i++)
        {
        GameObject obj = Instantiate(GoldCoinobj, this.transform.position, Quaternion.identity);
        obj.GetComponent<Gold_Coin>().CreatGold_Coin(enemy.facingDir);

        }
    }
}
