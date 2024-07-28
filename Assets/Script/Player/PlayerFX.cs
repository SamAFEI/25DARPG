using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    public List<GameObject> shalshFXs;

    private void Awake()
    {
        foreach (GameObject fxObj in shalshFXs)
        {
            fxObj.SetActive(false);
        }
    }

    private void Start()
    {
    }

    public void DoPlayShalsh(int _index)
    {
        if (shalshFXs.Count <= _index) { return; }
        StartCoroutine(PlayShalsh(_index));
    }

    private IEnumerator PlayShalsh(int _index)
    {
        shalshFXs[_index].SetActive(true);
        yield return new WaitForSeconds(0.3f);
        shalshFXs[_index].SetActive(false);
    }
}
