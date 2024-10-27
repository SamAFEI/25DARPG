using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    public List<GameObject> attackFXs;
    public List<GameObject> hitFXs;
    public List<GameObject> buffFXs;

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
        ParticleSystem particle = attackFXs[_index].GetComponent<ParticleSystem>();
        if (particle != null) 
        { 
            particle.Play();
            yield return new WaitForSeconds(particle.startLifetime);
            particle.Stop();
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        attackFXs[_index].SetActive(false);
    }

    public void DoPlayHitFX(int _index, Vector3 _point)
    {
        if (hitFXs.Count <= _index) { return; }
        GameObject obj = Instantiate(hitFXs[_index], _point, Quaternion.identity, transform);
        obj.SetActive(true);
        Destroy(obj, 0.3f);
    }

    public void DoPlayBuffFX(int _index, float time = 0)
    {
        if (buffFXs.Count <= _index) { return; }
        StartCoroutine(PlayBuffFX(_index, time));
    }

    private IEnumerator PlayBuffFX(int _index, float time = 0)
    {
        buffFXs[_index].SetActive(true);
        ParticleSystem particle = buffFXs[_index].GetComponent<ParticleSystem>();
        if (particle != null)
        {
            if (time == 0) { time = particle.startLifetime; }
            particle.Play();
            yield return new WaitForSeconds(time);
            particle.Stop();
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        buffFXs[_index].SetActive(false);
    }
}
