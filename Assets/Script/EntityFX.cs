using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    public List<GameObject> attackFXs;
    public List<GameObject> hitFXs;

    private void Awake()
    {
        foreach (GameObject fxObj in attackFXs)
        {
            fxObj.SetActive(false);
        }
        foreach (GameObject fxObj in hitFXs)
        {
            fxObj.SetActive(false);
        }
    }

    private void Start()
    {
    }

    public void DoPlayAttackFX(int _index)
    {
        if (attackFXs.Count <= _index) { return; }
        StartCoroutine(PlayAttackFX(_index));
    }

    private IEnumerator PlayAttackFX(int _index)
    {
        attackFXs[_index].SetActive(true);
        yield return new WaitForSeconds(0.3f);
        attackFXs[_index].SetActive(false);
    }

    public void DoPlayHitFX(int _index, Vector3 _point)
    {
        if (hitFXs.Count <= _index) { return; }
        GameObject obj = Instantiate(hitFXs[_index], _point, Quaternion.identity, transform);
        obj.SetActive(true);
        Destroy(obj, 0.3f);
    }
}
