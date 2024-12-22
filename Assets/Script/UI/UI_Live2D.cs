using System.Security.Claims;
using UnityEngine;
using UnityEngine.UI;

public class UI_Live2D : MonoBehaviour
{
    public Button BT_Setting;
    public Button BT_Idle;
    public Button BT_Insertion;
    public Button BT_Orgasm;
    public Button BT_Exit;
    public SettingManager settingManager;
    public CGTrigger CG;


    private void Awake()
    {
        BT_Idle.onClick.AddListener(() => { PlayIdle(); });
        BT_Insertion.onClick.AddListener(() => { PlayInsertion(); });
        BT_Orgasm.onClick.AddListener(() => { PlayOrgasm(); });
        BT_Setting.onClick.AddListener(() => { settingManager.OpenUI_Area(); });
        BT_Exit.onClick.AddListener(() => { GameManager.LoadGameScene(); });
    }

    private void Start()
    {
        PlayIdle();
    }

    private void PlayIdle()
    {
        CG.PlayIdle();
    }

    private void PlayOrgasm()
    {
        CG.PlayOrgasm();
    }

    private void PlayInsertion()
    {
        CG.PlayInsertion();
    }
}
