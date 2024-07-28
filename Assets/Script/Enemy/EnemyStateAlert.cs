public class EnemyStateAlert : EnemyState
{
    public EnemyStateAlert(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.SetZeroVelocity();
        stateTime = 0.1f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!enemy.IsAlerting)
        {
            FSM.ChangeState(enemy.idleState);
            return;
        }
        if (enemy.CanChase)
        {
            FSM.ChangeState(enemy.chaseState);
            return;
        }
        if (enemy.CanAttack && stateTime < 0f)
        {
            FSM.ChangeState(enemy.swordState);
            return;
        }
    }

}
