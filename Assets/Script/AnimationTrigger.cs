using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();

    private void AnimFinishTrigger()
    {
        entity.AnimationFinishTrigger();
    }
}
