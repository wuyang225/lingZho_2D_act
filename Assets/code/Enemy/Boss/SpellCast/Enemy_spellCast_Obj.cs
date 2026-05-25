using UnityEngine;

public class Enemy_spellCast_Obj : MonoBehaviour
{
    private Collider2D col;
    private Entity_Combat combat;
    private bool createDamage=false;
    private bool iscreateDamage=false;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private ElementType elementType=ElementType.Fire;
    [SerializeField] private float damageScale=1;

    public bool CanBeCounted => true;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public void nowCreatDamage(bool value)
    {
        createDamage = value;
    }
    public void SetupArrow(Entity_Combat combat,float bossMode)
    {
        if (col == null)
        {
            Awake(); // 兜底重新获取
        }
        if (bossMode == 2)
            damageScale *= 2;
        this.combat = combat;
        Destroy(gameObject, 3f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsTarget) != 0 && createDamage&& iscreateDamage==false)
        {
            combat.CreatSingleDamage(collision.transform, damageScale, elementType);
            iscreateDamage = true;
        }
    }
}
