using UnityEngine;

public class EnemyStateAttack2 : EnemyState
{
    public EnemyStateAttack2(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.SetZeroVelocity();
        enemy.IsAttacking = true;
        enemy.IsHeaveyAttack = enemy.Data.attack2IsHeavy;
        enemy.AttackDamage = enemy.Data.attack2Damage;
    }
    public override void OnExit()
    {
        base.OnExit();
        enemy.SetZeroVelocity();
        enemy.IsAttacking = false;
        enemy.LastAttack2Time = enemy.Data.attack2ResetTime + Random.Range(0.00f, 0.30f);
        if (enemy.IsNoCDAttack) { enemy.LastAttack2Time = 0; }
        enemy.IsNoCDAttack = false;
        enemy.DamageTrigger(0);
        enemy.StunnedTrigger(0);
        enemy.MoveToTargetTrigger(0);
        enemy.IgnoreLayersTrigger(0);
        enemy.SuperArmedTrigger(0);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isAnimFinish)
        {
            enemy.Attack2Finish();
            return;
        }
    }
}
