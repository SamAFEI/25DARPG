public class EnemyStateAlert : EnemyState
{
    public EnemyStateAlert(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.SetZeroVelocity();
        stateTime = 0.2f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (stateTime > 0) { return; }
        if (enemy.IsKeepawaying)
        {
            FSM.ChangeState(enemy.keepawayState);
            return;
        }
        if (enemy.CanChase)
        {
            FSM.ChangeState(enemy.chaseState);
            return;
        }
        if (enemy.CanCatch)
        {
            FSM.ChangeState(enemy.catchState);
            return;
        }
        if (enemy.CanAttack)
        {
            FSM.ChangeState(enemy.swordState);
            return;
        }
    }

}
