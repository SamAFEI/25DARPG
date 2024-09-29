public class OrcBoss : Enemy
{
    protected override void Update()
    {
        LastSuperArmedTime = 10f;
        base.Update();
    }
    public override void AlertStateAction()
    {
        if (IsKeepawaying)
        {
            FSM.ChangeState(keepawayState);
            return;
        }
        if (CanAttack3)
        {
            AttackMoveMaxSpeed = 3f;
            FSM.ChangeState(attack3State);
            return;
        }
        if (CanAttack1)
        {
            FSM.ChangeState(attack1State);
            return;
        }
        if (CanChase)
        {
            FSM.ChangeState(chaseState);
            return;
        }
        if (CanCatch)
        {
            FSM.ChangeState(catchState);
            return;
        }
    }
    public override void Attack1Finish()
    {
        AttackMoveMaxSpeed = 3f;
        FSM.ChangeState(attack2State);
    }
}
