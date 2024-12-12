using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public SettingStore Setting;
    public List<Inventory> Inventories;
    public string SavePointName;

    public GameData()
    {
    }
}

[System.Serializable]
public class SettingStore
{
    public string Resolution;
    public bool IsFullScreen;
    public string Language;
    public float BGMVolume;
    public float SEVolume;
    public float VoiceVolume;
    public SettingStore()
    {
    }
}