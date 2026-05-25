public class Enemy_Dead : Enemy_State
{
    protected float deadDuration;
    protected Enemy_VFX vfx;
    public Enemy_Dead(Enemy en, StateMachine sM, string sN) : base(en, sM, sN)
    {
    }
    public override void Enter()
    {
        base.Enter();
        vfx = enemy.GetComponent<Enemy_VFX>();
        deadDuration = enemy.deadDuration;
        //切换到死亡层级
        enemy.gameObject.layer = 4;
        statetimer = deadDuration;
    }

    public override void Update()
    {
        base.Update();
        vfx.enableAttackAlart(false);
        if (statetimer < 0)
            enemy.Invoke("DisableSelf", 3);
    }

}
