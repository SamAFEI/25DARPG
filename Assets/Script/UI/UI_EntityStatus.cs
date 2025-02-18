using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_EntityStatus : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private RectTransform myRectTransform => GetComponent<RectTransform>();

    public Slider hpSlider { get; private set; }
    private float hpSmooth;

    private void Awake()
    {
        hpSlider = transform.Find("UI_Health").GetComponent<Slider>();
    }

    private void Start()
    {
        hpSlider.maxValue = entity.MaxHp;
        hpSlider.value = hpSlider.maxValue;
        hpSlider.gameObject.SetActive(false);
    }

    public void DoLerpHealth()
    {
        hpSmooth = 0;
        StartCoroutine(LerpHealth());
    }

    private IEnumerator LerpHealth()
    {
        float smooth = 10;
        float startHP = hpSlider.value;
        while (hpSmooth < 1)
        {
            hpSmooth += Time.deltaTime * smooth;
            hpSlider.value = Mathf.Lerp(startHP, entity.CurrentHp, hpSmooth);
            hpSlider.gameObject.SetActive(!entity.IsSexing);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        hpSlider.gameObject.SetActive(false);
    }
}
