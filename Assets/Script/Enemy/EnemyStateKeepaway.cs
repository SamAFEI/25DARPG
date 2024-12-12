public class EnemyStateKeepaway : EnemyState
{
    public EnemyStateKeepaway(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!enemy.IsKeepawaying)
        {
            FSM.SetNextState(enemy.alertState);
        }
    }


    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        enemy.Keepaway();
    }
}
