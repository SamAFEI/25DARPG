using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.BaseShaderGUI;

public class DestructibleEntity : MonoBehaviour
{
    public MeshRenderer meshRenderer { get; private set; }
    public Collider collider { get; private set; }
    public Rigidbody rb { get; private set; }
    public GameObject broken { get; private set; }
    public List<MeshRenderer> breakenRenderers { get; private set; }

    public GameObject hitFX;

    public bool isCrush;
    public int hp = 10;
    public int droppedItemRate = 5;
    public float explosionForce = 200f;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        broken = transform.Find("Breaken").gameObject;
        breakenRenderers = broken.GetComponentsInChildren<MeshRenderer>().ToList();
    }

    private void Start()
    {
        broken.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttack")
        {
            DoPlayHitFX(GetComponent<Collider>().ClosestPoint(other.transform.position));
            Player _player = other.GetComponentInParent<Player>();
            Hurt(_player.AttackDamage);
        }
    }

    private void Hurt(float _damage)
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

    private void Crush()
    {
        isCrush = true;
        meshRenderer.enabled = false; 
        collider.excludeLayers = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Destructible");
        broken.SetActive(true);
        List<UnfreezeFragment> fragments = broken.GetComponentsInChildren<UnfreezeFragment>().ToList();
        foreach (UnfreezeFragment fragment in fragments)
        {
            fragment.Unfreeze();
            fragment.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, 1f, 1f, ForceMode.Impulse);
            fragment.GetComponent<Collider>().excludeLayers = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enemy");
        }
        StartCoroutine(Disappear());
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(1f); 
        float alpha = 1;
        MaterialToFadeMode();
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * 20;
            foreach (MeshRenderer renderer in breakenRenderers)
            {
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                foreach (Material material in renderer.materials)
                {
                    material.SetFloat("_Surface", (float)SurfaceType.Transparent);
                    Color color = material.color;
                    material.color = new Color(color.r, color.g, color.b, alpha);
                }
            }
            yield return new WaitForSeconds(.1f);
        }
        Destroy(transform.root.gameObject, 0.5f);
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
}
