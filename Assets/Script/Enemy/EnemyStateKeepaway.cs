public class EnemyStateKeepaway : EnemyState
{
    public EnemyStateKeepaway(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateTime = 1.5f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (stateTime < 0)
        {
            FSM.ChangeState(enemy.alertState);
        }
    }


    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        enemy.Keepaway();
    }
}
