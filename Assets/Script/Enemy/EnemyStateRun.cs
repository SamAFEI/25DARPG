using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateRun : EnemyState
{
    public EnemyStateRun(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
