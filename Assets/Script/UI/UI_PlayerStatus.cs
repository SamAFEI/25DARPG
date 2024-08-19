using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStatus : MonoBehaviour
{
    private Player player => GameObject.FindObjectOfType<Player>();
    private RectTransform myRectTransform => GetComponent<RectTransform>();

    public Slider hpSlider { get; private set; }
    private float hpSmooth;

    private void Awake()
    {
        hpSlider = transform.Find("UI_Health").GetComponent<Slider>();
    }

    private void Start()
    {
        hpSlider.maxValue = player.MaxHp;
        hpSlider.value = hpSlider.maxValue;
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
            hpSlider.value = Mathf.Lerp(startHP, player.CurrentHp, hpSmooth);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
    }
}
