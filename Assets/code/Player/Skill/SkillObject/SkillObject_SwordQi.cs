using UnityEngine;

public class SkillObject_SwordQi : SkillObject_Base
{
    [SerializeField] private float swoedQiSpeed;
    [SerializeField] private float swoedDurtion;

    public void setSwoedQiValue(float speed, float durtion, float fac,DamageScaleData damageData)
    {
        swoedQiSpeed = speed;
        swoedDurtion = durtion;
        swoedDirection = fac;
        damageScaleData = damageData;
        Destroy(this.gameObject, swoedDurtion);
    }

    private void Update()
    {
        this.transform.Translate(Vector2.right * swoedDirection * Time.deltaTime * swoedQiSpeed, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
            return;
        CreateDamage(damageScaleData);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("wall"))
            Destroy(this.gameObject);
    }
}
