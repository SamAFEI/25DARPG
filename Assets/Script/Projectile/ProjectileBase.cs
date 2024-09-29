using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public Rigidbody rb { get; private set; }
    public bool IsHeaveyAttack { get; set; }
    public bool IsAttackBeDefended { get; set; }
    public float AttackDamage;
    public float Speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Hit();
    }

    private void Hit()
    {
        Destroy(gameObject);
    }
}
