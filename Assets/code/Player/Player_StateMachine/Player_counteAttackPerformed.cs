public class Player_counteAttackPerformed : PlayerState
{
    public Player_counteAttackPerformed(Player py, StateMachine sM, string sN) : base(py, sM, sN)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.isEntercount = false;
    }
    public override void Update()
    {
        base.Update();
        if (triggeratt)
        {
            player.statemachine.ChangeState(player.idlestate);
        }
    }
}
