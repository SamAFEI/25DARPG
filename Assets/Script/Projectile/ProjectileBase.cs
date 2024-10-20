using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public Rigidbody rb { get; private set; }
    public bool IsHeaveyAttack { get; set; }
    public bool IsAttackBeDefended { get; set; }
    public GameObject hitFX;
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
        DoPlayHitFX();
        Destroy(gameObject);
    }

    public void DoPlayHitFX()
    {
        if (hitFX == null) { return; }
        GameObject obj = Instantiate(hitFX, transform.position, Quaternion.identity, transform);
        obj.SetActive(true);
        Destroy(obj, 0.3f);
    }
}
