using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SwordQi : Skill_Base
{
    [SerializeField] private GameObject SwordQiObj;
    [SerializeField] private float swoedQiSpeed;
    [SerializeField] private float swoedDurtion;
    public Color swordColor=Color.white;
    public void CreateSwordQi()
    {
        SetSkillOnCooldown();
        GameObject obj = Instantiate(SwordQiObj, transform.position, Quaternion.identity);
        SpriteRenderer spr = obj.GetComponentInChildren<SpriteRenderer>();
        if (isUpGrade)
            damageData.elementType = ElementType.Ice;
        switch (damageData.elementType)
        {
            case ElementType.None:
                break;
            case ElementType.Fire:
                spr.color = Color.red;
                break;
            case ElementType.Ice:
                spr.color = Color.blue;
                break;
            case ElementType.Lightning:
                spr.color = Color.yellow;
                break;
            default:
                break;
        }
        swordColor = spr.color;
        if (player.facingDir == -1)
        {
            obj.transform.Rotate(Vector3.up,-180);
        ;}
        obj.GetComponent<SkillObject_SwordQi>().setSwoedQiValue(swoedQiSpeed, swoedDurtion, player.facingDir, damageData);
    }
    public void ChangeSwordQiColor()
    {
        GameObject obj = Instantiate(SwordQiObj, transform.position, Quaternion.identity);
        SpriteRenderer spr = obj.GetComponentInChildren<SpriteRenderer>();
        if (isUpGrade)
            damageData.elementType = ElementType.Ice;
        switch (damageData.elementType)
        {
            case ElementType.None:
                break;
            case ElementType.Fire:
                spr.color = Color.red;
                break;
            case ElementType.Ice:
                spr.color = Color.blue;
                break;
            case ElementType.Lightning:
                spr.color = Color.yellow;
                break;
            default:
                break;
        }
        swordColor = spr.color;
        Destroy(obj);
    }
}
