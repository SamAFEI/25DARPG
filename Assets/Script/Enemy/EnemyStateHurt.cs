using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateHurt : EnemyState
{
    public EnemyStateHurt(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.SetZeroVelocity();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!enemy.IsHurting)
        {
            FSM.SetNextState(enemy.idleState);
            return;
        }
    }
}
