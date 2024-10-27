using UnityEngine;

public class EnemyStateAttack3 : EnemyState
{
    public EnemyStateAttack3(Enemy _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.SetZeroVelocity();
        enemy.IsAttacking = true;
        enemy.AttackDamage = enemy.Data.attack3Damage;
        enemy.IsHeaveyAttack = enemy.Data.attack3IsHeavy;
        enemy.IsRockAttack = true;
    }
    public override void OnExit()
    {
        base.OnExit();
        enemy.SetZeroVelocity();
        enemy.IsAttacking = false;
        enemy.IsHeaveyAttack = false;
        enemy.IsRockAttack = false;
        enemy.LastAttack3Time = enemy.Data.attack3ResetTime + Random.Range(0.00f, 0.30f);
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
            enemy.Attack3Finish();
            return;
        }
    }
}
