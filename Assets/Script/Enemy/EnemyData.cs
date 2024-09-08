using UnityEngine;

[CreateAssetMenu(menuName = "AFEI/EnemyData")]
public class EnemyData : EntityData
{
    [Header("Attack")]
    public float attackResetTime = 1f;

    [Header("Check")]
    public float alertDistance = 5f;
}
