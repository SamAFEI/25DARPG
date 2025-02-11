using UnityEngine;

public class Orc2 : Enemy
{

    protected override void Update()
    {
        LastSuperArmedTime = 10f;
        base.Update();
    }

    public override void AlertStateAction()
    {
        if (IsAmbushDash)
        {
            FSM.SetNextState(dashState);
            IsAmbushDash = false;
            return;
        }
        if (CanCatch && Random.Range(0.00f, 100.00f) < 80f)
        {
            FSM.SetNextState(catchState);
            return;
        }
        if (IsKeepawaying)
        {
            FSM.SetNextState(keepawayState);
            return;
        }
        if (CanAttack3)
        {
            AttackMoveMaxSpeed = 3f;
            FSM.SetNextState(attack3State);
            return;
        }
        if (CanAttack1)
        {
            FSM.SetNextState(attack1State);
            return;
        }
        if (CanChase)
        {
            if (Random.Range(0, 100) > 80)
            {
                FSM.SetNextState(dashState);
                return;
            }
            FSM.SetNextState(chaseState);
            return;
        }
    }
    public override void Attack1Finish()
    {
        AttackMoveMaxSpeed = 10f;
        FSM.SetNextState(attack2State);
    }
}
