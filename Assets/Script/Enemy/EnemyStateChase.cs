public class EnemyStateChase : EnemyState
{
    public EnemyStateChase(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!enemy.CanChase)
        {
            FSM.SetNextState(enemy.alertState);
            return;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        enemy.DoChase();
    }

}
