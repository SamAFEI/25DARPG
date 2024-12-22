using SharpUI.Source.Common.UI.Elements.Button;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_TitleCanvas : MonoBehaviour
{
    public Button BT_NewGame;
    public Button BT_Continue;
    public Button BT_Setting;
    public Button BT_Quit;
    public SettingManager settingManager;
    private bool hasSaveData;
    private void Awake()
    {
        BT_NewGame.onClick.AddListener(() => { LoadGame(true); });
        BT_Continue.onClick.AddListener(() => { LoadGame(false); });
        BT_Setting.onClick.AddListener(() => { settingManager.OpenUI_Area(); });
        BT_Quit.onClick.AddListener(() => { QuitGame(); });
    }

    private void Start()
    {
        CheckHaveSaveData();
        AudioManager.PlayBGM(0);
    }

    private void CheckHaveSaveData()
    {
        hasSaveData = SaveManager.CheckHaveSaveData();
        BT_Continue.GetComponent<RectButton>().SetEnabled(hasSaveData);
    }

    private void LoadGame(bool isNew)
    {
        AudioManager.PlaySelectSE();
        if (isNew)
        {
            if (hasSaveData)
            {
                UI_DialogCanvas.Show("是否覆蓋目前進度，重新開始遊戲？",
                    () =>
                    {
                        SaveManager.Instance.DeleteSaveData();
                        GameManager.LoadGameScene();
                    },
                    () =>
                    {
                        return;
                    }
                );
            }
            else
            {
                GameManager.LoadGameScene();
            }
        }
        else
        {
            GameManager.LoadGameScene();
        }
    }
    private void QuitGame()
    {
        AudioManager.PlaySelectSE();
        UI_DialogCanvas.Show("確定要離開遊戲？",
            () =>
            {
                Application.Quit();
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
            },
            () =>
            {
            }
        );
    }
}
