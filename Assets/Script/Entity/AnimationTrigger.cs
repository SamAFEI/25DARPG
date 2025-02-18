using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();

    private void AnimFinishTrigger()
    {
        entity.AnimationFinishTrigger();
    }
    private void PlayAttackFX(int _index)
    {
        entity.PlayAttackTrigger(_index);
    }
    private void DoSexHurt()
    {
        entity.SexHurt();
    }
    private void Shot(int _index)
    {
        entity.ShotProjectile(_index);
    }
    private void DamageTrigger(int _value)
    {
        entity.DamageTrigger(_value);
    }
    private void StunnedTrigger(int _value)
    {
        entity.StunnedTrigger(_value);
    }
    private void SuperArmedTrigger(int _value)
    {
        entity.SuperArmedTrigger(_value);
    }
    private void SetAttackMoveDirection()
    {
        entity.SetAttackMoveDirection();
    }
    private void MoveToTargetTrigger(int _value)
    {
        entity.MoveToTargetTrigger(_value);
    }
    private void IgnoreLayersTrigger(int _value)
    {
        entity.IgnoreLayersTrigger(_value);
    }
    private void CameraShakeTrigger()
    {
        entity.CameraShakeTrigger();
    }
    private void PlayAttackSFXTrigger(int _value)
    {
        entity.PlayAttackSFXTrigger(_value);
    }
    private void PlaySFXTrigger(int _value)
    {
        entity.PlaySFXTrigger(_value);
    }
    private void PlayVoiceTrigger(int _value)
    {
        entity.PlayVoiceTrigger(_value);
    }
    private void PlaySexSFXTrigger(int _value)
    {
        entity.PlaySexSFXTrigger(_value);
    }
    private void PlaySexVoiceTrigger(int _value)
    {
        entity.PlaySexVoiceTrigger(_value);
    }
    private void PlaySkillSFXTrigger(int _value)
    {
        entity.PlaySkillSFXTrigger(_value);
    }
    private void PlayHurtSFXTrigger()
    {
        entity.PlayHurtSFXTrigger();
    }
    private void SetAttackType(AttackTypeEnum type)
    {
        entity.SetAttackType(type);
    }
}
