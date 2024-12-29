using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public Rigidbody rb { get; private set; }
    public bool IsHeaveyAttack { get; set; }
    public GameObject hitFX;
    public AudioClip hitSFX;
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
        DoPlayHitSFX();
        DoPlayHitFX();
        Destroy(gameObject);
    }

    public void DoPlayHitFX()
    {
        if (hitFX == null) { return; }
        GameObject obj = Instantiate(hitFX, transform.position, Quaternion.identity);
        obj.SetActive(true);
        Destroy(obj, 0.3f);
    }

    public void DoPlayHitSFX()
    {
        if (hitSFX == null) { return; }
        AudioManager.PlayOnPoint(AudioManager.SFXSource, hitSFX, transform.position);
    }
}
