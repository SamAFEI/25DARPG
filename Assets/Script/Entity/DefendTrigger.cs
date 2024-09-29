using UnityEngine;

public class DefendTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Entity entity = other.GetComponentInParent<Entity>();
        if (entity != null && entity.IsAttacking && !entity.IsHeaveyAttack)
        {
            entity.IsAttackBeDefended = true;
        }
    }
}
