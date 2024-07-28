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
}
