using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHint : MonoBehaviour
{
    public GameObject ui_ResistH_Hint { get; private set; }
    public Image ui_A_Icon { get; private set; }
    public Image ui_D_Icon { get; private set; }
    public Slider ui_Slider { get; private set; }
    public bool isEnable;
    public Color lightenColor;
    public Color darkenColor;
    public Coroutine ctFlashResistIcon;

    private void Awake()
    {
        ui_ResistH_Hint = transform.Find("UI_ResistH_Hint").gameObject;
        ui_A_Icon = ui_ResistH_Hint.transform.Find("UI_A_Icon").GetComponent<Image>();
        ui_D_Icon = ui_ResistH_Hint.transform.Find("UI_D_Icon").GetComponent<Image>();
        ui_Slider = ui_ResistH_Hint.transform.Find("UI_Slider").GetComponent<Slider>();
        ui_ResistH_Hint.SetActive(false);
    }

    public void Enable()
    {
        ui_ResistH_Hint.SetActive(true);
        if (!isEnable)
        {
            ctFlashResistIcon = StartCoroutine(FlashResistIcon());
            ui_Slider.value = 0;
        }
        isEnable = true;
    }
    public void Disable()
    {
        if (ctFlashResistIcon != null) { StopCoroutine(ctFlashResistIcon); }
        isEnable = false;
        ui_ResistH_Hint.SetActive(false);
    }
    public void SetResistHint(SexResistEnum _mode, bool _value)
    {
        if (_mode == SexResistEnum.Horizontal)
        {
            ui_ResistH_Hint.gameObject.gameObject.SetActive(_value);
        }
    }

    public bool SetSliderValue(int value)
    {
        ui_Slider.value += value;
        return ui_Slider.value == ui_Slider.maxValue;
    }

    public IEnumerator FlashResistIcon()
    {
        while (true)
        {
            ui_A_Icon.color = lightenColor;
            ui_D_Icon.color = darkenColor;
            yield return new WaitForSeconds(0.2f);
            ui_A_Icon.color = darkenColor;
            ui_D_Icon.color = lightenColor;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
