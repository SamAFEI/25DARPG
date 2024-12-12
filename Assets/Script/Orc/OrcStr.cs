using System.Collections;
using UnityEngine;

public class OrcStr : Enemy
{
    public bool IsEventAtcion;
    public override void AlertStateAction()
    {
        if (CanCatch && Random.Range(0.00f, 100.00f) < 50f)
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
        FSM.SetNextState(attack2State);
    }

    public override void Attack2Finish()
    {
        base.Attack2Finish();
        if (IsEventAtcion)
        {
            IsAlerting = true;
            DashAttack();
            IsEventAtcion = false;
        }
    }
    public override bool CheckDashAttack()
    {
        return CheckPlayerDistance(Data.attack3Distance)
            && GameManager.CanAttackPlayer();
    }
    public override void DashAttack()
    {
        FSM.SetNextState(attack3State);
        return;
    }

    public override IEnumerator DoBreakAndDash()
    {
        FacingToPlayer();
        IsNoCDAttack = true;
        IsEventAtcion = true;
        FSM.SetNextState(attack2State);
        yield return null;
    }
}
