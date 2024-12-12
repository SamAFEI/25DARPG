using UnityEngine;
using UnityEngine.UI;

public class UI_Live2D : MonoBehaviour
{
    public Button BT_Setting;
    public Button BT_Insertion;
    public Button BT_Orgasm;
    public Button BT_Exit;
    public SettingManager settingManager;
    public Animator anim;


    private void Awake()
    {
        BT_Insertion.onClick.AddListener(() => { PlayInsertion(); });
        BT_Orgasm.onClick.AddListener(() => { PlayOrgasm(); });
        BT_Setting.onClick.AddListener(() => { settingManager.OpenUI_Area(); });
        BT_Exit.onClick.AddListener(() => { GameManager.LoadGameScene(); });
    }

    private void Start()
    {
        PlayInsertion();
    }

    private void PlayOrgasm()
    {
        anim.Play("Orgasm");
    }

    private void PlayInsertion()
    {
        anim.Play("Insertion");
    }
}
