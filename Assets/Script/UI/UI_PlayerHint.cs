using UnityEngine;

public class UI_PlayerHint : MonoBehaviour
{
    public GameObject ui_ResistH_Hint { get; private set; }

    private void Awake()
    {
        ui_ResistH_Hint = transform.Find("UI_ResistH_Hint").gameObject;

        ui_ResistH_Hint.gameObject.SetActive(false);
    }

    public void SetResistHint(SexResistEnum _mode, bool _value)
    {
        if (_mode == SexResistEnum.Horizontal)
        {
            ui_ResistH_Hint.gameObject.gameObject.SetActive(_value);
        }
    }
}
