using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCatch : EnemyState
{
    public EnemyStateCatch(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.SetZeroVelocity();
        enemy.IsAttacking = true;
        enemy.IsCatching = true;
    }
    public override void OnExit()
    {
        base.OnExit();
        enemy.IsAttacking = false;
        enemy.IsCatching = false;
        enemy.LastCatchTime = 5f + Random.Range(5f,10f);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isAnimFinish)
        {
            FSM.ChangeState(enemy.alertState);
            return;
        }
    }
}
