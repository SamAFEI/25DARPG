using UnityEngine;

public class EnemyStateAlert : EnemyState
{
    public EnemyStateAlert(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.SetZeroVelocity();
        stateTime = Random.Range(0.2f, 0.5f);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (stateTime > 0) { return; }
        enemy.AlertStateAction();
    }

}
