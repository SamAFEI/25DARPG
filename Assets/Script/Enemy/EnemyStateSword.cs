using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSword : EnemyState
{
    public EnemyStateSword(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.SetZeroVelocity();
        enemy.IsAttacking = true; 
    }
    public override void OnExit()
    {
        base.OnExit();
        enemy.IsAttacking = false;
        enemy.LastAttackTime = enemy.Data.attackResetTime + Random.Range(0.00f,0.30f);
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
