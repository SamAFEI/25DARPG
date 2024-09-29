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
}
