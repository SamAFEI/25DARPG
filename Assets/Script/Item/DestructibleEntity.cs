using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class DestructibleEntity : MonoBehaviour
{
    public MeshRenderer meshRenderer { get; private set; }
    public Collider collider { get; private set; }
    public Rigidbody rb { get; private set; }
    public GameObject broken { get; private set; }
    public List<MeshRenderer> breakenRenderers { get; private set; }
    public NavMeshObstacle obstacle { get; private set; }

    public bool isCrush { get; set; }

    public DestructibleType destructibleType = 0;
    public GameObject hitFX;
    public int hp = 10;
    public GameObject dropItem;
    public int droppedItemRate = 10;
    public float explosionForce = 200f;

    protected virtual void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        broken = transform.Find("Breaken").gameObject;
        breakenRenderers = broken.GetComponentsInChildren<MeshRenderer>().ToList();
        obstacle = GetComponent<NavMeshObstacle>();
    }

    protected virtual void Start()
    {
        broken.SetActive(false);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttack" || other.tag == "EnemyAttack")
        {
            Entity _entity = other.GetComponentInParent<Entity>();
            DoPlayHitFX(GetComponent<Collider>().ClosestPoint(other.transform.position));
            if (destructibleType == DestructibleType.rock)
            {
                AudioManager.PlayRockHitSFX(transform.position);
                if (_entity.AttackType != AttackTypeEnum.Earthshatter) { return; }
            }
            else
            {
                AudioManager.PlayWoodHitSFX(transform.position);
            }
            Hurt(_entity.AttackDamage);
        }
    }

    protected virtual void Hurt(float _damage)
    {
        if (isCrush) return;
        hp -= (int)_damage;
        if (hp <= 0)
        {
            Crush();
            return;
        }
        StartCoroutine(DoShark());
    }

    protected virtual void Crush()
    {
        isCrush = true;
        meshRenderer.enabled = false;
        //gameObject.layer = LayerMask.NameToLayer("Ignore");
        obstacle.enabled = false;
        //collider.isTrigger = true;
        collider.excludeLayers = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enemy") 
                | 1 << LayerMask.NameToLayer("Destructible") | 1 << LayerMask.NameToLayer("Interactable");
        broken.SetActive(true);
        List<UnfreezeFragment> fragments = broken.GetComponentsInChildren<UnfreezeFragment>().ToList();
        foreach (UnfreezeFragment fragment in fragments)
        {
            fragment.Unfreeze();
            fragment.GetComponent<Collider>().excludeLayers = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Interactable");
            fragment.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, 1f, 1f, ForceMode.Impulse);
        }
        if (destructibleType == DestructibleType.rock)
        {
            AudioManager.PlayRockBreakenSFX(transform.position);
        }
        else
        {
            AudioManager.PlayWoodBreakenSFX(transform.position);
        }
        StartCoroutine(Disappear());
    }

    protected virtual IEnumerator Disappear()
    {
        yield return new WaitForSeconds(0.5f); 
        float alpha = 1;
        MaterialToFadeMode();
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * 20;
            foreach (MeshRenderer renderer in breakenRenderers)
            {
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                foreach (Material material in renderer.materials)
                {
                    //material.SetFloat("_Surface", (float)SurfaceType.Transparent);
                    material.SetFloat("_Surface", 1);
                    Color color = material.color;
                    material.color = new Color(color.r, color.g, color.b, alpha);
                }
            }
            yield return new WaitForSeconds(.1f);
            if (alpha < 0.5f)
            {
                DoDropItem();
            }
            if (alpha <= 0.1f) alpha = 0f;
        }
        Destroy(gameObject);
    }

    private void MaterialToFadeMode()
    {
        foreach (MeshRenderer renderer in breakenRenderers)
        {
            foreach (Material material in renderer.materials)
            {
                HelperUtilities.MaterialToFadeMode(material);
            }
        }
    }

    private IEnumerator DoShark()
    {
        Vector3 original = transform.position;
        float time = 0.1f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            float offset = Mathf.Sin(Time.time * .1f) * .1f;
            transform.position = original + new Vector3(offset, 0, offset);
            yield return new WaitForSeconds(0.001f);
        }
        transform.position = original;
    }

    public void DoPlayHitFX(Vector3 _point)
    {
        if (hitFX == null) { return; }
        GameObject obj = Instantiate(hitFX, _point, Quaternion.identity, transform);
        obj.SetActive(true);
        Destroy(obj, 0.3f);
    }

    public void DoDropItem()
    {
        if (dropItem == null) return; 
        if (droppedItemRate <= Random.Range(0, 100)) return;
        Instantiate(dropItem, gameObject.transform.position + (Vector3.up), Quaternion.identity);
        dropItem = null;
    }
}

public enum DestructibleType
{
    wood, rock,
}
