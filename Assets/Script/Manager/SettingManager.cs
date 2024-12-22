using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour, ISaveManager
{
    public static SettingManager Instance { get; private set; }
    public GameObject UI_Area { get; private set; }
    public TMP_Dropdown DP_Resolutions { get; private set; }
    public TMP_Dropdown DP_Languages { get; private set; }
    public Toggle TG_FullScreen { get; private set; }
    public Slider SL_BGMVolume { get; private set; }
    public Slider SL_SEVolume { get; private set; }
    public Slider SL_VoiceVolume { get; private set; }
    public Button BT_Return { get; private set; }
    public Button BT_Title { get; private set; }
    public List<string> languageList { get; private set; } = new List<string>();
    public LanguageEnum language { get; private set; }
    public Resolution resolution { get; private set; }
    public bool isFullScreen { get; private set; }
    public List<Resolution> systemResolutions { get; private set; }

    private string languageText;
    private string resolutionText;
    private float seVolume_DelayTime;
    private float voiceVolume_DelayTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        UI_Area = transform.Find("UI_Area").gameObject;
        DP_Resolutions = transform.Find("UI_Area/UI_Menu/DP_Resolutions").GetComponent<TMP_Dropdown>();
        DP_Languages = transform.Find("UI_Area/UI_Menu/DP_Languages").GetComponent<TMP_Dropdown>();
        TG_FullScreen = transform.Find("UI_Area/UI_Menu/TG_FullScreen").GetComponent<Toggle>();
        SL_BGMVolume = transform.Find("UI_Area/UI_Menu/SL_BGMVolume").GetComponent<Slider>();
        SL_SEVolume = transform.Find("UI_Area/UI_Menu/SL_SEVolume").GetComponent<Slider>();
        SL_VoiceVolume = transform.Find("UI_Area/UI_Menu/SL_VoiceVolume").GetComponent<Slider>();
        BT_Return = transform.Find("UI_Area/UI_Menu/BT_Return").GetComponent<Button>();
        BT_Title = transform.Find("UI_Area/UI_Menu/BT_Title").GetComponent<Button>();

        SL_BGMVolume.onValueChanged.AddListener((value) => { OnChangeVolume(VolumeType.BGMVolume, value); });
        SL_SEVolume.onValueChanged.AddListener((value) => { OnChangeVolume(VolumeType.SEVolume, value); });
        SL_VoiceVolume.onValueChanged.AddListener((value) => { OnChangeVolume(VolumeType.VoiceVolume, value); });
        DP_Languages.onValueChanged.AddListener((value) => { OnChangeLanguage(value); });
        DP_Resolutions.onValueChanged.AddListener((value) => { OnChangeResolution(value); });
        TG_FullScreen.onValueChanged.AddListener((value) => { OnChangeFullScreen(value); });
        BT_Return.onClick.AddListener(() => { OpenUI_Area(); });
        BT_Title.onClick.AddListener(() => { ReturnTitle(); });
        UI_Area.SetActive(false);
    }

    private void Update()
    {
        DelayPlayTest();
    }

    private static void OnChangeVolume(VolumeType type, float volume)
    {
        AudioManager.AdjustVolume(type, volume);
        if (type == VolumeType.SEVolume)
        {
            Instance.seVolume_DelayTime = 0.3f;
        }
        else if (type == VolumeType.VoiceVolume)
        {
            Instance.voiceVolume_DelayTime = 0.3f;
        }
    }
    private static void OnChangeLanguage(int index)
    {
        Instance.language = (LanguageEnum)index;
        Instance.languageText = Instance.DP_Languages.captionText.text;
        Instance.StartCoroutine(Instance.SetLocale(index));
        if (Instance.UI_Area.activeSelf)
        {
            AudioManager.PlaySelectSE();
        }
    }
    private static void OnChangeResolution(int index)
    {
        Instance.resolution = Instance.systemResolutions[index];
        Instance.resolutionText = Instance.DP_Resolutions.captionText.text;
        Screen.SetResolution(Instance.resolution.width, Instance.resolution.height, Instance.isFullScreen);
        if (Instance.UI_Area.activeSelf)
        {
            AudioManager.PlaySelectSE();
        }
    }
    private static void OnChangeFullScreen(bool value)
    {
        Instance.isFullScreen = value;
        Screen.SetResolution(Instance.resolution.width, Instance.resolution.height, Instance.isFullScreen);
        if (Instance.UI_Area.activeSelf)
        {
            AudioManager.PlaySelectSE();
        }
    }
    private void DelayPlayTest()
    {
        if (!UI_Area.activeSelf)
        {
            seVolume_DelayTime = 0;
            voiceVolume_DelayTime = 0;
            return;
        }
        if (seVolume_DelayTime > 0)
        {
            seVolume_DelayTime -= Time.unscaledDeltaTime;
            if (seVolume_DelayTime < 0)
            {
                AudioManager.PlayTest(VolumeType.SEVolume);
            }
        }
        if (voiceVolume_DelayTime > 0)
        {
            voiceVolume_DelayTime -= Time.unscaledDeltaTime;
            if (voiceVolume_DelayTime < 0)
            {
                AudioManager.PlayTest(VolumeType.VoiceVolume);
            }
        }
    }

    private static void CreateDP_Language()
    {
        Instance.language = LanguageEnum.EN;
        if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
        {
            Instance.language = LanguageEnum.CHT;
        }
        else if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            Instance.language = LanguageEnum.JP;
        }
        else if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
        {
            Instance.language = LanguageEnum.CHS;
        }
        Instance.languageList.Clear();
        Instance.languageList.Add("简体中文");
        Instance.languageList.Add("繁體中文");
        Instance.languageList.Add("English");
        Instance.languageList.Add("日本語");
        Instance.DP_Languages.ClearOptions();
        Instance.DP_Languages.AddOptions(Instance.languageList);
        int _value = (int)Instance.language;
        for (int i = 0; i < Instance.languageList.Count; i++)
        {
            if (Instance.languageList[i].Equals(Instance.languageText))
            {
                _value = i;
                break;
            }
        }
        Instance.DP_Languages.value = _value;
        Instance.DP_Languages.RefreshShownValue();
        //OnChangeLanguage(Instance.DP_Languages.value);
    }
    private static void CreateDP_Resolution()
    {
        Instance.resolution = Screen.currentResolution;

        Instance.systemResolutions = new List<Resolution>();
        List<Resolution> _resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToList();
        Instance.DP_Resolutions.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < _resolutions.Count; i++)
        {
            if (Mathf.Approximately(9f / 16f, (float)_resolutions[i].height / (float)_resolutions[i].width))
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height;
                options.Add(option);
                Instance.systemResolutions.Add(_resolutions[i]);
            }
        }
        Instance.DP_Resolutions.AddOptions(options);
        int _value = Instance.systemResolutions.Count - 1;
        for (int i = 0; i < Instance.systemResolutions.Count; i++)
        {
            if (Instance.systemResolutions[i].width + " x " + Instance.systemResolutions[i].height 
                == Instance.resolutionText)
            {
                _value = i;
                break;
            }
        }
        Instance.TG_FullScreen.isOn = Instance.isFullScreen;
        Instance.DP_Resolutions.value = _value;
        Instance.DP_Resolutions.RefreshShownValue();
        //OnChangeResolution(Instance.DP_Resolutions.value);
    }

    private IEnumerator SetLocale(int _localeId)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeId];
    }

    public void SaveData(ref GameData data)
    {
        data.Setting = new SettingStore();
        data.Setting.BGMVolume = AudioManager.GetVolume(VolumeType.BGMVolume);
        data.Setting.SEVolume = AudioManager.GetVolume(VolumeType.SEVolume);
        data.Setting.VoiceVolume = AudioManager.GetVolume(VolumeType.VoiceVolume);
        data.Setting.Language = languageText;
        data.Setting.IsFullScreen = Instance.isFullScreen;
        data.Setting.Resolution = resolutionText;
    }

    public void LoadData(GameData data)
    {
        Instance.isFullScreen = Screen.fullScreen;
        if (data.Setting != null)
        {
            AudioManager.AdjustVolume(VolumeType.BGMVolume, data.Setting.BGMVolume);
            AudioManager.AdjustVolume(VolumeType.SEVolume, data.Setting.SEVolume);
            AudioManager.AdjustVolume(VolumeType.VoiceVolume, data.Setting.VoiceVolume);
            Instance.isFullScreen = data.Setting.IsFullScreen;
            languageText = data.Setting.Language;
            resolutionText = data.Setting.Resolution;
        }
#if !UNITY_WEBPLAYER
        TG_FullScreen.isOn = Instance.isFullScreen;
        CreateDP_Resolution();
#endif
        CreateDP_Language();
        Instance.SL_BGMVolume.value = AudioManager.GetVolume(VolumeType.BGMVolume);
        Instance.SL_SEVolume.value = AudioManager.GetVolume(VolumeType.SEVolume);
        Instance.SL_VoiceVolume.value = AudioManager.GetVolume(VolumeType.VoiceVolume);
    }

    public void OpenUI_Area()
    {
        AudioManager.PlaySelectSE();
        GameManager.PausedGame(!UI_Area.activeSelf);
        if (UI_Area.activeSelf)
        {
            SaveManager.SaveSetting();
            UI_DialogCanvas.Close();
            UI_Area.SetActive(false);
            return;
        }
        UI_Area.SetActive(true);
    }

    public void ReturnTitle()
    {
        UI_DialogCanvas.Show("是否確定返回標題畫面？",
                () =>
                {
                    SaveManager.SaveSetting();
                    GameManager.LoadTitleScene();
                },
                () => { }
            );
    }
}
public enum LanguageEnum
{
    CHS, CHT, EN, JP
}
