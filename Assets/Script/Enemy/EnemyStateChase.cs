using UnityEngine;

public class EnemyStateChase : EnemyState
{
    public EnemyStateChase(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        animName = "Run";
        if (enemy.ChaseSpeed >= 1.5f)
        {
            animName = "Dash";
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!enemy.CanChase)
        {
            FSM.SetNextState(enemy.alertState);
            return;
        }
        if (stateTime < 0 && Random.Range(0, 100) > 80)
        {
            FSM.SetNextState(enemy.dashState);
            return;
        }
        else
        {
            stateTime = 5f;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        enemy.DoChase();
    }

}
