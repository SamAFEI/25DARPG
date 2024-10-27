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
        enemy.LastCatchTime = Random.Range(3f,5f);
        enemy.SuperArmedTrigger(0);
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
