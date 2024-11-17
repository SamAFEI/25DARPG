using UnityEngine;

public class EnemyStateAttack1 : EnemyState
{
    public EnemyStateAttack1(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.SetZeroVelocity();
        enemy.IsAttacking = true;
        enemy.IsHeaveyAttack = enemy.Data.attack1IsHeavy;
        enemy.AttackDamage = enemy.Data.attack1Damage;
    }
    public override void OnExit()
    {
        base.OnExit();
        enemy.SetZeroVelocity();
        enemy.IsAttacking = false;
        enemy.LastAttack1Time = enemy.Data.attack1ResetTime + Random.Range(0.00f, 0.30f);
        if (enemy.IsNoCDAttack){ enemy.LastAttack1Time = 0; }
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
            enemy.Attack1Finish();
            return;
        }
    }
}
