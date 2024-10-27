using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossStatus : MonoBehaviour
{
    public Entity boss;
    private RectTransform myRectTransform => GetComponent<RectTransform>();

    public Slider hpSlider { get; private set; }
    private float hpSmooth;

    private void Awake()
    {
        hpSlider = transform.Find("UI_Health").GetComponent<Slider>();
        SetUIStateActive(false);
    }

    public void DoLerpHealth()
    {
        hpSmooth = 0;
        StartCoroutine(LerpHealth());
    }

    private IEnumerator LerpHealth()
    {
        float smooth = 5;
        float startHP = hpSlider.value;
        while (hpSmooth < 1)
        {
            hpSmooth += Time.deltaTime * smooth;
            hpSlider.value = Mathf.Lerp(startHP, boss.CurrentHp, hpSmooth);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }

    public UI_BossStatus SetUIStateActive(bool _value, Entity entity = null)
    {
        hpSlider.gameObject.SetActive(_value);
        if (_value)
        {
            boss = entity;
            hpSlider.transform.localScale = new Vector3(0, 1, 1);
            hpSlider.maxValue = boss.MaxHp;
            hpSlider.value = hpSlider.maxValue;
            StartCoroutine(LerpBarScale());
        }
        return this;
    }

    private IEnumerator LerpBarScale()
    {
        float smooth = 0.01f;
        hpSlider.transform.localScale = new Vector3(0, 1, 1);
        float ScaleSmooth = 0;
        while (ScaleSmooth < 1)
        {
            ScaleSmooth += Time.deltaTime * smooth;
            hpSlider.transform.localScale = new Vector3(Mathf.Lerp(hpSlider.transform.localScale.x, 1, ScaleSmooth), 1, 1);
            if (hpSlider.transform.localScale.x > 0.9)
            {
                ScaleSmooth = 1;
                hpSlider.transform.localScale = new Vector3(1, 1, 1);
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }
}
